namespace Sitecore.Framework.Diagnostics
{
    public interface IPageOutputSerializer
    {
        string ContentType { get; }

        string Serialize(object value);
    }
}