using MechanicShop.Domin.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Domin.WorkOrders.Biling
{
    public class InvioceLineItem
    {
       public Guid  InvoiceId { get;  }
        public int LineNumber { get; }
        public string Description { get; }
        public int Quantity { get; }
        public decimal UnitPrice { get; }
        public  decimal LineTotal => Quantity * UnitPrice;

        #pragma warning disable CS8618
        private InvioceLineItem() { }

        private InvioceLineItem(Guid invoiceId, int lineNumber, string description, int quantity, decimal unitPrice)
        {
            InvoiceId = invoiceId;
            LineNumber = lineNumber;
            Description = description;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public static Result< InvioceLineItem> Create(Guid invoiceId, int lineNumber, string description, int quantity, decimal unitPrice)
        {
            if (invoiceId == Guid.Empty)
            {
                return InvioceLineItemErrors.InvoiceIdRequired;
            }

            if (lineNumber <= 0)
            {
                return InvioceLineItemErrors.LineNumberInvalid;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return InvioceLineItemErrors.DescriptionRequired;
            }

            if (quantity <= 0)
            {
                return InvioceLineItemErrors.QuantityInvalid;
            }

            if (unitPrice <= 0)
            {
                return InvioceLineItemErrors.UnitPriceInvalid;
            }

            return new InvioceLineItem(invoiceId, lineNumber, description, quantity, unitPrice);
        }

    }


}
