using System;
using Ardalis.GuardClauses;
using Core.Data;
using MediatR;
using Orders.Application.Repositories;
using Orders.Domain;

namespace Orders.Application.Commands
{
	public class EditQuantityCommandHandler : IRequestHandler<EditQuantityCommand, Unit>
    {

        private readonly IOrderRepository _repository;
        private readonly IRepository<OrderProduct> _prodcutRepository;

        public EditQuantityCommandHandler(IOrderRepository repository, IRepository<OrderProduct> prodcutRepository)
        {
            _repository = repository;
            _prodcutRepository = prodcutRepository;
        }


        public async Task<Unit> Handle(EditQuantityCommand cmd, CancellationToken cancellationToken)
        {
            Order? order = await _repository.GetByIdAsync(cmd.OrderId);
            Guard.Against.Null(order, nameof(Order), "Order not found");
            OrderProduct? product = await _prodcutRepository.GetByIdAsync(cmd.ProdcutId);
            Guard.Against.Null(product, nameof(OrderProduct), "Product not found");

            order.SetQuantity(product, cmd.Quantity);

            await _repository.UpdateAsync(order);

            return Unit.Value;
        }
    }

    public class EditQuantityCommand : IRequest<Unit>
    {
        public Guid OrderId { get; private set; }
        public Guid ProdcutId { get; internal set; }
        public int Quantity { get; internal set; }

        public EditQuantityCommand(Guid orderId, Guid prodcutId, int quantity)
        {
            OrderId = orderId;
            ProdcutId = prodcutId;
            Quantity = quantity;
        }
    }
}

