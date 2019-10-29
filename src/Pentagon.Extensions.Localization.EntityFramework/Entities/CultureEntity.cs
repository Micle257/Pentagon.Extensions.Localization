// -----------------------------------------------------------------------
//  <copyright file="CultureEntity.cs">
//   Copyright (c) Michal Pokorný. All Rights Reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Pentagon.Extensions.Localization.EntityFramework.Entities
{
    using System.Collections.Generic;
    using EntityFrameworkCore;

    /// <summary> Represents an EF entity for table 'Culture'. </summary>
    public class CultureEntity : Entity
    {
        /// <summary> Gets or sets the name of culture. </summary>
        /// <value> The name. </value>
        public string Name { get; set; }

        /// <summary> Gets or sets a value indicating whether this culture is active. </summary>
        /// <value> <c> true </c> if this culture is active; otherwise, <c> false </c>. </value>
        public bool ActiveFlag { get; set; }

        /// <summary> Gets the resources. Used as eager loaded data. </summary>
        /// <value> The resources. </value>
        public virtual ICollection<CultureResourceEntity> Resources { get; } = new HashSet<CultureResourceEntity>();
    }
}