using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Core;
using NCSw.HERO.Core.Domain.Orders;
using NCSw.HERO.Services.Catalog;
using NCSw.HERO.Services.Orders;
using NCSw.HERO.Services.Security;
using NCSw.HERO.Services.Stores;
using NCSw.HERO.Web.Factories;
using NCSw.HERO.Web.Framework.Components;

namespace NCSw.HERO.Web.Components
{
    public class CrossSellProductsViewComponent : NopViewComponent
    {
        private readonly IAclService _aclService;
        private readonly IProductModelFactory _productModelFactory;
        private readonly IProductService _productService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IWorkContext _workContext;
        private readonly ShoppingCartSettings _shoppingCartSettings;

        public CrossSellProductsViewComponent(IAclService aclService,
            IProductModelFactory productModelFactory,
            IProductService productService,
            IStoreContext storeContext,
            IStoreMappingService storeMappingService,
            IWorkContext workContext,
            ShoppingCartSettings shoppingCartSettings)
        {
            this._aclService = aclService;
            this._productModelFactory = productModelFactory;
            this._productService = productService;
            this._storeContext = storeContext;
            this._storeMappingService = storeMappingService;
            this._workContext = workContext;
            this._shoppingCartSettings = shoppingCartSettings;
        }

        public IViewComponentResult Invoke(int? productThumbPictureSize)
        {
            var cart = _workContext.CurrentCustomer.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                .LimitPerStore(_storeContext.CurrentStore.Id)
                .ToList();

            var products = _productService.GetCrosssellProductsByShoppingCart(cart, _shoppingCartSettings.CrossSellsNumber);
            //ACL and store mapping
            products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();
            //availability dates
            products = products.Where(p => _productService.ProductIsAvailable(p)).ToList();
            //visible individually
            products = products.Where(p => p.VisibleIndividually).ToList();

            if (!products.Any())
                return Content("");

            //Cross-sell products are displayed on the shopping cart page.
            //We know that the entire shopping cart page is not refresh
            //even if "ShoppingCartSettings.DisplayCartAfterAddingProduct" setting  is enabled.
            //That's why we force page refresh (redirect) in this case
            var model = _productModelFactory.PrepareProductOverviewModels(products,
                    productThumbPictureSize: productThumbPictureSize, forceRedirectionAfterAddingToCart: true)
                .ToList();

            return View(model);
        }
    }
}