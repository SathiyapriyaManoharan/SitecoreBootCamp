using Newtonsoft.Json;

namespace Sitecore.Framework.Diagnostics
{
    public class JsonPageOutputSerializer : IPageOutputSerializer
    {
        public string ContentType => "application/json";

        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}