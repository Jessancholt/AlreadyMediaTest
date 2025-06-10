using Test.Core.Models;

namespace Test.Core.Services.Interfaces;

public interface IMeteoriteService
{
    Task<MeteoriteContext> GetAsync(MeteoritesFilter filter, CancellationToken cancellationToken);
}
