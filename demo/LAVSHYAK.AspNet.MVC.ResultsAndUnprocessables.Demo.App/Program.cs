using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.OperationFilters;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.OutputFormatters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.OpenApi.Models;

Console.WriteLine("CWD: " + Directory.GetCurrentDirectory());

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers(options =>
{
    options.OutputFormatters.RemoveType<StringOutputFormatter>(); // suddenly it won't work without it
    options.OutputFormatters.Insert(0, new SuccessOrUnprocessableOutputFormatter());
    options.OutputFormatters.Insert(0, new ResultOrUnprocessableOutputFormatter());
});

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API V1", Version = "v1" });

    var basePath = Directory.GetCurrentDirectory();

    options.IncludeXmlComments(Path.Combine(basePath, "LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Demo.App.xml"),
        true); //for documentation of models, etc.

    options.OperationFilter<ResultOrUnprocessableOperationFilter>();
    options.OperationFilter<SuccessOrUnprocessableOperationFilter>();
    options.OperationFilter<FileResultOperationFilter>();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();