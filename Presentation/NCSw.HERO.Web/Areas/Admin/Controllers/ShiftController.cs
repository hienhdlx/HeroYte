using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Core;
using NCSw.HERO.Core.Domain;
using NCSw.HERO.Services;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Services.Logging;
using NCSw.HERO.Services.Security;
using NCSw.HERO.Web.Areas.Admin.Factories;
using NCSw.HERO.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using NCSw.HERO.Web.Areas.Admin.Models;
using NCSw.HERO.Web.Framework.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NCSw.HERO.Web.Areas.Admin.Controllers
{
    public partial class ShiftController : BaseAdminController
    {
        #region Const


        #endregion

        #region Fields

        private readonly IShiftModelFactory _shiftModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IShiftService _shiftService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public ShiftController(
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IShiftModelFactory shiftModelFactory,
            ICustomerActivityService customerActivityService,
            IShiftService shiftService,
            IWorkContext workContext)
        {
            _localizationService = localizationService;
            _permissionService = permissionService;
            _shiftModelFactory = shiftModelFactory;
            _customerActivityService = customerActivityService;
            _shiftService = shiftService;
            _workContext = workContext;
        }

        #endregion

        #region Utilities

        #endregion

        #region Actions

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShifts))
                return AccessDeniedView();

            //prepare model
            var model = _shiftModelFactory.PrepareShiftSearchModel(new ShiftSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ShiftSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShifts))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _shiftModelFactory.PrepareShiftListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if(!_permissionService.Authorize(StandardPermissionProvider.ManageShifts))
                return AccessDeniedView();

            //prepare model
            var model = _shiftModelFactory.PrepareShiftModel(new ShiftModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ShiftModel model, bool continueEditing) 
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShifts))
                return AccessDeniedView();

            if (!string.IsNullOrWhiteSpace(model.Code) && _shiftService.GetShift(w => w.Code == model.Code) != null)
                ModelState.AddModelError(string.Empty, _localizationService.GetResource("Hero.Admin.Shifts.CodeIsRegister"));


            if (ModelState.IsValid)
            {
                var shift = model.ToEntity<Shift>();
                shift.CreatedOnUtc = DateTime.UtcNow;
                shift.CreatedBy = _workContext.CurrentCustomer.Id;
                shift.UpdatedBy = null;
                shift.UpdatedOnUtc = null;
                _shiftService.InsertShift(shift);

                //activity log
                _customerActivityService.InsertActivity("AddNewShift",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewShift"), shift.Id), shift);

                SuccessNotification(_localizationService.GetResource("Hero.Admin.Shifts.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = shift.Id });
                       
            }

            //prepare model
            model = _shiftModelFactory.PrepareShiftModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShifts))
                return AccessDeniedView();

            //try to get a customer role with the specified id
            var shift = _shiftService.GetShiftById(id);
            if (shift == null)
                return RedirectToAction("List");

            //prepare model
            var model = _shiftModelFactory.PrepareShiftModel(null, shift);

            return View(model);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(ShiftModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDoctors))
                return AccessDeniedView();

            //get a specified id shift
            var shift = _shiftService.GetShiftById(model.Id);
            if (shift == null)
                return RedirectToAction("List");


            try
            {
                if (ModelState.IsValid)
                {
                    shift = model.ToEntity(shift);
                    shift.UpdatedOnUtc = DateTime.UtcNow;
                    shift.UpdatedBy= _workContext.CurrentCustomer.Id;
                    _shiftService.UpdateShift(shift);
                    //activity log
                    _customerActivityService.InsertActivity("EditShift",
                        string.Format(_localizationService.GetResource("ActivityLog.EditShift"), shift.Name), shift);

                    SuccessNotification(_localizationService.GetResource("Hero.Admin.Shifts.Updated"));

                    return continueEditing ? RedirectToAction("Edit", new { id = shift.Id }) : RedirectToAction("List");
                }

                //prepare model
                model = _shiftModelFactory.PrepareShiftModel(model, shift, true);

                return View(model);
            }
            catch(Exception exc)
            {
                ErrorNotification(exc);

                return RedirectToAction("Edit", new { id = shift.Id });
            }
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShifts))
                return AccessDeniedView();

            //try to get a customer role with the specified id
            var shift = _shiftService.GetShiftById(id);
            if (shift == null)
                return RedirectToAction("List");

            try
            {
                _shiftService.DeleteShift(shift);

                //activity log
                _customerActivityService.InsertActivity("DeleteShift",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteShift"), shift.Name), shift);

                SuccessNotification(_localizationService.GetResource("Hero.Admin.Shifts.Deleted"));

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = shift.Id });
            }
        }
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageServices))
                return AccessDeniedView();
            if(selectedIds != null)
            {
                _shiftService.DeleteShift(_shiftService.GetByIds(selectedIds.ToArray()));
            }

            return Json(new { Result = true });
        }


        #endregion
    }
}