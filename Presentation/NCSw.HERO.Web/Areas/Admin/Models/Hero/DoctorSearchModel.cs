using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;

namespace NCSw.HERO.Web.Areas.Admin.Models
{
    public partial class DoctorSearchModel : BaseSearchModel
    {
        [NopResourceDisplayName("Hero.Admin.Doctors.Fields.SearchName")]
        public string SearchName { get; set; }
    }
}
