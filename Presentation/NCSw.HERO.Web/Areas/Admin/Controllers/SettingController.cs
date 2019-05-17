﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Core;
using NCSw.HERO.Core.Configuration;
using NCSw.HERO.Core.Domain;
using NCSw.HERO.Core.Domain.Blogs;
using NCSw.HERO.Core.Domain.Catalog;
using NCSw.HERO.Core.Domain.Common;
using NCSw.HERO.Core.Domain.Customers;
using NCSw.HERO.Core.Domain.Forums;
using NCSw.HERO.Core.Domain.Gdpr;
using NCSw.HERO.Core.Domain.Localization;
using NCSw.HERO.Core.Domain.Media;
using NCSw.HERO.Core.Domain.News;
using NCSw.HERO.Core.Domain.Orders;
using NCSw.HERO.Core.Domain.Security;
using NCSw.HERO.Core.Domain.Seo;
using NCSw.HERO.Core.Domain.Shipping;
using NCSw.HERO.Core.Domain.Tax;
using NCSw.HERO.Core.Domain.Vendors;
using NCSw.HERO.Services.Common;
using NCSw.HERO.Services.Configuration;
using NCSw.HERO.Services.Customers;
using NCSw.HERO.Services.Gdpr;
using NCSw.HERO.Services.Helpers;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Services.Logging;
using NCSw.HERO.Services.Media;
using NCSw.HERO.Services.Orders;
using NCSw.HERO.Services.Security;
using NCSw.HERO.Services.Stores;
using NCSw.HERO.Web.Areas.Admin.Factories;
using NCSw.HERO.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using NCSw.HERO.Web.Areas.Admin.Models.Settings;
using NCSw.HERO.Web.Framework;
using NCSw.HERO.Web.Framework.Controllers;
using NCSw.HERO.Web.Framework.Kendoui;
using NCSw.HERO.Web.Framework.Localization;
using NCSw.HERO.Web.Framework.Mvc;
using NCSw.HERO.Web.Framework.Mvc.Filters;

namespace NCSw.HERO.Web.Areas.Admin.Controllers
{
    public partial class SettingController : BaseAdminController
    {
        #region Fields

        private readonly IAddressService _addressService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ICustomerService _customerService;
        private readonly IEncryptionService _encryptionService;
        private readonly IFulltextService _fulltextService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IGdprService _gdprService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ILocalizationService _localizationService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly IOrderService _orderService;
        private readonly IPermissionService _permissionService;
        private readonly IPictureService _pictureService;
        private readonly ISettingModelFactory _settingModelFactory;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly NopConfig _config;

        #endregion

        #region Ctor

        public SettingController(IAddressService addressService,
            ICustomerActivityService customerActivityService,
            ICustomerService customerService,
            IEncryptionService encryptionService,
            IFulltextService fulltextService,
            IGenericAttributeService genericAttributeService,
            IGdprService gdprService,
            ILocalizedEntityService localizedEntityService,
            ILocalizationService localizationService,
            IMaintenanceService maintenanceService,
            IOrderService orderService,
            IPermissionService permissionService,
            IPictureService pictureService,
            ISettingModelFactory settingModelFactory,
            ISettingService settingService,
            IStoreContext storeContext,
            IStoreService storeService,
            IWorkContext workContext,
            NopConfig config)
        {
            this._addressService = addressService;
            this._customerActivityService = customerActivityService;
            this._customerService = customerService;
            this._encryptionService = encryptionService;
            this._fulltextService = fulltextService;
            this._genericAttributeService = genericAttributeService;
            this._gdprService = gdprService;
            this._localizedEntityService = localizedEntityService;
            this._localizationService = localizationService;
            this._maintenanceService = maintenanceService;
            this._orderService = orderService;
            this._permissionService = permissionService;
            this._pictureService = pictureService;
            this._settingModelFactory = settingModelFactory;
            this._settingService = settingService;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._workContext = workContext;
            this._config = config;
        }

        #endregion

        #region Utilites

        protected virtual void UpdateGDPRConsentLocales(GdprConsent gdprConsent, GdprConsentModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(gdprConsent,
                    x => x.Message,
                    localized.Message,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(gdprConsent,
                    x => x.RequiredMessage,
                    localized.RequiredMessage,
                    localized.LanguageId);
            }
        }

        #endregion

        #region Methods

        public virtual IActionResult ChangeStoreScopeConfiguration(int storeid, string returnUrl = "")
        {
            var store = _storeService.GetStoreById(storeid);
            if (store != null || storeid == 0)
            {
                _genericAttributeService
                    .SaveAttribute(_workContext.CurrentCustomer, NopCustomerDefaults.AdminAreaStoreScopeConfigurationAttribute, storeid);
            }

            //home page
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = Url.Action("Index", "Home", new { area = AreaNames.Admin });

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home", new { area = AreaNames.Admin });

            return Redirect(returnUrl);
        }

