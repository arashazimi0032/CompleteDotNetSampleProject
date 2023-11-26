using Application.ApplicationUsers.Share;
using MediatR;

namespace Application.ApplicationUsers.UpdateUser;

public record UpdateUserRoleCommand(Guid Id, UpdateUserRoleRequest Request) : IRequest<UserResponse>;
