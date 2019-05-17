﻿using System;
using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Core;
using NCSw.HERO.Core.Domain.Messages;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Services.Messages;
using NCSw.HERO.Web.Factories;
using NCSw.HERO.Web.Framework.Mvc.Filters;

namespace NCSw.HERO.Web.Controllers
{
    public partial class NewsletterController : BasePublicController
    {
        private readonly ILocalizationService _localizationService;
        private readonly INewsletterModelFactory _newsletterModelFactory;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly IWorkflowMessageService _workflowMessageService;

        public NewsletterController(ILocalizationService localizationService,
            INewsletterModelFactory newsletterModelFactory,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IStoreContext storeContext,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService)
        {
            this._localizationService = localizationService;
            this._newsletterModelFactory = newsletterModelFactory;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._storeContext = storeContext;
            this._workContext = workContext;
            this._workflowMessageService = workflowMessageService;
        }

        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        [HttpPost]
        public virtual IActionResult SubscribeNewsletter(string email, bool subscribe)
        {
            string result;
            var success = false;

            if (!CommonHelper.IsValidEmail(email))
            {
                result = _localizationService.GetResource("Newsletter.Email.Wrong");
            }
            else
            {
                email = email.Trim();

                var subscription = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(email, _storeContext.CurrentStore.Id);
                if (subscription != null)
                {
                    if (subscribe)
                    {
                        if (!subscription.Active)
                        {
                            _workflowMessageService.SendNewsLetterSubscriptionActivationMessage(subscription, _workContext.WorkingLanguage.Id);
                        }
                        result = _localizationService.GetResource("Newsletter.SubscribeEmailSent");
                    }
                    else
                    {
                        if (subscription.Active)
                        {
                            _workflowMessageService.SendNewsLetterSubscriptionDeactivationMessage(subscription, _workContext.WorkingLanguage.Id);
                        }
                        result = _localizationService.GetResource("Newsletter.UnsubscribeEmailSent");
                    }
                }
                else if (subscribe)
                {
                    subscription = new NewsLetterSubscription
                    {
                        NewsLetterSubscriptionGuid = Guid.NewGuid(),
                        Email = email,
                        Active = false,
                        StoreId = _storeContext.CurrentStore.Id,
                        CreatedOnUtc = DateTime.UtcNow
                    };
                    _newsLetterSubscriptionService.InsertNewsLetterSubscription(subscription);
                    _workflowMessageService.SendNewsLetterSubscriptionActivationMessage(subscription, _workContext.WorkingLanguage.Id);

                    result = _localizationService.GetResource("Newsletter.SubscribeEmailSent");
                }
                else
                {
                    result = _localizationService.GetResource("Newsletter.UnsubscribeEmailSent");
                }
                success = true;
            }

            return Json(new
            {
                Success = success,
                Result = result,
            });
        }

        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        public virtual IActionResult SubscriptionActivation(Guid token, bool active)
        {
            var subscription = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByGuid(token);
            if (subscription == null)
                return RedirectToRoute("HomePage");

            if (active)
            {
                subscription.Active = true;
                _newsLetterSubscriptionService.UpdateNewsLetterSubscription(subscription);
            }
            else
                _newsLetterSubscriptionService.DeleteNewsLetterSubscription(subscription);

            var model = _newsletterModelFactory.PrepareSubscriptionActivationModel(active);
            return View(model);
        }
    }
}