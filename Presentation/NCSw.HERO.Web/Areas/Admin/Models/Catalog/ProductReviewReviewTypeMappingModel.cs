﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;

namespace NCSw.HERO.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a product review and review type mapping model
    /// </summary>
    public class ProductReviewReviewTypeMappingModel : BaseNopEntityModel
    {
        #region Properties

        public int ProductReviewId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviewsExt.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviewsExt.Fields.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviewsExt.Fields.VisibleToAllCustomers")]
        public bool VisibleToAllCustomers { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviewsExt.Fields.Rating")]
        public int Rating { get; set; }

        #endregion
    }
}
