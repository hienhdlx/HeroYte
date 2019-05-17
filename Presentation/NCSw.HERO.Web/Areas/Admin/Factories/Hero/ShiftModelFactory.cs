using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NCSw.HERO.Core;
using NCSw.HERO.Core.Domain;
using NCSw.HERO.Services;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using NCSw.HERO.Web.Areas.Admin.Models;
using NCSw.HERO.Web.Framework.Extensions;
using NCSw.HERO.Web.Framework.Factories;

namespace NCSw.HERO.Web.Areas.Admin.Factories
{
    public partial class ShiftModelFactory : IShiftModelFactory
    {
        #region Fields

        private readonly IShiftService _shiftService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public ShiftModelFactory(IShiftService shiftService,
                                  ILocalizationService localizationService,
                                  ILocalizedModelFactory localizedModelFactory,
                                  IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory,
                                  IWorkContext workContext)
        {
            _shiftService = shiftService;
            _localizationService = localizationService;
            _localizedModelFactory = localizedModelFactory;
            _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare shift search model
        /// </summary>
        /// <param name="searchModel">Shift search model</param>
        /// <returns>Shift search model</returns>
        public virtual ShiftSearchModel PrepareShiftSearchModel(ShiftSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged shift list model
        /// </summary>
        /// <param name="searchModel">Shift search model</param>
        /// <returns>Shift list model</returns>
        public virtual ShiftListModel PrepareShiftListModel(ShiftSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var entities = _shiftService.GetAll(name: searchModel.SearchName, pageIndex:searchModel.Page-1, pageSize: searchModel.PageSize,showHidden: true);

            var model = new ShiftListModel
            {
                Data = entities.PaginationByRequestModel(searchModel).Select(s => s.ToModel<ShiftModel>()),
                Total = entities.TotalCount
            };

            return model;
        }

        public virtual ShiftModel PrepareShiftModel(ShiftModel model, Shift shift, bool excludeProperties = false)
        {
            //Action<DoctorLocalizedModel, int> localizedModelConfiguration = null;


            if (shift != null)
            {
                //fill in model values from the entity
                model = model ?? shift.ToModel<ShiftModel>();
            }

            //set default values for the new model
            if (shift == null)
            {
                model.Active = true;
                model.Deleted = false;
                model.CreatedBy = _workContext.CurrentCustomer.Id;
                model.CreatedOnUtc = DateTime.UtcNow;
            }
                
            return model;
        }

        #endregion
    }
}
