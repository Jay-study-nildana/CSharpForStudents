using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public class KiteDBContext : DbContext
{
    public DbSet<Kite> Kites { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(@"Server=tcp:servername.database.windows.net,1433;Initial Catalog=databasename;Persist Security Info=False;User ID=userid;Password=password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
}