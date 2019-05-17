using Microsoft.AspNetCore.Mvc.Rendering;
using NCSw.HERO.Core;
using NCSw.HERO.Core.Domain;
using NCSw.HERO.Services;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using NCSw.HERO.Web.Areas.Admin.Models;
using NCSw.HERO.Web.Framework.Extensions;
using NCSw.HERO.Web.Framework.Factories;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NCSw.HERO.Web.Areas.Admin.Factories
{
    public partial class AppointmentModelFactory : IAppointmentModelFactory
    {
        #region Fields

        private readonly IAppointmentService _AppointmentService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IWorkContext _workContext;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;

        #endregion

        #region Ctor

        public AppointmentModelFactory(
            IAppointmentService AppointmentService,
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            ILocalizedEntityService localizedEntityService,
            IWorkContext workContext,
            IBaseAdminModelFactory baseAdminModelFactory)
        {
            _AppointmentService = AppointmentService;
            _localizationService = localizationService;
            _localizedModelFactory = localizedModelFactory;
            _localizedEntityService = localizedEntityService;
            _workContext = workContext;
            _baseAdminModelFactory = baseAdminModelFactory;
        }

        #endregion

        #region Methods

        public virtual AppointmentModel PrepareModel(AppointmentModel model, Appointment service, bool excludeProperties = false)
        {
            if (service != null)
            {
                //fill in model values from the entity
                model = model ?? service.ToModel<AppointmentModel>();
            }

            //set default values for the new model
            if (service == null)
            {
                model.CreatedBy = _workContext.CurrentCustomer.Id;
                model.CreatedOnUtc = DateTime.UtcNow;
            }

            //prepare available department templates
            _baseAdminModelFactory.PrepareDepartmentTemplates(model.DepartmentListTemplates, false, null);

            //prepare available service templates
            _baseAdminModelFactory.PrepareServiceTemplates(model.ServiceListTemplates, false, null);

            //prepare available doctor templates
            _baseAdminModelFactory.PrepareDoctorTemplates(model.DoctorListTemplates, false, null);

            return model;
        }

        public AppointmentModel PrepareAppointmentModel(AppointmentModel model, Appointment department)
        {
            if (department != null)
            {
                //fill in model values from the entity
                model = model ?? department.ToModel<AppointmentModel>();

            }

            return model;
        }

        public virtual void UpdateLocales(Appointment e, AppointmentModel m)
        {
            //foreach (var localized in m.Locales)
            //{
            //    _localizedEntityService.SaveLocalizedValue(e,
            //        x => x.Name,
            //        localized.Name,
            //        localized.LanguageId);
            //    _localizedEntityService.SaveLocalizedValue(e,
            //       x => x.Address,
            //       localized.Address,
            //       localized.LanguageId);
            //    _localizedEntityService.SaveLocalizedValue(e,
            //       x => x.Description,
            //       localized.Description,
            //       localized.LanguageId);
            //}
        }

        #endregion

        #region Utilities



        #endregion
    }
}
