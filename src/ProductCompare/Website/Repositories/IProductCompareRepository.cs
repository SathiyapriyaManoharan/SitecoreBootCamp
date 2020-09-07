using Feature.Website.Models;
using Sitecore.Commerce.XA.Foundation.Common.Context;
using Sitecore.Commerce.XA.Foundation.Common.Models.JsonResults;
using Sitecore.Commerce.XA.Foundation.Connect;

namespace Feature.Website.Repositories
{
    public interface IProductCompareRepository
    {
        AddViewCompareButtonModel GetAddViewCompareButtonModel(IVisitorContext visitorContext);

        BaseJsonResult AddProductToCompareCollection(IVisitorContext visitorContext, string catalogName, string productId, string variantId);

        ProductCompareModel GetProductCompareModel(IVisitorContext visitorContext);

        BaseJsonResult RemoveProductFromCompareCollection(IVisitorContext visitorContext, string sellableItemId);
    }
}