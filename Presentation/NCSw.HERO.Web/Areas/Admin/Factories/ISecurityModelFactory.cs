﻿using NCSw.HERO.Web.Areas.Admin.Models.Security;

namespace NCSw.HERO.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the security model factory
    /// </summary>
    public partial interface ISecurityModelFactory
    {
        /// <summary>
        /// Prepare permission mapping model
        /// </summary>
        /// <param name="model">Permission mapping model</param>
        /// <returns>Permission mapping model</returns>
        PermissionMappingModel PreparePermissionMappingModel(PermissionMappingModel model);
    }
}