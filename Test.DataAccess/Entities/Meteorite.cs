using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Test.DataAccess.Entities.Predefined;

namespace Test.DataAccess.Entities;

public record class Meteorite
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public int ClassId { get; init; }
    public virtual MeteoriteClass Class { get; set; }
    public float Mass { get; init; }
    public FallType Fall { get; init; }
    public int YearId { get; init; }
    public virtual DiscoveryYear Year { get; set; }
    public float Reclat { get; init; }
    public float Reclong { get; init; }
    public int UniqueId { get; init; }
}

public record class MeteoriteClass
{
    public int Id { get; init; }
    public string Name { get; init; }
    public virtual Meteorite Meteorite { get; set; }
}

public record class DiscoveryYear
{
    public int Id { get; init; }
    public int Year { get; init; }
    public virtual Meteorite Meteorite { get; set; }
}

public class MeteoriteEqualityComparer : IEqualityComparer<Meteorite>
{
    public bool Equals([AllowNull] Meteorite x, [AllowNull] Meteorite y)
    {
        if (x == y)
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.Name == y.Name
           && x.Year == y.Year
           && x.Class.Name == y.Class.Name
           && x.Fall == y.Fall
           && x.Mass == y.Mass
           && x.Reclat == y.Reclat
           && x.Reclong == y.Reclong;
    }

    public int GetHashCode([DisallowNull] Meteorite obj)
    {
        return HashCode.Combine(obj.Id);
    }
}