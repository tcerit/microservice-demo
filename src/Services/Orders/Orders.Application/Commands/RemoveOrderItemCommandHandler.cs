using System;
using Ardalis.GuardClauses;
using Core.Data;
using MediatR;
using Orders.Application.Repositories;
using Orders.Domain;

namespace Orders.Application.Commands
{
	public class RemoveOrderItemCommandHandler : IRequestHandler<RemoveOrderItemCommand, Unit>
	{
        private readonly IOrderRepository _repository;
        private readonly IRepository<OrderProduct> _productRepository;

        public RemoveOrderItemCommandHandler(IOrderRepository repository, IRepository<OrderProduct> prodcutRepository)
        {
            _repository = repository;
            _productRepository = prodcutRepository;
        }


        public async Task<Unit> Handle(RemoveOrderItemCommand cmd, CancellationToken cancellationToken)
        {
            Order? order = await _repository.GetByIdAsync(cmd.OrderId);
            Guard.Against.Null(order, nameof(Order), "Order not found");
            OrderProduct? product = await _productRepository.GetByIdAsync(cmd.ProdcutId);
            Guard.Against.Null(product, nameof(OrderProduct), "Product not found");

            order.RemoveItem(product);

            await _repository.UpdateAsync(order);

            return Unit.Value;
        }
    }

    public class RemoveOrderItemCommand : IRequest<Unit>
    {
        internal string productName;

        public Guid OrderId { get; private set; }
        public Guid ProdcutId { get; internal set; }

        public RemoveOrderItemCommand(Guid orderId, Guid prodcutId)
        {
            OrderId = orderId;
            ProdcutId = prodcutId;
        }
    }
}

