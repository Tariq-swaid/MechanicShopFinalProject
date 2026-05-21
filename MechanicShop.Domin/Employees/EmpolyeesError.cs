using MechanicShop.Domin.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Domin.Employees
{
    public static class EmpolyeesError
    {
        public static readonly  Error IdRequired =
            Error.Validation("Employee.Id.Required", "Employee Id is required.");
        public static Error FirstNameRequired => Error.Validation("Employee.FirstName.Required", "First name is required.");

        public static Error LastNameRequired => Error.Validation("Employee.LastName.Required", "Last name is required.");

        public static Error RoleInvalid => Error.Validation("Employee.Role.Invalid", "Role is invalid.");
    }
}