        public virtual IActionResult Blog()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareBlogSettingsModel();

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult Blog(BlogSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var blogSettings = _settingService.LoadSetting<BlogSettings>(storeScope);
            blogSettings = model.ToSettings(blogSettings);

             //we do not clear cache after each setting update.
             //this behavior can increase performance because cached settings will not be cleared 
             //and loaded from database after each update
            _settingService.SaveSettingOverridablePerStore(blogSettings, x => x.Enabled, model.Enabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(blogSettings, x => x.PostsPageSize, model.PostsPageSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(blogSettings, x => x.AllowNotRegisteredUsersToLeaveComments, model.AllowNotRegisteredUsersToLeaveComments_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(blogSettings, x => x.NotifyAboutNewBlogComments, model.NotifyAboutNewBlogComments_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(blogSettings, x => x.NumberOfTags, model.NumberOfTags_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(blogSettings, x => x.ShowHeaderRssUrl, model.ShowHeaderRssUrl_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(blogSettings, x => x.BlogCommentsMustBeApproved, model.BlogCommentsMustBeApproved_OverrideForStore, storeScope, false);
            _settingService.SaveSetting(blogSettings, x => x.ShowBlogCommentsPerStore, clearCache: false);

            //now clear settings cache
            _settingService.ClearCache();

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            return RedirectToAction("Blog");
        }

        public virtual IActionResult Vendor()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareVendorSettingsModel();

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult Vendor(VendorSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var vendorSettings = _settingService.LoadSetting<VendorSettings>(storeScope);
            vendorSettings = model.ToSettings(vendorSettings);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSettingOverridablePerStore(vendorSettings, x => x.VendorsBlockItemsToDisplay, model.VendorsBlockItemsToDisplay_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(vendorSettings, x => x.ShowVendorOnProductDetailsPage, model.ShowVendorOnProductDetailsPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(vendorSettings, x => x.ShowVendorOnOrderDetailsPage, model.ShowVendorOnOrderDetailsPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(vendorSettings, x => x.AllowCustomersToContactVendors, model.AllowCustomersToContactVendors_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(vendorSettings, x => x.AllowCustomersToApplyForVendorAccount, model.AllowCustomersToApplyForVendorAccount_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(vendorSettings, x => x.TermsOfServiceEnabled, model.TermsOfServiceEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(vendorSettings, x => x.AllowSearchByVendor, model.AllowSearchByVendor_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(vendorSettings, x => x.AllowVendorsToEditInfo, model.AllowVendorsToEditInfo_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(vendorSettings, x => x.NotifyStoreOwnerAboutVendorInformationChange, model.NotifyStoreOwnerAboutVendorInformationChange_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(vendorSettings, x => x.MaximumProductNumber, model.MaximumProductNumber_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(vendorSettings, x => x.AllowVendorsToImportProducts, model.AllowVendorsToImportProducts_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            return RedirectToAction("Vendor");
        }

        public virtual IActionResult Forum()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareForumSettingsModel();

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult Forum(ForumSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var forumSettings = _settingService.LoadSetting<ForumSettings>(storeScope);
            forumSettings = model.ToSettings(forumSettings);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.ForumsEnabled, model.ForumsEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.RelativeDateTimeFormattingEnabled, model.RelativeDateTimeFormattingEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.ShowCustomersPostCount, model.ShowCustomersPostCount_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.AllowGuestsToCreatePosts, model.AllowGuestsToCreatePosts_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.AllowGuestsToCreateTopics, model.AllowGuestsToCreateTopics_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.AllowCustomersToEditPosts, model.AllowCustomersToEditPosts_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.AllowCustomersToDeletePosts, model.AllowCustomersToDeletePosts_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.AllowPostVoting, model.AllowPostVoting_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.MaxVotesPerDay, model.MaxVotesPerDay_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.AllowCustomersToManageSubscriptions, model.AllowCustomersToManageSubscriptions_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.TopicsPageSize, model.TopicsPageSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.PostsPageSize, model.PostsPageSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.ForumEditor, model.ForumEditor_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.SignaturesEnabled, model.SignaturesEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.AllowPrivateMessages, model.AllowPrivateMessages_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.ShowAlertForPM, model.ShowAlertForPM_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.NotifyAboutPrivateMessages, model.NotifyAboutPrivateMessages_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.ActiveDiscussionsFeedEnabled, model.ActiveDiscussionsFeedEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.ActiveDiscussionsFeedCount, model.ActiveDiscussionsFeedCount_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.ForumFeedsEnabled, model.ForumFeedsEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.ForumFeedCount, model.ForumFeedCount_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.SearchResultsPageSize, model.SearchResultsPageSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(forumSettings, x => x.ActiveDiscussionsPageSize, model.ActiveDiscussionsPageSize_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            return RedirectToAction("Forum");
        }

        public virtual IActionResult News()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareNewsSettingsModel();

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult News(NewsSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var newsSettings = _settingService.LoadSetting<NewsSettings>(storeScope);
            newsSettings = model.ToSettings(newsSettings);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSettingOverridablePerStore(newsSettings, x => x.Enabled, model.Enabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(newsSettings, x => x.AllowNotRegisteredUsersToLeaveComments, model.AllowNotRegisteredUsersToLeaveComments_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(newsSettings, x => x.NotifyAboutNewNewsComments, model.NotifyAboutNewNewsComments_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(newsSettings, x => x.ShowNewsOnMainPage, model.ShowNewsOnMainPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(newsSettings, x => x.MainPageNewsCount, model.MainPageNewsCount_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(newsSettings, x => x.NewsArchivePageSize, model.NewsArchivePageSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(newsSettings, x => x.ShowHeaderRssUrl, model.ShowHeaderRssUrl_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(newsSettings, x => x.NewsCommentsMustBeApproved, model.NewsCommentsMustBeApproved_OverrideForStore, storeScope, false);
            _settingService.SaveSetting(newsSettings, x => x.ShowNewsCommentsPerStore, clearCache: false);

            //now clear settings cache
            _settingService.ClearCache();

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            return RedirectToAction("News");
        }

        public virtual IActionResult Shipping()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareShippingSettingsModel();

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult Shipping(ShippingSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var shippingSettings = _settingService.LoadSetting<ShippingSettings>(storeScope);
            shippingSettings = model.ToSettings(shippingSettings);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.ShipToSameAddress, model.ShipToSameAddress_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.AllowPickUpInStore, model.AllowPickUpInStore_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.DisplayPickupPointsOnMap, model.DisplayPickupPointsOnMap_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.GoogleMapsApiKey, model.GoogleMapsApiKey_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.UseWarehouseLocation, model.UseWarehouseLocation_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.NotifyCustomerAboutShippingFromMultipleLocations, model.NotifyCustomerAboutShippingFromMultipleLocations_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.FreeShippingOverXEnabled, model.FreeShippingOverXEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.FreeShippingOverXValue, model.FreeShippingOverXValue_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.FreeShippingOverXIncludingTax, model.FreeShippingOverXIncludingTax_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.EstimateShippingEnabled, model.EstimateShippingEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.DisplayShipmentEventsToCustomers, model.DisplayShipmentEventsToCustomers_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.DisplayShipmentEventsToStoreOwner, model.DisplayShipmentEventsToStoreOwner_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.HideShippingTotal, model.HideShippingTotal_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.BypassShippingMethodSelectionIfOnlyOne, model.BypassShippingMethodSelectionIfOnlyOne_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shippingSettings, x => x.ConsiderAssociatedProductsDimensions, model.ConsiderAssociatedProductsDimensions_OverrideForStore, storeScope, false);

            if (model.ShippingOriginAddress_OverrideForStore || storeScope == 0)
            {
                //update address
                var addressId = _settingService.SettingExists(shippingSettings, x => x.ShippingOriginAddressId, storeScope) ?
                    shippingSettings.ShippingOriginAddressId : 0;
                var originAddress = _addressService.GetAddressById(addressId) ??
                    new Address
                    {
                        CreatedOnUtc = DateTime.UtcNow
                    };
                //update ID manually (in case we're in multi-store configuration mode it'll be set to the shared one)
                model.ShippingOriginAddress.Id = addressId;
                originAddress = model.ShippingOriginAddress.ToEntity(originAddress);
                if (originAddress.Id > 0)
                    _addressService.UpdateAddress(originAddress);
                else
                    _addressService.InsertAddress(originAddress);
                shippingSettings.ShippingOriginAddressId = originAddress.Id;

                _settingService.SaveSetting(shippingSettings, x => x.ShippingOriginAddressId, storeScope, false);
            }
            else if (storeScope > 0)
                _settingService.DeleteSetting(shippingSettings, x => x.ShippingOriginAddressId, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            return RedirectToAction("Shipping");
        }

        public virtual IActionResult Tax()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareTaxSettingsModel();

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult Tax(TaxSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var taxSettings = _settingService.LoadSetting<TaxSettings>(storeScope);
            taxSettings = model.ToSettings(taxSettings);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.PricesIncludeTax, model.PricesIncludeTax_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.AllowCustomersToSelectTaxDisplayType, model.AllowCustomersToSelectTaxDisplayType_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.TaxDisplayType, model.TaxDisplayType_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.DisplayTaxSuffix, model.DisplayTaxSuffix_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.DisplayTaxRates, model.DisplayTaxRates_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.HideZeroTax, model.HideZeroTax_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.HideTaxInOrderSummary, model.HideTaxInOrderSummary_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.ForceTaxExclusionFromOrderSubtotal, model.ForceTaxExclusionFromOrderSubtotal_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.DefaultTaxCategoryId, model.DefaultTaxCategoryId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.TaxBasedOn, model.TaxBasedOn_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.TaxBasedOnPickupPointAddress, model.TaxBasedOnPickupPointAddress_OverrideForStore, storeScope, false);

            if (model.DefaultTaxAddress_OverrideForStore || storeScope == 0)
            {
                //update address
                var addressId = _settingService.SettingExists(taxSettings, x => x.DefaultTaxAddressId, storeScope) ?
                    taxSettings.DefaultTaxAddressId : 0;
                var originAddress = _addressService.GetAddressById(addressId) ??
                    new Address
                    {
                        CreatedOnUtc = DateTime.UtcNow
                    };
                //update ID manually (in case we're in multi-store configuration mode it'll be set to the shared one)
                model.DefaultTaxAddress.Id = addressId;
                originAddress = model.DefaultTaxAddress.ToEntity(originAddress);
                if (originAddress.Id > 0)
                    _addressService.UpdateAddress(originAddress);
                else
                    _addressService.InsertAddress(originAddress);
                taxSettings.DefaultTaxAddressId = originAddress.Id;

                _settingService.SaveSetting(taxSettings, x => x.DefaultTaxAddressId, storeScope, false);
            }
            else if (storeScope > 0)
                _settingService.DeleteSetting(taxSettings, x => x.DefaultTaxAddressId, storeScope);

            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.ShippingIsTaxable, model.ShippingIsTaxable_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.ShippingPriceIncludesTax, model.ShippingPriceIncludesTax_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.ShippingTaxClassId, model.ShippingTaxClassId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.PaymentMethodAdditionalFeeIsTaxable, model.PaymentMethodAdditionalFeeIsTaxable_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.PaymentMethodAdditionalFeeIncludesTax, model.PaymentMethodAdditionalFeeIncludesTax_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.PaymentMethodAdditionalFeeTaxClassId, model.PaymentMethodAdditionalFeeTaxClassId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.EuVatEnabled, model.EuVatEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.EuVatShopCountryId, model.EuVatShopCountryId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.EuVatAllowVatExemption, model.EuVatAllowVatExemption_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.EuVatUseWebService, model.EuVatUseWebService_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.EuVatAssumeValid, model.EuVatAssumeValid_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(taxSettings, x => x.EuVatEmailAdminWhenNewVatSubmitted, model.EuVatEmailAdminWhenNewVatSubmitted_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            return RedirectToAction("Tax");
        }

        public virtual IActionResult Catalog()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareCatalogSettingsModel();

            return View(model);
        }[HttpPost]
        public virtual IActionResult Catalog(CatalogSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var catalogSettings = _settingService.LoadSetting<CatalogSettings>(storeScope);
            catalogSettings = model.ToSettings(catalogSettings);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.AllowViewUnpublishedProductPage, model.AllowViewUnpublishedProductPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.DisplayDiscontinuedMessageForUnpublishedProducts, model.DisplayDiscontinuedMessageForUnpublishedProducts_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ShowSkuOnProductDetailsPage, model.ShowSkuOnProductDetailsPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ShowSkuOnCatalogPages, model.ShowSkuOnCatalogPages_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ShowManufacturerPartNumber, model.ShowManufacturerPartNumber_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ShowGtin, model.ShowGtin_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ShowFreeShippingNotification, model.ShowFreeShippingNotification_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.AllowProductSorting, model.AllowProductSorting_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.AllowProductViewModeChanging, model.AllowProductViewModeChanging_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.DefaultViewMode, model.DefaultViewMode_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ShowProductsFromSubcategories, model.ShowProductsFromSubcategories_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ShowCategoryProductNumber, model.ShowCategoryProductNumber_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ShowCategoryProductNumberIncludingSubcategories, model.ShowCategoryProductNumberIncludingSubcategories_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.CategoryBreadcrumbEnabled, model.CategoryBreadcrumbEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ShowShareButton, model.ShowShareButton_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.PageShareCode, model.PageShareCode_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ProductReviewsMustBeApproved, model.ProductReviewsMustBeApproved_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.AllowAnonymousUsersToReviewProduct, model.AllowAnonymousUsersToReviewProduct_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ProductReviewPossibleOnlyAfterPurchasing, model.ProductReviewPossibleOnlyAfterPurchasing_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.NotifyStoreOwnerAboutNewProductReviews, model.NotifyStoreOwnerAboutNewProductReviews_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.NotifyCustomerAboutProductReviewReply, model.NotifyCustomerAboutProductReviewReply_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.EmailAFriendEnabled, model.EmailAFriendEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.AllowAnonymousUsersToEmailAFriend, model.AllowAnonymousUsersToEmailAFriend_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.RecentlyViewedProductsNumber, model.RecentlyViewedProductsNumber_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.RecentlyViewedProductsEnabled, model.RecentlyViewedProductsEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.NewProductsNumber, model.NewProductsNumber_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.NewProductsEnabled, model.NewProductsEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.CompareProductsEnabled, model.CompareProductsEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ShowBestsellersOnHomepage, model.ShowBestsellersOnHomepage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.NumberOfBestsellersOnHomepage, model.NumberOfBestsellersOnHomepage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.SearchPageProductsPerPage, model.SearchPageProductsPerPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.SearchPageAllowCustomersToSelectPageSize, model.SearchPageAllowCustomersToSelectPageSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.SearchPagePageSizeOptions, model.SearchPagePageSizeOptions_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ProductSearchAutoCompleteEnabled, model.ProductSearchAutoCompleteEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ProductSearchAutoCompleteNumberOfProducts, model.ProductSearchAutoCompleteNumberOfProducts_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ShowProductImagesInSearchAutoComplete, model.ShowProductImagesInSearchAutoComplete_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ShowLinkToAllResultInSearchAutoComplete, model.ShowLinkToAllResultInSearchAutoComplete_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ProductSearchTermMinimumLength, model.ProductSearchTermMinimumLength_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ProductsAlsoPurchasedEnabled, model.ProductsAlsoPurchasedEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ProductsAlsoPurchasedNumber, model.ProductsAlsoPurchasedNumber_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.NumberOfProductTags, model.NumberOfProductTags_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ProductsByTagPageSize, model.ProductsByTagPageSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ProductsByTagAllowCustomersToSelectPageSize, model.ProductsByTagAllowCustomersToSelectPageSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ProductsByTagPageSizeOptions, model.ProductsByTagPageSizeOptions_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.IncludeShortDescriptionInCompareProducts, model.IncludeShortDescriptionInCompareProducts_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.IncludeFullDescriptionInCompareProducts, model.IncludeFullDescriptionInCompareProducts_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ManufacturersBlockItemsToDisplay, model.ManufacturersBlockItemsToDisplay_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.DisplayTaxShippingInfoFooter, model.DisplayTaxShippingInfoFooter_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.DisplayTaxShippingInfoProductDetailsPage, model.DisplayTaxShippingInfoProductDetailsPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.DisplayTaxShippingInfoProductBoxes, model.DisplayTaxShippingInfoProductBoxes_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.DisplayTaxShippingInfoShoppingCart, model.DisplayTaxShippingInfoShoppingCart_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.DisplayTaxShippingInfoWishlist, model.DisplayTaxShippingInfoWishlist_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.DisplayTaxShippingInfoOrderDetailsPage, model.DisplayTaxShippingInfoOrderDetailsPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ShowProductReviewsPerStore, model.ShowProductReviewsPerStore_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ShowProductReviewsTabOnAccountPage, model.ShowProductReviewsOnAccountPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ProductReviewsPageSizeOnAccountPage, model.ProductReviewsPageSizeOnAccountPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ProductReviewsSortByCreatedDateAscending, model.ProductReviewsSortByCreatedDateAscending_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ExportImportProductAttributes, model.ExportImportProductAttributes_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ExportImportProductSpecificationAttributes, model.ExportImportProductSpecificationAttributes_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ExportImportProductCategoryBreadcrumb, model.ExportImportProductCategoryBreadcrumb_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ExportImportCategoriesUsingCategoryName, model.ExportImportCategoriesUsingCategoryName_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ExportImportAllowDownloadImages, model.ExportImportAllowDownloadImages_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ExportImportSplitProductsFile, model.ExportImportSplitProductsFile_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.RemoveRequiredProducts, model.RemoveRequiredProducts_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.ExportImportRelatedEntitiesByName, model.ExportImportRelatedEntitiesByName_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(catalogSettings, x => x.DisplayDatePreOrderAvailability, model.DisplayDatePreOrderAvailability_OverrideForStore, storeScope, false);
            
            //now settings not overridable per store
            _settingService.SaveSetting(catalogSettings, x => x.IgnoreDiscounts, 0, false);
            _settingService.SaveSetting(catalogSettings, x => x.IgnoreFeaturedProducts, 0, false);
            _settingService.SaveSetting(catalogSettings, x => x.IgnoreAcl, 0, false);
            _settingService.SaveSetting(catalogSettings, x => x.IgnoreStoreLimitations, 0, false);
            _settingService.SaveSetting(catalogSettings, x => x.CacheProductPrices, 0, false);

            //now clear settings cache
            _settingService.ClearCache();

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            return RedirectToAction("Catalog");
        }
        [HttpPost]
        public virtual IActionResult SortOptionsList(SortOptionSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _settingModelFactory.PrepareSortOptionListModel(searchModel);

            return Json(model);
        }
        [HttpPost]
        public virtual IActionResult SortOptionUpdate(SortOptionModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var catalogSettings = _settingService.LoadSetting<CatalogSettings>(storeScope);

            catalogSettings.ProductSortingEnumDisplayOrder[model.Id] = model.DisplayOrder;
            if (model.IsActive && catalogSettings.ProductSortingEnumDisabled.Contains(model.Id))
                catalogSettings.ProductSortingEnumDisabled.Remove(model.Id);
            if (!model.IsActive && !catalogSettings.ProductSortingEnumDisabled.Contains(model.Id))
                catalogSettings.ProductSortingEnumDisabled.Add(model.Id);

            _settingService.SaveSetting(catalogSettings);

            return new NullJsonResult();
        }

        public virtual IActionResult RewardPoints()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareRewardPointsSettingsModel();

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult RewardPoints(RewardPointsSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                //load settings for a chosen store scope
                var storeScope = _storeContext.ActiveStoreScopeConfiguration;
                var rewardPointsSettings = _settingService.LoadSetting<RewardPointsSettings>(storeScope);
                rewardPointsSettings = model.ToSettings(rewardPointsSettings);

                if (model.ActivatePointsImmediately)
                    rewardPointsSettings.ActivationDelay = 0;

                //we do not clear cache after each setting update.
                //this behavior can increase performance because cached settings will not be cleared 
                //and loaded from database after each update
                _settingService.SaveSettingOverridablePerStore(rewardPointsSettings, x => x.Enabled, model.Enabled_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(rewardPointsSettings, x => x.ExchangeRate, model.ExchangeRate_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(rewardPointsSettings, x => x.MinimumRewardPointsToUse, model.MinimumRewardPointsToUse_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(rewardPointsSettings, x => x.MaximumRewardPointsToUsePerOrder, model.MaximumRewardPointsToUsePerOrder_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(rewardPointsSettings, x => x.PointsForRegistration, model.PointsForRegistration_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(rewardPointsSettings, x => x.RegistrationPointsValidity, model.RegistrationPointsValidity_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(rewardPointsSettings, x => x.PointsForPurchases_Amount, model.PointsForPurchases_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(rewardPointsSettings, x => x.PointsForPurchases_Points, model.PointsForPurchases_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(rewardPointsSettings, x => x.MinOrderTotalToAwardPoints, model.MinOrderTotalToAwardPoints_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(rewardPointsSettings, x => x.PurchasesPointsValidity, model.PurchasesPointsValidity_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(rewardPointsSettings, x => x.ActivationDelay, model.ActivationDelay_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(rewardPointsSettings, x => x.ActivationDelayPeriodId, model.ActivationDelay_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(rewardPointsSettings, x => x.DisplayHowMuchWillBeEarned, model.DisplayHowMuchWillBeEarned_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(rewardPointsSettings, x => x.PageSize, model.PageSize_OverrideForStore, storeScope, false);
                _settingService.SaveSetting(rewardPointsSettings, x => x.PointsAccumulatedForAllStores, 0, false);

                //now clear settings cache
                _settingService.ClearCache();

                //activity log
                _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));
            }
            else
            {
                //if we got this far, something failed, redisplay form
                foreach (var modelState in ModelState.Values)
                    foreach (var error in modelState.Errors)
                        ErrorNotification(error.ErrorMessage);
            }

            return RedirectToAction("RewardPoints");
        }

        public virtual IActionResult Order()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareOrderSettingsModel();

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult Order(OrderSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                //load settings for a chosen store scope
                var storeScope = _storeContext.ActiveStoreScopeConfiguration;
                var orderSettings = _settingService.LoadSetting<OrderSettings>(storeScope);
                orderSettings = model.ToSettings(orderSettings);

                //we do not clear cache after each setting update.
                //this behavior can increase performance because cached settings will not be cleared 
                //and loaded from database after each update
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.IsReOrderAllowed, model.IsReOrderAllowed_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.MinOrderSubtotalAmount, model.MinOrderSubtotalAmount_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.MinOrderSubtotalAmountIncludingTax, model.MinOrderSubtotalAmountIncludingTax_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.MinOrderTotalAmount, model.MinOrderTotalAmount_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.AutoUpdateOrderTotalsOnEditingOrder, model.AutoUpdateOrderTotalsOnEditingOrder_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.AnonymousCheckoutAllowed, model.AnonymousCheckoutAllowed_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.CheckoutDisabled, model.CheckoutDisabled_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.TermsOfServiceOnShoppingCartPage, model.TermsOfServiceOnShoppingCartPage_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.TermsOfServiceOnOrderConfirmPage, model.TermsOfServiceOnOrderConfirmPage_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.OnePageCheckoutEnabled, model.OnePageCheckoutEnabled_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.OnePageCheckoutDisplayOrderTotalsOnPaymentInfoTab, model.OnePageCheckoutDisplayOrderTotalsOnPaymentInfoTab_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.DisableBillingAddressCheckoutStep, model.DisableBillingAddressCheckoutStep_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.DisableOrderCompletedPage, model.DisableOrderCompletedPage_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.AttachPdfInvoiceToOrderPlacedEmail, model.AttachPdfInvoiceToOrderPlacedEmail_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.AttachPdfInvoiceToOrderPaidEmail, model.AttachPdfInvoiceToOrderPaidEmail_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.AttachPdfInvoiceToOrderCompletedEmail, model.AttachPdfInvoiceToOrderCompletedEmail_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.ReturnRequestsEnabled, model.ReturnRequestsEnabled_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.ReturnRequestsAllowFiles, model.ReturnRequestsAllowFiles_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.ReturnRequestNumberMask, model.ReturnRequestNumberMask_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.NumberOfDaysReturnRequestAvailable, model.NumberOfDaysReturnRequestAvailable_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.CustomOrderNumberMask, model.CustomOrderNumberMask_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.ExportWithProducts, model.ExportWithProducts_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.AllowAdminsToBuyCallForPriceProducts, model.AllowAdminsToBuyCallForPriceProducts_OverrideForStore, storeScope, false);
                _settingService.SaveSettingOverridablePerStore(orderSettings, x => x.DeleteGiftCardUsageHistory, model.DeleteGiftCardUsageHistory_OverrideForStore, storeScope, false);
                _settingService.SaveSetting(orderSettings, x => x.ActivateGiftCardsAfterCompletingOrder, 0, false);
                _settingService.SaveSetting(orderSettings, x => x.DeactivateGiftCardsAfterCancellingOrder, 0, false);
                _settingService.SaveSetting(orderSettings, x => x.DeactivateGiftCardsAfterDeletingOrder, 0, false);
                _settingService.SaveSetting(orderSettings, x => x.CompleteOrderWhenDelivered, 0, false);

                //now clear settings cache
                _settingService.ClearCache();

                //order ident
                if (model.OrderIdent.HasValue)
                {
                    try
                    {
                        _maintenanceService.SetTableIdent<Order>(model.OrderIdent.Value);
                    }
                    catch (Exception exc)
                    {
                        ErrorNotification(exc.Message);
                    }
                }

                //activity log
                _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));
            }
            else
            {
                //if we got this far, something failed, redisplay form
                foreach (var modelState in ModelState.Values)
                    foreach (var error in modelState.Errors)
                        ErrorNotification(error.ErrorMessage);
            }

            //selected tab
            SaveSelectedTabName();

            return RedirectToAction("Order");
        }

        public virtual IActionResult ShoppingCart()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareShoppingCartSettingsModel();

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult ShoppingCart(ShoppingCartSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var shoppingCartSettings = _settingService.LoadSetting<ShoppingCartSettings>(storeScope);
            shoppingCartSettings = model.ToSettings(shoppingCartSettings);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.DisplayCartAfterAddingProduct, model.DisplayCartAfterAddingProduct_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.DisplayWishlistAfterAddingProduct, model.DisplayWishlistAfterAddingProduct_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.MaximumShoppingCartItems, model.MaximumShoppingCartItems_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.MaximumWishlistItems, model.MaximumWishlistItems_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.AllowOutOfStockItemsToBeAddedToWishlist, model.AllowOutOfStockItemsToBeAddedToWishlist_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.MoveItemsFromWishlistToCart, model.MoveItemsFromWishlistToCart_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.CartsSharedBetweenStores, model.CartsSharedBetweenStores_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.ShowProductImagesOnShoppingCart, model.ShowProductImagesOnShoppingCart_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.ShowProductImagesOnWishList, model.ShowProductImagesOnWishList_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.ShowDiscountBox, model.ShowDiscountBox_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.ShowGiftCardBox, model.ShowGiftCardBox_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.CrossSellsNumber, model.CrossSellsNumber_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.EmailWishlistEnabled, model.EmailWishlistEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.AllowAnonymousUsersToEmailWishlist, model.AllowAnonymousUsersToEmailWishlist_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.MiniShoppingCartEnabled, model.MiniShoppingCartEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.ShowProductImagesInMiniShoppingCart, model.ShowProductImagesInMiniShoppingCart_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.MiniShoppingCartProductNumber, model.MiniShoppingCartProductNumber_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shoppingCartSettings, x => x.AllowCartItemEditing, model.AllowCartItemEditing_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            return RedirectToAction("ShoppingCart");
        }

        public virtual IActionResult Media()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareMediaSettingsModel();

            return View(model);
        }
        [HttpPost]
        [FormValueRequired("save")]
        public virtual IActionResult Media(MediaSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var mediaSettings = _settingService.LoadSetting<MediaSettings>(storeScope);
            mediaSettings = model.ToSettings(mediaSettings);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.AvatarPictureSize, model.AvatarPictureSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.ProductThumbPictureSize, model.ProductThumbPictureSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.ProductDetailsPictureSize, model.ProductDetailsPictureSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.ProductThumbPictureSizeOnProductDetailsPage, model.ProductThumbPictureSizeOnProductDetailsPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.AssociatedProductPictureSize, model.AssociatedProductPictureSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.CategoryThumbPictureSize, model.CategoryThumbPictureSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.ManufacturerThumbPictureSize, model.ManufacturerThumbPictureSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.VendorThumbPictureSize, model.VendorThumbPictureSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.CartThumbPictureSize, model.CartThumbPictureSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.MiniCartThumbPictureSize, model.MiniCartThumbPictureSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.MaximumImageSize, model.MaximumImageSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.MultipleThumbDirectories, model.MultipleThumbDirectories_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.DefaultImageQuality, model.DefaultImageQuality_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.ImportProductImagesUsingHash, model.ImportProductImagesUsingHash_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(mediaSettings, x => x.DefaultPictureZoomEnabled, model.DefaultPictureZoomEnabled_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            return RedirectToAction("Media");
        }
        [HttpPost, ActionName("Media")]
        [FormValueRequired("change-picture-storage")]
        public virtual IActionResult ChangePictureStorage()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            _pictureService.StoreInDb = !_pictureService.StoreInDb;

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            return RedirectToAction("Media");
        }

        public virtual IActionResult CustomerUser()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareCustomerUserSettingsModel();

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult CustomerUser(CustomerUserSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var customerSettings = _settingService.LoadSetting<CustomerSettings>(storeScope);

            var lastUsernameValidationRule = customerSettings.UsernameValidationRule;
            var lastUsernameValidationEnabledValue = customerSettings.UsernameValidationEnabled;
            var lastUsernameValidationUseRegexValue = customerSettings.UsernameValidationUseRegex;

            var addressSettings = _settingService.LoadSetting<AddressSettings>(storeScope);
            var dateTimeSettings = _settingService.LoadSetting<DateTimeSettings>(storeScope);
            var externalAuthenticationSettings = _settingService.LoadSetting<ExternalAuthenticationSettings>(storeScope);

            customerSettings = model.CustomerSettings.ToSettings(customerSettings);

            if (customerSettings.UsernameValidationEnabled && customerSettings.UsernameValidationUseRegex)
            {
                try
                {
                    //validate regex rule
                    var unused = Regex.IsMatch("test_user_name", customerSettings.UsernameValidationRule);
                }
                catch (ArgumentException)
                {
                    //restoring previous settings
                    customerSettings.UsernameValidationRule = lastUsernameValidationRule;
                    customerSettings.UsernameValidationEnabled = lastUsernameValidationEnabledValue;
                    customerSettings.UsernameValidationUseRegex = lastUsernameValidationUseRegexValue;

                    ErrorNotification(_localizationService.GetResource("Admin.Configuration.Settings.CustomerSettings.RegexValidationRule.Error"));
                }
            }

            _settingService.SaveSetting(customerSettings);

            addressSettings = model.AddressSettings.ToSettings(addressSettings);
            _settingService.SaveSetting(addressSettings);

            dateTimeSettings.DefaultStoreTimeZoneId = model.DateTimeSettings.DefaultStoreTimeZoneId;
            dateTimeSettings.AllowCustomersToSetTimeZone = model.DateTimeSettings.AllowCustomersToSetTimeZone;
            _settingService.SaveSetting(dateTimeSettings);

            externalAuthenticationSettings.AllowCustomersToRemoveAssociations = model.ExternalAuthenticationSettings.AllowCustomersToRemoveAssociations;
            _settingService.SaveSetting(externalAuthenticationSettings);

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            //selected tab
            SaveSelectedTabName();

            return RedirectToAction("CustomerUser");
        }

        #region GDPR

        public virtual IActionResult Gdpr()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareGdprSettingsModel();

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult Gdpr(GdprSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var gdprSettings = _settingService.LoadSetting<GdprSettings>(storeScope);
            gdprSettings = model.ToSettings(gdprSettings);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSettingOverridablePerStore(gdprSettings, x => x.GdprEnabled, model.GdprEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gdprSettings, x => x.LogPrivacyPolicyConsent, model.LogPrivacyPolicyConsent_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gdprSettings, x => x.LogNewsletterConsent, model.LogNewsletterConsent_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));
                        
            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            return RedirectToAction("Gdpr");
        }
        [HttpPost]
        public virtual IActionResult GdprConsentList(GdprConsentSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _settingModelFactory.PrepareGdprConsentListModel(searchModel);

            return Json(model);
        }
        public virtual IActionResult CreateGdprConsent()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareGdprConsentModel(new GdprConsentModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult CreateGdprConsent(GdprConsentModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var gdprConsent = model.ToEntity<GdprConsent>();
                _gdprService.InsertConsent(gdprConsent);

                //locales                
                UpdateGDPRConsentLocales(gdprConsent, model);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.Gdpr.Consent.Added"));

                return continueEditing ? RedirectToAction("EditGdprConsent", new { gdprConsent.Id }) : RedirectToAction("Gdpr");
            }

            //prepare model
            model = _settingModelFactory.PrepareGdprConsentModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }
        public virtual IActionResult EditGdprConsent(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a consent with the specified id
            var gdprConsent = _gdprService.GetConsentById(id);
            if (gdprConsent == null)
                return RedirectToAction("Gdpr");

            //prepare model
            var model = _settingModelFactory.PrepareGdprConsentModel(null, gdprConsent);

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult EditGdprConsent(GdprConsentModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a GDPR consent with the specified id
            var gdprConsent = _gdprService.GetConsentById(model.Id);
            if (gdprConsent == null)
                return RedirectToAction("Gdpr");

            if (ModelState.IsValid)
            {
                gdprConsent = model.ToEntity(gdprConsent);
                _gdprService.UpdateConsent(gdprConsent);

                //selected tab
                SaveSelectedTabName();

                //locales                
                UpdateGDPRConsentLocales(gdprConsent, model);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.Gdpr.Consent.Updated"));

                return continueEditing ? RedirectToAction("EditGdprConsent", gdprConsent.Id) : RedirectToAction("Gdpr");
            }

            //prepare model
            model = _settingModelFactory.PrepareGdprConsentModel(model, gdprConsent, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpPost]
        public virtual IActionResult DeleteGdprConsent(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a GDPR consent with the specified id
            var gdprConsent = _gdprService.GetConsentById(id);
            if (gdprConsent == null)
                return RedirectToAction("Gdpr");

            _gdprService.DeleteConsent(gdprConsent);

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.Gdpr.Consent.Deleted"));

            return RedirectToAction("Gdpr");
        }

        #endregion

        public virtual IActionResult GeneralCommon()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareGeneralCommonSettingsModel();

            //notify admin that CSS bundling is not allowed in virtual directories
            if (model.SeoSettings.EnableCssBundling && this.HttpContext.Request.PathBase.HasValue)
                WarningNotification(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EnableCssBundling.Warning"), false);

            return View(model);
        }
        [HttpPost]
        [FormValueRequired("save")]
        public virtual IActionResult GeneralCommon(GeneralCommonSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;

            //store information settings
            var storeInformationSettings = _settingService.LoadSetting<StoreInformationSettings>(storeScope);
            var commonSettings = _settingService.LoadSetting<CommonSettings>(storeScope);
            storeInformationSettings.StoreClosed = model.StoreInformationSettings.StoreClosed;
            storeInformationSettings.DefaultStoreTheme = model.StoreInformationSettings.DefaultStoreTheme;
            storeInformationSettings.AllowCustomerToSelectTheme = model.StoreInformationSettings.AllowCustomerToSelectTheme;
            storeInformationSettings.LogoPictureId = model.StoreInformationSettings.LogoPictureId;
            //EU Cookie law
            storeInformationSettings.DisplayEuCookieLawWarning = model.StoreInformationSettings.DisplayEuCookieLawWarning;
            //social pages
            storeInformationSettings.FacebookLink = model.StoreInformationSettings.FacebookLink;
            storeInformationSettings.TwitterLink = model.StoreInformationSettings.TwitterLink;
            storeInformationSettings.YoutubeLink = model.StoreInformationSettings.YoutubeLink;
            storeInformationSettings.GooglePlusLink = model.StoreInformationSettings.GooglePlusLink;
            //contact us
            commonSettings.SubjectFieldOnContactUsForm = model.StoreInformationSettings.SubjectFieldOnContactUsForm;
            commonSettings.UseSystemEmailForContactUsForm = model.StoreInformationSettings.UseSystemEmailForContactUsForm;
            //terms of service
            commonSettings.PopupForTermsOfServiceLinks = model.StoreInformationSettings.PopupForTermsOfServiceLinks;            
            //sitemap
            commonSettings.SitemapEnabled = model.StoreInformationSettings.SitemapEnabled;
            commonSettings.SitemapPageSize = model.StoreInformationSettings.SitemapPageSize;
            commonSettings.SitemapIncludeCategories = model.StoreInformationSettings.SitemapIncludeCategories;
            commonSettings.SitemapIncludeManufacturers = model.StoreInformationSettings.SitemapIncludeManufacturers;
            commonSettings.SitemapIncludeProducts = model.StoreInformationSettings.SitemapIncludeProducts;
            commonSettings.SitemapIncludeProductTags = model.StoreInformationSettings.SitemapIncludeProductTags;

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSettingOverridablePerStore(storeInformationSettings, x => x.StoreClosed, model.StoreInformationSettings.StoreClosed_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(storeInformationSettings, x => x.DefaultStoreTheme, model.StoreInformationSettings.DefaultStoreTheme_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(storeInformationSettings, x => x.AllowCustomerToSelectTheme, model.StoreInformationSettings.AllowCustomerToSelectTheme_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(storeInformationSettings, x => x.LogoPictureId, model.StoreInformationSettings.LogoPictureId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(storeInformationSettings, x => x.DisplayEuCookieLawWarning, model.StoreInformationSettings.DisplayEuCookieLawWarning_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(storeInformationSettings, x => x.FacebookLink, model.StoreInformationSettings.FacebookLink_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(storeInformationSettings, x => x.TwitterLink, model.StoreInformationSettings.TwitterLink_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(storeInformationSettings, x => x.YoutubeLink, model.StoreInformationSettings.YoutubeLink_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(storeInformationSettings, x => x.GooglePlusLink, model.StoreInformationSettings.GooglePlusLink_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(commonSettings, x => x.SubjectFieldOnContactUsForm, model.StoreInformationSettings.SubjectFieldOnContactUsForm_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(commonSettings, x => x.UseSystemEmailForContactUsForm, model.StoreInformationSettings.UseSystemEmailForContactUsForm_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(commonSettings, x => x.PopupForTermsOfServiceLinks, model.StoreInformationSettings.PopupForTermsOfServiceLinks_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(commonSettings, x => x.SitemapEnabled, model.StoreInformationSettings.SitemapEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(commonSettings, x => x.SitemapPageSize, model.StoreInformationSettings.SitemapPageSize_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(commonSettings, x => x.SitemapIncludeCategories, model.StoreInformationSettings.SitemapIncludeCategories_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(commonSettings, x => x.SitemapIncludeManufacturers, model.StoreInformationSettings.SitemapIncludeManufacturers_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(commonSettings, x => x.SitemapIncludeProducts, model.StoreInformationSettings.SitemapIncludeProducts_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(commonSettings, x => x.SitemapIncludeProductTags, model.StoreInformationSettings.SitemapIncludeProductTags_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            //seo settings
            var seoSettings = _settingService.LoadSetting<SeoSettings>(storeScope);
            seoSettings.PageTitleSeparator = model.SeoSettings.PageTitleSeparator;
            seoSettings.PageTitleSeoAdjustment = (PageTitleSeoAdjustment)model.SeoSettings.PageTitleSeoAdjustment;
            seoSettings.DefaultTitle = model.SeoSettings.DefaultTitle;
            seoSettings.DefaultMetaKeywords = model.SeoSettings.DefaultMetaKeywords;
            seoSettings.DefaultMetaDescription = model.SeoSettings.DefaultMetaDescription;
            seoSettings.GenerateProductMetaDescription = model.SeoSettings.GenerateProductMetaDescription;
            seoSettings.ConvertNonWesternChars = model.SeoSettings.ConvertNonWesternChars;
            seoSettings.CanonicalUrlsEnabled = model.SeoSettings.CanonicalUrlsEnabled;
            seoSettings.WwwRequirement = (WwwRequirement)model.SeoSettings.WwwRequirement;
            seoSettings.EnableJsBundling = model.SeoSettings.EnableJsBundling;
            seoSettings.EnableCssBundling = model.SeoSettings.EnableCssBundling;
            seoSettings.TwitterMetaTags = model.SeoSettings.TwitterMetaTags;
            seoSettings.OpenGraphMetaTags = model.SeoSettings.OpenGraphMetaTags;
            seoSettings.CustomHeadTags = model.SeoSettings.CustomHeadTags;

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.PageTitleSeparator, model.SeoSettings.PageTitleSeparator_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.PageTitleSeoAdjustment, model.SeoSettings.PageTitleSeoAdjustment_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.DefaultTitle, model.SeoSettings.DefaultTitle_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.DefaultMetaKeywords, model.SeoSettings.DefaultMetaKeywords_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.DefaultMetaDescription, model.SeoSettings.DefaultMetaDescription_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.GenerateProductMetaDescription, model.SeoSettings.GenerateProductMetaDescription_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.ConvertNonWesternChars, model.SeoSettings.ConvertNonWesternChars_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.CanonicalUrlsEnabled, model.SeoSettings.CanonicalUrlsEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.WwwRequirement, model.SeoSettings.WwwRequirement_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.EnableJsBundling, model.SeoSettings.EnableJsBundling_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.EnableCssBundling, model.SeoSettings.EnableCssBundling_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.TwitterMetaTags, model.SeoSettings.TwitterMetaTags_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.OpenGraphMetaTags, model.SeoSettings.OpenGraphMetaTags_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(seoSettings, x => x.CustomHeadTags, model.SeoSettings.CustomHeadTags_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            //security settings
            var securitySettings = _settingService.LoadSetting<SecuritySettings>(storeScope);
            if (securitySettings.AdminAreaAllowedIpAddresses == null)
                securitySettings.AdminAreaAllowedIpAddresses = new List<string>();
            securitySettings.AdminAreaAllowedIpAddresses.Clear();
            if (!string.IsNullOrEmpty(model.SecuritySettings.AdminAreaAllowedIpAddresses))
                foreach (var s in model.SecuritySettings.AdminAreaAllowedIpAddresses.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    if (!string.IsNullOrWhiteSpace(s))
                        securitySettings.AdminAreaAllowedIpAddresses.Add(s.Trim());
            securitySettings.ForceSslForAllPages = model.SecuritySettings.ForceSslForAllPages;
            securitySettings.EnableXsrfProtectionForAdminArea = model.SecuritySettings.EnableXsrfProtectionForAdminArea;
            securitySettings.EnableXsrfProtectionForPublicStore = model.SecuritySettings.EnableXsrfProtectionForPublicStore;
            securitySettings.HoneypotEnabled = model.SecuritySettings.HoneypotEnabled;
            _settingService.SaveSetting(securitySettings);

            //captcha settings
            var captchaSettings = _settingService.LoadSetting<CaptchaSettings>(storeScope);
            captchaSettings.Enabled = model.CaptchaSettings.Enabled;
            captchaSettings.ShowOnLoginPage = model.CaptchaSettings.ShowOnLoginPage;
            captchaSettings.ShowOnRegistrationPage = model.CaptchaSettings.ShowOnRegistrationPage;
            captchaSettings.ShowOnContactUsPage = model.CaptchaSettings.ShowOnContactUsPage;
            captchaSettings.ShowOnEmailWishlistToFriendPage = model.CaptchaSettings.ShowOnEmailWishlistToFriendPage;
            captchaSettings.ShowOnEmailProductToFriendPage = model.CaptchaSettings.ShowOnEmailProductToFriendPage;
            captchaSettings.ShowOnBlogCommentPage = model.CaptchaSettings.ShowOnBlogCommentPage;
            captchaSettings.ShowOnNewsCommentPage = model.CaptchaSettings.ShowOnNewsCommentPage;
            captchaSettings.ShowOnProductReviewPage = model.CaptchaSettings.ShowOnProductReviewPage;
            captchaSettings.ShowOnApplyVendorPage = model.CaptchaSettings.ShowOnApplyVendorPage;
            captchaSettings.ReCaptchaPublicKey = model.CaptchaSettings.ReCaptchaPublicKey;
            captchaSettings.ReCaptchaPrivateKey = model.CaptchaSettings.ReCaptchaPrivateKey;

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSettingOverridablePerStore(captchaSettings, x => x.Enabled, model.CaptchaSettings.Enabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(captchaSettings, x => x.ShowOnLoginPage, model.CaptchaSettings.ShowOnLoginPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(captchaSettings, x => x.ShowOnRegistrationPage, model.CaptchaSettings.ShowOnRegistrationPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(captchaSettings, x => x.ShowOnContactUsPage, model.CaptchaSettings.ShowOnContactUsPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(captchaSettings, x => x.ShowOnEmailWishlistToFriendPage, model.CaptchaSettings.ShowOnEmailWishlistToFriendPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(captchaSettings, x => x.ShowOnEmailProductToFriendPage, model.CaptchaSettings.ShowOnEmailProductToFriendPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(captchaSettings, x => x.ShowOnBlogCommentPage, model.CaptchaSettings.ShowOnBlogCommentPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(captchaSettings, x => x.ShowOnNewsCommentPage, model.CaptchaSettings.ShowOnNewsCommentPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(captchaSettings, x => x.ShowOnProductReviewPage, model.CaptchaSettings.ShowOnProductReviewPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(captchaSettings, x => x.ShowOnApplyVendorPage, model.CaptchaSettings.ShowOnApplyVendorPage_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(captchaSettings, x => x.ReCaptchaPublicKey, model.CaptchaSettings.ReCaptchaPublicKey_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(captchaSettings, x => x.ReCaptchaPrivateKey, model.CaptchaSettings.ReCaptchaPrivateKey_OverrideForStore, storeScope, false);

            // now clear settings cache
            _settingService.ClearCache();

            if (captchaSettings.Enabled &&
                (string.IsNullOrWhiteSpace(captchaSettings.ReCaptchaPublicKey) || string.IsNullOrWhiteSpace(captchaSettings.ReCaptchaPrivateKey)))
            {
                //captcha is enabled but the keys are not entered
                ErrorNotification(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.CaptchaAppropriateKeysNotEnteredError"));
            }

            //PDF settings
            var pdfSettings = _settingService.LoadSetting<PdfSettings>(storeScope);
            pdfSettings.LetterPageSizeEnabled = model.PdfSettings.LetterPageSizeEnabled;
            pdfSettings.LogoPictureId = model.PdfSettings.LogoPictureId;
            pdfSettings.DisablePdfInvoicesForPendingOrders = model.PdfSettings.DisablePdfInvoicesForPendingOrders;
            pdfSettings.InvoiceFooterTextColumn1 = model.PdfSettings.InvoiceFooterTextColumn1;
            pdfSettings.InvoiceFooterTextColumn2 = model.PdfSettings.InvoiceFooterTextColumn2;

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            _settingService.SaveSettingOverridablePerStore(pdfSettings, x => x.LetterPageSizeEnabled, model.PdfSettings.LetterPageSizeEnabled_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(pdfSettings, x => x.LogoPictureId, model.PdfSettings.LogoPictureId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(pdfSettings, x => x.DisablePdfInvoicesForPendingOrders, model.PdfSettings.DisablePdfInvoicesForPendingOrders_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(pdfSettings, x => x.InvoiceFooterTextColumn1, model.PdfSettings.InvoiceFooterTextColumn1_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(pdfSettings, x => x.InvoiceFooterTextColumn2, model.PdfSettings.InvoiceFooterTextColumn2_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            //localization settings
            var localizationSettings = _settingService.LoadSetting<LocalizationSettings>(storeScope);
            localizationSettings.UseImagesForLanguageSelection = model.LocalizationSettings.UseImagesForLanguageSelection;
            if (localizationSettings.SeoFriendlyUrlsForLanguagesEnabled != model.LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                localizationSettings.SeoFriendlyUrlsForLanguagesEnabled = model.LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled;

                //clear cached values of routes
                RouteData.Routers.ClearSeoFriendlyUrlsCachedValueForRoutes();
            }

            localizationSettings.AutomaticallyDetectLanguage = model.LocalizationSettings.AutomaticallyDetectLanguage;
            localizationSettings.LoadAllLocaleRecordsOnStartup = model.LocalizationSettings.LoadAllLocaleRecordsOnStartup;
            localizationSettings.LoadAllLocalizedPropertiesOnStartup = model.LocalizationSettings.LoadAllLocalizedPropertiesOnStartup;
            localizationSettings.LoadAllUrlRecordsOnStartup = model.LocalizationSettings.LoadAllUrlRecordsOnStartup;
            _settingService.SaveSetting(localizationSettings);

            //full-text (not overridable)
            commonSettings = _settingService.LoadSetting<CommonSettings>();
            commonSettings.FullTextMode = (FulltextSearchMode)model.FullTextSettings.SearchMode;
            _settingService.SaveSetting(commonSettings);

            //display default menu item
            var displayDefaultMenuItemSettings = _settingService.LoadSetting<DisplayDefaultMenuItemSettings>(storeScope);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            displayDefaultMenuItemSettings.DisplayHomePageMenuItem = model.DisplayDefaultMenuItemSettings.DisplayHomePageMenuItem;
            displayDefaultMenuItemSettings.DisplayNewProductsMenuItem = model.DisplayDefaultMenuItemSettings.DisplayNewProductsMenuItem;
            displayDefaultMenuItemSettings.DisplayProductSearchMenuItem = model.DisplayDefaultMenuItemSettings.DisplayProductSearchMenuItem;
            displayDefaultMenuItemSettings.DisplayCustomerInfoMenuItem = model.DisplayDefaultMenuItemSettings.DisplayCustomerInfoMenuItem;
            displayDefaultMenuItemSettings.DisplayBlogMenuItem = model.DisplayDefaultMenuItemSettings.DisplayBlogMenuItem;
            displayDefaultMenuItemSettings.DisplayForumsMenuItem = model.DisplayDefaultMenuItemSettings.DisplayForumsMenuItem;
            displayDefaultMenuItemSettings.DisplayContactUsMenuItem = model.DisplayDefaultMenuItemSettings.DisplayContactUsMenuItem;

            _settingService.SaveSettingOverridablePerStore(displayDefaultMenuItemSettings, x => x.DisplayHomePageMenuItem, model.DisplayDefaultMenuItemSettings.DisplayHomePageMenuItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultMenuItemSettings, x => x.DisplayNewProductsMenuItem, model.DisplayDefaultMenuItemSettings.DisplayNewProductsMenuItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultMenuItemSettings, x => x.DisplayProductSearchMenuItem, model.DisplayDefaultMenuItemSettings.DisplayProductSearchMenuItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultMenuItemSettings, x => x.DisplayCustomerInfoMenuItem, model.DisplayDefaultMenuItemSettings.DisplayCustomerInfoMenuItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultMenuItemSettings, x => x.DisplayBlogMenuItem, model.DisplayDefaultMenuItemSettings.DisplayBlogMenuItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultMenuItemSettings, x => x.DisplayForumsMenuItem, model.DisplayDefaultMenuItemSettings.DisplayForumsMenuItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultMenuItemSettings, x => x.DisplayContactUsMenuItem, model.DisplayDefaultMenuItemSettings.DisplayContactUsMenuItem_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            //display default footer item
            var displayDefaultFooterItemSettings = _settingService.LoadSetting<DisplayDefaultFooterItemSettings>(storeScope);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            displayDefaultFooterItemSettings.DisplaySitemapFooterItem = model.DisplayDefaultFooterItemSettings.DisplaySitemapFooterItem;
            displayDefaultFooterItemSettings.DisplayContactUsFooterItem = model.DisplayDefaultFooterItemSettings.DisplayContactUsFooterItem;
            displayDefaultFooterItemSettings.DisplayProductSearchFooterItem = model.DisplayDefaultFooterItemSettings.DisplayProductSearchFooterItem;
            displayDefaultFooterItemSettings.DisplayNewsFooterItem = model.DisplayDefaultFooterItemSettings.DisplayNewsFooterItem;
            displayDefaultFooterItemSettings.DisplayBlogFooterItem = model.DisplayDefaultFooterItemSettings.DisplayBlogFooterItem;
            displayDefaultFooterItemSettings.DisplayForumsFooterItem = model.DisplayDefaultFooterItemSettings.DisplayForumsFooterItem;
            displayDefaultFooterItemSettings.DisplayRecentlyViewedProductsFooterItem = model.DisplayDefaultFooterItemSettings.DisplayRecentlyViewedProductsFooterItem;
            displayDefaultFooterItemSettings.DisplayCompareProductsFooterItem = model.DisplayDefaultFooterItemSettings.DisplayCompareProductsFooterItem;
            displayDefaultFooterItemSettings.DisplayNewProductsFooterItem = model.DisplayDefaultFooterItemSettings.DisplayNewProductsFooterItem;
            displayDefaultFooterItemSettings.DisplayCustomerInfoFooterItem = model.DisplayDefaultFooterItemSettings.DisplayCustomerInfoFooterItem;
            displayDefaultFooterItemSettings.DisplayCustomerOrdersFooterItem = model.DisplayDefaultFooterItemSettings.DisplayCustomerOrdersFooterItem;
            displayDefaultFooterItemSettings.DisplayCustomerAddressesFooterItem = model.DisplayDefaultFooterItemSettings.DisplayCustomerAddressesFooterItem;
            displayDefaultFooterItemSettings.DisplayShoppingCartFooterItem = model.DisplayDefaultFooterItemSettings.DisplayShoppingCartFooterItem;
            displayDefaultFooterItemSettings.DisplayWishlistFooterItem = model.DisplayDefaultFooterItemSettings.DisplayWishlistFooterItem;
            displayDefaultFooterItemSettings.DisplayApplyVendorAccountFooterItem = model.DisplayDefaultFooterItemSettings.DisplayApplyVendorAccountFooterItem;

            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplaySitemapFooterItem, model.DisplayDefaultFooterItemSettings.DisplaySitemapFooterItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplayContactUsFooterItem, model.DisplayDefaultFooterItemSettings.DisplayContactUsFooterItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplayProductSearchFooterItem, model.DisplayDefaultFooterItemSettings.DisplayProductSearchFooterItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplayNewsFooterItem, model.DisplayDefaultFooterItemSettings.DisplayNewsFooterItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplayBlogFooterItem, model.DisplayDefaultFooterItemSettings.DisplayBlogFooterItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplayForumsFooterItem, model.DisplayDefaultFooterItemSettings.DisplayForumsFooterItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplayRecentlyViewedProductsFooterItem, model.DisplayDefaultFooterItemSettings.DisplayRecentlyViewedProductsFooterItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplayCompareProductsFooterItem, model.DisplayDefaultFooterItemSettings.DisplayCompareProductsFooterItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplayNewProductsFooterItem, model.DisplayDefaultFooterItemSettings.DisplayNewProductsFooterItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplayCustomerInfoFooterItem, model.DisplayDefaultFooterItemSettings.DisplayCustomerInfoFooterItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplayCustomerOrdersFooterItem, model.DisplayDefaultFooterItemSettings.DisplayCustomerOrdersFooterItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplayCustomerAddressesFooterItem, model.DisplayDefaultFooterItemSettings.DisplayCustomerAddressesFooterItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplayShoppingCartFooterItem, model.DisplayDefaultFooterItemSettings.DisplayShoppingCartFooterItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplayWishlistFooterItem, model.DisplayDefaultFooterItemSettings.DisplayWishlistFooterItem_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(displayDefaultFooterItemSettings, x => x.DisplayApplyVendorAccountFooterItem, model.DisplayDefaultFooterItemSettings.DisplayApplyVendorAccountFooterItem_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            //admin area
            var adminAreaSettings = _settingService.LoadSetting<AdminAreaSettings>(storeScope);

            //we do not clear cache after each setting update.
            //this behavior can increase performance because cached settings will not be cleared 
            //and loaded from database after each update
            adminAreaSettings.UseRichEditorInMessageTemplates = model.AdminAreaSettings.UseRichEditorInMessageTemplates;

            _settingService.SaveSettingOverridablePerStore(adminAreaSettings, x => x.UseRichEditorInMessageTemplates, model.AdminAreaSettings.UseRichEditorInMessageTemplates_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            return RedirectToAction("GeneralCommon");
        }
        [HttpPost, ActionName("GeneralCommon")]
        [FormValueRequired("changeencryptionkey")]
        public virtual IActionResult ChangeEncryptionKey(GeneralCommonSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var securitySettings = _settingService.LoadSetting<SecuritySettings>(storeScope);

            try
            {
                if (model.SecuritySettings.EncryptionKey == null)
                    model.SecuritySettings.EncryptionKey = string.Empty;

                model.SecuritySettings.EncryptionKey = model.SecuritySettings.EncryptionKey.Trim();

                var newEncryptionPrivateKey = model.SecuritySettings.EncryptionKey;
                if (string.IsNullOrEmpty(newEncryptionPrivateKey) || newEncryptionPrivateKey.Length != 16)
                    throw new NopException(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.TooShort"));

                var oldEncryptionPrivateKey = securitySettings.EncryptionKey;
                if (oldEncryptionPrivateKey == newEncryptionPrivateKey)
                    throw new NopException(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.TheSame"));

                //update encrypted order info
                var orders = _orderService.SearchOrders();
                foreach (var order in orders)
                {
                    var decryptedCardType = _encryptionService.DecryptText(order.CardType, oldEncryptionPrivateKey);
                    var decryptedCardName = _encryptionService.DecryptText(order.CardName, oldEncryptionPrivateKey);
                    var decryptedCardNumber = _encryptionService.DecryptText(order.CardNumber, oldEncryptionPrivateKey);
                    var decryptedMaskedCreditCardNumber = _encryptionService.DecryptText(order.MaskedCreditCardNumber, oldEncryptionPrivateKey);
                    var decryptedCardCvv2 = _encryptionService.DecryptText(order.CardCvv2, oldEncryptionPrivateKey);
                    var decryptedCardExpirationMonth = _encryptionService.DecryptText(order.CardExpirationMonth, oldEncryptionPrivateKey);
                    var decryptedCardExpirationYear = _encryptionService.DecryptText(order.CardExpirationYear, oldEncryptionPrivateKey);

                    var encryptedCardType = _encryptionService.EncryptText(decryptedCardType, newEncryptionPrivateKey);
                    var encryptedCardName = _encryptionService.EncryptText(decryptedCardName, newEncryptionPrivateKey);
                    var encryptedCardNumber = _encryptionService.EncryptText(decryptedCardNumber, newEncryptionPrivateKey);
                    var encryptedMaskedCreditCardNumber = _encryptionService.EncryptText(decryptedMaskedCreditCardNumber, newEncryptionPrivateKey);
                    var encryptedCardCvv2 = _encryptionService.EncryptText(decryptedCardCvv2, newEncryptionPrivateKey);
                    var encryptedCardExpirationMonth = _encryptionService.EncryptText(decryptedCardExpirationMonth, newEncryptionPrivateKey);
                    var encryptedCardExpirationYear = _encryptionService.EncryptText(decryptedCardExpirationYear, newEncryptionPrivateKey);

                    order.CardType = encryptedCardType;
                    order.CardName = encryptedCardName;
                    order.CardNumber = encryptedCardNumber;
                    order.MaskedCreditCardNumber = encryptedMaskedCreditCardNumber;
                    order.CardCvv2 = encryptedCardCvv2;
                    order.CardExpirationMonth = encryptedCardExpirationMonth;
                    order.CardExpirationYear = encryptedCardExpirationYear;
                    _orderService.UpdateOrder(order);
                }

                //update password information
                //optimization - load only passwords with PasswordFormat.Encrypted
                var customerPasswords = _customerService.GetCustomerPasswords(passwordFormat: PasswordFormat.Encrypted);
                foreach (var customerPassword in customerPasswords)
                {
                    var decryptedPassword = _encryptionService.DecryptText(customerPassword.Password, oldEncryptionPrivateKey);
                    var encryptedPassword = _encryptionService.EncryptText(decryptedPassword, newEncryptionPrivateKey);

                    customerPassword.Password = encryptedPassword;
                    _customerService.UpdateCustomerPassword(customerPassword);
                }

                securitySettings.EncryptionKey = newEncryptionPrivateKey;
                _settingService.SaveSetting(securitySettings);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.Changed"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }

            return RedirectToAction("GeneralCommon");
        }
        [HttpPost, ActionName("GeneralCommon")]
        [FormValueRequired("togglefulltext")]
        public virtual IActionResult ToggleFullText(GeneralCommonSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var commonSettings = _settingService.LoadSetting<CommonSettings>(storeScope);
            try
            {
                if (!_fulltextService.IsFullTextSupported())
                    throw new NopException(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.FullTextSettings.NotSupported"));

                if (commonSettings.UseFullTextSearch)
                {
                    _fulltextService.DisableFullText();

                    commonSettings.UseFullTextSearch = false;
                    _settingService.SaveSetting(commonSettings);

                    SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.FullTextSettings.Disabled"));
                }
                else
                {
                    _fulltextService.EnableFullText();

                    commonSettings.UseFullTextSearch = true;
                    _settingService.SaveSetting(commonSettings);

                    SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.FullTextSettings.Enabled"));
                }
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }

            return RedirectToAction("GeneralCommon");
        }

        public virtual IActionResult AllSettings()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _settingModelFactory.PrepareSettingSearchModel(new SettingSearchModel());

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult AllSettings(SettingSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _settingModelFactory.PrepareSettingListModel(searchModel);

            return Json(model);
        }
        [HttpPost]
        public virtual IActionResult SettingUpdate(SettingModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            if (model.Name != null)
                model.Name = model.Name.Trim();

            if (model.Value != null)
                model.Value = model.Value.Trim();

            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            //try to get a setting with the specified id
            var setting = _settingService.GetSettingById(model.Id)
                ?? throw new ArgumentException("No setting found with the specified id");

            var storeId = model.StoreId;

            if (!setting.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase) ||
                setting.StoreId != storeId)
            {
                //setting name or store has been changed
                _settingService.DeleteSetting(setting);
            }

            _settingService.SetSetting(model.Name, model.Value, storeId);

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"), setting);

            return new NullJsonResult();
        }
        [HttpPost]
        public virtual IActionResult SettingAdd(SettingModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            if (model.Name != null)
                model.Name = model.Name.Trim();

            if (model.Value != null)
                model.Value = model.Value.Trim();

            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            var storeId = model.StoreId;
            _settingService.SetSetting(model.Name, model.Value, storeId);

            //activity log
            _customerActivityService.InsertActivity("AddNewSetting",
                string.Format(_localizationService.GetResource("ActivityLog.AddNewSetting"), model.Name),
                _settingService.GetSetting(model.Name, storeId));

            return new NullJsonResult();
        }
        [HttpPost]
        public virtual IActionResult SettingDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a setting with the specified id
            var setting = _settingService.GetSettingById(id)
                ?? throw new ArgumentException("No setting found with the specified id", nameof(id));

            _settingService.DeleteSetting(setting);

            //activity log
            _customerActivityService.InsertActivity("DeleteSetting",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteSetting"), setting.Name), setting);

            return new NullJsonResult();
        }

        //action displaying notification (warning) to a store owner about a lot of traffic 
        //between the Redis server and the application when LoadAllLocaleRecordsOnStartup setting is set
        public IActionResult RedisCacheHighTrafficWarning(bool loadAllLocaleRecordsOnStartup)
        {
            //LoadAllLocaleRecordsOnStartup is set and Redis cache is used, so display warning
            if (_config.RedisCachingEnabled && loadAllLocaleRecordsOnStartup)
                return Json(new
                {
                    Result = _localizationService.GetResource(
                        "Admin.Configuration.Settings.GeneralCommon.LoadAllLocaleRecordsOnStartup.Warning")
                });

            return Json(new { Result = string.Empty });
        }

        #endregion
    }
}