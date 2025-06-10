using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Test.DatasetClient.Clients.Interfaces;
using Test.DatasetClient.Configurations;
using Test.DatasetConnect.Models;
using Test.Shared.Exceptions;

namespace Test.DatasetClient.Clients;

internal class NasaDatasetClient : INasaDatasetClient
{
    private readonly DatasetClientOptions _settings;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<NasaDatasetClient> _logger;

    public NasaDatasetClient(
        IOptions<DatasetClientOptions> options,
        IHttpClientFactory httpClientFactory,
        ILogger<NasaDatasetClient> logger)
    {
        _settings = options.Value;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<List<DatasetResponse>> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = _httpClientFactory.CreateClient();

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, _settings.BaseUrl);
            var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken);
            var json = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            if (string.IsNullOrEmpty(json))
            {
                _logger.LogError("Response is empty");
                return null;
            }

            return JsonConvert.DeserializeObject<List<DatasetResponse>>(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw new CoreException($"Failed to get dataset response", ex);
        }
    }
}
