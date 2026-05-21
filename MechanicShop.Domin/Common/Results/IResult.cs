using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Domin.Common.Results
{
    // this interface repersents the error that can occur during an operation, it contains a message to describe the error and a kind to categorize the error. it also has a constructor to initialize the properties and a deconstructor to deconstruct the properties.
    public interface IResult // this interface is used to represent the result of an operation, it can be either a success or a failure. It contains a list of errors if the operation failed and a boolean property to indicate whether the operation was successful or not.
    {
        List<Error>? Errors { get; }
        bool IsSuccess { get; }
    }


    // this interface is used to represent the result of an operation that returns a value, it inherits from IResult and adds a Value property to hold the returned value if the operation was successful.
    // why out ? because we want to make it covariant so that we can use it in a more flexible way and also we want to make it immutable for better performance and memory efficiency.
    public interface IResult <out Tvalue> : IResult 
    
    {
        Tvalue? Value { get; }
    }

}
