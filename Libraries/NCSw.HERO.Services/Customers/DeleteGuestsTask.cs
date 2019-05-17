﻿using System;
using NCSw.HERO.Core.Domain.Customers;
using NCSw.HERO.Services.Tasks;

namespace NCSw.HERO.Services.Customers
{
    /// <summary>
    /// Represents a task for deleting guest customers
    /// </summary>
    public partial class DeleteGuestsTask : IScheduleTask
    {
        #region Fields

        private readonly CustomerSettings _customerSettings;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public DeleteGuestsTask(CustomerSettings customerSettings,
            ICustomerService customerService)
        {
            this._customerSettings = customerSettings;
            this._customerService = customerService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            var olderThanMinutes = _customerSettings.DeleteGuestTaskOlderThanMinutes;
            // Default value in case 0 is returned.  0 would effectively disable this service and harm performance.
            olderThanMinutes = olderThanMinutes == 0 ? 1440 : olderThanMinutes;

            _customerService.DeleteGuestCustomers(null, DateTime.UtcNow.AddMinutes(-olderThanMinutes), true);
        }

        #endregion
    }
}