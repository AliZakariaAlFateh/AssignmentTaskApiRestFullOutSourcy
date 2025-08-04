using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Implementaion
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRepository<Customer> Customers { get; }
        public IRepository<Product> Products { get; }
        public IRepository<Order> Orders { get; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Customers = new Repository<Customer>(context);
            Products = new Repository<Product>(context);
            Orders = new Repository<Order>(context);
        }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    }
}
