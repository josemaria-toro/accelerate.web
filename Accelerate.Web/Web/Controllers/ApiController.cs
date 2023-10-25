using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Accelerate.Web.Controllers
{
    /// <summary>
    /// Controller for web api applications.
    /// </summary>
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        /// <summary>
        /// Build a response based on status code and contents.
        /// </summary>
        /// <param name="statusCode">
        /// Response status code.
        /// </param>
        protected static IActionResult StatusCode(HttpStatusCode statusCode)
        {
            return new StatusCodeResult((Int32)statusCode);
        }
        /// <summary>
        /// Build a response based on status code and contents.
        /// </summary>
        /// <param name="statusCode">
        /// Response status code.
        /// </param>
        /// <param name="contents">
        /// Contents of response.
        /// </param>
        protected static IActionResult StatusCode<T>(HttpStatusCode statusCode, T contents)
        {
            return StatusCode<T>(statusCode, contents, "application/json");
        }
        /// <summary>
        /// Build a response based on status code and contents.
        /// </summary>
        /// <param name="statusCode">
        /// Response status code.
        /// </param>
        /// <param name="contents">
        /// Contents of response.
        /// </param>
        /// <param name="contentType">
        /// Content type of response contents.
        /// </param>
        protected static IActionResult StatusCode<T>(HttpStatusCode statusCode, T contents, String contentType)
        {
            var objectResult = new ObjectResult(contents)
            {
                StatusCode = (Int32)statusCode
            };

            objectResult.ContentTypes.Add(contentType);
            objectResult.DeclaredType = typeof(T);

            return objectResult;
        }
    }
}