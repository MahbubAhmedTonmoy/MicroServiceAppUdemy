using AutoMapper;
using Ordering.Application.Commands;
using Ordering.Application.Responses;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();
        }
    }
}
