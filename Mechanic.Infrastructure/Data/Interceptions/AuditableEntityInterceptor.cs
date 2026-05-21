using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

// -------------------------------------------------------------------------
// Main class that intercepts database save operations
// -------------------------------------------------------------------------
public class AuditableEntityInterceptor(IUser user, TimeProvider dateTime) : SaveChangesInterceptor
{
    // Field to store the current user (to know who requested the save/update)
    private readonly IUser _user = user;

    // حقل لتخزين مزود الوقت (لجلب الوقت الحالي بدقة)
    // Field to store the time provider (to get the exact current time)
    private readonly TimeProvider _dateTime = dateTime;

    // -------------------------------------------------------------------------
    // Synchronous save interception method (called before final DB save)
    // -------------------------------------------------------------------------
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        // استدعاء الدالة المسؤولة عن تحديث حقول التدقيق (الإنشاء والتعديل)
        // Call the method responsible for updating audit fields (Created/Modified)
        UpdateEntities(eventData.Context);

        // Allow the normal Entity Framework save process to continue
        return base.SavingChanges(eventData, result);
    }

    // -------------------------------------------------------------------------
    // Asynchronous save interception method (async version of the above)
    // -------------------------------------------------------------------------
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        // Update entity fields before saving
        UpdateEntities(eventData.Context);

        // Continue the async save process
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    // -------------------------------------------------------------------------
    // The core method that modifies the record values
    // -------------------------------------------------------------------------
    public void UpdateEntities(DbContext? context)
    {
        // Exit early if the database context is null (safety check)
        if (context == null)
        {
            return;
        }

        // Get all tracked entries that inherit from AuditableEntity
        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            // Check if the record is: Added, Modified, or has changed Owned Entities
            if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                // Get the current UTC time
                var utcNow = _dateTime.GetUtcNow();

                // If the record is completely new (being inserted for the first time)
                if (entry.State == EntityState.Added)
                {
                    // Set the user ID as the "Creator" of the record
                    entry.Entity.CreatedBy = _user.Id;

                    // تعيين وقت وتاريخ الإنشاء
                    // Set the creation time and date
                    entry.Entity.CreatedAtUtc = utcNow;
                }

                // In all cases (Added or Modified), update the "Last Modified" data

                // Set current user ID as the last modifier
                entry.Entity.LastModifiedBy = _user.Id;

                // تعيين وقت التعديل للوقت الحالي
                // Set the modification time to now
                entry.Entity.LastModifiedUtc = utcNow;

                // -------------------------------------------------------------------------
                // Handle Owned Entities associated with this record
                // -------------------------------------------------------------------------

                // Loop through references and sub-entities inside this record
                foreach (var ownedEntry in entry.References)
                {
                  
                    // Check: Has data AND is AuditableEntity AND state is Added or Modified
                    if (ownedEntry.TargetEntry is { Entity: AuditableEntity ownedEntity } &&
                        ownedEntry.TargetEntry.State is EntityState.Added or EntityState.Modified)
                    {
                        // If the owned entity is newly added
                        if (ownedEntry.TargetEntry.State == EntityState.Added)
                        {
                            ownedEntity.CreatedBy = _user.Id;
                            ownedEntity.CreatedAtUtc = utcNow;
                        }

                        // Always update modification data for the owned entity
                        ownedEntity.LastModifiedBy = _user.Id;
                        ownedEntity.LastModifiedUtc = utcNow;
                    }
                }
            }
        }
    }
}

// -------------------------------------------------------------------------
// Class for Extension Methods
// -------------------------------------------------------------------------
public static class Extensions
{
    // Extension method used on EntityEntry to check its owned entities
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>

        // Search through all references of this record
        entry.References.Any(r =>

            // Ensure the reference is configured as an Owned Entity in EF Core
            r.TargetEntry?.Metadata.IsOwned() == true &&

            // Ensure this owned entity state is either Added or Modified
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}