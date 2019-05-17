using NCSw.HERO.Core;
using NCSw.HERO.Core.Domain;
using System.Collections.Generic;

namespace NCSw.HERO.Services
{
    public partial interface IServiceService
    {
        void Insert(Service e);

        void Update(Service e);

        void Updates(IList<Service> lst);

        void Delete(Service e);

        void Deletes(IList<Service> lst);
        IList<Service> GetAllItem(int languageId = 0, bool showHidden = false);
        IList<Service> GetAll(int languageId = 0, bool showHidden = false);

        IPagedList<Service> GetAll(string name = "",
            int pageIndex = 0, int pageSize = int.MaxValue,
            bool showHidden = false);

        IList<Service> GetByIds(int[] ids);

        Service GetById(int id);
    }
}
