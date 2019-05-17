using NCSw.HERO.Core;
using NCSw.HERO.Core.Caching;
using NCSw.HERO.Core.Data;
using NCSw.HERO.Core.Domain;
using NCSw.HERO.Services.Events;
using NCSw.HERO.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NCSw.HERO.Services
{
    public partial class ShiftService: IShiftService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly ILocalizationService _localizationService;
        private readonly IRepository<Shift> _shiftRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public ShiftService(
            ICacheManager cacheManager,
            ILocalizationService localizationService,
            IEventPublisher eventPublisher,
            IRepository<Shift> ShiftRepository)
        {
            _cacheManager = cacheManager;
            _localizationService = localizationService;
            _eventPublisher = eventPublisher;
            _shiftRepository = ShiftRepository;
        }

        #endregion


        #region Methods




        /// <summary>
        /// get a shift by id
        /// </summary>
        /// <param name="shiftId">get shift by id</param>
        /// <returns></returns>
        public virtual Shift GetShiftById(int shiftId)
        {
            if(shiftId == 0)
            {
                return null;
            }
            return _shiftRepository.GetById(shiftId);
        }

        /// <summary>
        /// get all shift
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        public virtual IList<Shift> GetAll(int languageId = 0, bool showHidden = false)
        {
            var key = string.Format(HeroDefaults.ServicesAllCacheKey, languageId, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = _shiftRepository.Table;

                query = query.Where(s => s.Deleted == false).OrderBy(o => o.Id).ThenBy(c => c.Name);

                var entities = query.ToList();

                if (languageId > 0)
                {
                    entities = entities.OrderBy(o => o.Id)
                                       .ThenBy(t => _localizationService.GetLocalized(t, g => g.Name, languageId))
                                       .ToList();
                }

                return entities;
            });
        }

        public virtual IPagedList<Shift> GetAll(string name = "",
            int pageIndex = 0, int pageSize = int.MaxValue,
            bool showHidden = false)
        {
            var query = _shiftRepository.Table;
            if (!showHidden)
                query = query.Where(m => m.Active);
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(m => m.Name.Contains(name));
            query = query.Where(m => !m.Deleted);

            return new PagedList<Shift>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// get a shift 
        /// </summary>
        /// <param name="exception">getshift</param>
        /// <returns></returns>
        public virtual Shift GetShift(Expression<Func<Shift, bool>> exception)
        {
            if (exception == null)
                return null;

            var query = _shiftRepository.Table;

            var entities = query.Where(exception).FirstOrDefault();

            return entities;
        }


        /// <summary>
        /// insert a shift
        /// </summary>
        /// <param name="shift">insertshift</param>
        public virtual void InsertShift(Shift shift)
        {
            if (shift == null)
                throw new ArgumentNullException(nameof(shift));
            _shiftRepository.Insert(shift);

            _cacheManager.RemoveByPattern(HeroDefaults.ShiftsPatternCacheKey);

            _eventPublisher.EntityInserted(shift);
        }

        /// <summary>
        /// update a shift
        /// </summary>
        /// <param name="shift">Shift</param>
        public virtual void UpdateShift(Shift shift)
        {
            if (shift == null)
                throw new ArgumentNullException(nameof(shift));

            _shiftRepository.Update(shift);

            _cacheManager.RemoveByPattern(HeroDefaults.ShiftsPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(shift);
        }
        /// <summary>
        /// delete a shift
        /// </summary>
        /// <param name="shift">Shift</param>
        public virtual void DeleteShift(Shift shift)
        {
            if (shift == null)
                throw new ArgumentNullException(nameof(shift));

            _shiftRepository.Delete(shift);

            _cacheManager.RemoveByPattern(HeroDefaults.ShiftsPatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(shift);
        }

        public virtual IList<Shift> GetByIds(int[] ids)
        {
            if(ids == null || ids.Length == 0)
            {
                return new List<Shift>();
            }
            var query = from c in _shiftRepository.Table where ids.Contains(c.Id) select c;

            var entities = query.ToList();
            return entities;
        }

        public void DeleteShift(IList<Shift> list)
        {
            if(list == null)
            {
                throw new NotImplementedException();
            }
            foreach(var x in list)
            {
                x.Deleted = true;
            }

            UpdateShift(list);

            foreach (var x in list)
            {
                _eventPublisher.EntityDeleted(x);
            }
        }

        private void UpdateShift(IList<Shift> list)
        {
            if(list == null)
            {
                throw new NotImplementedException();
            }
            //update
            _shiftRepository.Update(list);

            //cache
            _cacheManager.RemoveByPattern(HeroDefaults.ServicesPatternCacheKey);

            //event notification
            foreach (var x in list)
            {
                _eventPublisher.EntityUpdated(x);
            }
        }



        #endregion
    }
}
