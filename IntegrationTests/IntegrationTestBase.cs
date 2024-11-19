using System;
using JwtPracticeProject.Data;
using Microsoft.EntityFrameworkCore;

namespace JwtPracticeProject.Tests.IntegrationTests
{


    public abstract class IntegrationTestBase : IDisposable
    {
        protected readonly ApplicationDbContext Context;
        public IntegrationTestBase()
        {
            // Set up SQLite in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            Context = new ApplicationDbContext(options);

            // Open the connection and ensure the database is created
            Context.Database.OpenConnection();
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            // Dispose of the database connection
            Context.Database.CloseConnection();
            Context.Dispose();
        }
    }
}