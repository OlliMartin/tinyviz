using TinyViz.RestApi.Extensions;
using TinyViz.WebUi.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddRestApi(configuration).AddWebUi(configuration);

var app = builder.Build();

app.UseRouting();

app.MapRestApi().MapWebUi();

app.Run();
