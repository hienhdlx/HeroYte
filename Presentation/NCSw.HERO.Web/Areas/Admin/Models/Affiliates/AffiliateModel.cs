using NCSw.HERO.Web.Areas.Admin.Models.Common;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;

namespace NCSw.HERO.Web.Areas.Admin.Models.Affiliates
{
    /// <summary>
    /// Represents an affiliate model
    /// </summary>
    public partial class AffiliateModel : BaseNopEntityModel
    {
        #region Ctor

        public AffiliateModel()
        {
            Address = new AddressModel();
            AffiliatedOrderSearchModel= new AffiliatedOrderSearchModel();
            AffiliatedCustomerSearchModel = new AffiliatedCustomerSearchModel();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Affiliates.Fields.URL")]
        public string Url { get; set; }
        
        [NopResourceDisplayName("Admin.Affiliates.Fields.AdminComment")]
        public string AdminComment { get; set; }

        [NopResourceDisplayName("Admin.Affiliates.Fields.FriendlyUrlName")]
        public string FriendlyUrlName { get; set; }
        
        [NopResourceDisplayName("Admin.Affiliates.Fields.Active")]
        public bool Active { get; set; }

        public AddressModel Address { get; set; }

        public AffiliatedOrderSearchModel AffiliatedOrderSearchModel { get; set; }

        public AffiliatedCustomerSearchModel AffiliatedCustomerSearchModel { get; set; }

        #endregion
    }
}