using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NCSw.HERO.Core;
using NCSw.HERO.Core.Domain;
using NCSw.HERO.Services;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Services.Security;
using NCSw.HERO.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using NCSw.HERO.Web.Areas.Admin.Models;
using NCSw.HERO.Web.Framework.Factories;
using NCSw.HERO.Web.Framework.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NCSw.HERO.Web.Areas.Admin.Controllers
{
    public partial class ServiceController : BaseAdminController
    {
        #region Fields

        private readonly IServiceService _serviceService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public ServiceController(
            IServiceService serviceService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            ILocalizedModelFactory localizedModelFactory,
            IWorkContext workContext)
        {
            _serviceService = serviceService;
            _localizationService = localizationService;
            _localizedEntityService = localizedEntityService;
            _permissionService = permissionService;
            _localizedModelFactory = localizedModelFactory;
            _workContext = workContext;
        }

        #endregion

        #region Actions

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageServices))
                return AccessDeniedView();

            //prepare model
            var model = PrepareSearchModel(new ServiceSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ServiceSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageServices))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = PrepareListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageServices))
                return AccessDeniedView();

            //prepare model
            var model = PrepareModel(new ServiceModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ServiceModel m, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageServices))
                return AccessDeniedView();

            m.HospitalId = _workContext.HospitalId;
            if (ModelState.IsValid)
            {
                var e = m.ToEntity<Service>();
                e.CreatedOnUtc = DateTime.UtcNow;
                e.CreatedBy = _workContext.CurrentCustomer.Id;
                _serviceService.Insert(e);

                //locales
                UpdateLocales(e, m);

                ////activity log
                //_customerActivityService.InsertActivity("AddNewManufacturer",
                //    string.Format(_localizationService.GetResource("ActivityLog.AddNewManufacturer"), e.Name), e);

                SuccessNotification(_localizationService.GetResource("Hero.Common.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = e.Id });
            }

            //prepare model
            m = PrepareModel(m, null, true);

            //if we got this far, something failed, redisplay form
            return View(m);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageServices))
                return AccessDeniedView();

            var e = _serviceService.GetById(id);
            if (e == null || e.Deleted)
                return RedirectToAction("List");

            //prepare model
            var model = PrepareModel(null, e);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(ServiceModel m, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageServices))
                return AccessDeniedView();

            var e = _serviceService.GetById(m.Id);
            if (e == null || e.Deleted)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                e = m.ToEntity(e);
                e.UpdatedOnUtc = DateTime.UtcNow;
                e.UpdatedBy = _workContext.CurrentCustomer.Id;
                _serviceService.Update(e);

                //locales
                UpdateLocales(e, m);

                ////activity log
                //_customerActivityService.InsertActivity("EditManufacturer",
                //    string.Format(_localizationService.GetResource("ActivityLog.EditManufacturer"), e.Name), e);

                SuccessNotification(_localizationService.GetResource("Hero.Common.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = e.Id });
            }

            //prepare model
            m = PrepareModel(m, e, true);

            //if we got this far, something failed, redisplay form
            return View(m);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageServices))
                return AccessDeniedView();

            var e = _serviceService.GetById(id);
            if (e == null)
                return RedirectToAction("List");

            _serviceService.Delete(e);

            ////activity log
            //_customerActivityService.InsertActivity("DeleteManufacturer",
            //    string.Format(_localizationService.GetResource("ActivityLog.DeleteManufacturer"), e.Name), e);

            SuccessNotification(_localizationService.GetResource("Hero.Common.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageServices))
                return AccessDeniedView();

            if (selectedIds != null)
            {
                _serviceService.Deletes(_serviceService.GetByIds(selectedIds.ToArray()));
            }

            return Json(new { Result = true });
        }

        #endregion

        #region Utilities

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

        protected virtual void UpdateLocales(Service e, ServiceModel m)
        {
            foreach (var localized in m.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(e,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
            }
        }

        #endregion
    }
}