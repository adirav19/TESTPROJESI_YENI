namespace TESTPROJESI.Core.Mapping
{
    /// <summary>
    /// 🔄 Generic Mapper Interface
    /// DTO ↔ Entity dönüşümleri için temel arayüz
    /// </summary>
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);
        TSource MapBack(TDestination destination);
        IEnumerable<TDestination> MapList(IEnumerable<TSource> sources);
    }

    /// <summary>
    /// 🔄 Base Mapper - Ortak mapping logic'i
    /// </summary>
    public abstract class BaseMapper<TSource, TDestination> : IMapper<TSource, TDestination>
    {
        public abstract TDestination Map(TSource source);
        public abstract TSource MapBack(TDestination destination);

        public virtual IEnumerable<TDestination> MapList(IEnumerable<TSource> sources)
        {
            return sources?.Select(Map) ?? Enumerable.Empty<TDestination>();
        }
    }
}
