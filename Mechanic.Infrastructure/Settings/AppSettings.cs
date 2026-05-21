using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanic.Infrastructure.Settings
{
    public class AppSettings
    {
        public TimeOnly OpeningTime { get; set; }
        public TimeOnly ClosingTime { get; set; }
        public int MaxSpots { get; set; }
        public int MinmumAppointmentDurationInMinutes { get; set; }
        public int LocalCacheExpirationInMins { get; set; }
        public int DefaultPageNumber { get; set; }
        public int DefaultPageSize { get; set; }
        public int BookingClellationThreaholdMinutes { get; set; }
        public int OverdueBookingCleanupFrequencyMinutes { get; set; }
        public string CorsPolicyName { get; set; } = default!;
        public string[] AllowedOrigins { get; set; } = default!;
    }
}
