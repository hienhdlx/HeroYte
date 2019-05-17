using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NCSw.HERO.Core;
using NCSw.HERO.Core.Caching;
using NCSw.HERO.Core.Data;
using NCSw.HERO.Core.Domain;
using NCSw.HERO.Services.Events;
using NCSw.HERO.Services.Localization;

namespace NCSw.HERO.Services
{
    public partial class DoctorService : IDoctorService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly ILocalizationService _localizationService;
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public DoctorService(
            ICacheManager cacheManager,
            ILocalizationService localizationService,
            IEventPublisher eventPublisher,
            IRepository<Doctor> doctorRepository)
        {
            _cacheManager = cacheManager;
            _localizationService = localizationService;
            _doctorRepository = doctorRepository;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a doctor 
        /// </summary>
        /// <param name="doctorId">Doctor identifier</param>
        /// <returns>Doctor</returns>
        public virtual Doctor GetDoctorById(int doctorId)
        {
            if (doctorId == 0)
                return null;

            return _doctorRepository.GetById(doctorId);
        }

        /// <summary>
        /// Get many doctors
        /// </summary>
        /// <param name="ids">arr id</param>
        /// <returns>Doctor</returns>
        public virtual IList<Doctor> GetDoctorByIds(int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return new List<Doctor>();

            var query = from d in _doctorRepository.Table
                        where ids.Contains(d.Id)
                        select d;
            var entities = query.ToList();

            return entities;
        }

        /// <summary>
        /// Gets a doctor 
        /// </summary>
        /// <param name="doctorId">Doctor identifier</param>
        /// <returns>Doctor</returns>
        public virtual Doctor GetDoctor(Expression<Func<Doctor, bool>> expression)
        {
            if (expression == null)
                return null;

            var query = _doctorRepository.Table;

            var entities = query.Where(expression).FirstOrDefault();

            return entities;
        }

        /// <summary>
        /// Gets all doctor
        /// </summary>
        /// <param name="languageId">Language identifier. It's used to sort doctor by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Doctor</returns>
        public virtual IList<Doctor> GetAll(int languageId = 0, bool showHidden = false)
        {
            var key = string.Format(HeroDefaults.DoctorsAllCacheKey, languageId, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = _doctorRepository.Table;

                query = query.OrderByDescending(o => o.Id).ThenByDescending(c => c.FullName);

                var entities = query.ToList();

                if (languageId > 0)
                {
                    entities = entities.OrderByDescending(o => o.Id)
                                       .ThenBy(t => _localizationService.GetLocalized(t, g => g.FullName, languageId))
                                       .ToList();
                }

                return entities;
            });
        }

        public IPagedList<Doctor> GetAll(string keyWord = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _doctorRepository.Table;
            if (!showHidden)
            { /*do something*/ }
            if (!string.IsNullOrWhiteSpace(keyWord))
                query = query.Where(w => w.Code.Contains(keyWord) || 
                                         w.FullName.Contains(keyWord) ||
                                         w.IdentityCard.Contains(keyWord) ||
                                         w.Email.Contains(keyWord) ||
                                         w.PhoneNumber.Contains(keyWord) ||
                                         w.Address.Contains(keyWord));

            query = query.Distinct().OrderByDescending(o => o.Id).ThenByDescending(c => c.FullName);

            return new PagedList<Doctor>(query, pageIndex, pageSize);
        }
        /// <summary>
        /// Inserts a doctor
        /// </summary>
        /// <param name="doctor">Doctor</param>
        public virtual void InsertDoctor(Doctor doctor)
        {
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor));

            _doctorRepository.Insert(doctor);

            _cacheManager.RemoveByPattern(HeroDefaults.DoctorsPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(doctor);
        }

        /// <summary>
        /// Updates the doctor
        /// </summary>
        /// <param name="doctor">Doctor</param>
        public virtual void UpdateDoctor(Doctor doctor)
        {
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor));

            _doctorRepository.Update(doctor);

            _cacheManager.RemoveByPattern(HeroDefaults.DoctorsPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(doctor);
        }

        /// <summary>
        /// Delete a doctor
        /// </summary>
        /// <param name="doctor">Doctor</param>
        public virtual void DeleteDoctor(Doctor doctor)
        {
            if (doctor == null)
                throw new ArgumentNullException(nameof(doctor));

            _doctorRepository.Delete(doctor);

            _cacheManager.RemoveByPattern(HeroDefaults.DoctorsPatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(doctor);
        }

        /// <summary>
        /// Delete many doctors
        /// </summary>
        /// <param name="doctor">Doctor</param>
        public virtual void DeleteDoctors(IList<Doctor> lst)
        {
            if (lst == null || lst.Count <= 0)
                throw new ArgumentNullException(nameof(lst));

            _doctorRepository.Delete(lst);

            _cacheManager.RemoveByPattern(HeroDefaults.DoctorsPatternCacheKey);

            foreach (var x in lst)
            {
                //event notification
                _eventPublisher.EntityDeleted(x);
            }
        }

        #endregion
    }
}
