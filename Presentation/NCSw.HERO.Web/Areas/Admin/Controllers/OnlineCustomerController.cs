using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Services.Security;
using NCSw.HERO.Web.Areas.Admin.Factories;
using NCSw.HERO.Web.Areas.Admin.Models.Customers;

namespace NCSw.HERO.Web.Areas.Admin.Controllers
{
    public partial class OnlineCustomerController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerModelFactory _customerModelFactory;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public OnlineCustomerController(ICustomerModelFactory customerModelFactory,
            IPermissionService permissionService)
        {
            this._customerModelFactory = customerModelFactory;
            this._permissionService = permissionService;
        }

        #endregion
        
        #region Methods

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            //prepare model
            var model = _customerModelFactory.PrepareOnlineCustomerSearchModel(new OnlineCustomerSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(OnlineCustomerSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _customerModelFactory.PrepareOnlineCustomerListModel(searchModel);

            return Json(model);
        }

        #endregion
    }
}