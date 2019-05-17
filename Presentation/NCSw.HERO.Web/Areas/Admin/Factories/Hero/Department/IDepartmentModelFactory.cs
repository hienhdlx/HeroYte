using NCSw.HERO.Core.Domain;
using NCSw.HERO.Web.Areas.Admin.Models;

namespace NCSw.HERO.Web.Areas.Admin.Factories
{
    public partial interface IDepartmentModelFactory
    {
        DepartmentSearchModel PrepareSearchModel(DepartmentSearchModel searchModel);

        DepartmentListModel PrepareListModel(DepartmentSearchModel searchModel);

        DepartmentModel PrepareModel(DepartmentModel model, Department department, bool excludeProperties = false);
        DepartmentModel PrepareDepartmentModel(DepartmentModel model, Department department);
        void UpdateLocales(Department e, DepartmentModel m);
    }
}
