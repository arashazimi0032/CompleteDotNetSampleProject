using Application.ApplicationUsers.Share;
using MediatR;

namespace Application.ApplicationUsers.GetAllUsers;

public record GetAllUsersQuery() : IRequest<UserQueryResponse>;
