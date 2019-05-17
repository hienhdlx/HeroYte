using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using NCSw.HERO.Core.Domain;
using NCSw.HERO.Core.Domain.Common;
using NCSw.HERO.Services;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using NCSw.HERO.Web.Areas.Admin.Models;
using NCSw.HERO.Web.Framework.Extensions;
using NCSw.HERO.Web.Framework.Factories;

namespace NCSw.HERO.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the doctor model factory implementation
    /// </summary>
    public partial class DoctorModelFactory : IDoctorModelFactory
    {
        #region Fields

        private readonly IDoctorService _doctorService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;

        #endregion

        #region Ctor

        public DoctorModelFactory(IDoctorService doctorService,
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory)
        {
            _doctorService = doctorService;
            _localizationService = localizationService;
            _localizedModelFactory = localizedModelFactory;
            _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare doctor search model
        /// </summary>
        /// <param name="searchModel">Doctor search model</param>
        /// <returns>Doctor search model</returns>
        public virtual DoctorSearchModel PrepareDoctorSearchModel(DoctorSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged doctor list model
        /// </summary>
        /// <param name="searchModel">Doctor search model</param>
        /// <returns>Doctor list model</returns>
        public virtual DoctorListModel PrepareDoctorListModel(DoctorSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var entities = _doctorService.GetAll(keyWord: searchModel.SearchName,
                                                 pageIndex: searchModel.Page - 1, 
                                                 pageSize: searchModel.PageSize,
                                                 showHidden: true);

            //var model = new DoctorListModel
            //{
            //    Data = entities.PaginationByRequestModel(searchModel).Select(s =>
            //    {
            //        //fill in model values from the entity
            //        var customerRoleModel = s.ToModel<DoctorModel>();
            //        return customerRoleModel;
            //    }),
            //    Total = entities.TotalCount
            //};

            //prepare list model
            var model = new DoctorListModel
            {
                //fill in model values from the entity
                Data = entities.Select(x => x.ToModel<DoctorModel>()),
                Total = entities.TotalCount
            };

            return model;
        }

        public virtual DoctorModel PrepareDoctorModel(DoctorModel model, Doctor doctor, bool excludeProperties = false)
        {
            //Action<DoctorLocalizedModel, int> localizedModelConfiguration = null;


            if (doctor != null)
            {
                //fill in model values from the entity
                model = model ?? doctor.ToModel<DoctorModel>();
                //localizedModelConfiguration = (locale, languageId) =>
                //{
                //    locale.FullName = _localizationService.GetLocalized(doctor, entity => entity.FullName, languageId, false, false);
                //};
            }

            //set default values for the new model
            if (doctor == null)
                model = new DoctorModel();

            //prepare localized models
            //if (!excludeProperties)
            //    model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            model.AvailableGenders = PrepareDoctorGender(model.AvailableGenders);

            return model;
        }

        /// <summary>
        /// Prepare available doctor gender
        /// </summary>
        /// <param name="items">Doctors gender items</param>
        public virtual IList<SelectListItem> PrepareDoctorGender(IList<SelectListItem> items)
        {
            //prepare available gender
            if(items == null)
                items = new List<SelectListItem>();

            items.Add(new SelectListItem { Value = Convert.ToInt32(Gender.Female).ToString(), Text = "Nữ" });
            items.Add(new SelectListItem { Value = Convert.ToInt32(Gender.Male).ToString(), Text = "Nam" });
            items.Add(new SelectListItem { Value = Convert.ToInt32(Gender.Other).ToString(), Text = "Khác" });

            return items;
        }
        #endregion
    }
}
