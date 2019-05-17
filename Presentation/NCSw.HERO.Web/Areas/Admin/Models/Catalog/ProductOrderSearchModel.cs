﻿using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a product order search model
    /// </summary>
    public partial class ProductOrderSearchModel : BaseSearchModel
    {
        #region Properties

        public int ProductId { get; set; }
        
        #endregion
    }
}