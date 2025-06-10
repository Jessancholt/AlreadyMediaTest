using Test.DatasetConnect.Models;

namespace Test.DatasetClient.Clients.Interfaces;

public interface INasaDatasetClient
{
    Task<List<DatasetResponse>> GetAsync(CancellationToken cancellationToken);
}
