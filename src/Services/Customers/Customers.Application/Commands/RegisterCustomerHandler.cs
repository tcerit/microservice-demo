using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Data;
using Customers.Domain;
using MediatR;

namespace Customers.Application.Commands
{
    public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, Guid>
    {
        private readonly IRepository<Customer> _repository;

        public RegisterCustomerCommandHandler(IRepository<Customer> repository)
        {
            _repository = repository;
        }


        public async Task<Guid> Handle(RegisterCustomerCommand cmd, CancellationToken cancellationToken)
        {

            Customer customer = Customer.Create(cmd.Name, cmd.LastName);
            await _repository.AddAsync(customer);
            return customer.Id;
            
        }
    }

    public class RegisterCustomerCommand : IRequest<Guid>
    {
        public string Name { get; internal set; }
        public string LastName { get; internal set; }

        public RegisterCustomerCommand(string name, string lastName)
        {
            Name = name;
            LastName = lastName;
        }
    }
}