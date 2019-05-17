﻿using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;

namespace NCSw.HERO.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents a customer associated external authentication model
    /// </summary>
    public partial class CustomerAssociatedExternalAuthModel : BaseNopEntityModel
    {
        #region Properties

        [NopResourceDisplayName("Admin.Customers.Customers.AssociatedExternalAuth.Fields.Email")]
        public string Email { get; set; }

        [NopResourceDisplayName("Admin.Customers.Customers.AssociatedExternalAuth.Fields.ExternalIdentifier")]
        public string ExternalIdentifier { get; set; }
        
        [NopResourceDisplayName("Admin.Customers.Customers.AssociatedExternalAuth.Fields.AuthMethodName")]
        public string AuthMethodName { get; set; }

        #endregion
    }
}