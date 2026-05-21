using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MechanicShop.Contracts.Requests.WorkOrders
{
    public class AssignLaborRequest
    {
        [Required(ErrorMessage = "LaborId is required.")]
        public string LaborId { get; set; } = string.Empty;
    }
}
