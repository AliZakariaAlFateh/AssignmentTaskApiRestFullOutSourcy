using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Customer> Customers { get; }
        IRepository<Product> Products { get; }
        IRepository<Order> Orders { get; }
        Task<int> SaveAsync();
    }
}
