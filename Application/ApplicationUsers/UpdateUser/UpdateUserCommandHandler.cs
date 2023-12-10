using Application.ApplicationUsers.Share;
using Domain.ApplicationUsers;
using Domain.Exceptions;
using Domain.IRepositories.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.ApplicationUsers.UpdateUser;

internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserResponse> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.Id.ToString()) ?? throw new UserNotFoundException(command.Id);

        var customer = await _unitOfWork.Queries.Customer.GetByIdAsync(user.CustomerId, cancellationToken);

        if (!string.IsNullOrWhiteSpace(command.Request.UserName))
        {
            user.UserName = command.Request.UserName;
            customer?.Update(command.Request.UserName, null);
        }

        if (!string.IsNullOrWhiteSpace(command.Request.Email))
        {
            user.Email = command.Request.Email;
            customer?.Update(null, command.Request.Email);
        }

        await using (var transaction = await _unitOfWork.GetDbContext().Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                if (customer is not null)
                {
                    _unitOfWork.Commands.Customer.Update(customer);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                var userResponse = await _userManager.UpdateAsync(user);
                if (!userResponse.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new UserResponse
                    {
                        Message = "Something wrong! Could not update user details!",
                        IsSuccess = false,
                        Errors = userResponse.Errors.Select(e => e.Description)
                    };
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
            Message = "User details updated successfully.",
            IsSuccess = true
        };
    }
}