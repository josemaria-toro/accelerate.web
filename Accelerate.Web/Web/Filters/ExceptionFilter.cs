using Accelerate.Web.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Accelerate.Web.Filters
{
    /// <summary>
    /// A filter for perform exceptions handling.
    /// </summary>
    public abstract class ExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// Occurs when an unhandled exception was throwed.
        /// </summary>
        /// <param name="context">
        /// Context of the filter.
        /// </param>
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            context.HttpContext.SetTraceIdentifier();
        }
    }
}