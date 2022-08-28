using System;
using AutoMapper;
using Customers.Application.Models;
using Customers.Domain;

namespace Customers.Application.Services
{
	public class CustomersMapper : Profile
	{
		public CustomersMapper()
		{
            CreateMap<Customer, CustomerDto>();
        }
    }
}

