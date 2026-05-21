
using System.ComponentModel.DataAnnotations.Schema;

namespace MechanicShop.Domin.Common
{
    // why we need an Entity base class in our domain model?
    // An Entity base class is a fundamental concept in domain-driven design (DDD) that provides a common structure and behavior for all entities in the domain model.
    // It serves as a base class for specific entity classes, allowing them to inherit common properties and methods.
    // Here are some reasons why we need an Entity base class in our domain model:
    public abstract class Entity 
    // we decide to make it abstract to prevent direct instantiation of the base class, as it is meant to be inherited by specific entity classes in the domain model.
    {
        public Guid Id { get; }

        private readonly List<DomainEventEntity> _domainEvents = [];

        [NotMapped]
        public IReadOnlyCollection<DomainEventEntity> DomainEvents => _domainEvents.AsReadOnly();

        protected Entity()
        { }

        protected Entity(Guid id)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
        }

        public void AddDomainEvent(DomainEventEntity domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(DomainEventEntity domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

    }
}
