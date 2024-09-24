using Bogus;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RapidEMT.Models
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed data
            builder.Entity<Employee>().HasData(GetEmployees());
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
