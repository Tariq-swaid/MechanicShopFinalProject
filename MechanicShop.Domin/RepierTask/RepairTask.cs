using MechanicShop.Domin.Common;
using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.RepierTask.Enums;
using MechanicShop.Domin.RepierTask.Parts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Domin.RepierTask
{
    public sealed class RepairTask : AuditableEntity
    {
        public string Name { get; private set; }
        public decimal LaborCost { get; private set; }
        private readonly List<Part> _parts = [];
        public IEnumerable<Part> Parts => _parts.AsReadOnly();
        public decimal TotalCost => LaborCost + Parts.Sum(p => p.Cost * p.Quantity);
        public RepairDurationInMinutes RepairDurationInMinutes { get; private set; }


        #pragma warning disable CS8618

        private RepairTask() { }
        private RepairTask(Guid id, string name, decimal laborCost, List<Part> parts, RepairDurationInMinutes repairDurationInMinutes) : base(id)
        {
            Name = name;
            LaborCost = laborCost;
            _parts = parts;
            RepairDurationInMinutes = repairDurationInMinutes;
        }

        public static Result<RepairTask> Create(Guid id, string name, decimal laborCost, List<Part> parts, RepairDurationInMinutes repairDurationInMinutes)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return RepairTaskErros.NameRequired;
            }
            if (laborCost <= 0 || laborCost > 10000)
            {
                return RepairTaskErros.LaborCostInvalid;
            }
            if (!Enum.IsDefined(repairDurationInMinutes))
            {
                return RepairTaskErros.DurationInvalid;
            }
            return new RepairTask(id, name.Trim(), laborCost, parts, repairDurationInMinutes);


        }

        public Result<Updated> UpsertParts(List<Part> incomingParts)
        {
            _parts.RemoveAll(existing => incomingParts.All(p => p.Id != existing.Id));

            foreach (var incoming in incomingParts)
            {
                var existing = _parts.FirstOrDefault(p => p.Id == incoming.Id);
                if (existing is null)
                {
                    _parts.Add(incoming);
                }
                else
                {
                    var updatePartResult = existing.Update(incoming.Name!, incoming.Quantity, incoming.Cost);
                    if (updatePartResult.IsError)
                    {
                        return updatePartResult.Errors!;
                    }
                }
            }

            return Result.Updated;
        }

        public Result<Updated> Update(string name, decimal laborCost, RepairDurationInMinutes estimatedDurationInMins)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return RepairTaskErros.NameRequired;
            }

            if (laborCost <= 0 || laborCost > 10000)
            {
                return RepairTaskErros.LaborCostInvalid;
            }

            if (!Enum.IsDefined(estimatedDurationInMins))
            {
                return RepairTaskErros.DurationInvalid;
            }

            Name = name.Trim();
            LaborCost = laborCost;
            RepairDurationInMinutes = estimatedDurationInMins;

            return Result.Updated;
        }

    }
}
