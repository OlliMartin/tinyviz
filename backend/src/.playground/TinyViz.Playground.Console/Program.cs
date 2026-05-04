// See https://aka.ms/new-console-template for more information

using Plotly.NET;
using Plotly.NET.CSharp;
using Chart = Plotly.NET.CSharp.Chart;
using GenericChartExtensions = Plotly.NET.CSharp.GenericChartExtensions;

GenericChartExtensions.Show(
    GenericChartExtensions.WithTraceInfo(
            Chart.Point<double, double, string>(
                [1, 2,],
                [5, 10,]
            ),
            "Hello from C#",
            ShowLegend: true
        )
        .WithXAxisStyle<double, double, string>(Title: Title.init("xAxis"))
        .WithYAxisStyle<double, double, string>(Title: Title.init("yAxis"))
);