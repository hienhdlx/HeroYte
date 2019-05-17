using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Services.Security;
using NCSw.HERO.Web.Areas.Admin.Factories;
using NCSw.HERO.Web.Areas.Admin.Models;
using NCSw.HERO.Web.Areas.Admin.Models.Customers;
using NCSw.HERO.Core.Domain;
using NCSw.HERO.Services;
using NCSw.HERO.Web.Framework.Mvc.Filters;
using NCSw.HERO.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using NCSw.HERO.Core.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using NCSw.HERO.Core;

namespace NCSw.HERO.Web.Areas.Admin.Controllers
{
    public partial class DepartmentController : BaseAdminController
    {
        #region Fields
        private readonly IDepartmentModelFactory _departmentModelFactory;
        private readonly IDepartmentService _departmentService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor

        public DepartmentController(
            IDepartmentModelFactory departmentModelFactory,
            IDepartmentService departmentService,
            ILocalizedEntityService localizedEntityService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext)
        {
            _departmentModelFactory = departmentModelFactory;
            _departmentService = departmentService;
            _localizedEntityService = localizedEntityService;
            _localizationService = localizationService;
            _permissionService = permissionService;
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
            var model = _departmentModelFactory.PrepareSearchModel(new DepartmentSearchModel());
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(DepartmentSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageServices))
                return AccessDeniedKendoGridJson();
            //prepare model
            var model = _departmentModelFactory.PrepareListModel(searchModel);
            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult DepartmentList(DepartmentSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageServices))
                return AccessDeniedKendoGridJson();
            //prepare model
            var model = _departmentModelFactory.PrepareListModel(searchModel);
            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();
            //prepare model
            var model = _departmentModelFactory.PrepareModel(new DepartmentModel(), null);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(DepartmentModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();
            model.HospitalId = _workContext.HospitalId;
            if (ModelState.IsValid)
            {
                model.UpdatedOnUtc = DateTime.UtcNow;
                model.CreatedOnUtc = DateTime.UtcNow;
                model.CreatedBy = _workContext.CurrentCustomer.Id;
                model.UpdatedBy = _workContext.CurrentCustomer.Id;
                var department = model.ToEntity<Department>();
                if(department.Path == null || department.Path == "0")
                {
                    department.Path = string.Empty;
                    department.ParentId = 0;
                }
                else
                {
                    var listPath = department.Path.Split(",").ToList();
                    int.TryParse(listPath.LastOrDefault(), out int j);
                    department.ParentId = j;
                }
                _departmentService.Insert(department);
                
                //update locale
                _departmentModelFactory.UpdateLocales(department, model);
                //activity log
                //_customerActivityService.InsertActivity("AddNewCustomerRole",
                //    string.Format(_localizationService.GetResource("ActivityLog.AddNewCustomerRole"), customerRole.Name), customerRole);

                SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerRoles.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = department.Id }) : RedirectToAction("List");
            }
            //prepare model
            model = _departmentModelFactory.PrepareModel(model, null, true);
            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();
            //try to get a customer role with the specified id
            var department = _departmentService.GetById(id);
            if (department == null)
                return RedirectToAction("List");
            //prepare model
            var model = _departmentModelFactory.PrepareModel(null, department);
            if(model.Path == string.Empty)
            {
                model.Path = "0";
            }
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(DepartmentModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();
            //try to get a customer role with the specified id
            var department = _departmentService.GetById(model.Id);
            if (department == null)
                return RedirectToAction("List");
            try
            {
                if (ModelState.IsValid)
                {
                    department = model.ToEntity(department);
                    department.UpdatedOnUtc = DateTime.UtcNow;
                    department.UpdatedBy = _workContext.CurrentCustomer.Id;
                    department.HospitalId = _workContext.HospitalId;

                    if(department.Path != "0")
                    {
                        var listPath = department.Path.Split(",").ToList();
                        int.TryParse(listPath.LastOrDefault(), out int j);
                        department.ParentId = j;
                    }
                    else
                    {
                        department.Path = string.Empty;
                    }
                    _departmentService.Update(department);

                    //update locale
                    _departmentModelFactory.UpdateLocales(department, model);
                    SuccessNotification(_localizationService.GetResource("Admin.Department.Updated"));
                    return continueEditing ? RedirectToAction("Edit", new { id = department.Id }) : RedirectToAction("List");
                }

                //prepare model
                model = _departmentModelFactory.PrepareModel(model, department);
                if (model.Path == string.Empty)
                {
                    model.Path = "0";
                }
                //if we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = department.Id });
            }
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();
            //try to get a customer role with the specified id
            var department = _departmentService.GetById(id);
            if (department == null)
                return RedirectToAction("List");
            try
            {
                _departmentService.Delete(department);
                SuccessNotification(_localizationService.GetResource("Admin.Department.Deleted"));
                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", id);
            }
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageServices))
                return AccessDeniedView();

            if (selectedIds != null)
            {
                _departmentService.Deletes(_departmentService.GetByIds(selectedIds.ToArray()));
            }

            return Json(new { Result = true });
        }

        #endregion

        #region Utilities


        #endregion
    }
}