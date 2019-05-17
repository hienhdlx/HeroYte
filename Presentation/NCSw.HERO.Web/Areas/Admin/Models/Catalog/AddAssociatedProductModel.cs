using System.Collections.Generic;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents an associated product model to add to the product
    /// </summary>
    public partial class AddAssociatedProductModel : BaseNopModel
    {
        #region Ctor

        public AddAssociatedProductModel()
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