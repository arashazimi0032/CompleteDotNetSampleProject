using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Test;

public class MockDatabaseFacade : DatabaseFacade
{
    public MockDatabaseFacade(DbContext context) : base(context)
    {
    }

    public override Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Mock.Of<IDbContextTransaction>());
    }
}