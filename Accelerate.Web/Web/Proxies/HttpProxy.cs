using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Accelerate.Web.Proxies
{
    /// <summary>
    /// Base class for http proxies.
    /// </summary>
    public abstract class HttpProxy<T> : IHttpProxy where T : HttpProxyOptions, new()
    {
        private Version _assemblyVersion;
        private Boolean _disposed;
        private T _options;

        /// <summary>
        /// Initialize a new instance of <seealso cref="HttpProxy{T}" /> class.
        /// </summary>
        /// <param name="options">
        /// HTTP proxy configuration options.
        /// </param>
        protected HttpProxy(IOptions<T> options)
        {
            if (options == null)
            {
                throw new ArgumentException($"Argument '{nameof(options)}' cannot be null or empty", nameof(options));
            }

            _assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            _options = options.Value;
        }

        /// <summary>
        /// Configuration options for HTTP proxy.
        /// </summary>
        protected T Options => _options;

        /// <summary>
        /// Build the uri of the request.
        /// </summary>
        /// <param name="proxyRequest">
        /// Request information.
        /// </param>
        private Uri BuildRequestUri(HttpProxyRequest proxyRequest)
        {
            var uriBuilder = new UriBuilder(Options.BaseUrl);

            if (proxyRequest.Parameters != null && proxyRequest.Parameters.Any())
            {
                var paramsArray = proxyRequest.Parameters.Select(x => x.Key + "=" + x.Value)
                                                         .ToArray();

                var queryParams = String.Join("&", paramsArray);

                if (String.IsNullOrEmpty(uriBuilder.Query))
                {
                    uriBuilder.Query = queryParams;
                }
                else
                {
                    uriBuilder.Query = $"{uriBuilder.Query}&{queryParams}";
                }
            }

            if (String.IsNullOrEmpty(uriBuilder.Path))
            {
                uriBuilder.Path = proxyRequest.Path;
            }
            else if (uriBuilder.Path.EndsWith("/"))
            {
                uriBuilder.Path = $"{uriBuilder.Path}{proxyRequest.Path}";
            }
            else
            {
                uriBuilder.Path = $"{uriBuilder.Path}/{proxyRequest.Path}";
            }

            uriBuilder.Path = uriBuilder.Path.Replace("//", "/");

            return uriBuilder.Uri;
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        /// Indicate if object is currently freeing, releasing, or resetting unmanaged resources.
        /// </param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (disposing)
            {
                _assemblyVersion = null;
                _options = null;
            }

            _disposed = true;
        }
        /// <summary>
        /// Handle the response.
        /// </summary>
        /// <param name="httpResponse">
        /// Response information.
        /// </param>
        private static HttpProxyResponse HandleResponse(HttpResponseMessage httpResponse)
        {
            var proxyResponse = new HttpProxyResponse();

            if (httpResponse.Content != null)
            {
                var readTask = httpResponse.Content.ReadAsStringAsync();
                readTask.Wait();
                proxyResponse.Body = readTask.Result;
            }

            proxyResponse.Headers = new Dictionary<String, String>();

            if (httpResponse.Headers != null && httpResponse.Headers.Any())
            {
                foreach (var header in httpResponse.Headers)
                {
                    if (header.Value.Count() > 1)
                    {
                        var headerValue = String.Join("; ", header.Value);
                        proxyResponse.Headers.Add(header.Key, headerValue);
                    }
                    else
                    {
                        var headerValue = header.Value.FirstOrDefault();
                        proxyResponse.Headers.Add(header.Key, headerValue);
                    }
                }
            }

            proxyResponse.Message = httpResponse.ReasonPhrase;
            proxyResponse.StatusCode = httpResponse.StatusCode;

            return proxyResponse;
        }
        /// <summary>
        /// Send a request to a remote endpoint.
        /// </summary>
        /// <param name="proxyRequest">
        /// Request information.
        /// </param>
        protected virtual HttpProxyResponse Send(HttpProxyRequest proxyRequest)
        {
            if (proxyRequest == null)
            {
                throw new ArgumentException($"Argument '{nameof(proxyRequest)}' cannot be null or empty", nameof(proxyRequest));
            }

            HttpProxyResponse proxyResponse = null;

            using (var httpClient = new HttpClient())
            {
                if (String.IsNullOrEmpty(proxyRequest.ContentType))
                {
                    proxyRequest.ContentType = "application/json";
                }

                if (proxyRequest.Cookies != null && proxyRequest.Cookies.Any())
                {
                    var cookies = proxyRequest.Cookies.Select(x => x.Name + "=" + x.Value)
                                                      .ToArray();

                    httpClient.DefaultRequestHeaders.Add("Cookie", String.Join(";", cookies));
                }

                var contentType = new MediaTypeWithQualityHeaderValue(proxyRequest.ContentType);
                var productName = String.IsNullOrEmpty(Options.UserAgent) ? "Accelerate" : Options.UserAgent;
                var userAgent = new ProductInfoHeaderValue(productName, $"{_assemblyVersion}");

                httpClient.DefaultRequestHeaders.Accept.Add(contentType);
                httpClient.DefaultRequestHeaders.UserAgent.Add(userAgent);

                if (proxyRequest.Headers != null && proxyRequest.Headers.Any())
                {
                    foreach (var header in proxyRequest.Headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                httpClient.Timeout = TimeSpan.FromSeconds(Options.Timeout);

                var requestMessage = new HttpRequestMessage
                {
                    Method = proxyRequest.Method,
                    RequestUri = BuildRequestUri(proxyRequest)
                };

                if (!String.IsNullOrEmpty(proxyRequest.Body))
                {
                    requestMessage.Content = new StringContent(proxyRequest.Body, Encoding.UTF8, proxyRequest.ContentType);
                }

                var sendTask = httpClient.SendAsync(requestMessage);

                try
                {
                    sendTask.Wait();
                    proxyResponse = HandleResponse(sendTask.Result);
                }
                catch (AggregateException ex) when (ex.InnerException is HttpRequestException)
                {
                    proxyResponse = new HttpProxyResponse
                    {
                        Message = ex.InnerException.Message,
                        StatusCode = HttpStatusCode.ServiceUnavailable
                    };
                }
                catch (AggregateException ex) when (ex.InnerException is TaskCanceledException)
                {
                    proxyResponse = new HttpProxyResponse
                    {
                        Message = ex.InnerException.Message,
                        StatusCode = HttpStatusCode.RequestTimeout
                    };
                }
            }

            return proxyResponse;
        }
    }
}
