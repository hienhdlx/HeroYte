using Microsoft.AspNetCore.Mvc.Rendering;
using NCSw.HERO.Core.Domain;
using NCSw.HERO.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCSw.HERO.Web.Areas.Admin.Factories
{
    public partial interface IDoctorModelFactory
    {
        /// <summary>
        /// Prepare doctor search model
        /// </summary>
        /// <param name="searchModel">Doctor search model</param>
        /// <returns>Doctor search model</returns>
        DoctorSearchModel PrepareDoctorSearchModel(DoctorSearchModel searchModel);

        /// <summary>
        /// Prepare paged doctor list model
        /// </summary>
        /// <param name="searchModel">Doctor search model</param>
        /// <returns>Doctor list model</returns>
        DoctorListModel PrepareDoctorListModel(DoctorSearchModel searchModel);

        /// <summary>
        /// Prepare doctor model
        /// </summary>
        /// <param name="model">Doctor model</param>
        /// <param name="doctor">Doctor</param>
        /// <returns></returns>
        DoctorModel PrepareDoctorModel(DoctorModel model, Doctor doctor, bool excludeProperties = false);

        /// <summary>
        /// Prepare doctor gender
        /// </summary>
        /// <param name="model">Doctor model</param>
        /// <param name="doctor">Doctor</param>
        /// <returns></returns>
        IList<SelectListItem> PrepareDoctorGender(IList<SelectListItem> items);
    }
}
