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
    public partial class DepartmentModelFactory : IDepartmentModelFactory
    {
        #region Fields

        private readonly IDepartmentService _departmentService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public DepartmentModelFactory(
            IDepartmentService DepartmentService,
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            ILocalizedEntityService localizedEntityService,
            IWorkContext workContext)
        {
            _departmentService = DepartmentService;
            _localizationService = localizationService;
            _localizedModelFactory = localizedModelFactory;
            _localizedEntityService = localizedEntityService;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        public virtual DepartmentSearchModel PrepareSearchModel(DepartmentSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual DepartmentListModel PrepareListModel(DepartmentSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            var entities = _departmentService.GetAll(name: searchModel.SearchName,
                                                    pageIndex: searchModel.Page - 1,
                                                    pageSize: searchModel.PageSize,
                                                    showHidden: true);
            //Get follow Path
            foreach (var template in entities)
            {
                var templateName = "";
                if (template.Path != string.Empty)
                {
                    var listPath = template.Path.Split(",").ToList();
                    List<int> listPathInt = listPath.Select(s => int.Parse(s)).ToList();
                    foreach (var listPathItem in listPathInt)
                    {
                        foreach (var templateItem in entities)
                        {
                            if (listPathItem == templateItem.Id)
                            {
                                templateName += templateItem.Name + " >> ";
                            }
                        }
                    }
                }
                if(templateName != string.Empty)
                {
                    templateName = templateName + template.Name;
                }
                else
                {
                    templateName = template.Name;
                }
                template.PathName = templateName;
            }
            
            //prepare list model
            var model = new DepartmentListModel
            {
                //fill in model values from the entity
                Data = entities.Select(x => x.ToModel<DepartmentModel>()),
                Total = entities.Count
            };
            return model;
        }

        public virtual DepartmentModel PrepareModel(DepartmentModel model, Department service, bool excludeProperties = false)
        {
            Action<DepartmentLocalizedModel, int> localizedModelConfiguration = null;

            if (service != null)
            {
                //fill in model values from the entity
                model = model ?? service.ToModel<DepartmentModel>();

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(service, entity => entity.Name, languageId, false, false);
                    locale.Description = _localizationService.GetLocalized(service, entity => entity.Description, languageId, false, false);
                    locale.Address = _localizationService.GetLocalized(service, entity => entity.Address, languageId, false, false);
                };
            }

            //set default values for the new model
            if (service == null)
            {
                model.Active = true;
                model.Deleted = false;
                model.CreatedBy = _workContext.CurrentCustomer.Id;
                model.CreatedOnUtc = DateTime.UtcNow;
                PrepareDepartmentTemplatesForCreate(model.DepartmentListTemplates, false, null);
            }
            else
            {
                //prepare available category templates
                PrepareDepartmentTemplatesForEdit(model.DepartmentListTemplates, false, null);

                model.DepartmentListTemplates = model.DepartmentListTemplates.GroupBy(s => s.Value, i => i, (k, item) => new SelectListItem
                {
                    Text = item.First().Text,
                    Value = k,
                    Selected = item.First().Selected

                }).ToList();
            }

            //prepare localized models
            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);
            
            return model;
        }

        public void PrepareDepartmentTemplatesForEdit(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available category templates
            var availableDepTemplates = _departmentService.GetAll(languageId: 0, showHidden: false);
            items.Add(new SelectListItem()
            {
                Text = _localizationService.GetResource("Admin.Department.Select2.EmptyItem"),
                Value = "0"
            });
            //Get follow Path
            foreach (var template in availableDepTemplates)
            {
                var templateName = "";
                if(template.Path != string.Empty)
                {
                    var listPath = template.Path.Split(",").ToList();
                    List<int> listPathInt = listPath.Select(s => int.Parse(s)).ToList();
                    foreach (var listPathItem in listPathInt)
                    {
                        foreach (var templateItem in availableDepTemplates)
                        {
                            if (listPathItem == templateItem.Id)
                            {
                                templateName += templateItem.Name + " >> ";
                            }
                        }
                    }
                }
                if (templateName != string.Empty)
                {
                    templateName = templateName.Substring(0, templateName.Length - 4);
                    //templateName = templateName + template.Name;
                }
                else
                {
                    templateName = template.Name;
                    template.Path = template.Id.ToString();
                }
                items.Add(new SelectListItem { Value = template.Path.ToString(), Text = templateName });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
            
        }

        public void PrepareDepartmentTemplatesForCreate(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available category templates
            var availableDepTemplates = _departmentService.GetAll(languageId: 0, showHidden: false);
            items.Add(new SelectListItem()
            {
                Text = _localizationService.GetResource("Admin.Department.Select2.EmptyItem"),
                Value = "0"
            });
            //Get follow Path
            foreach (var template in availableDepTemplates)
            {
                var templateName = "";
                if (template.Path != string.Empty)
                {
                    var listPath = template.Path.Split(",").ToList();
                    List<int> listPathInt = listPath.Select(s => int.Parse(s)).ToList();
                    foreach (var listPathItem in listPathInt)
                    {
                        foreach (var templateItem in availableDepTemplates)
                        {
                            if (listPathItem == templateItem.Id)
                            {
                                templateName += templateItem.Name + " >> ";
                            }
                        }
                    }
                }
                if (templateName != string.Empty)
                {
                    //templateName = templateName.Substring(0, templateName.Length - 4);
                    templateName = templateName + template.Name;
                }
                else
                {
                    templateName = template.Name;
                    template.Path = template.Id.ToString();
                }
                items.Add(new SelectListItem { Value = template.Path.ToString(), Text = templateName });
            }


            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        protected virtual void PrepareDefaultItem(IList<SelectListItem> items, bool withSpecialDefaultItem, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //whether to insert the first special item for the default value
            if (!withSpecialDefaultItem)
                return;

            //at now we use "0" as the default value
            const string value = "0";

            //prepare item text
            defaultItemText = defaultItemText ?? _localizationService.GetResource("Admin.Common.All");

            //insert this default item at first
            items.Insert(0, new SelectListItem { Text = defaultItemText, Value = value });
        }

        public DepartmentModel PrepareDepartmentModel(DepartmentModel model, Department department)
        {
            if (department != null)
            {
                //fill in model values from the entity
                model = model ?? department.ToModel<DepartmentModel>();

            }
            //set default values for the new model
            if (department == null)
                model.Active = true;

            return model;
        }

        public virtual void UpdateLocales(Department e, DepartmentModel m)
        {
            foreach (var localized in m.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(e,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(e,
                   x => x.Address,
                   localized.Address,
                   localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(e,
                   x => x.Description,
                   localized.Description,
                   localized.LanguageId);
            }
        }

        #endregion

        #region Utilities



        #endregion
    }
}
