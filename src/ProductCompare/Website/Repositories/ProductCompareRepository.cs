using System.Collections.Generic;
using System.Linq;
using Feature.Website.Managers;
using Feature.Website.Models;
using Microsoft.OData.Client;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.XA.Feature.Catalog.MockData;
using Sitecore.Commerce.XA.Feature.Catalog.Models;
using Sitecore.Commerce.XA.Feature.Catalog.Repositories;
using Sitecore.Commerce.XA.Foundation.Catalog.Managers;
using Sitecore.Commerce.XA.Foundation.Common;
using Sitecore.Commerce.XA.Foundation.Common.Context;
using Sitecore.Commerce.XA.Foundation.Common.Models;
using Sitecore.Commerce.XA.Foundation.Common.Models.JsonResults;
using Sitecore.Commerce.XA.Foundation.Common.Search;
using Sitecore.Commerce.XA.Foundation.Connect;
using Sitecore.Commerce.XA.Foundation.Connect.Managers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;

namespace Feature.Website.Repositories
{
    public class ProductCompareRepository : BaseCatalogRepository, IProductCompareRepository
    {
        private readonly IModelProvider _modelProvider;
        private readonly ICompareManager _compareManager;
        private readonly ISiteContext _siteContext;
        private readonly IStorefrontContext _storefrontContext;
        private const int DefaultIntValue = -1;

        public ProductCompareRepository(IModelProvider modelProvider, ISiteContext siteContext, ICompareManager compareManager, IStorefrontContext storefrontContext, ISearchInformation searchInformation, ISearchManager searchManager, ICatalogManager catalogManager, ICatalogUrlManager catalogUrlManager, IContext context)
            : base(modelProvider, storefrontContext, siteContext, searchInformation, searchManager, catalogManager, catalogUrlManager, context)
        {
            Assert.ArgumentNotNull(modelProvider, nameof(modelProvider));
            _modelProvider = modelProvider;
            _compareManager = compareManager;
            _siteContext = siteContext;
            _storefrontContext = storefrontContext;
        }

        public virtual AddViewCompareButtonModel GetAddViewCompareButtonModel(IVisitorContext visitorContext)
        {
            var model = _modelProvider.GetModel<AddViewCompareButtonModel>();
            Init(model);
            if (Sitecore.Context.PageMode.IsExperienceEditor)
            {
                model.IsProductInCompareList = false;
                model.IsEdit = true;
            }
            else
            {
                var currentCatalogItem = _siteContext.CurrentCatalogItem;
                if (currentCatalogItem != null && _siteContext.IsProduct)
                    model.Initialize(currentCatalogItem);

                model.CatalogName = StorefrontContext.CurrentStorefront.Catalog;
                var productCompare = _compareManager.GetCurrentProductCompare(visitorContext, _storefrontContext);
                var productIsInCompare = productCompare?.Result != null &&
                                         productCompare.Result.Products.Any(x => x.FriendlyId == currentCatalogItem?.Name);

                model.IsProductInCompareList = productIsInCompare;
            }

            model.ViewCompareButtonText = Rendering.DataSourceItem["View Compare Text"];
            model.AddToCompareButtonText = Rendering.DataSourceItem["Add to Compare Text"];

            LinkField linkField = Rendering.DataSourceItem.Fields["Compare Page"];
            if (linkField != null && linkField.IsInternal)
            {
                model.IsValid = true;
                model.ComparePageLink = linkField.TargetItem != null ? LinkManager.GetItemUrl(linkField.TargetItem) : "";
            }
            else
            {
                model.IsValid = false;
                Log.Error("Unable to render add/view compare link, Compare Page field not populated in datasource.", this);
            }

            return model;
        }
      
        private static int GetIntegerFromField(Item scItem, string fieldName)
        {
            if (scItem?.Fields[fieldName] == null)
                return DefaultIntValue;

            return int.TryParse(scItem.Fields[fieldName].Value, out int val)
                   ? val
                   : DefaultIntValue;
        }

