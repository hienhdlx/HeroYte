﻿using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a cross-sell product search model
    /// </summary>
    public partial class CrossSellProductSearchModel : BaseSearchModel
    {
        #region Properties

        public int ProductId { get; set; }

        #endregion
    }
}