using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Portfolio_Box.Models;

public class RemoteFileAvailabilityChecker : IDisposable
{
	private readonly CancellationTokenSource _canceller = new();
	private readonly IConfiguration _configuration;
	private readonly HttpClient _httpClient;
	private readonly ILogger<RemoteFileAvailabilityChecker> _logger;

	public RemoteFileAvailabilityChecker(ILogger<RemoteFileAvailabilityChecker> logger, IConfiguration configuration)
	{
		_configuration = configuration;
		_logger = logger;
#pragma warning disable S4830 // Server certificates should be verified during SSL/TLS connections
		var handler = new HttpClientHandler
		{
			ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
		};
#pragma warning restore S4830 // Server certificates should be verified during SSL/TLS connections
		_httpClient = new HttpClient(handler);
		_ = DoCheck();
	}

	public bool IsAvailable { get; private set; }

	public void Dispose()
	{
		_canceller.Cancel();
		_canceller.Dispose();
	}

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
				_logger.LogError(ex, "Remote computer seems unavailable");
			}

			if (!_canceller.IsCancellationRequested)
				await Task.Delay(wait);
		}
	}
}