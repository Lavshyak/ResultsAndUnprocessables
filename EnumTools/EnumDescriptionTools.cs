using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.EnumTools;

internal static class EnumDescriptionTools
{
    private static readonly Dictionary<string, Dictionary<int, string>> Cache = new();


    public static string GetDescription<TEnum>(TEnum value) where TEnum : Enum
    {
        // Get the type
        var type = typeof(TEnum);

        var fullName = type.FullName ?? throw new UnreachableException();

        var found = Cache.TryGetValue(fullName, out var dict);

        if (found)
            return dict![Convert.ToInt32(value)];

        dict = new Dictionary<int, string>();

        var fields = type.GetFields().Where(field => field.FieldType.FullName != typeof(Int32).FullName).ToArray();
        for (var i = 0; i < fields.Length; i++)
        {
            var description = fields[i].GetCustomAttribute<DescriptionAttribute>();
            dict.Add(i, description?.Description ?? fields[i].Name);
        }
        
        Cache[fullName] = dict;

        return dict[Convert.ToInt32(value)];
    }

    public static OpenApiResponse? GetResponseForOpenApi(Type errorEnumType, OperationFilterContext context)
    {
        if (errorEnumType.BaseType?.FullName != "System.Enum")
        {
            throw new ArgumentException("Should be System.Enum", nameof(errorEnumType));
        }

        var props = errorEnumType.GetFields().Skip(1).ToArray();

        if (props.Length == 0)
            return null;


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
            Schema = context.SchemaGenerator.GenerateSchema(typeof(UnprocessableHttpRequestInfo),
                context.SchemaRepository),
            Examples =
            {
                ["application/json"] = new OpenApiExample
                {
                    Value = new OpenApiObject()
                    {
                        [nameof(UnprocessableHttpRequestInfo.Code).ToLower()] = new OpenApiInteger(0),
                        [nameof(UnprocessableHttpRequestInfo.Description).ToLower()] =
                            new OpenApiString(descriptions.First()),
                    }
                }
            }
        });

        return response;
    }
}