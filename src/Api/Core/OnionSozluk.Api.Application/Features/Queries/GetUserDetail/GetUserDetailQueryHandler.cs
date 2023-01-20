using AutoMapper;
using MediatR;
using OnionSozluk.Api.Application.Interfaces.Repositories;
using OnionSozluk.Api.Domain.Models;
using OnionSozluk.Common.ViewModels.Queries;

namespace OnionSozluk.Api.Application.Features.Queries.GetUserDetail
{
    public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQuery, UserDetailViewModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserDetailQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDetailViewModel> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
        {
            User user = null;
            if (request.UserId == Guid.Empty)
                user = await _userRepository.GetSingleAsync(p => p.UserName == request.UserName);
            else if (request.UserId != Guid.Empty)
                user = await _userRepository.GetByIdAsync(request.UserId);

            return _mapper.Map<UserDetailViewModel>(user);
        }

    }
}
