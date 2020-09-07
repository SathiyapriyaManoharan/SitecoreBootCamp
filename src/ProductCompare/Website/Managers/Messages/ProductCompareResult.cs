using Sitecore.Commerce.Services;
using Plugin.Bootcamp.Exercises.ProductCompare.Entities;

namespace Feature.Website.Managers.Messages
{
    public class ProductCompareResult : ServiceProviderResult
    {
        public ProductComparison ProductComparison { get; set; }
    }
}