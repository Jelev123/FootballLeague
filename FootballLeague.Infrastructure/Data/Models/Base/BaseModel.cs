namespace FootballLeague.Infrastructure.Models
{
    using System.ComponentModel.DataAnnotations;
    using System;

    public class BaseModel<TKey> : IAuditInfo
    {
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
