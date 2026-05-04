using Plotly.NET.CSharp;

Chart
    .Point<double, double, string>([1, 2], [5, 10])
    .WithTraceInfo("Hello from C#", ShowLegend: true)
    .WithXAxisStyle<double, double, string>(Title: Plotly.NET.Title.init("xAxis"))
    .WithYAxisStyle<double, double, string>(Title: Plotly.NET.Title.init("yAxis"))
    .Show();
