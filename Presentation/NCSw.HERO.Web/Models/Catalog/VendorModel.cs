﻿using System.Collections.Generic;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Models.Media;

namespace NCSw.HERO.Web.Models.Catalog
{
    public partial class VendorModel : BaseNopEntityModel
    {
        public VendorModel()
        {
            PictureModel = new PictureModel();
            Products = new List<ProductOverviewModel>();
            PagingFilteringContext = new CatalogPagingFilteringModel();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        public bool AllowCustomersToContactVendors { get; set; }

        public PictureModel PictureModel { get; set; }

        public CatalogPagingFilteringModel PagingFilteringContext { get; set; }

        public IList<ProductOverviewModel> Products { get; set; }
    }
}