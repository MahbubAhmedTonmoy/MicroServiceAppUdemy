using AutoMapper;
using FluentValidation;
using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
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
            var result = await _orderRepository.GetOrderByUserName(request.UserName);
            var response = _mapper.Map<IEnumerable<OrderResponse>>(result);
            return response;
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
