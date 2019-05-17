using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NCSw.HERO.Web.Areas.Admin.Models.Doctors;

namespace NCSw.HERO.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the doctor model factory implementation
    /// </summary>
    public partial class DoctorModelFactory : IDoctorModelFactory
    {
        #region Fields

        #endregion

        #region Ctor

        #endregion

        #region Methods

        public virtual DoctorSearchModel PrepareDoctorListModel(DoctorSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual DoctorSearchModel PrepareDoctorSearchModel(DoctorSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get doctor
            //var doctors = 
            return searchModel;
        }

        #endregion
    }
}
