using Application.ApplicationUsers.Share;
using MediatR;

namespace Application.ApplicationUsers.GetUser;

public record GetUserQuery(Guid Id) : IRequest<UserQueryResponse>;
