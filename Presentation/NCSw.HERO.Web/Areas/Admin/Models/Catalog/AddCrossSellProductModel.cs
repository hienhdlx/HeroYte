﻿using System.Collections.Generic;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a cross-sell product model to add to the product
    /// </summary>
    public partial class AddCrossSellProductModel : BaseNopModel
    {
        #region Ctor

        public AddCrossSellProductModel()
        {
            SelectedProductIds = new List<int>();
        }
        #endregion

        #region Properties

        public int ProductId { get; set; }

        public IList<int> SelectedProductIds { get; set; }

        #endregion
    }
}