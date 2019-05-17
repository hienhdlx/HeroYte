using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Models.Common;

namespace NCSw.HERO.Web.Models.Customer
{
    public partial class CustomerAddressEditModel : BaseNopModel
    {
        public CustomerAddressEditModel()
        {
            this.Address = new AddressModel();
        }
        
        public AddressModel Address { get; set; }
    }
}