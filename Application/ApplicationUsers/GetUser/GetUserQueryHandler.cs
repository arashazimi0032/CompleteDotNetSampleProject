using Application.ApplicationUsers.Share;
using Application.DTOs;
using Domain.ApplicationUsers;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.ApplicationUsers.GetUser;

internal sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserQueryResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserQueryResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString()) ?? throw new UserNotFoundException(request.Id);

        return new UserQueryResponse
        {
            IsSuccess = true,
            Message = "One User found successfully.",
            Users = new List<UserDto>()
            {
                new(user.UserName, user.Email)
            }
        };
    }
}