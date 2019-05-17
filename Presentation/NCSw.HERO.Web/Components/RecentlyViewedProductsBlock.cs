using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Core.Domain.Catalog;
using NCSw.HERO.Services.Catalog;
using NCSw.HERO.Services.Security;
using NCSw.HERO.Services.Stores;
using NCSw.HERO.Web.Factories;
using NCSw.HERO.Web.Framework.Components;
using NCSw.HERO.Web.Models.Catalog;

namespace NCSw.HERO.Web.Components
{
    public class RecentlyViewedProductsBlockViewComponent : NopViewComponent
    {
        private readonly CatalogSettings _catalogSettings;
        private readonly IAclService _aclService;
        private readonly IProductModelFactory _productModelFactory;
        private readonly IProductService _productService;
        private readonly IRecentlyViewedProductsService _recentlyViewedProductsService;
        private readonly IStoreMappingService _storeMappingService;

        public RecentlyViewedProductsBlockViewComponent(CatalogSettings catalogSettings,
            IAclService aclService,
            IProductModelFactory productModelFactory,
            IProductService productService,
            IRecentlyViewedProductsService recentlyViewedProductsService,
            IStoreMappingService storeMappingService)
        {
            this._catalogSettings = catalogSettings;
            this._aclService = aclService;
            this._productModelFactory = productModelFactory;
            this._productService = productService;
            this._recentlyViewedProductsService = recentlyViewedProductsService;
            this._storeMappingService = storeMappingService;
        }

        public IViewComponentResult Invoke(int? productThumbPictureSize, bool? preparePriceModel)
        {
            if (!_catalogSettings.RecentlyViewedProductsEnabled)
                return Content("");

            var preparePictureModel = productThumbPictureSize.HasValue;
            var products = _recentlyViewedProductsService.GetRecentlyViewedProducts(_catalogSettings.RecentlyViewedProductsNumber);

            //ACL and store mapping
            products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();
            //availability dates
            products = products.Where(p => _productService.ProductIsAvailable(p)).ToList();

            if (!products.Any())
                return Content("");

            //prepare model
            var model = new List<ProductOverviewModel>();
            model.AddRange(_productModelFactory.PrepareProductOverviewModels(products,
                preparePriceModel.GetValueOrDefault(),
                preparePictureModel,
                productThumbPictureSize));

            return View(model);
        }
    }
}