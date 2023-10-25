using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Accelerate.Web.Extensions
{
    /// <summary>
    /// Extensions class for <see cref="HttpContext" /> class.
    /// </summary>
    internal static class HttpContextExtensions
    {
        /// <summary>
        /// Set a proper value for trace identifier.
        /// </summary>
        /// <param name="httpContext">
        /// Http context information.
        /// </param>
        public static void SetTraceIdentifier(this HttpContext httpContext)
        {
            var buildTraceId = true;
            var headerKeys = new String[]
            {
                "correlation-id",
                "x-correlation-id",
                "request-id",
                "x-request-id"
            };

            foreach (var requestHeader in httpContext.Request.Headers)
            {
                if (headerKeys.Any(x => x == requestHeader.Key.ToLowerInvariant()))
                {
                    buildTraceId = false;
                    httpContext.TraceIdentifier = $"{requestHeader.Value}";
                    break;
                }
            }

            if (buildTraceId)
            {
                httpContext.TraceIdentifier = $"{Guid.NewGuid()}";
            }
        }
    }
}