using Domain.ApplicationUsers;
using Domain.Customers;
using Domain.Orders;
using Domain.Orders.Entities;
using Domain.Primitive.Events;
using Domain.Primitive.Models;
using Domain.Products;
using infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;
    private readonly UpdateAuditableEntitiesInterceptor _updateAuditableEntitiesInterceptor;
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        PublishDomainEventsInterceptor publishDomainEventsInterceptor,
        UpdateAuditableEntitiesInterceptor updateAuditableEntitiesInterceptor) : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
        _updateAuditableEntitiesInterceptor = updateAuditableEntitiesInterceptor;
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<LineItem> LineItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(
            _publishDomainEventsInterceptor, 
            _updateAuditableEntitiesInterceptor);
        base.OnConfiguring(optionsBuilder);
    }
}