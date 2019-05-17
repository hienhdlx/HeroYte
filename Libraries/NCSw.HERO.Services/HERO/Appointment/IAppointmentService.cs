using NCSw.HERO.Core;
using NCSw.HERO.Core.Domain;
using System.Collections.Generic;

namespace NCSw.HERO.Services
{
    public partial interface IAppointmentService
    {
        void Insert(Appointment e);

        void Update(Appointment e);

        void Updates(IList<Appointment> lst);

        void Delete(Appointment e);

        IList<Appointment> GetAll(int languageId = 0, bool showHidden = false);

        IPagedList<Appointment> GetAll(string name = "",
            int pageIndex = 0, int pageSize = int.MaxValue,
            bool showHidden = false);

        IList<Appointment> GetByIds(int[] ids);

        Appointment GetById(int id);
    }
}
