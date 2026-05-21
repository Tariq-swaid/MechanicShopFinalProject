using Mechanic.Infrastructure.Identity;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common;
using MechanicShop.Domin.Customer.Vehicles;
using MechanicShop.Domin.Customers;
using MechanicShop.Domin.Employees;
using MechanicShop.Domin.Identity;
using MechanicShop.Domin.RepierTask;
using MechanicShop.Domin.RepierTask.Parts;
using MechanicShop.Domin.WorkOrders;
using MechanicShop.Domin.WorkOrders.Biling;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Mechanic.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions options,IMediator mediator) : IdentityDbContext<AppUser>(options), IAppDbContext
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Part> Parts => Set<Part>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();   
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvioceLineItem> InvoicesLineItems => Set<InvioceLineItem>();
        public DbSet<Employe> Employes => Set<Employe>();
        public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
        public DbSet<RepairTask> RepairTasks => Set<RepairTask>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await DispatchDomainEventsAsync(cancellationToken);
            return await base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }


        private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken) 
            // this method is responsible for dispatching domain events before saving changes to the database.
            // It retrieves all entities that have domain events, collects those events,
            // publishes them using the mediator
            // , and then clears the domain events from the entities.
        {
            var domainEntities = ChangeTracker.Entries()
                .Where(e => e.Entity is Entity baseEntity && baseEntity.DomainEvents.Count != 0)
                .Select(e => (Entity)e.Entity)
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent, cancellationToken);
            }

            foreach (var entity in domainEntities)
            {
                entity.ClearDomainEvents();
            }
        }

    }
}
