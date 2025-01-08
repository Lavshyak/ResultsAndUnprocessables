using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.DocumentFilters;

public class RemoveResultOrUnprocessableDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        List<string> keysToRemove = [];

        foreach (var key in context.SchemaRepository.Schemas.Keys)
        {
            if (key.EndsWith("ResultOrUnprocessable"))
            {
                keysToRemove.Add(key);
            }
        }

        foreach (var key in keysToRemove)
        {
            context.SchemaRepository.Schemas.Remove(key);
        }
    }
}