using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Domin.Common
{
    // An AuditableEntity is a type of Entity that includes additional properties to track the creation and modification of the entity.
    public abstract class AuditableEntity : Entity
    {
        protected AuditableEntity() { }

        protected AuditableEntity(Guid id) : base(id)
        {
            
        }

        public DateTimeOffset CreatedAtUtc { get; set; }

        public string? CreatedBy { get; set; }
        public DateTimeOffset LastModifiedUtc { get; set; }
        public string? LastModifiedBy { get; set; }

    }
}
