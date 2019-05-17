namespace NCSw.HERO.Services
{
    public static partial class HeroDefaults
    {
        #region Services

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : show hidden records?
        /// </remarks>
        public static string ServicesAllCacheKey => "Hero.service.all-{0}-{1}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ServicesPatternCacheKey => "Hero.service.";

        #endregion

        #region Doctors

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : show hidden records?
        /// </remarks>
        public static string DoctorsAllCacheKey => "Hero.doctor.all-{0}-{1}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string DoctorsPatternCacheKey => "Hero.doctor.";

        #endregion

        #region Shifts

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : show hidden records?
        /// </remarks>
        public static string ShiftsAllCacheKey => "Hero.shift.all-{0}-{1}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ShiftsPatternCacheKey => "Hero.shift.";

        #endregion
    }
}
