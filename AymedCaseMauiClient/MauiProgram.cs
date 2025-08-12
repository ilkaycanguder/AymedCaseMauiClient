using Microsoft.Extensions.Logging;
using AymedCaseMauiClient.Services;
using AymedCaseMauiClient.ViewModels;
using AymedCaseMauiClient.Views;
using AymedCaseMauiClient.Helpers;
using Grpc.Net.Client;
using System.Net;

namespace AymedCaseMauiClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // gRPC için HTTP/2 desteğini etkinleştir
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // gRPC için HTTP/2 desteğini yapılandır
            var defaultHandler = new SocketsHttpHandler
            {
                EnableMultipleHttp2Connections = true,
                KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1)
            };

            // gRPC istemci fabrikası yapılandırması
            builder.Services.AddSingleton(services =>
            {
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<GrpcServerService>();
                return new GrpcServerService(logger);
            });

            // DI kayıtları
            builder.Services.AddSingleton<IGrpcClientService, GrpcClientService>();
            builder.Services.AddSingleton<ServerViewModel>();
            builder.Services.AddSingleton<AnswerViewModel>();
            builder.Services.AddSingleton<Views.ServerPage>();
            builder.Services.AddSingleton<Views.AnswerPage>();

            var app = builder.Build();
            ServiceHelper.Init(app.Services);
            return app;
        }
    }
}
