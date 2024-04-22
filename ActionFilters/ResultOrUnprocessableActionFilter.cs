using LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.ActionFilters;

public class ResultOrUnprocessableActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is not ObjectResult objectResult) return;
        
        if (objectResult.Value is not IResultOrUnprocessable resultOrUnprocessable) return;

        if (!resultOrUnprocessable.IsSuccess) return;
        
        if (resultOrUnprocessable.ResultObject is IActionResult actionResultForReplace)
        {
            context.Result = actionResultForReplace;
        }
        else
        {
            var objectResultForReplace = new ObjectResult(resultOrUnprocessable.ResultObject);
            context.Result = objectResultForReplace;
        }
    }
}