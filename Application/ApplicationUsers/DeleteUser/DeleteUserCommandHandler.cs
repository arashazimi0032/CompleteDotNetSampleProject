using Application.ApplicationUsers.Share;
using Domain.ApplicationUsers;
using Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.ApplicationUsers.DeleteUser;

internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, UserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
        {
            return new UserResponse
            {
                Message = "User not found!",
                IsSuccess = false
            };
        }

        await using (var transaction =
                     await _unitOfWork.GetDbContext().Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                var response = await _userManager.DeleteAsync(user);
                if (!response.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new UserResponse
                    {
                        Message = "Can not delete user.Something went wrong!",
                        IsSuccess = false,
                        Errors = response.Errors.Select(e => e.Description)
                    };
                }
                
                var customer = await _unitOfWork.Customer.GetByIdAsync(user.CustomerId, cancellationToken);
                if (customer is not null)
                {
                    _unitOfWork.Customer.Remove(customer);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new UserResponse
                {
                    Message = e.Message,
                    IsSuccess = false
                };
            }
        }

        return new UserResponse
        {
            Message = "User deleted successfully.",
            IsSuccess = true
        };
    }
}