using System.Threading.Tasks;

namespace Sitecore.Framework.Eventing
{
    public interface IDistributor
    {
        Task Distribute(IMessage message);
    }
}