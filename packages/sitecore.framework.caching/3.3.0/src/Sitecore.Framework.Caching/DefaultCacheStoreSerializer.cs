using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sitecore.Framework.Caching
{
    public class DefaultCacheStoreSerializer : ICacheStoreSerializer
    {
        public byte[] Serialize(object obj)
        {
            IFormatter formatter = new BinaryFormatter();

            byte[] bytes;

            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, obj);

                bytes = ms.ToArray();
            }

            return bytes; ;
        }

        public object Deserialize(byte[] data)
        {
            IFormatter formatter = new BinaryFormatter();

            object deserializedData;
            using (MemoryStream ms = new MemoryStream(data))
            {
                deserializedData = formatter.Deserialize(ms);
            }
            return deserializedData;
        }
    }
}