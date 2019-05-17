using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;

namespace NCSw.HERO.Web.Areas.Admin.Models
{
    public partial class DepartmentSearchModel : BaseSearchModel
    {
        [NopResourceDisplayName("Hero.Admin.Department.Fields.Name")]
        public string SearchName { get; set; }
    }
}
