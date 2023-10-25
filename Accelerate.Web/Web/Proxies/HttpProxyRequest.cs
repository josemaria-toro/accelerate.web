using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Accelerate.Web.Proxies
{
    /// <summary>
    /// Http proxy request information.
    /// </summary>
    public class HttpProxyRequest
    {
        /// <summary>
        /// Body of the request.
        /// </summary>
        public String Body { get; set; }
        /// <summary>
        /// Type of body contents.
        /// </summary>
        public String ContentType { get; set; }
        /// <summary>
        /// List of cookies to send.
        /// </summary>
        public IEnumerable<Cookie> Cookies { get; set; }
        /// <summary>
        /// List of headers to send.
        /// </summary>
        public IDictionary<String, String> Headers { get; set; }
        /// <summary>
        /// Method to use in the request.
        /// </summary>
        public HttpMethod Method { get; set; }
        /// <summary>
        /// Url parameters to send.
        /// </summary>
        public IDictionary<String, Object> Parameters { get; set; }
        /// <summary>
        /// Url path of the request.
        /// </summary>
        public String Path { get; set; }
    }
}
