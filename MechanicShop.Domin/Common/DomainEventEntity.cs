using MediatR;
using System;

// why we need a DomainEventEntity base class in our domain model?
// A DomainEventEntity base class is a fundamental concept in domain-driven design (DDD) that provides a common structure and behavior for all domain events in the domain model.
namespace MechanicShop.Domin.Common
{
    public abstract class DomainEventEntity : INotification;  // this interface is from MediatR library and it is used to mark a class as a domain event. It allows the class to be published and handled by MediatR's event handling mechanism.

}
