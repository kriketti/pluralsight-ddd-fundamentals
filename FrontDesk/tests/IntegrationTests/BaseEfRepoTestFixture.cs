﻿using FrontDesk.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace IntegrationTests
{
  public abstract class BaseEfRepoTestFixture
  {
    protected AppDbContext _dbContext;

    protected static DbContextOptions<AppDbContext> CreateNewContextOptions()
    {
      // Create a fresh service provider, and therefore a fresh
      // InMemory database instance.
      var serviceProvider = new ServiceCollection()
          .AddEntityFrameworkInMemoryDatabase()
          .BuildServiceProvider();

      // Create a new options instance telling the context to use an
      // InMemory database and the new service provider.
      var builder = new DbContextOptionsBuilder<AppDbContext>();
      builder.UseInMemoryDatabase("TestFrontDesk")
             .UseInternalServiceProvider(serviceProvider);

      return builder.Options;
    }

    protected EfRepository GetRepository()
    {
      var options = CreateNewContextOptions();
      var mockMediator = new Mock<IMediator>();

      _dbContext = new AppDbContext(options, mockMediator.Object);

      return new EfRepository(_dbContext);
    }
  }
}