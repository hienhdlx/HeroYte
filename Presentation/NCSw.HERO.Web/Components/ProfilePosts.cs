using System;
using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Services.Customers;
using NCSw.HERO.Web.Factories;
using NCSw.HERO.Web.Framework.Components;

namespace NCSw.HERO.Web.Components
{
    public class ProfilePostsViewComponent : NopViewComponent
    {
        private readonly ICustomerService _customerService;
        private readonly IProfileModelFactory _profileModelFactory;

        public ProfilePostsViewComponent(ICustomerService customerService, IProfileModelFactory profileModelFactory)
        {
            this._customerService = customerService;
            this._profileModelFactory = profileModelFactory;
        }

        public IViewComponentResult Invoke(int customerProfileId, int pageNumber)
        {
            var customer = _customerService.GetCustomerById(customerProfileId);
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var model = _profileModelFactory.PrepareProfilePostsModel(customer, pageNumber);
            return View(model);
        }
    }
}
