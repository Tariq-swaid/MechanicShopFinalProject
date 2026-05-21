using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.RepairTasks.Command.CreateRepairTask
{
    public sealed class CreateRepairTaskPartCommandValidator : AbstractValidator<CreateRepairTaskPartsCommand>
    {
        public CreateRepairTaskPartCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Part name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Cost)
                .GreaterThan(0).WithMessage("Part cost must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1.");
        }
    }
}
