using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaChequesNuevo.Models;

namespace SistemaChequesNuevo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<SolicitudCheque> SolicitudCheques { get; set; }

        //public override void OnModelCreating(ModelBuilder modelBuilder) 
        //{
        //    modelBuilder.Entity<SolicitudCheque>()
        //        .HasOne(p => p.Proveedor)
        //        .WithMany(a => a.SolicitudCheques)
        //        .HasForeignKey(p => p.ProveedorId);
        //} 
    }
}