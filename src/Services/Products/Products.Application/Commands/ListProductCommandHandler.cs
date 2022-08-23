using System;
using Ardalis.GuardClauses;
using Core.Data;
using MediatR;
using Products.Domain;

namespace Products.Application.Commands
{
	public class ListProductCommandHandler : IRequestHandler<ListProductCommand,Unit>
	{
        private readonly IRepository<Product> _repository;

        public ListProductCommandHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }


        public async Task<Unit> Handle(ListProductCommand cmd, CancellationToken cancellationToken)
        {
            Product? product = await _repository.GetByIdAsync(cmd.Id);

            Guard.Against.Null(product, nameof(Product));

            product.List();

            await _repository.UpdateAsync(product);

            return Unit.Value;
        }
    }

    public class ListProductCommand : IRequest<Unit>
    {
        public Guid Id { get; internal set; }

        public ListProductCommand(Guid id)
        {
            Id = id;
        }
    }
}