        public virtual BaseJsonResult AddProductToCompareCollection(IVisitorContext visitorContext, string catalogName, string productId, string variantId)
        {
            Assert.ArgumentNotNull(visitorContext, nameof(visitorContext));
            Assert.ArgumentNotNullOrEmpty(catalogName, nameof(catalogName));
            Assert.ArgumentNotNullOrEmpty(productId, nameof(productId));

            var model = _modelProvider.GetModel<BaseJsonResult>();

            var productCompare = _compareManager.AddProductToCompareCollection(visitorContext, _storefrontContext, catalogName, productId, variantId);
            if (!productCompare.ServiceProviderResult.Success)
            {
                model.SetErrors(productCompare.ServiceProviderResult);
                return model;
            }

            model.Success = true;
            return model;
        }

        public virtual ProductCompareModel GetProductCompareModel(IVisitorContext visitorContext)
        {
            var model = _modelProvider.GetModel<ProductCompareModel>();
            if (Sitecore.Context.PageMode.IsExperienceEditor)
            {
                model.Products = new List<ProductCompareListItemModel>();
                for (var i = 0; i <= 3; i++)
                {
                    model.Products.Add(GenerateMockProductCompareListModel());
                }
            }
            else
            {
                var productCompare = _compareManager.GetCurrentProductCompare(visitorContext, _storefrontContext);
                if (productCompare.ServiceProviderResult.Success)
                {
                    model.Products = ConvertSellableItemsToModelList(productCompare.Result.Products, visitorContext);
                    model.RemoveFromCompareText = Rendering.DataSourceItem["Remove from Compare Text"];
                    model.IsValid = true;
                }
            }
            return model;
        }

        private List<ProductCompareListItemModel> ConvertSellableItemsToModelList(DataServiceCollection<SellableItem> resultProducts, IVisitorContext visitorContext)
        {
            var items = new List<ProductCompareListItemModel>();

            foreach (var sellableItem in resultProducts)
            {
                var scItem = SearchManager.GetProduct(sellableItem.FriendlyId, StorefrontContext.CurrentStorefront.Catalog);
                if (scItem != null)
                {
                    var productCompareListItemModel = GenerateProductCompareListModel(scItem, sellableItem, visitorContext);
                    items.Add(productCompareListItemModel);
                }
                SiteContext.Items.Remove("CurrentCatalogItemRenderingModel");
            }
            return items;
        }

        private ProductCompareListItemModel GenerateProductCompareListModel(Item scItem, SellableItem sellableItem, IVisitorContext visitorContext)
        {
            return new ProductCompareListItemModel
            {
                CatalogItemRenderingModel = GetCatalogItemRenderingModel(visitorContext, scItem),
                Height = GetIntegerFromField(scItem, "Height"),
                Depth = GetIntegerFromField(scItem, "Depth"),
                Width = GetIntegerFromField(scItem, "Width"),
                SellableItemId = sellableItem.Id
            };
        }

        private ProductCompareListItemModel GenerateMockProductCompareListModel()
        {
            var model = _modelProvider.GetModel<CatalogItemRenderingModel>();
            return new ProductCompareListItemModel
            {
                CatalogItemRenderingModel = CatalogItemRenderingModelMockData.InitializeMockData(model),
                Height = 999,
                Depth = 999,
                Width = 999,
                IsEdit = true
            };
        }

        public BaseJsonResult RemoveProductFromCompareCollection(IVisitorContext visitorContext, string sellableItemId)
        {
            Assert.ArgumentNotNull(visitorContext, nameof(visitorContext));
            Assert.ArgumentNotNullOrEmpty(sellableItemId, nameof(sellableItemId));

            var model = _modelProvider.GetModel<BaseJsonResult>();            

            var productCompare = _compareManager.RemoveProductFromCompareCollection(visitorContext, _storefrontContext, sellableItemId);
            if (!productCompare.ServiceProviderResult.Success)
            {
                model.SetErrors(productCompare.ServiceProviderResult);
                return model;
            }

            model.Success = true;
            return model;
        }
    }
}