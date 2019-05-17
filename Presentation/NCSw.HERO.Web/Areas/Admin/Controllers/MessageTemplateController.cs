﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Core.Domain.Messages;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Services.Logging;
using NCSw.HERO.Services.Messages;
using NCSw.HERO.Services.Security;
using NCSw.HERO.Services.Stores;
using NCSw.HERO.Web.Areas.Admin.Factories;
using NCSw.HERO.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using NCSw.HERO.Web.Areas.Admin.Models.Messages;
using NCSw.HERO.Web.Framework.Controllers;
using NCSw.HERO.Web.Framework.Mvc.Filters;

namespace NCSw.HERO.Web.Areas.Admin.Controllers
{
    public partial class MessageTemplateController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IMessageTemplateModelFactory _messageTemplateModelFactory;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IPermissionService _permissionService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IStoreService _storeService;
        private readonly IWorkflowMessageService _workflowMessageService;

        #endregion Fields

        #region Ctor

        public MessageTemplateController(ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IMessageTemplateModelFactory messageTemplateModelFactory,
            IMessageTemplateService messageTemplateService,
            IPermissionService permissionService,
            IStoreMappingService storeMappingService,
            IStoreService storeService,
            IWorkflowMessageService workflowMessageService)
        {
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._messageTemplateModelFactory = messageTemplateModelFactory;
            this._messageTemplateService = messageTemplateService;
            this._permissionService = permissionService;
            this._storeMappingService = storeMappingService;
            this._storeService = storeService;
            this._workflowMessageService = workflowMessageService;
        }

        #endregion

        #region Utilities

        protected virtual void UpdateLocales(MessageTemplate mt, MessageTemplateModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(mt,
                    x => x.BccEmailAddresses,
                    localized.BccEmailAddresses,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(mt,
                    x => x.Subject,
                    localized.Subject,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(mt,
                    x => x.Body,
                    localized.Body,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(mt,
                    x => x.EmailAccountId,
                    localized.EmailAccountId,
                    localized.LanguageId);
            }
        }

        protected virtual void SaveStoreMappings(MessageTemplate messageTemplate, MessageTemplateModel model)
        {
            messageTemplate.LimitedToStores = model.SelectedStoreIds.Any();

            var existingStoreMappings = _storeMappingService.GetStoreMappings(messageTemplate);
            var allStores = _storeService.GetAllStores();
            foreach (var store in allStores)
            {
                if (model.SelectedStoreIds.Contains(store.Id))
                {
                    //new store
                    if (existingStoreMappings.Count(sm => sm.StoreId == store.Id) == 0)
                        _storeMappingService.InsertStoreMapping(messageTemplate, store.Id);
                }
                else
                {
                    //remove store
                    var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
                    if (storeMappingToDelete != null)
                        _storeMappingService.DeleteStoreMapping(storeMappingToDelete);
                }
            }
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            //prepare model
            var model = _messageTemplateModelFactory.PrepareMessageTemplateSearchModel(new MessageTemplateSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(MessageTemplateSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _messageTemplateModelFactory.PrepareMessageTemplateListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            //try to get a message template with the specified id
            var messageTemplate = _messageTemplateService.GetMessageTemplateById(id);
            if (messageTemplate == null)
                return RedirectToAction("List");

            //prepare model
            var model = _messageTemplateModelFactory.PrepareMessageTemplateModel(null, messageTemplate);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(MessageTemplateModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            //try to get a message template with the specified id
            var messageTemplate = _messageTemplateService.GetMessageTemplateById(model.Id);
            if (messageTemplate == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                messageTemplate = model.ToEntity(messageTemplate);

                //attached file
                if (!model.HasAttachedDownload)
                    messageTemplate.AttachedDownloadId = 0;
                if (model.SendImmediately)
                    messageTemplate.DelayBeforeSend = null;
                _messageTemplateService.UpdateMessageTemplate(messageTemplate);

                //activity log
                _customerActivityService.InsertActivity("EditMessageTemplate",
                    string.Format(_localizationService.GetResource("ActivityLog.EditMessageTemplate"), messageTemplate.Id), messageTemplate);

                //stores
                SaveStoreMappings(messageTemplate, model);

                //locales
                UpdateLocales(messageTemplate, model);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                return RedirectToAction("Edit", new { id = messageTemplate.Id });
            }

            //prepare model
            model = _messageTemplateModelFactory.PrepareMessageTemplateModel(model, messageTemplate, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            //try to get a message template with the specified id
            var messageTemplate = _messageTemplateService.GetMessageTemplateById(id);
            if (messageTemplate == null)
                return RedirectToAction("List");

            _messageTemplateService.DeleteMessageTemplate(messageTemplate);

            //activity log
            _customerActivityService.InsertActivity("DeleteMessageTemplate",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteMessageTemplate"), messageTemplate.Id), messageTemplate);

            SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("message-template-copy")]
        public virtual IActionResult CopyTemplate(MessageTemplateModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            //try to get a message template with the specified id
            var messageTemplate = _messageTemplateService.GetMessageTemplateById(model.Id);
            if (messageTemplate == null)
                return RedirectToAction("List");

            try
            {
                var newMessageTemplate = _messageTemplateService.CopyMessageTemplate(messageTemplate);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Copied"));

                return RedirectToAction("Edit", new { id = newMessageTemplate.Id });
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = model.Id });
            }
        }

        public virtual IActionResult TestTemplate(int id, int languageId = 0)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            //try to get a message template with the specified id
            var messageTemplate = _messageTemplateService.GetMessageTemplateById(id);
            if (messageTemplate == null)
                return RedirectToAction("List");

            //prepare model
            var model = _messageTemplateModelFactory
                .PrepareTestMessageTemplateModel(new TestMessageTemplateModel(), messageTemplate, languageId);

            return View(model);
        }

        [HttpPost, ActionName("TestTemplate")]
        [FormValueRequired("send-test")]
        public virtual IActionResult TestTemplate(TestMessageTemplateModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMessageTemplates))
                return AccessDeniedView();

            //try to get a message template with the specified id
            var messageTemplate = _messageTemplateService.GetMessageTemplateById(model.Id);
            if (messageTemplate == null)
                return RedirectToAction("List");

            var tokens = new List<Token>();
            var form = model.Form;
            foreach (var formKey in form.Keys)
                if (formKey.StartsWith("token_", StringComparison.InvariantCultureIgnoreCase))
                {
                    var tokenKey = formKey.Substring("token_".Length).Replace("%", string.Empty);
                    var stringValue = form[formKey].ToString();

                    //try get non-string value
                    object tokenValue;
                    if (bool.TryParse(stringValue, out var boolValue))
                        tokenValue = boolValue;
                    else if (int.TryParse(stringValue, out var intValue))
                        tokenValue = intValue;
                    else if (decimal.TryParse(stringValue, out var decimalValue))
                        tokenValue = decimalValue;
                    else
                        tokenValue = stringValue;

                    tokens.Add(new Token(tokenKey, tokenValue));
                }

            _workflowMessageService.SendTestEmail(messageTemplate.Id, model.SendTo, tokens, model.LanguageId);

            if (ModelState.IsValid)
            {
                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Test.Success"));
            }

            return RedirectToAction("Edit", new { id = messageTemplate.Id });
        }

        #endregion
    }
}