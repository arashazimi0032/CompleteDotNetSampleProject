using Domain.IRepositories.Commands;
using infrastructure.Persistence;
using infrastructure.Persistence.Interceptors;
using infrastructure.Persistence.Repositories.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Test.Persistence.Repositories.Commands;

public class CommandUnitOfWorkTests
{
    private readonly ICommandUnitOfWork _commandUnitOfWork;
    public CommandUnitOfWorkTests()
    {
        Mock<ApplicationDbContext> context = new(
            new DbContextOptions<ApplicationDbContext>(),
            new PublishDomainEventsInterceptor(new Mock<IPublisher>().Object),
            new UpdateAuditableEntitiesInterceptor());

        _commandUnitOfWork = new CommandUnitOfWork(context.Object);
    }

    [Fact]
    public void CommandOrderUnitOfWork_ShouldBeOfType_OrderCommandRepository()
    {
        // Arrange

        // Act

        // Assert
        _commandUnitOfWork.Order.Should().BeAssignableTo<IOrderCommandRepository>();
        _commandUnitOfWork.Order.Should().NotBeNull();
    }

    [Fact]
    public void CommandCustomerUnitOfWork_ShouldBeOfType_CustomerCommandRepository()
    {
        // Arrange

        // Act

        // Assert
        _commandUnitOfWork.Customer.Should().BeAssignableTo<ICustomerCommandRepository>();
        _commandUnitOfWork.Customer.Should().NotBeNull();
    }

    [Fact]
    public void CommandProductUnitOfWork_ShouldBeOfType_ProductCommandRepository()
    {
        // Arrange

        // Act

        // Assert
        _commandUnitOfWork.Product.Should().BeAssignableTo<IProductCommandRepository>();
        _commandUnitOfWork.Product.Should().NotBeNull();
    }

    [Fact]
    public void CommandLineItemUnitOfWork_ShouldBeOfType_LineItemCommandRepository()
    {
        // Arrange


        // Act

        // Assert
        _commandUnitOfWork.LineItem.Should().BeAssignableTo<ILineItemCommandRepository>();
        _commandUnitOfWork.LineItem.Should().NotBeNull();
    }
}