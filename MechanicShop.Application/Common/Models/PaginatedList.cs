using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Models
{
    public class PaginatedList<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public IReadOnlyCollection<T>? Items { get; init; } 
    }
}
