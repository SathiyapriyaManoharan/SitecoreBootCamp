namespace Sitecore.Framework.Caching
{
    public class Cachable<T> : ICachable<T>
    {
        public long SizeInBytes { get; }
        public T Value { get; }
        object ICachable.Value => Value;

        public Cachable(T value, long sizeInBytes)
        {
            Value = value;
            SizeInBytes = sizeInBytes;
        }
    }
}