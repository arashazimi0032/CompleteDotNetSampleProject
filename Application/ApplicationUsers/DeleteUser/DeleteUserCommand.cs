using Application.ApplicationUsers.Share;
using MediatR;

namespace Application.ApplicationUsers.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<UserResponse>;
