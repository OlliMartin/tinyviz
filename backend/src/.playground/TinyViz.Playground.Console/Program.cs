using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Plotly.NET;
using Plotly.NET.CSharp;
using Plotly.NET.LayoutObjects;
using TinyViz.Contracts;
using TinyViz.DataSource.Prometheus.Extensions;
using TinyViz.Renderer;
using TinyViz.Renderer.Renderers;

var configuration = new ConfigurationBuilder()
    .AddInMemoryCollection(new Dictionary<string, string?> { ["TinyViz:DataSources:Prom"] = "" })
    .Build();

await using var serviceProvider = new ServiceCollection().AddPrometheusDataSource(configuration).BuildServiceProvider();

var dataSource = serviceProvider.GetRequiredService<IDataSource>();

var res = await dataSource.QueryRangeAsync(
    new(
        Query: """
        power_consumption{device=~"NAS|audio-interface"}
        """,
        "prom"
    ),
    CancellationToken.None
);

IGraphRenderer renderer = new PngGraphRenderer();

var name = res.TimeSeries.First().Descriptor.Name;
var x = res.TimeSeries.First().Values.Select(v => v.Timestamp).ToArray();
var y = res.TimeSeries.First().Values.Select(v => v.Value).ToArray();

double? value = 44.5; // null => N/A
double max = 100;

// build gauge object
var gauge = new Dictionary<string, object>();

// axis: NO ticks or labels
var axis = new Dictionary<string, object>
{
    { "range", new[] { 0, max } },
    { "tickwidth", 0 },
    { "tickcolor", "rgba(0,0,0,0)" },
    { "showticklabels", false },
    { "showline", false }, // no outline at the edges
};

gauge["axis"] = axis;

// remove border and threshold artifacts
gauge["borderwidth"] = 0;
gauge["bordercolor"] = "rgba(0,0,0,0)";

gauge["threshold"] = new Dictionary<string, object>
{
    {
        "line",
        new Dictionary<string, object> { { "color", "rgba(0,0,0,0)" }, { "width", 0 } }
    },
    { "value", 0 }, // dummy
};

// value indicator bar (THIN white bar from 0 to value)
var bar = new Dictionary<string, object>
{
    { "color", "#ffffff" }, // visible indicator color
    { "thickness", 0.3 }, // fraction of gauge thickness
};

gauge["bar"] = bar;

// thickness of the colored arc (background bands)
gauge["thickness"] = 0.5;
gauge["shape"] = "angular";
gauge["bgcolor"] = "rgba(0,0,0,0)";

// color bands (steps)
var steps = new[]
{
    new Dictionary<string, object> { { "range", new[] { 0, 70 } }, { "color", "#2ECC71" } },
    new Dictionary<string, object> { { "range", new[] { 70, 90 } }, { "color", "#F1C40F" } },
    new Dictionary<string, object> { { "range", new[] { 90, 100 } }, { "color", "#E74C3C" } },
};

gauge["steps"] = steps;

// indicator trace
var gaugeTrace = new Trace("indicator");
gaugeTrace.SetValue("type", "indicator");
gaugeTrace.SetValue("mode", "gauge");
gaugeTrace.SetValue("value", value ?? 0.0);
gaugeTrace.SetValue("gauge", gauge);

// title
var title = new Dictionary<string, object>
{
    { "text", "CPU Busy" },
    {
        "font",
        new Dictionary<string, object> { { "size", 20 }, { "color", "#ffffff" } }
    },
};

gaugeTrace.SetValue("title", title);

// domain (you can tweak y to adjust how “deep” the arc looks)
var domain = new Dictionary<string, object> { { "x", new[] { 0.0, 1.0 } }, { "y", new[] { 0.2, 1.0 } } };
gaugeTrace.SetValue("domain", domain);

// main layout
var layout = new Layout();
layout.SetValue("paper_bgcolor", "#000000");
layout.SetValue("plot_bgcolor", "#000000");
layout.SetValue("showlegend", value: false);

layout.SetValue("font", new Dictionary<string, object> { { "color", "#ffffff" }, { "family", "Inter, Arial, sans-serif" } });

// margins
const double marginVal = 3;

var margin = new Dictionary<string, double>
{
    { "l", marginVal },
    { "r", marginVal },
    { "t", marginVal + 25 },
    { "b", 0 },
};

layout.SetValue("margin", margin);

// central label (N/A or value)
var displayText = value.HasValue ? $"{value.Value:F0}%" : "N/A";

var annotation = new Dictionary<string, object>
{
    { "x", 0.5 },
    { "y", 0.3 },
    { "xref", "paper" },
    { "yref", "paper" },
    { "text", displayText },
    { "showarrow", false },
    {
        "font",
        new Dictionary<string, object> { { "size", 30 }, { "color", "#ffffff" } }
    },
};

layout.SetValue("annotations", new[] { annotation });

var chart = GenericChart.ofTraceObject(useDefaults: false, gaugeTrace).WithLayout(layout);

var tempFile = Path.GetTempFileName();

var renderedChart = await renderer.RenderAsync(chart, dimensions: 144);

Console.WriteLine("hi");
