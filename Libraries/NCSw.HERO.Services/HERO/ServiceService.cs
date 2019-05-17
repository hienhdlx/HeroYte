using NCSw.HERO.Core;
using NCSw.HERO.Core.Caching;
using NCSw.HERO.Core.Data;
using NCSw.HERO.Core.Domain;
using NCSw.HERO.Services.Events;
using NCSw.HERO.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NCSw.HERO.Services
{
    public partial class ServiceService : IServiceService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly IRepository<Service> _serviceRepository;
        private readonly string _entityName;

        #endregion

        #region Ctor

        public ServiceService(
            ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            ILocalizationService localizationService,
            IRepository<Service> serviceRepository)
        {
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _localizationService = localizationService;
            _serviceRepository = serviceRepository;
        }

        #endregion

        #region Methods

        public virtual void Insert(Service e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(Service));

            _serviceRepository.Insert(e);

            _cacheManager.RemoveByPattern(HeroDefaults.ServicesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(e);
        }

        public virtual void Update(Service e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(Service));

            _serviceRepository.Update(e);

            _cacheManager.RemoveByPattern(HeroDefaults.ServicesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(e);
        }

        public virtual void Updates(IList<Service> lst)
        {
            if (lst == null)
                throw new ArgumentNullException(nameof(lst));

            //update
            _serviceRepository.Update(lst);

            //cache
            _cacheManager.RemoveByPattern(HeroDefaults.ServicesPatternCacheKey);

            //event notification
            foreach (var x in lst)
            {
                _eventPublisher.EntityUpdated(x);
            }
        }

        public virtual void Delete(Service e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(Service));

            _serviceRepository.Delete(e);

            _cacheManager.RemoveByPattern(HeroDefaults.ServicesPatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(e);
        }

        public virtual void Deletes(IList<Service> lst)
        {
            if (lst == null)
                throw new ArgumentNullException(nameof(lst));

            foreach (var x in lst)
            {
                x.Deleted = true;
            }

            Updates(lst);

            foreach (var x in lst)
            {
                //event notification
                _eventPublisher.EntityDeleted(x);
            }
        }

        public virtual IList<Service> GetAllItem(int languageId = 0, bool showHidden = false)
        {
            var query = _serviceRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.Active);
            query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);

            var entities = query.ToList();

            if (languageId > 0)
            {
                entities = entities
                    .OrderBy(c => c.DisplayOrder)
                    .ThenBy(c => _localizationService.GetLocalized(c, x => x.Name, languageId))
                    .ToList();
            }
            
            return entities;
        }

        public virtual IList<Service> GetAll(int languageId = 0, bool showHidden = false)
        {
            var key = string.Format(HeroDefaults.ServicesPatternCacheKey, languageId, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = _serviceRepository.Table;
                if (!showHidden)
                    query = query.Where(c => c.Active);
                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);

                var entities = query.ToList();

                if (languageId > 0)
                {
                    entities = entities
                        .OrderBy(c => c.DisplayOrder)
                        .ThenBy(c => _localizationService.GetLocalized(c, x => x.Name, languageId))
                        .ToList();
                }

                return entities;
            });
        }

        public virtual IPagedList<Service> GetAll(string name = "",
            int pageIndex = 0, int pageSize = int.MaxValue,
            bool showHidden = false)
        {
            var query = _serviceRepository.Table;
            if (!showHidden)
                query = query.Where(m => m.Active);
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name));
            query = query.Where(m => !m.Deleted);
            query = query.OrderBy(m => m.DisplayOrder).ThenBy(m => m.Id);

            query = query.Distinct().OrderBy(m => m.DisplayOrder).ThenBy(m => m.Id);

            return new PagedList<Service>(query, pageIndex, pageSize);
        }

        public virtual IList<Service> GetByIds(int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return new List<Service>();

            var query = from c in _serviceRepository.Table
                        where ids.Contains(c.Id)
                        select c;
            var entities = query.ToList();

            return entities;
        }

        public virtual Service GetById(int id)
        {
            if (id == 0)
                return null;

            return _serviceRepository.GetById(id);
        }

        #endregion
    }
}
