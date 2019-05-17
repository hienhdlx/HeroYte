using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Core;
using NCSw.HERO.Core.Domain;
using NCSw.HERO.Services;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Services.Security;
using NCSw.HERO.Web.Areas.Admin.Factories;
using NCSw.HERO.Web.Areas.Admin.Models;

namespace NCSw.HERO.Web.Areas.Admin.Controllers
{
    public partial class SwitchboardController : BaseAdminController
    {
        #region Fields
        private readonly IAppointmentModelFactory _AppointmentModelFactory;
        private readonly IAppointmentService _AppointmentService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor

        public SwitchboardController(
            IAppointmentModelFactory AppointmentModelFactory,
            IAppointmentService AppointmentService,
            ILocalizedEntityService localizedEntityService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext)
        {
            _AppointmentModelFactory = AppointmentModelFactory;
            _AppointmentService = AppointmentService;
            _localizedEntityService = localizedEntityService;
            _localizationService = localizationService;
            _permissionService = permissionService;
            _workContext = workContext;
        }

        #endregion

        #region Utilities

        #endregion

        #region Actions

        public virtual IActionResult Index()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageServices))
                return AccessDeniedView();

            var model = _AppointmentModelFactory.PrepareModel(new AppointmentModel(), null);
            return View(model);
        }

        #endregion
    }
}