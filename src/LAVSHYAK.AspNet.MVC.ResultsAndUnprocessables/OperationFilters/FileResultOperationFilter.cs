using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.OperationFilters;

public class FileResultOperationFilter : IOperationFilter
{
    private bool CanReturnFileResult(Type type)
    {
        var next = type;
        while (true)
        {
            if (next == typeof(FileResult)) return true;

            next = next.GenericTypeArguments.FirstOrDefault();

            if (next is null) return false;
        }
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!CanReturnFileResult(context.MethodInfo.ReturnType))
        {
            return;
        }

        var response200 = operation.Responses["200"];
        response200.Content.Clear();
        var response200Content = new OpenApiMediaType()
        {
            Example = new OpenApiBinary([99, 55, 133, 58, 166, 88, 52, 66])
        };
        response200.Content.Add("application/octet-stream", response200Content);

        var replaced = response200.Description.Replace("Success", "Success. File.<br/>");

        if (replaced == response200.Description)
        {
            response200.Description = "Success. File.<br/>" + response200.Description;
        }
        else
        {
            response200.Description = replaced;
        }

        response200.Headers.Add("Content-Type",
            new OpenApiHeader() {Description = "Type of the content.", Example = new OpenApiString("application/octet-stream")});
        response200.Headers.Add("Content-Length",
            new OpenApiHeader() {Description = "Body length in bytes.", Example = new OpenApiInteger(200)});
    }
}