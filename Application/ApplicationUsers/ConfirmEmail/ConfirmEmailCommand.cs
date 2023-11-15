using Application.ApplicationUsers.Share;
using MediatR;

namespace Application.ApplicationUsers.ConfirmEmail;

public record ConfirmEmailCommand(string UserId, string Token) : IRequest<UserResponse>;
