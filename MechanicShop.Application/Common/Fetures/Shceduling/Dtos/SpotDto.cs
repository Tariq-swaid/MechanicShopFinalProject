using MechanicShop.Domin.WorkOrders.Enums;


namespace MechanicShop.Application.Common.Fetures.Shceduling.Dtos
{
    public class SpotDto
    {
        public Spot Spot { get; set; }
        public List<AvailabilitySlotDto> Slots { get; set; } = [];
    }
}
