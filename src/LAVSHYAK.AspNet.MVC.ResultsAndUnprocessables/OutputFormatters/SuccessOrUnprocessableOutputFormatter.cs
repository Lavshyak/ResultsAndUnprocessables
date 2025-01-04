using System.Diagnostics;
using System.Net.Mime;
using System.Text.Json;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Interfaces;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.OutputFormatters;

public class SuccessOrUnprocessableOutputFormatter : IOutputFormatter
{
    public bool CanWriteResult(OutputFormatterCanWriteContext context)
    {
        if(context.Object is ISuccessOrUnprocessable)
        {
            return true;
        }

        return false;
    }

    public async Task WriteAsync(OutputFormatterWriteContext context)
    {
        if (context.Object is not ISuccessOrUnprocessable successOrUnprocessable)
        {
            throw new UnreachableException();
        }
        
        var response = context.HttpContext.Response;
        
        if (successOrUnprocessable.IsSuccess)
        {
            response.StatusCode = StatusCodes.Status200OK;
        }
        else
        {
            var serialized = JsonSerializer.Serialize(successOrUnprocessable.UnprocessableInfoObject);
            response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            response.ContentType = MediaTypeNames.Application.Json;
            await response.WriteAsync(serialized);
        }
    }
}