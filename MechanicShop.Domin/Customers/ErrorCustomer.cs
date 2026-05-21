using MechanicShop.Domin.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Domin.Customers
{
    public static class ErrorCustomer
    {
        public static Error NameRequired 
            => Error.Validation("Customer_Name_Required"," Customer Name Is Required");

        public static Error EmailRequired
           => Error.Validation(" Customer_Eamil_Required", " Customer Email Is Required");

      public static Error PhonNumberRequired
             => Error.Validation("Customer_PhoneNumber_Required", " Customer PhoneNumber Is Required");

        public static Error EmailInvalid
            => Error.Validation("Customer_Email_Invalid", " Customer Email is Ivalid");

        public static Error CustomerExists =>
            Error.Conflict("Customer_Email_Exists", "A customer with this email already exists.");

        public static readonly Error InvalidPhoneNumber =
            Error.Conflict("Customer.InvalidPhoneNumber", "Phone number must be 7–15 digits and may start with '+'.");

        public static readonly Error CannotDeleteCustomerWithWorkOrders =
            Error.Conflict("Customer.CannotDelete", "Customer cannot be deleted due to existing work orders.");
    }
}
