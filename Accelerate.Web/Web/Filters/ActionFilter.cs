using Accelerate.Web.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Accelerate.Web.Filters
{
    /// <summary>
    /// A filter that surrounds execution of the action.
    /// </summary>
    public abstract class ActionFilter : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            context.HttpContext.SetTraceIdentifier();
        }
    }
}