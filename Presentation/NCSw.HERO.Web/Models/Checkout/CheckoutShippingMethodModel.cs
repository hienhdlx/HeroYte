﻿using System.Collections.Generic;
using NCSw.HERO.Core.Domain.Shipping;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Checkout
{
    public partial class CheckoutShippingMethodModel : BaseNopModel
    {
        public CheckoutShippingMethodModel()
        {
            ShippingMethods = new List<ShippingMethodModel>();
            Warnings = new List<string>();
        }

        public IList<ShippingMethodModel> ShippingMethods { get; set; }

        public bool NotifyCustomerAboutShippingFromMultipleLocations { get; set; }

        public IList<string> Warnings { get; set; }

        #region Nested classes

        public partial class ShippingMethodModel : BaseNopModel
        {
            public string ShippingRateComputationMethodSystemName { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Fee { get; set; }
            public bool Selected { get; set; }

            public ShippingOption ShippingOption { get; set; } 
        }

        #endregion
    }
}