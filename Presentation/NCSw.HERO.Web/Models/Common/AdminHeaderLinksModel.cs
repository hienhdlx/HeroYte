using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Common
{
    public partial class AdminHeaderLinksModel : BaseNopModel
    {
        public string ImpersonatedCustomerName { get; set; }
        public bool IsCustomerImpersonated { get; set; }
        public bool DisplayAdminLink { get; set; }
        public string EditPageUrl { get; set; }
    }
}