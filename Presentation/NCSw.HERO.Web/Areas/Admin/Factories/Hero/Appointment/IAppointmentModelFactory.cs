using NCSw.HERO.Core.Domain;
using NCSw.HERO.Web.Areas.Admin.Models;

namespace NCSw.HERO.Web.Areas.Admin.Factories
{
    public partial interface IAppointmentModelFactory
    {
        AppointmentModel PrepareModel(AppointmentModel model, Appointment department, bool excludeProperties = false);
        AppointmentModel PrepareAppointmentModel(AppointmentModel model, Appointment department);
        void UpdateLocales(Appointment e, AppointmentModel m);
    }
}
