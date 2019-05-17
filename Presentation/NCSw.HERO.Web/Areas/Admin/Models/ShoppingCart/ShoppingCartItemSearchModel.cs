using NCSw.HERO.Core.Domain.Orders;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.ShoppingCart
{
    /// <summary>
    /// Represents a shopping cart item search model
    /// </summary>
    public partial class ShoppingCartItemSearchModel : BaseSearchModel
    {
        #region Properties

        public int CustomerId { get; set; }

        public ShoppingCartType ShoppingCartType { get; set; }

        #endregion
    }
}