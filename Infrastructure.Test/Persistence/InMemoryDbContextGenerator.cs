using infrastructure.Persistence;
using infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Test.Persistence;

public class InMemoryDbContextGenerator
{
    public static ApplicationDbContext GetDbContext()
    {
        var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());

        return new ApplicationDbContext(
            optionBuilder.Options,
            new PublishDomainEventsInterceptor(new Mock<IPublisher>().Object),
            new UpdateAuditableEntitiesInterceptor());
    }
}