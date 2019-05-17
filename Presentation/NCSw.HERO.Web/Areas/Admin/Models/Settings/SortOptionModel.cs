﻿using NCSw.HERO.Web.Framework.Mvc.ModelBinding;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a sort option model
    /// </summary>
    public partial class SortOptionModel : BaseNopEntityModel
    {
        #region Properties

        [NopResourceDisplayName("Admin.Configuration.Settings.Catalog.SortOptions.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.Catalog.SortOptions.IsActive")]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Settings.Catalog.SortOptions.DisplayOrder")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}