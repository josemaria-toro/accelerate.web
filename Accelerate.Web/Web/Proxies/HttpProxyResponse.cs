using System;
using System.Collections.Generic;
using System.Net;

namespace Accelerate.Web.Proxies
{
    /// <summary>
    /// Http proxy response information.
    /// </summary>
    public class HttpProxyResponse
    {
        /// <summary>
        /// Body of the response in json format.
        /// </summary>
        public String Body { get; set; }
        /// <summary>
        /// List of headers received.
        /// </summary>
        public IDictionary<String, String> Headers { get; set; }
        /// <summary>
        /// Status message of the response.
        /// </summary>
        public String Message { get; set; }
        /// <summary>
        /// Status code of the response.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
    }
}
