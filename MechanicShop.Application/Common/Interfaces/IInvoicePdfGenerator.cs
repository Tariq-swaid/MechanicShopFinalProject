using MechanicShop.Domin.WorkOrders.Biling;


namespace MechanicShop.Application.Common.Interfaces
{
    public interface IInvoicePdfGenerator
    {
        byte[] Generate(Invoice invoice);
    }
}
