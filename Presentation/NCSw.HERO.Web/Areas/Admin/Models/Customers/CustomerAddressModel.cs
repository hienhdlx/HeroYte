using NCSw.HERO.Web.Areas.Admin.Models.Common;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Customers
{
    /// <summary>
    /// Represents a customer address model
    /// </summary>
    public partial class CustomerAddressModel : BaseNopModel
    {
        #region Ctor

        public CustomerAddressModel()
        {
            this.Address = new AddressModel();
        }

        #endregion

        #region Properties

        public int CustomerId { get; set; }

        public AddressModel Address { get; set; }

        #endregion
    }
}