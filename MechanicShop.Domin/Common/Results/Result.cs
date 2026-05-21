
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace MechanicShop.Domin.Common.Results
{

    public static class Result // this class is used to create instances of Result for common success scenarios without having to specify a value. It provides static properties for Success, Created, Deleted, and Updated results, which can be used to indicate the outcome of an operation without needing to provide additional information.
    {
        public static Success Success => default;
        public static Created Created => default;
        public static Deleted Deleted => default;
        public static Updated Updated => default;


    }

    public sealed class Result<Tvalue> : IResult<Tvalue> // why sealed ? because we want to prevent inheritance and also we want to make it immutable for better performance and memory efficiency.
    { 
    
        private readonly Tvalue? _value= default; // this Tvalue is the value that is returned when the operation is successful, it is nullable because it can be null when the operation fails. we use a private field to store the value and a public property to access it, this way we can control the access to the value and also we can make it immutable by not providing a setter for the property.

        private readonly List<Error>? _errors = default; // this is the list of errors that occurred during the operation, it is nullable because it can be null when the operation is successful. we use a private field to store the errors and a public property to access it, this way we can control the access to the errors and also we can make it immutable by not providing a setter for the property.

        public bool IsSuccess { get; } // if the operation is successful, this property will be true and the Value property will contain the returned value. if the operation failed, this property will be false and the Errors property will contain the list of errors that occurred during the operation.

        public bool IsError { get; } // if the operation failed, this property will be true and the Errors property will contain the list of errors that occurred during the operation. if the operation is successful, this property will be false and the Value property will contain the returned value.

        public List<Error>? Errors => IsError ? _errors!: []; // if the operation failed, this property will return the list of errors that occurred during the operation. if the operation is successful, this property will return an empty list.
        public Tvalue Value => IsSuccess ? _value! : default!; // if the operation is successful, this property will return the value that is returned when the operation is successful. if the operation failed, this property will return the default value of Tvalue, which is null for reference types and zero for value types.

        public Error TopError => (_errors?.Count>0)? _errors[0] : default; // this property is used to get the top error from the list of errors, it returns the first error in the list if there are any errors, otherwise it returns the default value of Error, which is a struct and will have all its properties set to their default values.


        [JsonConstructor]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This constructor is for serialization purposes only.", true)]
        public Result(Tvalue? value, List<Error>? errors, bool isSuccess)
        {

            if (isSuccess)
            {

                _value = value ?? throw new ArgumentException(nameof(value));
                _errors = [];
                IsSuccess = true;
            }
            else {
                if (errors == null || errors.Count == 0)
                {
                    throw new ArgumentException("provied at least on error ",nameof(errors));
                }
                _errors = errors;
                _value = default;
                IsSuccess = false;  

            }
        
        } 
            private Result( Error error) // this constructor is used to create an instance of Result<Tvalue> from a single error, it takes an Error as a parameter and initializes the Errors property with a list containing that error, it also sets the IsSuccess property to false and the Value property to default.
        {
            _errors = [error];
        } 

        private Result(List<Error> errors) // this constructor is used to create an instance of Result<Tvalue> from a list of errors, it takes a List<Error> as a parameter and initializes the Errors property with that list, it also sets the IsSuccess property to false and the Value property to default.
        { 
        
        if (errors is null || errors.Count == 0)
            {
                throw new ArgumentException("Can't create an Error<Tvalue> from the collection of erros . Provide ar lesat one error. ", nameof(errors));
            }
            _errors = errors;
            IsSuccess = false;
        } 

        public TNextVaule Match <TNextVaule> (Func<Tvalue,TNextVaule> onSuccess, Func<List<Error>,TNextVaule> onError)
        => IsSuccess ? onSuccess(_value!) : onError(_errors!); // this method is used to match the result of an operation and return a value of type TNextVaule based on whether the operation was successful or not. it takes two functions as parameters, one for the success case and one for the error case, it returns the result of the appropriate function based on the value of IsSuccess.

        private Result(Tvalue value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value), "Can't create a Success<Tvalue> with a null value. Provide a non-null value.");
            }
            _value = value;
            IsSuccess = true;
        }  // this constructor is used to create an instance of Result<Tvalue> from a value, it takes a Tvalue as a parameter and initializes the Value property with that value, it also sets the IsSuccess property to true and the Errors property to an empty list.
     
        
        public static implicit operator  Result<Tvalue>(Tvalue value) 
            => new(value); // Implicit conversion from Tvalue to Result<Tvalue>
  
        public static implicit operator  Result<Tvalue>(Error error) 
            => new(error); 
        
        public static implicit operator  Result<Tvalue>(List<Error> errors) 
            => new(errors);
    } // this calass used to create  Result <T>

    public readonly record struct Success;
    public readonly record struct Created;
    public readonly record struct Deleted;
    public readonly record struct Updated;


}
