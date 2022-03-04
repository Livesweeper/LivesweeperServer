namespace BombDetect.Network.Server;

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        // give me an IWebHostEnvironment
        var env = builder.Environment;

        app.MapGet("/", () => "this is a BombDetect server, you should go away unless you're a BombDetect game");

        // enable web sockets
        app.UseWebSockets();

        // use!
        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/test")
            {
                // is this a web socket request
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                }
                // no
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("hey this isn't a websocket request, what are you doing here?");
                }
            }
            else
            {
                await next();
            }
        });

        app.Run();

    }
}