using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.EnumTools;
using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Models;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.OperationFilters;

public class SuccessOrUnprocessableOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var returnType = context.MethodInfo.ReturnType;

        var taskType = typeof(Task<>);
        if (returnType.Name == taskType.Name && returnType.Namespace == taskType.Namespace)
        {
            returnType = returnType.GenericTypeArguments.First();
        }

        var rightType = typeof(SuccessOrUnprocessable<>);
        if (returnType.Name != rightType.Name || returnType.Namespace != rightType.Namespace)
        {
            return;
        }

        if (!operation.Responses.TryGetValue("200", out var response200))
        {
            response200 = new OpenApiResponse();
            operation.Responses.Add("200", response200);
        }

        if (string.IsNullOrWhiteSpace(response200.Description))
        {
            response200.Description = "Success";
        }

        response200.Content.Clear();


        // error
        var errorEnumType = returnType.GenericTypeArguments.First();

        const string key = "422";
        var response = EnumDescriptionTools.GetResponseForOpenApi(errorEnumType, context);

        if (response is null)
            return;

        operation.Responses.Add(key, response);
    }
}