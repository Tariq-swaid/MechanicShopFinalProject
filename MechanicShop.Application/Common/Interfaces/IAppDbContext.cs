using MechanicShop.Domin.Customer;
using MechanicShop.Domin.Customer.Vehicles;
using MechanicShop.Domin.Customers;
using MechanicShop.Domin.Employees;
using MechanicShop.Domin.Identity;
using MechanicShop.Domin.RepierTask;
using MechanicShop.Domin.RepierTask.Parts;
using MechanicShop.Domin.WorkOrders;
using MechanicShop.Domin.WorkOrders.Biling;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MechanicShop.Application.Common.Interfaces
{
    public interface IAppDbContext 
    {
        public DbSet<Customer> Customers { get;}
        public DbSet <Part> Parts { get;  }
        public DbSet<Vehicle> Vehicles { get;  }
        public DbSet <Invoice> Invoices { get;  }
        public DbSet<InvioceLineItem> InvoicesLineItems { get;  } 
        public DbSet<Employe> Employes { get;  }
        public DbSet <WorkOrder> WorkOrders { get; }
        public DbSet <RepairTask> RepairTasks   { get;  }
        public DbSet<RefreshToken> RefreshTokens { get;  }

        Task <int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
