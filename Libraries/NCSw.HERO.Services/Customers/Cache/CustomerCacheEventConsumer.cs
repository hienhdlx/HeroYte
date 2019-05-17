using NCSw.HERO.Core.Caching;
using NCSw.HERO.Core.Domain.Customers;
using NCSw.HERO.Services.Events;

namespace NCSw.HERO.Services.Customers.Cache
{
    /// <summary>
    /// Customer cache event consumer (used for caching of current customer password)
    /// </summary>
    public partial class CustomerCacheEventConsumer : IConsumer<CustomerPasswordChangedEvent>
    {
        #region Fields

        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public CustomerCacheEventConsumer(IStaticCacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        //password changed
        public void HandleEvent(CustomerPasswordChangedEvent eventMessage)
        {
            _cacheManager.Remove(string.Format(NopCustomerServiceDefaults.CustomerPasswordLifetimeCacheKey, eventMessage.Password.CustomerId));
        }

        #endregion
    }
}