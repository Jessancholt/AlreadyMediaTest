using Test.DataAccess.Entities;

namespace Test.Core.Models;

public class MeteoriteContext
{
    public List<MeteoriteData> Data { get; set; }
    public List<int> PossibleYears { get; set; }
    public List<string> PossibleClasses { get; set; }
}

public class MeteoriteData
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Recclass { get; init; }
    public float Mass { get; init; }
    public string Fall { get; init; }
    public int Year { get; init; }
    public float Reclat { get; init; }
    public float Reclong { get; init; }

    public MeteoriteData(Meteorite entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        Id = entity.Id;
        Name = entity.Name;
        Recclass = entity.Class.Name;
        Mass = entity.Mass;
        Fall = entity.Fall.ToString();
        Year = entity.Year.Year;
        Reclat = entity.Reclat;
        Reclong = entity.Reclong;
    }
}
