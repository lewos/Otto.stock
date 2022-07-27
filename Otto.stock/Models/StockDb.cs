using Microsoft.EntityFrameworkCore;

namespace Otto.stock.Models
{
    public class StockDb : DbContext
    {
        public StockDb(DbContextOptions options) : base(options) { }
        public DbSet<Stock> Stocks { get; set; }
    }
    
}
