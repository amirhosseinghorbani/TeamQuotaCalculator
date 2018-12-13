using System;
using Microsoft.EntityFrameworkCore;
using TeamQuotaCalculator.Models;

namespace TeamQuotaCalculator.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }
        public DbSet<Manager> Managers { get; set; }

        public void Save()
        {
            this.SaveChanges();
        }
    }

    public interface IAppDbContext 
    {
        DbSet<Manager> Managers { get; set; }
        void Save();
    }
}