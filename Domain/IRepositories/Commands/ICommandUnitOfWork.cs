using Microsoft.EntityFrameworkCore;

namespace Domain.IRepositories.Commands;

public interface ICommandUnitOfWork
{
    IOrderCommandRepository Order { get; }
    
    ICustomerCommandRepository Customer { get; }
    
    IProductCommandRepository Product { get; }

    ILineItemCommandRepository LineItem { get; }
}