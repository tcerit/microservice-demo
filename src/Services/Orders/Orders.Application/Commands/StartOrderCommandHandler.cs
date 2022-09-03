using System;
using System.Threading;
using System.Threading.Tasks;
using Orders.Domain;
using MediatR;
using Core.Data;
using Ardalis.GuardClauses;
using Orders.Application.Repositories;

namespace Orders.Application.Commands
{
    public class StartOrderCommandHandler : IRequestHandler<StartOrderCommand, Guid>
    {
        private readonly IOrderRepository _repository;
        private readonly IRepository<Buyer> _buyerRepository;

        public StartOrderCommandHandler(IOrderRepository repository, IRepository<Buyer> buyerRepository)
        {
            _repository = repository;
            _buyerRepository = buyerRepository;
        }


        public async Task<Guid> Handle(StartOrderCommand cmd, CancellationToken cancellationToken)
        {
            Buyer? buyer = await _buyerRepository.GetByIdAsync(cmd.CustomerId);
            Guard.Against.Null(buyer, nameof(Buyer), "Buyer not found");

            Order order = buyer.StartOrdering();

            await _repository.AddAsync(order);

            return order.Id;
        }
    }

    public class StartOrderCommand : IRequest<Guid>
    {
        public Guid CustomerId { get; private set; }

        public StartOrderCommand(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}