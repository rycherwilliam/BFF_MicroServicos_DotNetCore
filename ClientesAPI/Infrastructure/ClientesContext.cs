using Microsoft.EntityFrameworkCore;
using ClientesAPI.Domain.Models;

namespace ClientesAPI.Infrastructure
{
    public class ClientesContext : DbContext
    {
        public ClientesContext(DbContextOptions<ClientesContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Cliente>(entity =>  
            {
                entity.HasKey(c => c.CpfOuCnpj);                
            });

            modelBuilder.Entity<Cliente>()
            .Property(c => c.RendaBruta)
            .HasColumnType("decimal(18,2)");
        }
    }
}
