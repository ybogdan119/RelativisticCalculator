using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RelativisticCalculator.API.Filters;

/// <summary>
/// An action filter that automatically returns a 400 Bad Request
/// response if the incoming model is invalid.
/// </summary>
public class ValidateModelAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Called before the action executes. Checks the ModelState and returns
    /// a 400 response if the model is invalid.
    /// </summary>
    /// <param name="context">The action executing context.</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }
}