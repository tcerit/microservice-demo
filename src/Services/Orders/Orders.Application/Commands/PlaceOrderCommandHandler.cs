using System;
using Ardalis.GuardClauses;
using Core.Data;
using MediatR;
using Orders.Application.Repositories;
using Orders.Domain;

namespace Orders.Application.Commands
{
	public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<Buyer> _buyerRepository;

        public PlaceOrderCommandHandler(IOrderRepository repository, IRepository<Buyer> buyerRepository)
        {
            _orderRepository = repository;
            _buyerRepository = buyerRepository;
        }


        public async Task<Unit> Handle(PlaceOrderCommand cmd, CancellationToken cancellationToken)
        {
            Buyer? buyer = await _buyerRepository.GetByIdAsync(cmd.CustomerId);
            Guard.Against.Null(buyer, nameof(Buyer), "Buyer not found");
            Order? order = await _orderRepository.GetOrder(cmd.OrderId);
            Guard.Against.Null(order, nameof(Buyer), "Order not found");

            order.Place();

            return Unit.Value;

        }
    }

    public class PlaceOrderCommand : IRequest<Unit>
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }

        public PlaceOrderCommand(Guid customerId, Guid orderId)
        {
            CustomerId = customerId;
            OrderId = orderId;
        }
    }
}

