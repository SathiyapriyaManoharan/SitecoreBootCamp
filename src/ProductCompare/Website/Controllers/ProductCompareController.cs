using System;
using System.Web.Mvc;
using System.Web.UI;
using Feature.Website.Models;
using Feature.Website.Repositories;
using Sitecore.Commerce.XA.Foundation.Common.Context;
using Sitecore.Commerce.XA.Foundation.Common.Controllers;
using Sitecore.Commerce.XA.Foundation.Common.Models;
using Sitecore.Commerce.XA.Foundation.Common.Models.JsonResults;
using Sitecore.Commerce.XA.Foundation.Connect;

namespace Feature.Website.Controllers
{
    public class ProductCompareController : BaseCommerceStandardController
    {
        private readonly IProductCompareRepository _productCompareRepository;
        private readonly IVisitorContext _visitorContext;
        private readonly IModelProvider _modelProvider;
        private readonly IStorefrontContext _storefrontContext;

        public ProductCompareController(IProductCompareRepository productCompareRepository, IVisitorContext visitorContext, IStorefrontContext storefrontContext, IModelProvider modelProvider, IContext sitecoreContext) : base(storefrontContext, sitecoreContext)
        {
            _productCompareRepository = productCompareRepository;
            _visitorContext = visitorContext;
            _modelProvider = modelProvider;
            _storefrontContext = storefrontContext;
        }

        [HttpGet]
        public ActionResult AddViewCompareButton()
        {
            return View(GetRenderingView(nameof(AddViewCompareButton)), _productCompareRepository.GetAddViewCompareButtonModel(_visitorContext));
        }

        [HttpGet]
        public ActionResult ProductCompare()
        {
            return View(GetRenderingView(nameof(ProductCompare)), _productCompareRepository.GetProductCompareModel(_visitorContext));
        }

        [AllowAnonymous]
        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult RemoveProductFromCompareList(string removeFromCompareProductId)
        {
            BaseJsonResult baseJsonResult;
            try
            {
                baseJsonResult = _productCompareRepository.RemoveProductFromCompareCollection(_visitorContext, removeFromCompareProductId);
            }
            catch (Exception ex)
            {
                baseJsonResult = _modelProvider.GetModel<RemoveFromProductCompareModel>();
                baseJsonResult.SetErrors(nameof(RemoveProductFromCompareList), ex);
            }
            return Json(baseJsonResult);
        }

        [AllowAnonymous]
        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult AddProductToCompareList(string addToCompareCatalogName, string addToCompareProductId, string addToCompareVariantId)
        {
            BaseJsonResult baseJsonResult;
            try
            {
                baseJsonResult = _productCompareRepository.AddProductToCompareCollection(_visitorContext, addToCompareCatalogName, addToCompareProductId, addToCompareVariantId);
            }
            catch (Exception ex)
            {
                baseJsonResult = _modelProvider.GetModel<BaseJsonResult>();
                baseJsonResult.SetErrors(nameof(AddProductToCompareList), ex);
            }
            return Json(baseJsonResult);
        }
    }
}