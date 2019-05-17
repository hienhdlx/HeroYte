using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents a customer address search model
    /// </summary>
    public partial class CustomerAddressSearchModel : BaseSearchModel
    {
        #region Properties

        public int CustomerId { get; set; }

        #endregion
    }
}