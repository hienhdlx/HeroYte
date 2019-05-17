﻿using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Core.Domain.Cms;
using NCSw.HERO.Core.Plugins;
using NCSw.HERO.Services.Cms;
using NCSw.HERO.Services.Configuration;
using NCSw.HERO.Services.Plugins;
using NCSw.HERO.Services.Security;
using NCSw.HERO.Web.Areas.Admin.Factories;
using NCSw.HERO.Web.Areas.Admin.Models.Cms;
using NCSw.HERO.Web.Framework.Mvc;

namespace NCSw.HERO.Web.Areas.Admin.Controllers
{
    public partial class WidgetController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly IPluginFinder _pluginFinder;
        private readonly ISettingService _settingService;
        private readonly IWidgetModelFactory _widgetModelFactory;
        private readonly IWidgetService _widgetService;
        private readonly WidgetSettings _widgetSettings;

        #endregion

        #region Ctor

        public WidgetController(IPermissionService permissionService,
            IPluginFinder pluginFinder,
            ISettingService settingService,
            IWidgetModelFactory widgetModelFactory,
            IWidgetService widgetService,
            WidgetSettings widgetSettings)
        {
            this._permissionService = permissionService;
            this._pluginFinder = pluginFinder;
            this._settingService = settingService;
            this._widgetModelFactory = widgetModelFactory;
            this._widgetService = widgetService;
            this._widgetSettings = widgetSettings;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //prepare model
            var model = _widgetModelFactory.PrepareWidgetSearchModel(new WidgetSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(WidgetSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _widgetModelFactory.PrepareWidgetListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult WidgetUpdate(WidgetModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var widget = _widgetService.LoadWidgetBySystemName(model.SystemName);
            if (_widgetService.IsWidgetActive(widget))
            {
                if (!model.IsActive)
                {
                    //mark as disabled
                    _widgetSettings.ActiveWidgetSystemNames.Remove(widget.PluginDescriptor.SystemName);
                    _settingService.SaveSetting(_widgetSettings);
                }
            }
            else
            {
                if (model.IsActive)
                {
                    //mark as active
                    _widgetSettings.ActiveWidgetSystemNames.Add(widget.PluginDescriptor.SystemName);
                    _settingService.SaveSetting(_widgetSettings);
                }
            }

            var pluginDescriptor = widget.PluginDescriptor;

            //display order
            pluginDescriptor.DisplayOrder = model.DisplayOrder;

            //update the description file
            PluginManager.SavePluginDescriptor(pluginDescriptor);

            //reset plugin cache
            _pluginFinder.ReloadPlugins(pluginDescriptor);

            return new NullJsonResult();
        }

        #endregion
    }
}