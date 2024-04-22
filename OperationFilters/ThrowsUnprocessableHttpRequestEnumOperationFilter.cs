using System.ComponentModel;
using System.Reflection;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Attributes;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.OperationFilters;

/// <summary>
/// better don't use exceptions
/// </summary>
public class ThrowsUnprocessableHttpRequestEnumOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var attr = context.MethodInfo.GetCustomAttribute<ThrowsUnprocessableHttpRequestEnumAttribute>();
        if(attr == null)
            return;

        var props = attr.EnumType.GetFields().Skip(1).ToArray();
		
        if(props.Length == 0)
            return;
		
        const string key = "422";
        List<string> descriptions = [];
        foreach (var propertyInfo in props)
        {
            var descriptionAttribute = propertyInfo.GetCustomAttribute<DescriptionAttribute>();
            descriptions.Add(descriptionAttribute is not null ? descriptionAttribute.Description : propertyInfo.Name);
        }

        var strings = descriptions.Select((d, i) => $"{i} : {d}");
        var response = new OpenApiResponse
        {
            Description = string.Join("<br/>", strings)
        };
        response.Content.Add("application/json", new OpenApiMediaType
        {
            Schema = context.SchemaGenerator.GenerateSchema(typeof(UnprocessableHttpRequestInfo), context.SchemaRepository),
            Examples = { ["application/json"] = new OpenApiExample
            {
                Value = new OpenApiObject()
                {
                    [nameof(UnprocessableHttpRequestInfo.Code).ToLower()] = new OpenApiInteger(0),
                    [nameof(UnprocessableHttpRequestInfo.Description).ToLower()] = new OpenApiString(descriptions.First()),
                }
            } }
        });
        operation.Responses.Add(key, response);
    }
}