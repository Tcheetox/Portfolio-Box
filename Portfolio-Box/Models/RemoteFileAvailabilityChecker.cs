using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Portfolio_Box.Models
{
    public class RemoteFileAvailabilityChecker : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RemoteFileAvailabilityChecker> _logger;
        private readonly HttpClient _httpClient;

        private readonly CancellationTokenSource _canceller = new();
        public RemoteFileAvailabilityChecker(ILogger<RemoteFileAvailabilityChecker> logger, IConfiguration configuration, HttpClient client)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = client;
            _ = DoCheck();
        }

        public bool IsAvailable { get; private set; }

        private async Task DoCheck()
        {
            var endpoint = $"{_configuration.GetValue<string>("Remoting:Endpoint")}/healthz";
            var wait = _configuration.GetValue<TimeSpan>("Remoting:CheckInterval");
            while (!_canceller.IsCancellationRequested) 
            {
                try
                {
                    var q = await _httpClient.GetAsync(endpoint, _canceller.Token);
                    q.EnsureSuccessStatusCode();
                    IsAvailable = true;
                }
                catch (Exception ex)
                {
                    IsAvailable = false;
                    _logger.LogWarning("Remote computer seems unavailable: {message}", ex.Message);
                }

                if (!_canceller.IsCancellationRequested)
                    await Task.Delay(wait);
            }
        }

        public void Dispose()
        {
            _canceller.Cancel();
            _canceller.Dispose();
        }
    }
}
