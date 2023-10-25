using Accelerate.Web.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Accelerate.Web.Filters
{
    /// <summary>
    /// A filter that confirms request authorization.
    /// </summary>
    public abstract class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Called early in the filter pipeline to confirm request is authorized.
        /// </summary>
        /// <param name="context">
        /// Context of the filter.
        /// </param>
        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            context.HttpContext.SetTraceIdentifier();
        }
    }
}