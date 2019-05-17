using NCSw.HERO.Core.Domain;
using NCSw.HERO.Web.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCSw.HERO.Web.Areas.Admin.Factories
{
    public partial interface IShiftModelFactory
    {
        /// <summary>
        /// Prepare shift search model
        /// </summary>
        /// <param name="searchModel">Shift search model</param>
        /// <returns>Shift search model</returns>
        ShiftSearchModel PrepareShiftSearchModel(ShiftSearchModel searchModel);

        /// <summary>
        /// Prepare paged Shift list model
        /// </summary>
        /// <param name="searchModel">Shift search model</param>
        /// <returns>Shift list model</returns>
        ShiftListModel PrepareShiftListModel(ShiftSearchModel searchModel);

        /// <summary>
        /// Prepare Shift model
        /// </summary>
        /// <param name="model">shift model</param>
        /// <param name="customer">Shift</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Shift model</returns>
        ShiftModel PrepareShiftModel(ShiftModel model, Shift shift, bool excludeProperties = false);
    }
}
