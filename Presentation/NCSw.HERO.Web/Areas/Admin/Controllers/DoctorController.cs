using Microsoft.AspNetCore.Mvc;
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
    public partial class DoctorController : BaseAdminController
    {
        #region Const


        #endregion

        #region Fields

        private readonly IDoctorModelFactory _doctorModelFactory;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IDoctorService _doctorService;
        private readonly ICustomerActivityService _customerActivityService;

        #endregion

        #region Ctor

        public DoctorController(
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IDoctorModelFactory doctorModelFactory,
            ICustomerActivityService customerActivityService,
            IDoctorService doctorService)
        {
            _localizationService = localizationService;
            _permissionService = permissionService;
            _doctorModelFactory = doctorModelFactory;
            _customerActivityService = customerActivityService;
            _doctorService = doctorService;
        }

        #endregion

        #region Actions

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDoctors))
                return AccessDeniedView();

            //prepare model
            var model = _doctorModelFactory.PrepareDoctorSearchModel(new DoctorSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(DoctorSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDoctors))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _doctorModelFactory.PrepareDoctorListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDoctors))
                return AccessDeniedView();

            //prepare model
            var model = _doctorModelFactory.PrepareDoctorModel(new DoctorModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(DoctorModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDoctors))
                return AccessDeniedView();

            var doctorsError = ValidateDoctors(model);
            if (!string.IsNullOrEmpty(doctorsError))
            {
                ModelState.AddModelError(string.Empty, doctorsError);
                ErrorNotification(doctorsError, false);
            }

            if (ModelState.IsValid)
            {
                var doctor = model.ToEntity<Doctor>();
                _doctorService.InsertDoctor(doctor);

                //activity log
                _customerActivityService.InsertActivity("AddNewDoctor",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewDoctor"), doctor.Id), doctor);

                SuccessNotification(_localizationService.GetResource("Hero.Admin.Doctors.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = doctor.Id });
            }

            //prepare model
            model = _doctorModelFactory.PrepareDoctorModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDoctors))
                return AccessDeniedView();

            //try to get a customer role with the specified id
            var doctor = _doctorService.GetDoctorById(id);
            if (doctor == null)
                return RedirectToAction("List");

            //prepare model
            var model = _doctorModelFactory.PrepareDoctorModel(null, doctor);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(DoctorModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDoctors))
                return AccessDeniedView();

            //try to get a doctor with the specified id
            var doctor = _doctorService.GetDoctorById(model.Id);

            if (doctor == null)
                return RedirectToAction("List");

            var doctorsError = ValidateDoctors(model);
            if (!string.IsNullOrEmpty(doctorsError))
            {
                ModelState.AddModelError(string.Empty, doctorsError);
                ErrorNotification(doctorsError, false);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    doctor = model.ToEntity(doctor);
                    _doctorService.UpdateDoctor(doctor);

                    //activity log
                    _customerActivityService.InsertActivity("EditDoctor",
                        string.Format(_localizationService.GetResource("ActivityLog.EditDoctor"), doctor.FullName), doctor);

                    SuccessNotification(_localizationService.GetResource("Hero.Admin.Doctors.Updated"));

                    return continueEditing ? RedirectToAction("Edit", new { id = doctor.Id }) : RedirectToAction("List");
                }

                //prepare model
                model = _doctorModelFactory.PrepareDoctorModel(model, doctor, true);

                //if we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = doctor.Id });
            }
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            //try to get a customer role with the specified id
            var doctor = _doctorService.GetDoctorById(id);
            if (doctor == null)
                return RedirectToAction("List");

            try
            {
                _doctorService.DeleteDoctor(doctor);

                //activity log
                _customerActivityService.InsertActivity("DeleteDoctor",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteDoctor"), doctor.FullName), doctor);

                SuccessNotification(_localizationService.GetResource("Hero.Admin.Doctors.Deleted"));

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = doctor.Id });
            }
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageServices))
                return AccessDeniedView();

            if (selectedIds != null)
            {
                _doctorService.DeleteDoctors(_doctorService.GetDoctorByIds(selectedIds.ToArray()));
            }

            return Json(new { Result = true });
        }
        #endregion

        #region Utilities

        /// <summary>
        /// Validations
        /// </summary>
        /// <param name="model">Doctor model</param>
        public virtual string ValidateDoctors(DoctorModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            string validator = "";

            //check duplicate value
            if (model.Id > 0)
            {
                if (!string.IsNullOrWhiteSpace(model.Code) &&
                _doctorService.GetDoctor(w => w.Code == model.Code && w.Id != model.Id) != null)
                    validator += $@"{string.Format(_localizationService.GetResource("Hero.Admin.InputFields.IsRegistered"), _localizationService.GetResource("Hero.Admin.Doctors.Fields.Code"))} ";
                if (!string.IsNullOrWhiteSpace(model.IdentityCard) &&
                    _doctorService.GetDoctor(w => w.IdentityCard == model.IdentityCard && w.Id != model.Id) != null)
                    validator += $@"{string.Format(_localizationService.GetResource("Hero.Admin.InputFields.IsRegistered"), _localizationService.GetResource("Hero.Admin.Doctors.Fields.IdentityCard"))} ";
                if (!string.IsNullOrWhiteSpace(model.Email) &&
                    _doctorService.GetDoctor(w => w.Email == model.Email && w.Id != model.Id) != null)
                    validator += $@"{string.Format(_localizationService.GetResource("Hero.Admin.InputFields.IsRegistered"), _localizationService.GetResource("Hero.Admin.Doctors.Fields.Email"))} ";
                if (!string.IsNullOrWhiteSpace(model.PhoneNumber) &&
                    _doctorService.GetDoctor(w => w.PhoneNumber == model.PhoneNumber && w.Id != model.Id) != null)
                    validator += $@"{string.Format(_localizationService.GetResource("Hero.Admin.InputFields.IsRegistered"), _localizationService.GetResource("Hero.Admin.Doctors.Fields.PhoneNumber"))} ";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(model.Code) &&
                _doctorService.GetDoctor(w => w.Code == model.Code) != null)
                    validator += $@"{string.Format(_localizationService.GetResource("Hero.Admin.InputFields.IsRegistered"), _localizationService.GetResource("Hero.Admin.Doctors.Fields.Code"))} ";
                if (!string.IsNullOrWhiteSpace(model.IdentityCard) &&
                    _doctorService.GetDoctor(w => w.IdentityCard == model.IdentityCard) != null)
                    validator += $@"{string.Format(_localizationService.GetResource("Hero.Admin.InputFields.IsRegistered"), _localizationService.GetResource("Hero.Admin.Doctors.Fields.IdentityCard"))} ";
                if (!string.IsNullOrWhiteSpace(model.Email) &&
                    _doctorService.GetDoctor(w => w.Email == model.Email) != null)
                    validator += $@"{string.Format(_localizationService.GetResource("Hero.Admin.InputFields.IsRegistered"), _localizationService.GetResource("Hero.Admin.Doctors.Fields.Email"))} ";
                if (!string.IsNullOrWhiteSpace(model.PhoneNumber) &&
                    _doctorService.GetDoctor(w => w.PhoneNumber == model.PhoneNumber) != null)
                    validator += $@"{string.Format(_localizationService.GetResource("Hero.Admin.InputFields.IsRegistered"), _localizationService.GetResource("Hero.Admin.Doctors.Fields.PhoneNumber"))} ";
            }
            //end

            if (!string.IsNullOrEmpty(validator))
                return validator;

            return string.Empty;
        }
        #endregion
    }
}