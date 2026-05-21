using MechanicShop.Application.Common.Fetures.Labors.Dtos;
using MechanicShop.Domin.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Labors.Mappers
{
    public static class LaborMapper
    {
        public static LaborDto ToDto(this Employe employee)
        {
            return new LaborDto { LaborId = employee.Id, Name = employee.FullName };
        }

        public static List<LaborDto> ToDtos(this IEnumerable<Employe> entities)
        {
            return entities.Select(l => l.ToDto()).ToList();
        }
    }
}
