using System;
using System.Threading;
using System.Threading.Tasks;
using Products.Domain;
using MediatR;
using Core.Data;

namespace Products.Application.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IRepository<Product> _repository;

        public CreateProductCommandHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }


        public async Task<Guid> Handle(CreateProductCommand cmd, CancellationToken cancellationToken)
        {
            Product product = Product.Create(cmd.Name, cmd.Price);

            await _repository.AddAsync(product);

            return product.Id;
        }
    }

    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get; internal set; }
        public decimal Price { get; internal set; }

        public CreateProductCommand(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }
}