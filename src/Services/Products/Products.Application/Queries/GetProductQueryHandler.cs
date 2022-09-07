using System;
using Ardalis.GuardClauses;
using AutoMapper;
using Core.Data;
using MediatR;
using Products.Application.Models;
using Products.Domain;

namespace Products.Application.Queries
{
	public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
	{
        private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IRepository<Product> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            Product? product = await _repository.GetByIdAsync(request.Id);

            Guard.Against.Null(product, nameof(Product));

            return _mapper.Map<ProductDto>(product);
        }
    }

    public class GetProductQuery : IRequest<ProductDto>
    {
        public Guid Id { get; internal set; }

        public GetProductQuery(Guid id)
        {
            Id = id;
        }
    }
}

