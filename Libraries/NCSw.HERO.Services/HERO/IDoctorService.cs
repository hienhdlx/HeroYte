using NCSw.HERO.Core;
using NCSw.HERO.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NCSw.HERO.Services
{
    public partial interface IDoctorService
    {
        /// <summary>
        /// Get a doctor 
        /// </summary>
        /// <param name="doctorId">Doctor identifier</param>
        /// <returns>Doctor</returns>
        Doctor GetDoctorById(int doctorId);

        /// <summary>
        /// Get many doctors
        /// </summary>
        /// <param name="ids">arr id</param>
        /// <returns>Doctor</returns>
        IList<Doctor> GetDoctorByIds(int[] ids);

        /// <summary>
        /// Gets a doctor 
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns>Doctor</returns>
        Doctor GetDoctor(Expression<Func<Doctor, bool>> expression);

        /// <summary>
        /// Gets all doctor
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Doctor</returns>
        IList<Doctor> GetAll(int languageId = 0, bool showHidden = false);

        IPagedList<Doctor> GetAll(string keyWord = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Inserts a doctor
        /// </summary>
        /// <param name="doctor">Doctor</param>
        void InsertDoctor(Doctor doctor);

        /// <summary>
        /// Updates the doctor
        /// </summary>
        /// <param name="doctor">Doctor</param>
        void UpdateDoctor(Doctor doctor);

        /// <summary>
        /// Delete a doctor
        /// </summary>
        /// <param name="doctor">Doctor</param>
        void DeleteDoctor(Doctor doctor);

        /// <summary>
        /// Deletes many doctors
        /// </summary>
        /// <param name="doctor">Doctor</param>
        void DeleteDoctors(IList<Doctor> lst);
    }
}
