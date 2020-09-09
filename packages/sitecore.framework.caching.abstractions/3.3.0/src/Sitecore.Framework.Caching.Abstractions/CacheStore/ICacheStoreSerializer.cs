namespace Sitecore.Framework.Caching
{
    public interface ICacheStoreSerializer
    {
        byte[] Serialize(object obj);
        object Deserialize(byte[] data);
    }
}