﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Ordering.Application.Mapper;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Entities;
using Ordering.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers
{
    public class GetOrderByUserNameQueryHandler : IRequestHandler<GetOrderByUserNameQuery, IEnumerable<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public GetOrderByUserNameQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<OrderResponse>> Handle(GetOrderByUserNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);

                var orderResponseList = OrderMapper.Mapper.Map<IEnumerable<OrderResponse>>(orderList);
                return orderResponseList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

    public class GetOrderByUserValidator : AbstractValidator<GetOrderByUserNameQuery>
    {
        public GetOrderByUserValidator()
        {
            RuleFor(m => m.UserName).NotEmpty();
        }
    }
}
