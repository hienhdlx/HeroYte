﻿using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;

namespace NCSw.HERO.Web.Areas.Admin.Models.ShoppingCart
{
    /// <summary>
    /// Represents a shopping cart model
    /// </summary>
    public partial class ShoppingCartModel : BaseNopModel
    {
        #region Properties

        [NopResourceDisplayName("Admin.CurrentCarts.Customer")]
        public int CustomerId { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.Customer")]
        public string CustomerEmail { get; set; }

        [NopResourceDisplayName("Admin.CurrentCarts.TotalItems")]
        public int TotalItems { get; set; }

        #endregion
    }
}