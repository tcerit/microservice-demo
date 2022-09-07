using System;
using Ardalis.GuardClauses;
using Core.Data;
using MediatR;
using Orders.Application.Services;
using Orders.Domain;

namespace Orders.Application.Commands
{
    public class AddOrderItemCommandHandler : IRequestHandler<AddOrderItemCommand, Unit>
    {

        private readonly IOrderRepository _repository;
        private readonly IRepository<OrderProduct> _prodcutRepository;

        public AddOrderItemCommandHandler(IOrderRepository repository, IRepository<OrderProduct> prodcutRepository)
        {
            _repository = repository;
            _prodcutRepository = prodcutRepository;
        }


        public async Task<Unit> Handle(AddOrderItemCommand cmd, CancellationToken cancellationToken)
        {
            Order? order = await _repository.GetByIdAsync(cmd.OrderId);
            Guard.Against.Null(order, nameof(Order), "Order not found");
            OrderProduct? product = await _prodcutRepository.GetByIdAsync(cmd.ProdcutId);
            Guard.Against.Null(product, nameof(OrderProduct), "Product not found");

            order.AddItem(product, cmd.Quantity);

            await _repository.UpdateAsync(order);

            return Unit.Value;
        }
    }

    public class AddOrderItemCommand : IRequest<Unit>
    {
        public Guid OrderId { get; private set; }
        public Guid ProdcutId { get; internal set; }
        public int Quantity { get; internal set; }

        public AddOrderItemCommand(Guid orderId, Guid prodcutId, int quantity)
        {
            OrderId = orderId;
            ProdcutId = prodcutId;
            Quantity = quantity;
        }
    }
}



