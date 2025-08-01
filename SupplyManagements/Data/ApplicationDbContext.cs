using Microsoft.EntityFrameworkCore;
using SupplyManagements.Models;
using SupplyManagements.DTO.AppsUsers;

namespace SupplyManagements.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Company> Company { get; set; }
        public DbSet<Vendor> Vendor{ get; set; }
        public DbSet<ApplicationUser> ApplicationUser{ get; set; }
        public DbSet<SupplyManagements.DTO.AppsUsers.ApplicationUserDto> ApplicationUserDto { get; set; } = default!;

    }
}
