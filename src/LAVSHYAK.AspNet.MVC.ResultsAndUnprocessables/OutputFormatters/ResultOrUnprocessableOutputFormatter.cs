using System.Diagnostics;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.OutputFormatters;

public class ResultOrUnprocessableOutputFormatter : IOutputFormatter
{
    public bool CanWriteResult(OutputFormatterCanWriteContext context)
    {
        if (context.Object is not IResultOrUnprocessable resultOrUnprocessable)
        {
            return false;
        }

        if (resultOrUnprocessable.IsSuccess)
        {
            return false;
        }

        return true;
    }

    public async Task WriteAsync(OutputFormatterWriteContext context)
    {
        if (context.Object is not IResultOrUnprocessable resultOrUnprocessable)
        {
            throw new InvalidCastException();
        }

        var response = context.HttpContext.Response;

        if (!resultOrUnprocessable.IsSuccess)
        {
            var serialized = JsonSerializer.Serialize(resultOrUnprocessable.UnprocessableInfoObject);
            response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            response.ContentType = MediaTypeNames.Application.Json;
            await response.WriteAsync(serialized);
        }
        else
        {
            // ResultOrUnprocessableOperationFilter replaces resultOrUnprocessable
            // with TOnSuccess if resultOrUnprocessable.IsSuccess is true
            throw new UnreachableException();
        }
    }
}