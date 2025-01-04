using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.EnumTools;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Models;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.OperationFilters;

public class ResultOrUnprocessableOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var returnType = context.MethodInfo.ReturnType;
        
        var taskType = typeof(Task<>);
        if (returnType.Name == taskType.Name && returnType.Namespace == taskType.Namespace)
        {
            returnType = returnType.GenericTypeArguments.First();
        }

        var rightType = typeof(ResultOrUnprocessable<,>);
        if (returnType.Name != rightType.Name || returnType.Namespace != rightType.Namespace)
        {
            return;
        }

        operation.Responses.Clear();

        var resultType = returnType.GenericTypeArguments.First();

        if (!operation.Responses.TryGetValue("200", out var response200))
        {
            response200 = new OpenApiResponse();
            operation.Responses.Add("200", response200);
        }

        response200.Description = "Success";
        response200.Content.Add("application/json", new OpenApiMediaType
        {
            Schema = context.SchemaGenerator.GenerateSchema(resultType, context.SchemaRepository)
        });

        //operation.Responses.Add("200", response200);

        // error
        var errorEnumType = returnType.GenericTypeArguments.Last();

        var response422 = EnumDescriptionTools.GetResponseForOpenApi(errorEnumType, context);

        if (response422 is null)
            return;

        operation.Responses.Add("422", response422);
    }
}