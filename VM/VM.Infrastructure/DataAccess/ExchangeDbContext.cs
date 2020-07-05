using Microsoft.EntityFrameworkCore;
using VM.Core.Entities;

namespace VM.Infrastructure.DataAccess
{
    public class ExchangeDbContext : DbContext
    {
        public ExchangeDbContext(DbContextOptions<ExchangeDbContext> options)
            : base(options)
        { }

        public DbSet<ExchangePurchases> ExchangePurchases { get; set; }
    }
}