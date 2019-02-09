namespace Pentagon.Extensions.Localization.EntityFramework.Entities {
    using System;
    using EntityFrameworkCore;
    using EntityFrameworkCore.Abstractions.Entities;

    /// <summary> Represents an EF entity for table 'CultureResource'. </summary>
    public class CultureResourceEntity : Entity,
                                         ICreateTimeStampSupport
    {
        public int CultureId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        /// <inheritdoc />
        public DateTimeOffset CreatedAt { get; set; }
        
        public virtual CultureEntity Culture { get; set; }
    }
}