using System;
using Ardalis.GuardClauses;
using Core.Data;
using MediatR;
using Products.Domain;

namespace Products.Application.Commands
{
	public class DelistProductCommandHandler : IRequestHandler<DelistProductCommand,Unit>
	{
        private readonly IRepository<Product> _repository;

        public DelistProductCommandHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }


        public async Task<Unit> Handle(DelistProductCommand cmd, CancellationToken cancellationToken)
        {
            Product? product = await _repository.GetByIdAsync(cmd.Id);

            Guard.Against.Null(product, nameof(Product));

            product.Delist();

            await _repository.UpdateAsync(product);

            return Unit.Value;
        }
    }

    public class DelistProductCommand : IRequest<Unit>
    {
        public Guid Id { get; internal set; }

        public DelistProductCommand(Guid id)
        {
            Id = id;
        }
    }
}


