using Sitecore.Commerce.Services;

namespace Feature.Website.Managers.Messages
{
    public class RemoveFromCompareResult : ServiceProviderResult
    {
        public string RemovedSellableItemId { get; set; }
    }
}