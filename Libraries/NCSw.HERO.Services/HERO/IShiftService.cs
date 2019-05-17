using NCSw.HERO.Core;
using NCSw.HERO.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NCSw.HERO.Services
{
    public partial interface IShiftService
    {
        /// <summary>
        /// Gets all customer roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer roles</returns>
        IList<Shift> GetAll(int languageId = 0, bool showHidden = false);

        IPagedList<Shift> GetAll(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets a Shift
        /// </summary>
        /// <param name="shiftId">Shift Identifier</param>
        /// <returns></returns>
        Shift GetShiftById(int shiftId);

        IList<Shift> GetByIds(int[] ids);


        /// <summary>
        /// Gets a Shift
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns></returns>
        Shift GetShift(Expression<Func<Shift, bool>> expression);


        /// <summary>
        /// insert a Shift
        /// </summary>
        /// <param name="shift"></param>
        void InsertShift(Shift shift);

        /// <summary>
        /// update a shift
        /// </summary>
        /// <param name="shift">Update</param>
        void UpdateShift(Shift shift);


        /// <summary>
        /// delete a shift
        /// </summary>
        /// <param name="shift">Delete</param>
        void DeleteShift(Shift shift);
        void DeleteShift(IList<Shift> list);
    }
}
