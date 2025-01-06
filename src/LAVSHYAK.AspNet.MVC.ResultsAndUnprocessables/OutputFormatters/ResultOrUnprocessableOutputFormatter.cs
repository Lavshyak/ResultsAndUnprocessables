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

    private static readonly PropertyInfo ObjectPropertyInfo;
    private static readonly PropertyInfo ObjectTypePropertyInfo;
    static ResultOrUnprocessableOutputFormatter()
    {
        var t = typeof(OutputFormatterCanWriteContext);
        ObjectPropertyInfo = t.GetProperty("Object") ?? throw new MissingMemberException();
        ObjectTypePropertyInfo = t.GetProperty("ObjectType") ?? throw new MissingMemberException();
    }
    
    public bool CanWriteResult(OutputFormatterCanWriteContext context)
    {
        if (context.Object is not IResultOrUnprocessable resultOrUnprocessable)
        {
            return false;
        }

        if (resultOrUnprocessable.IsSuccess)
        {
            //поменять обьект ответа, чтобы асп нет его сам записал в ответ обычным способом
            ObjectPropertyInfo.SetValue(context, resultOrUnprocessable.ResultObject);
            ObjectTypePropertyInfo.SetValue(context, resultOrUnprocessable.ResultObject?.GetType());
            
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
            throw new UnreachableException();
            var serialized = JsonSerializer.Serialize(resultOrUnprocessable.ResultObject);
            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = MediaTypeNames.Application.Json;
            await response.WriteAsync(serialized);
        }
    }
}