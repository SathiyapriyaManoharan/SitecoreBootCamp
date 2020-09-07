using Plugin.Bootcamp.Exercises.ProductCompare.Entities;
using Feature.Website.Managers.Messages;
using Sitecore.Commerce.XA.Foundation.Connect;
using Sitecore.Commerce.XA.Foundation.Connect.Managers;
using Sitecore.Commerce.XA.Foundation.Common.Context;

namespace Feature.Website.Managers
{
    public interface ICompareManager
    {
        ManagerResponse<ProductCompareResult, ProductComparison> GetCurrentProductCompare(IVisitorContext visitorContext, IStorefrontContext storefrontContext);
        ManagerResponse<ProductCompareResult, ProductComparison> AddProductToCompareCollection(IVisitorContext visitorContext, IStorefrontContext storefrontContext, string catalogName, string productId, string variantId);
        ManagerResponse<RemoveFromCompareResult, string> RemoveProductFromCompareCollection(IVisitorContext visitorContext, IStorefrontContext storefrontContext, string sellableItemId);
    }
}