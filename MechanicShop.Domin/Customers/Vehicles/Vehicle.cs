using MechanicShop.Domin.Common;
using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.Customers;

namespace MechanicShop.Domin.Customer.Vehicles
{
    public sealed class Vehicle: AuditableEntity
    {
        
        public Guid CustmoerId { get;}
        public string Make { get; private set; }
        public string Model { get; private set; }
        public int Year { get; private set; }
        public string LicensePlate { get; private set; }
        public string VehicaleInfo => $"{Make} | {Model}| {Year} ";
        public Customers.Customer Customer { get; private set; }
#pragma warning disable CS8618
        private Vehicle() { }
    
        private Vehicle(Guid id ,string make , string model, int year, string licenseplate) :base(id)
        {

            Make = make;
            Model = model;
            Year = year;
            LicensePlate = licenseplate;
            
        }

        public static Result<Vehicle> Create(Guid id, string make, int year, string model, string licespeplate) 
        {

            if (string.IsNullOrEmpty(make))
            {
                return ErrorVehicle.MakeRequired;
            }

            if (string.IsNullOrEmpty(model))
            {
                return ErrorVehicle.ModelRequired;
            }
            if (year < 1886 || year > DateTime.UtcNow.Year)
            {
                return ErrorVehicle.YearInvalid;
            }
            
            if (string.IsNullOrEmpty(licespeplate))
            {
                return ErrorVehicle.LicensePlateRequired;
            }
            return new Vehicle(id, make, model, year, licespeplate);
        }
        public  Result<Updated> Updete(string make, string model, int year, string licensePlate)
        {
            if (string.IsNullOrEmpty(make))
            {
                return ErrorVehicle.MakeRequired;
            }

            if (string.IsNullOrEmpty(model))
            { 
                return  ErrorVehicle.ModelRequired;
            }
            if (year < 1886 || year > DateTime.UtcNow.Year) 
            {
                return ErrorVehicle.YearInvalid;
            }
            if (string.IsNullOrEmpty(licensePlate))
            { 
                return ErrorVehicle.LicensePlateRequired;
            }

            Make = make;
            Model = model;
            Year = year;
            LicensePlate = licensePlate;

            return Result.Updated;
        
        }
    }
}
