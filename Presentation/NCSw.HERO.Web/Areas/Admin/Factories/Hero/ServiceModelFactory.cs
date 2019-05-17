using NCSw.HERO.Core;
using NCSw.HERO.Core.Domain;
using NCSw.HERO.Services;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using NCSw.HERO.Web.Areas.Admin.Models;
using NCSw.HERO.Web.Framework.Factories;
using System;
using System.Linq;

namespace NCSw.HERO.Web.Areas.Admin.Factories
{
    public partial class ServiceModelFactory : IServiceModelFactory
    {
        #region Fields

        private readonly IServiceService _serviceService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public ServiceModelFactory(
            IServiceService ServiceService,
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            IWorkContext workContext)
        {
            _serviceService = ServiceService;
            _localizationService = localizationService;
            _localizedModelFactory = localizedModelFactory;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        public virtual ServiceSearchModel PrepareSearchModel(ServiceSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual ServiceListModel PrepareListModel(ServiceSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var entities = _serviceService.GetAll(name: searchModel.SearchName,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize,
                showHidden: true);

            //prepare list model
            var model = new ServiceListModel
            {
                //fill in model values from the entity
                Data = entities.Select(x => x.ToModel<ServiceModel>()),
                Total = entities.TotalCount
            };

            return model;
        }

        public virtual ServiceModel PrepareModel(ServiceModel m, Service e, bool excludeProperties = false)
        {
            Action<ServiceLocalizedModel, int> localizedModelConfiguration = null;

            if (e != null)
            {
                //fill in model values from the entity
                m = m ?? e.ToModel<ServiceModel>();

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(e, entity => entity.Name, languageId, false, false);
                    locale.Description = _localizationService.GetLocalized(e, entity => entity.Description, languageId, false, false);
                };
            }

            //set default values for the new model
            if (e == null)
            {
                m.Active = true;
                m.Deleted = false;
                m.DisplayOrder = 0;
                m.CreatedBy = _workContext.CurrentCustomer.Id;
                m.CreatedOnUtc = DateTime.UtcNow;
            }

            //prepare localized models
            if (!excludeProperties)
                m.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            return m;
        }

        #endregion

        #region Utilities

        

        #endregion
    }
}
