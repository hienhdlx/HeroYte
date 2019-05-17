using System;
using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Services.Customers;
using NCSw.HERO.Services.Orders;
using NCSw.HERO.Services.Security;
using NCSw.HERO.Web.Areas.Admin.Factories;
using NCSw.HERO.Web.Areas.Admin.Models.ShoppingCart;
using NCSw.HERO.Web.Framework.Mvc;

namespace NCSw.HERO.Web.Areas.Admin.Controllers
{
    public partial class ShoppingCartController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IPermissionService _permissionService;
        private readonly IShoppingCartModelFactory _shoppingCartModelFactory;
        private readonly IShoppingCartService _shoppingCartService;

        #endregion

        #region Ctor

        public ShoppingCartController(ICustomerService customerService,
            IPermissionService permissionService, 
            IShoppingCartService shoppingCartService,
            IShoppingCartModelFactory shoppingCartModelFactory)
        {
            this._customerService = customerService;
            this._permissionService = permissionService;
            this._shoppingCartModelFactory = shoppingCartModelFactory;
            this._shoppingCartService = shoppingCartService;
        }

        #endregion
        
        #region Methods
        
        public virtual IActionResult CurrentCarts()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCurrentCarts))
                return AccessDeniedView();

            //prepare model
            var model = _shoppingCartModelFactory.PrepareShoppingCartSearchModel(new ShoppingCartSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult CurrentCarts(ShoppingCartSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCurrentCarts))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _shoppingCartModelFactory.PrepareShoppingCartListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult GetCartDetails(ShoppingCartItemSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCurrentCarts))
                return AccessDeniedKendoGridJson();

            //try to get a customer with the specified id
            var customer = _customerService.GetCustomerById(searchModel.CustomerId)
                ?? throw new ArgumentException("No customer found with the specified id");

            //prepare model
            var model = _shoppingCartModelFactory.PrepareShoppingCartItemListModel(searchModel, customer);

            return Json(model);
        }
        
        [HttpPost]
        public virtual IActionResult DeleteItem(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCurrentCarts))
                return AccessDeniedKendoGridJson();
            
            _shoppingCartService.DeleteShoppingCartItem(id);

            return new NullJsonResult();
        }

        #endregion
    }
}