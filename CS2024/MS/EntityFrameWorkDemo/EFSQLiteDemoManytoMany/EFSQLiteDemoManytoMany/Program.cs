// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

// Print the current directory to debug output
// Note: Sometimes the SQLite DB gets created in the solution folder instead of the Current Directory.
// This can happen due to the way the application is executed in different environments (e.g., Visual Studio, command line).
// When running the application from Visual Studio, the working directory might be set to the solution folder.
// As a result, the SQLite database file may be created in the solution folder instead of the intended current directory.
// To ensure the database file is in the correct location, you may need to manually copy it from the solution folder to the current directory.
// This behavior is due to the environment's working directory settings and is generally unavoidable.
Console.WriteLine("Current Directory: " + Environment.CurrentDirectory);

// Method to add some posts with tags
void AddPosts()
{
    using (var db = new ManyDBContext())
    {
        var tag1 = new Tag();
        var tag2 = new Tag();

        var post1 = new Post();
        post1.Tags.Add(tag1);
        post1.Tags.Add(tag2);

        var post2 = new Post();
        post2.Tags.Add(tag1);

        db.Add(post1);
        db.Add(post2);
        db.SaveChanges();

        Console.WriteLine("Posts and tags were added to the database.");
    }
}

// Method to display posts and their tags
void DisplayPosts()
{
    using (var db = new ManyDBContext())
    {
        var posts = db.Posts.Include(p => p.Tags).ToList();

        foreach (var post in posts)
        {
            Console.WriteLine($"Post Id: {post.Id}");
            foreach (var tag in post.Tags)
            {
                Console.WriteLine($"  Tag Id: {tag.Id}");
            }
        }
    }
}

// Add and display posts
AddPosts();
DisplayPosts();



public class Post
{
    public int Id { get; set; }
    public List<Tag> Tags { get; } = new List<Tag>();
}

public class Tag
{
    public int Id { get; set; }
    public List<Post> Posts { get; } = new List<Post>();
}

public class ManyDBContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }

    // The following configures EF to create a Sqlite database file as `C:\blogging.db`.
    // For Mac or Linux, change this to `/tmp/blogging.db` or any other absolute path.
    // The SQLITE db file gets created if you have not already created it
    // in fact, let .NET create the file for you.
    //protected override void OnConfiguring(DbContextOptionsBuilder options)
    //    => options.UseSqlite(@"Data Source=EFSQLiteDemo1.db");

    // The following configures EF to create a Sqlite database file in the current directory.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // Use the current directory for the SQLite database file
        string dbPath = System.IO.Path.Combine(Environment.CurrentDirectory, "EFSQLiteDemo1.db");
        options.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>()
            .HasMany(e => e.Tags)
            .WithMany(e => e.Posts);
    }
}

