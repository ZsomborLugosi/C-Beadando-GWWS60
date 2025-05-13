using GWWS60Beadando.Models;

namespace GWWS60Beadando.Data;

using Microsoft.EntityFrameworkCore;

public class DockyardContext : DbContext
{
    public DockyardContext(DbContextOptions<DockyardContext> options) : base(options) { }

    public DbSet<Ship> Ships { get; set; }
}
