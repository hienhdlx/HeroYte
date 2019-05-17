using NCSw.HERO.Core;
using NCSw.HERO.Core.Caching;
using NCSw.HERO.Core.Data;
using NCSw.HERO.Core.Domain;
using NCSw.HERO.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NCSw.HERO.Services
{
    public partial class DepartmentService : IDepartmentService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Department> _departmentRepository;
        private readonly string _entityName;

        #endregion

        #region Ctor

        public DepartmentService(
            ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IRepository<Department> departmentRepository)
        {
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _departmentRepository = departmentRepository;
        }

        #endregion

        #region Methods

        public virtual void Insert(Department e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(Department));

            _departmentRepository.Insert(e);

            _cacheManager.RemoveByPattern(HeroDefaults.ServicesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(e);
        }

        public virtual void Update(Department e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(Department));

            _departmentRepository.Update(e);

            _cacheManager.RemoveByPattern(HeroDefaults.ServicesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(e);
        }

        public virtual void Updates(IList<Department> lst)
        {
            if (lst == null)
                throw new ArgumentNullException(nameof(lst));

            //update
            _departmentRepository.Update(lst);

            //cache
            _cacheManager.RemoveByPattern(HeroDefaults.ServicesPatternCacheKey);

            //event notification
            foreach (var x in lst)
            {
                _eventPublisher.EntityUpdated(x);
            }
        }

        public virtual void Delete(Department e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(Department));

            _departmentRepository.Delete(e);

            _cacheManager.RemoveByPattern(HeroDefaults.ServicesPatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(e);
        }

        public virtual void Deletes(IList<Department> lst)
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

        public virtual IList<Department> GetAll(int languageId = 0, bool showHidden = false)
        {
            var key = string.Format(HeroDefaults.ServicesPatternCacheKey, languageId, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = _departmentRepository.Table;
                if (!showHidden)
                    query = query.Where(c => c.Active);
                    query = query.Where(m => !m.Deleted);
                    query = query.OrderBy(m => m.Path);

                var entities = query.ToList();
                return entities;
            });
        }

        public virtual IPagedList<Department> GetAll(string name = "",
            int pageIndex = 0, int pageSize = int.MaxValue,
            bool showHidden = false)
        {
            var query = _departmentRepository.Table;
            if (!showHidden)
                query = query.Where(m => m.Active);
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name) || m.Address.Contains(name) || m.Code.Contains(name) || m.Description.Contains(name));
            query = query.Where(m => !m.Deleted);
            query = query.OrderBy(m => m.Path);

            return new PagedList<Department>(query, pageIndex, pageSize);
        }

        public virtual IList<Department> GetByIds(int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return new List<Department>();

            var query = from c in _departmentRepository.Table
                        where ids.Contains(c.Id)
                        select c;
            var entities = query.ToList();

            return entities;
        }

        public virtual Department GetById(int id)
        {
            if (id == 0)
                return null;

            return _departmentRepository.GetById(id);
        }

        #endregion
    }
}
