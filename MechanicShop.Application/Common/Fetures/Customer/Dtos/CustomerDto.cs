using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Customer.Dtos
{
    public class CustomerDto
    {
     public Guid Id { get; set; }
     public string? Name { get; set; }
     public string? Email { get; set; }
     public string? PhoneNumber { get; set; }
     public List<VehicleDto>? Vehicles { get; set; }
    
    }
}
