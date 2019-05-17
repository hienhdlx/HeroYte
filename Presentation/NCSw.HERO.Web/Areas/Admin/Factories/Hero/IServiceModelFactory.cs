using NCSw.HERO.Core.Domain;
using NCSw.HERO.Web.Areas.Admin.Models;

namespace NCSw.HERO.Web.Areas.Admin.Factories
{
    public partial interface IServiceModelFactory
    {
        ServiceSearchModel PrepareSearchModel(ServiceSearchModel searchModel);

        ServiceListModel PrepareListModel(ServiceSearchModel searchModel);

        ServiceModel PrepareModel(ServiceModel m, Service e, bool excludeProperties = false);
    }
}
