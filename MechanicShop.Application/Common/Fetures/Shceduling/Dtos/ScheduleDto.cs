using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Shceduling.Dtos
{
    public class ScheduleDto
    {
        public DateOnly OnDate { get; set; }
        public bool EndOfDay { get; set; }
        public List<SpotDto> Spots { get; set; } = [];
    }
}
