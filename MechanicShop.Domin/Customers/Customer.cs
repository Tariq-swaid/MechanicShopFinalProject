using MechanicShop.Domin.Common;
using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.Customer.Vehicles;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace MechanicShop.Domin.Customers
{
    public sealed class Customer : AuditableEntity
    {
        public string? Name { get; private set; }
        public string? Email { get; private set; }
        public string? PhoneNumber { get; private set; }

        private readonly List<Vehicle> _vehicles = [];
        public IEnumerable<Vehicle> Vehicles => _vehicles.AsReadOnly();
        private Customer() { }

        private Customer(Guid id, string name, string email, string phoneNumber, List<Vehicle> vehicles)
            : base(id)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            _vehicles = vehicles;

        }

        public static Result<Customer> Create(Guid id, string name, string email, string phoneNumber, List<Vehicle> vehicles)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ErrorCustomer.NameRequired;
            }

            if (string.IsNullOrWhiteSpace(phoneNumber) || !Regex.IsMatch(phoneNumber, @"^\+?\d{7,15}$"))
            {
                return ErrorCustomer.InvalidPhoneNumber;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                return ErrorCustomer.EmailRequired;
            }

            try
            {
                _ = new MailAddress(email);
            }
            catch
            {
                return ErrorCustomer.EmailInvalid;
            }

            return new Customer(id, name,email ,phoneNumber, vehicles);
        }

        public Result<Updated> Update(string name, string email, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return ErrorCustomer.NameRequired;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                return ErrorCustomer.EmailRequired;
            }

            if (string.IsNullOrWhiteSpace(phoneNumber) || !Regex.IsMatch(phoneNumber, @"^\+?\d{7,15}$"))
            {
                return ErrorCustomer.InvalidPhoneNumber;
            }

            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            return Result.Updated;
        }

        public Result<Updated> UpsertParts(List<Vehicle> incomingVehicle)
        {
            _vehicles.RemoveAll(existing => incomingVehicle.All(v => v.Id != existing.Id));

            foreach (var incoming in incomingVehicle)
            {
                var existing = _vehicles.FirstOrDefault(v => v.Id == incoming.Id);
                if (existing is null)
                {
                    _vehicles.Add(incoming);
                }
                else
                {
                    var updateVehicleResult = existing.Updete(incoming.Make, incoming.Model, incoming.Year, incoming.LicensePlate);

                    if (updateVehicleResult.IsError)
                    {
                        return updateVehicleResult.Errors!;
                    }
                }
            }

            return Result.Updated;
        }

    }

}





