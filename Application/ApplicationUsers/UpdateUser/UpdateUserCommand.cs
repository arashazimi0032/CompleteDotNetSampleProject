using Application.ApplicationUsers.Share;
using MediatR;

namespace Application.ApplicationUsers.UpdateUser;

public record UpdateUserCommand(Guid Id, UpdateUserRequest Request) : IRequest<UserResponse>;
