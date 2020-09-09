namespace Sitecore.Framework.Caching
{
    public interface ICachable
    {
        long SizeInBytes { get; }
        object Value { get; }
    }

    public interface ICachable<T> : ICachable
    {
        new T Value { get;  }
    }
}