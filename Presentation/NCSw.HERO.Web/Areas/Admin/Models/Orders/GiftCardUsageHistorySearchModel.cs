using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a gift card usage history search model
    /// </summary>
    public partial class GiftCardUsageHistorySearchModel : BaseSearchModel
    {
        #region Properties

        public int GiftCardId { get; set; }

        #endregion
    }
}