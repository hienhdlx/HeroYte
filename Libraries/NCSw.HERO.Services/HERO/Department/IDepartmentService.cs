using NCSw.HERO.Core;
using NCSw.HERO.Core.Domain;
using System.Collections.Generic;

namespace NCSw.HERO.Services
{
    public partial interface IDepartmentService
    {
        void Insert(Department e);

        void Update(Department e);

        void Updates(IList<Department> lst);

        void Delete(Department e);

        void Deletes(IList<Department> lst);

        IList<Department> GetAll(int languageId = 0, bool showHidden = false);

        IPagedList<Department> GetAll(string name = "",
            int pageIndex = 0, int pageSize = int.MaxValue,
            bool showHidden = false);

        IList<Department> GetByIds(int[] ids);

        Department GetById(int id);
    }
}
