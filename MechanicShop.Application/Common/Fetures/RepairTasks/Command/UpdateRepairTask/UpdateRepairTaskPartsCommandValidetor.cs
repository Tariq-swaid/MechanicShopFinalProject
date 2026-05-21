using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.RepairTasks.Command.UpdateRepairTask
{
    public class UpdateRepairTaskPartCommandValidator : AbstractValidator<UpdateRepairTaskPartsCommand>
    {
        public UpdateRepairTaskPartCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Part name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Cost)
                .InclusiveBetween(1, 10_000)
                .WithMessage("Cost must be between 1 and 10,000.");

            RuleFor(x => x.Quantity)
                .InclusiveBetween(1, 10)
                .WithMessage("Quantity must be between 1 and 10.");
        }
    }
}
