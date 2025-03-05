using Bogus;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidEMT.Models
{
    public class DataContext : IdentityDbContext
    {
        private readonly ILogger<DataContext> _logger;

        public DataContext(DbContextOptions<DataContext> options, ILogger<DataContext> logger) : base(options)
        {
            _logger = logger;
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed data
            builder.Entity<Employee>().HasData(GetEmployees());
        }

        public override int SaveChanges()
        {
            TrackChanges();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            TrackChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void TrackChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    LogInsert(entry);
                }
                else if (entry.State == EntityState.Modified)
                {
                    LogUpdate(entry);
                }
                else if (entry.State == EntityState.Deleted)
                {
                    LogDelete(entry);
                }
            }
        }

        private void LogInsert(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
        {
            var entityName = entry.Entity.GetType().Name;
            var entityId = entry.Property("Id")?.CurrentValue ?? "Unknown ID";
            _logger.LogInformation("New {EntityName} (ID: {EntityId}) added.", entityName, entityId);
        }

        private void LogUpdate(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
        {
            var entityName = entry.Entity.GetType().Name;
            var entityId = entry.Property("Id")?.CurrentValue ?? "Unknown ID";

            foreach (var property in entry.OriginalValues.Properties)
            {
                var oldValue = entry.OriginalValues[property];
                var newValue = entry.CurrentValues[property];

                if (!Equals(oldValue, newValue)) // Log only changed values
                {
                    _logger.LogInformation("{EntityName} (ID: {EntityId}) updated: {Property} changed from '{OldValue}' to '{NewValue}'",
                        entityName, entityId, property.Name, oldValue, newValue);
                }
            }
        }

        private void LogDelete(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
        {
            var entityName = entry.Entity.GetType().Name;
            var entityId = entry.Property("Id")?.CurrentValue ?? "Unknown ID";
            _logger.LogInformation("{EntityName} (ID: {EntityId}) deleted.", entityName, entityId);
        }

        private List<Employee> GetEmployees()
        {
            var employees = new List<Employee>();
            var faker = new Faker("en");
            var random = new Random();

            for (int i = -1; i >= -100; i--) // Negative IDs to avoid conflicts
            {
                var employee = new Employee
                {
                    Id = i,
                    ImgUrl = $"https://picsum.photos/200?random={random.Next()}",
                    Name = faker.Name.FullName(),
                    Salary = GetRandomSalary(random),
                    Type = GetRandomEmployeeType(random),
                    Position = GetRandomPosition(random)
                };
                employees.Add(employee);
            }
            return employees;
        }

        private decimal GetRandomSalary(Random random)
        {
            return random.Next(30000, 100000); // Generates a random salary between $30,000 and $100,000
        }

        private EmployeeType GetRandomEmployeeType(Random random)
        {
            var types = Enum.GetValues(typeof(EmployeeType));
            return (EmployeeType)types.GetValue(random.Next(types.Length));
        }

        private Position GetRandomPosition(Random random)
        {
            var positions = Enum.GetValues(typeof(Position));
            return (Position)positions.GetValue(random.Next(positions.Length));
        }
    }
}
