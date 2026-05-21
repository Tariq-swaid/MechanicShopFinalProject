using MechanicShop.Domin.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Domin.RepierTask.Parts
{
    public static class PartsError
    {
      
        public static readonly Error NameRequired = 
            Error.Validation("Part.Name.Required", "Part name is required.");
        public static readonly Error CostInvalid =
            Error.Validation("Part.Cost.Invalid", "Part cost must be between 1 and 10,000.");
        public static readonly Error QuantityInvalid =
            Error.Validation("Part.Quantity.Invalid", "Quantity must be between 1 and 10.");
    }
}
