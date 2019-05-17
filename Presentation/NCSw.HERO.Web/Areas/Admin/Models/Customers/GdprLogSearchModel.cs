﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents a GDPR log search model
    /// </summary>
    public partial class GdprLogSearchModel : BaseSearchModel
    {
        #region Ctor

        public GdprLogSearchModel()
        {
            AvailableRequestTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Customers.GdprLog.List.SearchEmail")]
        public string SearchEmail { get; set; }

        [NopResourceDisplayName("Admin.Customers.GdprLog.List.SearchRequestType")]
        public int SearchRequestTypeId { get; set; }

        public IList<SelectListItem> AvailableRequestTypes { get; set; }

        #endregion
    }
}