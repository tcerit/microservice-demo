using System;
using AutoMapper;
using Core.Data;
using MediatR;
using Orders.Application.Models;
using Orders.Application.Repositories;
using Orders.Domain;

namespace Orders.Application.Queries
{
    public class ListPlacedOrdersQueryHandler : IRequestHandler<ListPlacedOrdersQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public ListPlacedOrdersQueryHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OrderDto>> Handle(ListPlacedOrdersQuery query, CancellationToken cancellationToken)
        {
            List<Order> orders = await _repository.PlacedOrdersWithItems(query.DateStart, query.DateEnd);

            return _mapper.Map<List<OrderDto>>(orders);
        }
    }

    public class ListPlacedOrdersQuery : IRequest<List<OrderDto>>
    {
        public DateTime DateStart { get; internal set; }
        public DateTime DateEnd { get; internal set; }

        public ListPlacedOrdersQuery(DateTime dateStart, DateTime dateEnd)
        {
            DateStart = dateStart;
            DateEnd = dateEnd;
        }
    }
}

