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
    public partial class AppointmentService : IAppointmentService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Appointment> _AppointmentRepository;
        private readonly string _entityName;

        #endregion

        #region Ctor

        public AppointmentService(
            ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IRepository<Appointment> AppointmentRepository)
        {
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
            _AppointmentRepository = AppointmentRepository;
        }

        #endregion

        #region Methods

        public virtual void Insert(Appointment e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(Appointment));

            _AppointmentRepository.Insert(e);

            _cacheManager.RemoveByPattern(HeroDefaults.ServicesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(e);
        }

        public virtual void Update(Appointment e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(Appointment));

            _AppointmentRepository.Update(e);

            _cacheManager.RemoveByPattern(HeroDefaults.ServicesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(e);
        }

        public virtual void Updates(IList<Appointment> lst)
        {
            if (lst == null)
                throw new ArgumentNullException(nameof(lst));

            //update
            _AppointmentRepository.Update(lst);

            //cache
            _cacheManager.RemoveByPattern(HeroDefaults.ServicesPatternCacheKey);

            //event notification
            foreach (var x in lst)
            {
                _eventPublisher.EntityUpdated(x);
            }
        }

        public virtual void Delete(Appointment e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(Appointment));

            _AppointmentRepository.Delete(e);

            _cacheManager.RemoveByPattern(HeroDefaults.ServicesPatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(e);
        }

        public virtual IList<Appointment> GetAll(int languageId = 0, bool showHidden = false)
        {
            var key = string.Format(HeroDefaults.ServicesPatternCacheKey, languageId, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = _AppointmentRepository.Table;

                var entities = query.ToList();
                return entities;
            });
        }

        public virtual IPagedList<Appointment> GetAll(string name = "",
            int pageIndex = 0, int pageSize = int.MaxValue,
            bool showHidden = false)
        {
            var query = _AppointmentRepository.Table;

            return new PagedList<Appointment>(query, pageIndex, pageSize);
        }

        public virtual IList<Appointment> GetByIds(int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return new List<Appointment>();

            var query = from c in _AppointmentRepository.Table
                        where ids.Contains(c.Id)
                        select c;
            var entities = query.ToList();

            return entities;
        }

        public virtual Appointment GetById(int id)
        {
            if (id == 0)
                return null;

            return _AppointmentRepository.GetById(id);
        }

        #endregion
    }
}
