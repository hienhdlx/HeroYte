using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Core.Domain.Customers;
using NCSw.HERO.Services.Customers;
using NCSw.HERO.Services.Security;
using NCSw.HERO.Web.Factories;
using NCSw.HERO.Web.Framework;
using NCSw.HERO.Web.Framework.Mvc.Filters;
using NCSw.HERO.Web.Framework.Security;

namespace NCSw.HERO.Web.Controllers
{
    [HttpsRequirement(SslRequirement.No)]
    public partial class ProfileController : BasePublicController
    {
        private readonly CustomerSettings _customerSettings;
        private readonly ICustomerService _customerService;
        private readonly IPermissionService _permissionService;
        private readonly IProfileModelFactory _profileModelFactory;

        public ProfileController(CustomerSettings customerSettings,
            ICustomerService customerService,
            IPermissionService permissionService,
            IProfileModelFactory profileModelFactory)
        {
            this._customerSettings = customerSettings;
            this._customerService = customerService;
            this._permissionService = permissionService;
            this._profileModelFactory = profileModelFactory;
        }

        public virtual IActionResult Index(int? id, int? pageNumber)
        {
            if (!_customerSettings.AllowViewingProfiles)
            {
                return RedirectToRoute("HomePage");
            }

            var customerId = 0;
            if (id.HasValue)
            {
                customerId = id.Value;
            }

            var customer = _customerService.GetCustomerById(customerId);
            if (customer == null || customer.IsGuest())
            {
                return RedirectToRoute("HomePage");
            }

            //display "edit" (manage) link
            if (_permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel) && _permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                DisplayEditLink(Url.Action("Edit", "Customer", new { id = customer.Id, area = AreaNames.Admin }));

            var model = _profileModelFactory.PrepareProfileIndexModel(customer, pageNumber);
            return View(model);
        }
    }
}