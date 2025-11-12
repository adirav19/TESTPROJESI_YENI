using System.Collections.Generic;

namespace TESTPROJESI.Services.Base
{
    public class ModuleServiceOptions
    {
        public int? DefaultLimit { get; init; } = 50;
        public string? DefaultSortField { get; init; }
        public bool DefaultSortDescending { get; init; }
        public IReadOnlyDictionary<string, string>? DefaultQueryParameters { get; init; }
    }
}
