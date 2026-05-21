using MechanicShop.Domin.Common;
using MechanicShop.Domin.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Domin.RepierTask.Parts
{
    public sealed class Part : AuditableEntity
    {
        public string? Name { get; private set; }
        public decimal Cost { get; private set; }
        public int Quantity { get; private set; }

        private Part() { }

        private Part(Guid id, string name, decimal cost, int quatity) : base(id)
        {
            Name = name;
            Cost = cost;
            Quantity = quatity;
        }

        public static Result<Part> Create(Guid id, string name, int qountity, decimal cost)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return PartsError.NameRequired;

            }
            if (cost <= 0 || cost > 1000)
            {
                return PartsError.CostInvalid;
            }
            if (qountity <= 0 || qountity > 10)
            {
                return PartsError.QuantityInvalid;
            }

            return new Part(id, name, cost, qountity);

        }

        public Result<Updated> Update(string name, int qountity, decimal cost)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return PartsError.NameRequired;
            }
            if (cost <= 0 || cost > 1000)
            {
                return PartsError.CostInvalid;
            }
            if (qountity <= 0 || qountity > 10)
            {
                return PartsError.QuantityInvalid;
            }
            Name = name;
            Cost = cost;
            Quantity = qountity;
            return Result.Updated;
        }
    }
}


