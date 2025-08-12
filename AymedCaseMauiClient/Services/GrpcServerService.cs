using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;



namespace AymedCaseMauiClient.Services
{
    public class GrpcServerService
    {
        private bool _isServerRunning;
        private readonly ILogger? _logger;
        private CancellationTokenSource? _cts;
        private bool _isRunning;
        private int _port;

        public GrpcServerService(ILogger<GrpcServerService>? logger = null)
        {
            _logger = logger;
            _isRunning = false;
        }

        public bool IsRunning => _isRunning;
        public int Port => _port;

        public Task<bool> StartServerAsync(int port)
        {
            if (_isRunning)
            {
                _logger?.LogWarning("Server is already running on port {Port}", _port);
                return Task.FromResult(false);
            }

            try
            {
                _port = port;
                _cts = new CancellationTokenSource();

                // Simüle edilmiş sunucu başlatma
                _isServerRunning = true;
                _isRunning = true;

                _logger?.LogInformation("Server started on port {Port}", port);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to start server on port {Port}", port);
                _isRunning = false;
                return Task.FromResult(false);
            }
        }

        public Task<bool> StopServerAsync()
        {
            if (!_isRunning)
            {
                _logger?.LogWarning("Server is not running");
                return Task.FromResult(false);
            }

            try
            {
                _cts?.Cancel();

                // Simüle edilmiş sunucu durdurma
                _isServerRunning = false;
                _isRunning = false;
                _logger?.LogInformation("Server stopped");
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to stop server");
                return Task.FromResult(false);
            }
        }
    }

    // Simüle edilmiş servis implementasyonu - gerçekte kullanılmıyor
    // GrpcClientService sınıfı doğrudan simüle edilmiş mesajları üretiyor
}
