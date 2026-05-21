using MechanicShop.Domin.Common;
using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Domin.Employees
{
    public sealed class Employe : AuditableEntity
    {
        public string FisrtName { get; }
        public string LastName { get; }

        public Role Role { get; }
        public string FullName => $"{FisrtName} {LastName}";

#pragma warning disable CS8618
        private Employe() { }

#pragma warning restore CS8618

        private Employe(Guid id, string fisrtName, string lastName, Role role) : base(id)
        {
            FisrtName = fisrtName;
            LastName = lastName;
            Role = role;
        }


        public static Result<Employe> Create(Guid id, string fisrtName, string lastName, Role role) 
      
        { 
            if (id == Guid.Empty)
            {
               return EmpolyeesError.IdRequired;
            }
            if (string.IsNullOrWhiteSpace(fisrtName))

            {
                    return EmpolyeesError.FirstNameRequired;
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                    return EmpolyeesError.LastNameRequired;
            }
            if (!Enum.IsDefined(typeof(Role), role))
            {
                     return EmpolyeesError.RoleInvalid;
            }
            return new Employe(id, fisrtName.Trim(), lastName.Trim(), role);


        }
    }
}