using System;

namespace Accelerate.Web.Proxies
{
    /// <summary>
    /// Configuration options for proxies.
    /// </summary>
    public class HttpProxyOptions
    {
        /// <summary>
        /// Base url of remote endpoint.
        /// </summary>
        public String BaseUrl { get; set; }
        /// <summary>
        /// Requests timeout in seconds.
        /// </summary>
        public Int32 Timeout { get; set; }
        /// <summary>
        /// User agent used by proxy.
        /// </summary>
        public String UserAgent { get; set; }
    }
}
