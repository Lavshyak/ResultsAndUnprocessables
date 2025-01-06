using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.EnumTools;

public record EnumFieldInfo(int ValueInt, string Name, string Description);

internal static class EnumDescriptions<TEnum> where TEnum : Enum
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly IReadOnlyList<EnumFieldInfo> EnumFieldInfos;

    static EnumDescriptions()
    {
        var type = typeof(TEnum);
        var fields = type.GetFields().Where(field => field.FieldType.FullName != typeof(Int32).FullName).ToArray();
        var enumFieldInfos = new EnumFieldInfo[fields.Length];
        
        for (var i = 0; i < fields.Length; i++)
        {
            var field = fields[i];

            var value = (TEnum) field.GetRawConstantValue()!;
            var valueInt = Convert.ToInt32(value);
            if (valueInt != i)
                throw new UnreachableException("Что-то пошло не так)");

            var name = field.Name;
            
            var descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();
            var description = descriptionAttribute?.Description;
            if (string.IsNullOrWhiteSpace(description))
                description = name;
            
            var enumFieldInfo =
                new EnumFieldInfo(valueInt, name, description);
            enumFieldInfos[i] = enumFieldInfo;
        }

        EnumFieldInfos = enumFieldInfos; //new string[fields.Length];
    }

    public static EnumFieldInfo GetDescription(TEnum value) => EnumFieldInfos[Convert.ToInt32(value)];

    public static IEnumerable<EnumFieldInfo> GetDescriptions() => EnumFieldInfos;
}

internal static class EnumDescriptionTools
{
    public static EnumFieldInfo GetEnumFieldInfo<TEnum>(TEnum value) where TEnum : Enum
    {
        return EnumDescriptions<TEnum>.GetDescription(value);
    }

    public static OpenApiResponse GetResponseForOpenApi(Type errorEnumType, OperationFilterContext context)
    {
        if (errorEnumType.BaseType?.FullName != "System.Enum")
        {
            throw new ArgumentException();
        }

        var enumDescriptionType = typeof(EnumDescriptions<>).MakeGenericType(errorEnumType);
        var method = enumDescriptionType.GetMethod("GetDescriptions") ?? throw new UnreachableException();
        var invokeResult = method.Invoke(null, null) ?? throw new UnreachableException();
        var descriptions = ((IEnumerable<EnumFieldInfo>) invokeResult).ToArray();

        var strings = descriptions.Select(enumFieldInfo =>
            $"{enumFieldInfo.ValueInt} : {enumFieldInfo.Name} : {enumFieldInfo.Description}");

        var firstDescription = descriptions.First();

        var response = new OpenApiResponse
        {
            Description = string.Join("<br/>", strings)
        };

        var newOpenApiMediaType = new OpenApiMediaType
        {
            Schema = context.SchemaGenerator.GenerateSchema(typeof(UnprocessableHttpRequestInfo<>).MakeGenericType(errorEnumType),
                context.SchemaRepository),
            Examples =
            {
                ["application/json"] = new OpenApiExample
                {
                    Value = new OpenApiObject()
                    {
                        [nameof(UnprocessableHttpRequestInfo<Enum>.Code).ToLower()] =
                            new OpenApiInteger(firstDescription.ValueInt),
                        [nameof(UnprocessableHttpRequestInfo<Enum>.Name).ToLower()] =
                            new OpenApiString(firstDescription.Name),
                        [nameof(UnprocessableHttpRequestInfo<Enum>.Description).ToLower()] =
                            new OpenApiString(firstDescription.Description),
                    }
                }
            }
        };
        
        response.Content.Add("application/json", newOpenApiMediaType);

        return response;
    }
}