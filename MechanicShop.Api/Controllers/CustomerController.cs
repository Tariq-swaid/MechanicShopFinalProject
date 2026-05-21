using MechanicShop.Application.Common.Fetures.Customer.Commands.CreateCustomer;
using MechanicShop.Application.Common.Fetures.Customer.Commands.RemoveCustomer;
using MechanicShop.Application.Common.Fetures.Customer.Commands.UpdateCustomer;
using MechanicShop.Application.Common.Fetures.Customer.Dtos;
using MechanicShop.Application.Common.Fetures.Customer.Queries.GetCustomerById;
using MechanicShop.Application.Common.Fetures.Customer.Queries.GetCustomers;
using MechanicShop.Contracts.Requests.Customers;
using MechanicShop.Domin.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace MechanicShop.Api.Controllers
{
    [Route("api/v{version:apiVersion}/customers")] 
    public sealed class CustomerController(ISender sender) : ApiController
    {
        private readonly ISender _sender = sender;

        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves a list of customers")]
        [EndpointDescription("Returns all customers associated with the current user.")]
        [EndpointName("GetCustomers")]
        [MapToApiVersion("1.0")]
        [ProducesDefaultResponseType] // This attribute indicates that the endpoint can return a default response type, which is typically used for error handling. It allows the API to return a standardized error response format (like ProblemDetails) when an unexpected error occurs, without having to specify it for each individual endpoint.
        [OutputCache(Duration = 60)] // Caches the response for 60 seconds
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            var result = await _sender.Send(new GetCustomersQuery());

            return result.Match(request =>
                Ok(request),
                Problem
            );
        }


        [HttpGet("{customerId:guid}", Name = "GetCustomerById")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Retrieves a customer by ID.")]
        [EndpointDescription("Returns detailed information about the specified customer if found.")]
        [EndpointName("GetCustomerById")]
        [MapToApiVersion("1.0")]
        [OutputCache(Duration = 60)]

        public async Task<IActionResult> GetById(Guid customerId, CancellationToken ct)
        {

            var result = await _sender.Send(new GetCustmoerByIdQuery(customerId), ct);
            return result.Match(
                response => Ok(response),
                Problem
            );
        }

        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Creates a new customer.")]
        [EndpointDescription("Adds a new customer to the system.")]
        [EndpointName("CreateCustomer")]
        [MapToApiVersion("1.0")]


        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest requset, CancellationToken ct)

        {
            var Vehical = requset.Vehicles.ConvertAll(v => new CreateVehicleCommand(v.Make, v.Model, v.Year, v.LicensePlate));

            var result = await _sender.Send(
                new CreateCustomerCommand(
                    requset.Name,
                    requset.Email,
                    requset.PhoneNumber,
                    Vehical
                ), ct);

            return result.Match(
                e => CreatedAtRoute(
                 routeName: "GetCustomerById",
                routeValues: new { version = "1.0", customerId = e.Id },
                value: e),
                Problem
                );
        }

        [HttpPut("{customerId:guid}")]
        [Authorize(Roles = nameof(Role.Manager))]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Updates an existing customer.")]
        [EndpointDescription("Updates a customer and its associated vehicle.")]
        [EndpointName("UpdateCustomer")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> Update(Guid customerId, [FromBody] UpdateCustomerRequest request, CancellationToken ct)
        {
            var vehicles = request.Vehicles
                .ConvertAll(v => new UpdateVehicleCommand
                (v.VehicleId, v.Make, v.Model, v.Year, v.LicensePlate));

            var command = new UpdateCustomerCommand(
                customerId,
                request.Name,
                request.PhoneNumber,
               request.Email,
                vehicles
            );
            var reuslt = await _sender.Send(command, ct);
            return reuslt.Match(
                _ => Ok(_),
                Problem);
        }


        [HttpDelete("{customerId:guid}")]
        [Authorize(Roles = nameof(Role.Manager))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Removes a customer.")]
        [EndpointDescription("Deletes the specified customer from the system.")]
        [EndpointName("RemoveCustomer")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(Guid customerId, CancellationToken ct)
        {
                var result = await _sender.Send(new RemoveCustomerCommand(customerId), ct);
        
                return result.Match(
                    _ => NoContent(),
                    Problem
                );
        }
 
    }
}
