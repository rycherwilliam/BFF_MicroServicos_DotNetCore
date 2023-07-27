using Microsoft.EntityFrameworkCore;
using PagamentosAPI.Domain.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PagamentosAPI.Infrastructure
{
    public class PagamentosContext : DbContext
    {
        public PagamentosContext(DbContextOptions<PagamentosContext> options) : base(options)
        {
        }
        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pagamento>()
            .Property(p => p.Valor)
            .HasColumnType("decimal(18, 2)");
        }
    }
}
