using Test.DataAccess.Entities;
using Test.DataAccess.Entities.Predefined;
using Test.DatasetConnect.Models;

namespace Test.Core.Converters;

internal static class MeteoriteConverter
{
    public static bool TryConvert(this DatasetResponse response, Guid id, List<MeteoriteClass> classes, List<DiscoveryYear> years, out Meteorite meteorite)
    {
        if (!Enum.TryParse(response.Fall, out FallType fall))
        {
            meteorite = null;
            return false;
        }

        var cl = classes.Find(x => x.Name == response.Recclass);
        var year = years.Find(x => x.Year == response.Year.Year);

        meteorite = new Meteorite()
        {
            Id = id,
            Name = response.Name,
            Year = year,
            Mass = response.Mass,
            Fall = fall,
            Class = cl,
            Reclat = response.Reclat,
            Reclong = response.Reclong,
            UniqueId = response.Id,
        };

        return true;
    }
}
