using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.RepairTasks.Queries.GetRepairTaskById
{
    public sealed class GetRepairTaskByIdQueryValidator : AbstractValidator<GetRepairTaskByIdQuery>
    {
        public GetRepairTaskByIdQueryValidator()
        {
            RuleFor(request => request.RepairTaskId)
                .NotEmpty()
                .WithErrorCode("RepairTaskId_Is_Required")
                .WithMessage("CustomerId is required.");
        }
    }
}
