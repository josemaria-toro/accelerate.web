# Base objects for web layer
## Introduction
This library help us to increase the speed for develop web controllers or custom filters and proxies, providing classes for that.  
## How to install
For installing this package, you must execute the following command:  
```
dotnet add package accelerate.web
```
## How to use
### Writing a custom api controller
``` csharp
public class MyController : ApiController
{
    /// <summary>
    /// Add the documentation of the endpoint.
    /// </summary>
    /// <response code="000">
    /// Add the documentation when returning a status code.
    /// </response>
    [HttpGet]
    [Route("getall")]
    [ProducesResponseType(typeof(Object), 200, "application/json")]
    public IAsyncResult GetAll()
    {
        // Add your code here.
    }
}
```
### Writing a custom web controller
``` csharp
public class MyController : WebController
{
    /// <summary>
    /// Add the documentation of the endpoint.
    /// </summary>
    /// <response code="000">
    /// Add the documentation when returning a status code.
    /// </response>
    [HttpGet]
    [Route("getall")]
    [ProducesResponseType(typeof(String), 200, "text/html")]
    public IAsyncResult GetAll()
    {
        // Add your code here.
        return View();
    }
}
```
### Writing a custom action filter
``` csharp
public class MyActionFilter : ActionFilter
{
    /// <inheritdoc />
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        base.OnActionExecuted(context);

        // Add your code here.
    }
    /// <inheritdoc />
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        // Add your code here.
    }
}
```
### Writing a custom authorization filter
``` csharp
public class MyAuthorizationFilter : AuthorizationFilter
{
    /// <inheritdoc />
    public override void OnAuthorization(AuthorizationFilterContext context)
    {
        base.OnAuthorization(context);

        // Add your code here and set the proper user information.
    }
}
```
### Writing a custom exception handler filter
``` csharp
public class MyExceptionFilter : ExceptionFilter
{
    /// <inheritdoc />
    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);

        // Add your code here and set the proper response.
    }
}
```
### Writing a custom http proxy
#### Writing a custom http proxy options
``` csharp
public class MyHttpProxyOptions : HttpProxyOptions
{
    // Declare properties to expose
}
```
#### Writing a custom http proxy
``` csharp
public class MyHttpProxy : HttpProxy<MyHttpProxyOptions>
{
    private Boolean _disposed;

    /// <summary>
    /// Initialize a new instance of <seealso cref="MyHttpProxy" /> class.
    /// </summary>
    /// <param name="options">
    /// Configuration options of http proxy.
    /// </param>
    public MyHttpProxy(IOptions<MyHttpProxyOptions> options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override void Dispose(Boolean disposing)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }

        base.Dispose(disposing);

        if (disposing)
        {
            // Free resources memory allocation
        }

        _disposed = true;
    }

    public IEnumerable<Object> GetAll()
    {
        // Add your code here.

        return Array.Empty<Object>();
    }
}
```
## Changes history
**Version 6.0.0**
- Changed version to a system based on .NET Core version supported.  
**Version 1.0.0**
- Include base class for api and web controllers, http proxies, action, authorization and exception handling filters.  
- Feature to capture or create the correlation ID has been included in all web filters.  