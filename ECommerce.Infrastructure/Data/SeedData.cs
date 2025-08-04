using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // Seed Products only if none exist
            if (!context.Products.Any())
            {
                context.Products.AddRange(new List<Product>
            {
                new() { Name = "Laptop", Description = "Gaming Laptop", Price = 1500, Stock = 10 },
                new() { Name = "Phone", Description = "Smartphone", Price = 800, Stock = 20 },
                new() { Name = "Headphones", Description = "Wireless", Price = 200, Stock = 15 }
            });

                context.SaveChanges();
            }
        }
    }
}
