using MechanicShop.Domin.Common;
using MechanicShop.Domin.Common.Results;


namespace MechanicShop.Domin.WorkOrders.Biling
{
    public class Invoice : AuditableEntity
    {
        public Guid WorkOrderId { get; }
        public DateTimeOffset IssuedAtUtc { get; }
        public decimal DiscountAmount { get; private set; }
        public decimal TaxAmount { get; private set; }
        public decimal Subtotal => LineItems.Sum(x => x.LineTotal);
        public decimal Total => Subtotal - DiscountAmount + TaxAmount;
        public WorkOrder? WorkOrder { get; set; }

        public DateTimeOffset? PaidAt { get; private set; }

        private readonly List<InvioceLineItem> _lineItems = [];

        public IReadOnlyList<InvioceLineItem> LineItems => _lineItems;

        public InvoiceState Status { get; private set; }


        private Invoice() { }

        private Invoice(Guid id, Guid workOrderId, DateTimeOffset issuedAt, List<InvioceLineItem> lineItems, decimal discountAmount, decimal taxAmount) : base(id)
        {
            WorkOrderId = workOrderId;
            IssuedAtUtc = issuedAt;
            DiscountAmount = discountAmount;
            Status = InvoiceState.Unpaid;
            TaxAmount = taxAmount;
            _lineItems = lineItems;
        }

        public static Result<Invoice> Create(Guid id, Guid workOrderId, DateTimeOffset issuedAt, List<InvioceLineItem> Items, decimal discountAmount, decimal taxAmount)
        {
            if (workOrderId == Guid.Empty)
            {
                return InvoiceErrors.WorkOrderIdInvalid;
            }

            if (Items is null || Items.Count == 0)
            {
                return InvoiceErrors.LineItemsEmpty;
            }
            return new Invoice(id, workOrderId, issuedAt, Items, discountAmount, taxAmount);
        }

        public Result<Updated> ApplyDiscount(decimal discountAmount)
        {
            if (Status != InvoiceState.Unpaid)
            {
                return InvoiceErrors.InvoiceLocked;
            }

            if (discountAmount < 0)
            {
                return InvoiceErrors.DiscountNegative;
            }

            if (discountAmount > Subtotal)
            {
                return InvoiceErrors.DiscountExceedsSubtotal;
            }
            DiscountAmount = discountAmount;
            return Result.Updated;
        }

        public Result<Updated> MarkAsPaid(TimeProvider timeProvider)

        {

            if (Status != InvoiceState.Unpaid)
            {
                return InvoiceErrors.InvoiceLocked;

            }
            Status = InvoiceState.Paid;
            PaidAt = timeProvider.GetUtcNow();
            return Result.Updated;



        }

    }
}
