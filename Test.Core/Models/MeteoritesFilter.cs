using Test.Core.Models.Predefined;

namespace Test.Core.Models;

public class MeteoritesFilter
{
    public string Name { get; set; }
    public string Recclass { get; set; }
    public short? From { get; set; }
    public short? To { get; set; }
    public SortBy SortBy { get; set; }
    public bool IsEmpty => string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Recclass) && From is null && To is null;
}
