using System;
using AutoMapper;
using Core.Data;
using Customers.Application.Commands;
using Customers.Application.Models;
using Customers.Domain;
using MediatR;

namespace Customers.Application.Queries
{
	public class ListCustomersQueryHandler : IRequestHandler<ListCustomersQuery, List<CustomerDto>>
    {
        private readonly IRepository<Customer> _repository;
        private readonly IMapper _mapper;

        public ListCustomersQueryHandler(IRepository<Customer> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<List<CustomerDto>> Handle(ListCustomersQuery query, CancellationToken cancellationToken)
        {

             return _mapper.Map<List<CustomerDto>>(await _repository.GetAll());

        }
    }

    public class ListCustomersQuery : IRequest<List<CustomerDto>>
    {
    }
}

