using System.Collections.Generic;
using EFSQLiteDemo.Model;
using Microsoft.EntityFrameworkCore;

public class KiteDBContext : DbContext
{
    public DbSet<Kite> Kites { get; set; }
    public DbSet<KiteUpdateOctober4th> KiteUpdateOctober4ths { get; set; }

    // The following configures EF to create a Sqlite database file as `C:\blogging.db`.
    // For Mac or Linux, change this to `/tmp/blogging.db` or any other absolute path.
    // The SQLITE db file gets created if you have not already created it
    // in fact, let .NET create the file for you.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=EFSQLiteDemo1.db");
}