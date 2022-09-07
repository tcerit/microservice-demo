using System;
using AutoMapper;
using Core.Data;
using MediatR;
using Products.Application.Models;
using Products.Domain;

namespace Products.Application.Queries
{
	public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery,List<ProductDto>>
	{
		private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;

        public ListProductsQueryHandler(IRepository<Product> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
        {
            List<Product> products = await _repository.GetAll();

            return _mapper.Map<List<ProductDto>>(products);
        }
    }

    public class ListProductsQuery : IRequest<List<ProductDto>>
    {
    }
}

