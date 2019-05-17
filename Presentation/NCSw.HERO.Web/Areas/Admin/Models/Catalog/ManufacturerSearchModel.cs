using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a manufacturer search model
    /// </summary>
    public partial class ManufacturerSearchModel : BaseSearchModel
    {
        #region Ctor

        public ManufacturerSearchModel()
        {
            AvailableStores = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Catalog.Manufacturers.List.SearchManufacturerName")]
        public string SearchManufacturerName { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Manufacturers.List.SearchStore")]
        public int SearchStoreId { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        #endregion
    }
}