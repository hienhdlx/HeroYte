﻿using System.Collections.Generic;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Models.Common;

namespace NCSw.HERO.Web.Models.Checkout
{
    public partial class CheckoutShippingAddressModel : BaseNopModel
    {
        public CheckoutShippingAddressModel()
        {
            Warnings = new List<string>();
            ExistingAddresses = new List<AddressModel>();
            ShippingNewAddress = new AddressModel();
            PickupPoints = new List<CheckoutPickupPointModel>();
        }
        
        public IList<string> Warnings { get; set; }

        public IList<AddressModel> ExistingAddresses { get; set; }
        public AddressModel ShippingNewAddress { get; set; }
        public bool NewAddressPreselected { get; set; }

        public IList<CheckoutPickupPointModel> PickupPoints { get; set; }
        public bool AllowPickUpInStore { get; set; }
        public bool PickUpInStore { get; set; }
        public bool PickUpInStoreOnly { get; set; }
        public bool DisplayPickupPointsOnMap { get; set; }
        public string GoogleMapsApiKey { get; set; }
    }
}