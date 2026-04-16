using Core.Interfaces;

namespace API.Middleware;

public class RedirectMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IShortLinkService linkService)
    {
        // 1. get the path, trim leading slash → "3KwDnTc"
        var path = context.Request.Path.Value?.TrimStart('/');
        // 2. skip if empty or starts with reserved prefixes (api, swagger)
        var pathHead = path?.Split('/')[0];

        if ( string.IsNullOrEmpty(path)
            || pathHead == "api"
            || pathHead == "swagger"
            || pathHead == "favicon.ico")
        {
            await next(context);
            return;
        }
        // 3. call linkService.ResolveAndTrackAsync with path + request headers
        var url = await linkService.ResolveAndTrackAsync(
            path,
            context.Connection.RemoteIpAddress?.ToString(),
            context.Request.Headers["User-Agent"].ToString(),
            context.Request.Headers["Referer"].ToString()
        );
        // 4. if url returned → context.Response.Redirect(url) and return
        if (url != null)
        {
            context.Response.Redirect(url);
            return;
        }
        // 5. if null → await next(context)
        await next(context);
        return;
    }
}