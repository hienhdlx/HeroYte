using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a product model to associate to the product attribute value
    /// </summary>
    public partial class AssociateProductToAttributeValueModel : BaseNopModel
    {
        #region Properties
        
        public int AssociatedToProductId { get; set; }

        #endregion
    }
}