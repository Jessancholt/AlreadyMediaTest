using Test.DataAccess.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Test.Core.Models;

internal class Changes<T>
{
    public List<T> Removed { get; } = new();
    public List<T> Updated { get; } = new();
    public List<T> Added { get; } = new();
    public bool Has => Removed.Count != 0 || Updated.Count != 0 || Added.Count != 0;
}

internal class MeteoriteChanges
{
    public Changes<Meteorite> Meteorites { get; } = new();
    public Changes<MeteoriteClass> Classes { get; } = new();
    public Changes<DiscoveryYear> Years { get; } = new();
    public bool Has => Meteorites.Has || Classes.Has || Years.Has;
}
