namespace DotNet6APIEFCoreSQLite.Models
{
    public class OtherStuff
    {
        public void CreateDbIfNotExists(WebApplication app)
        {

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<KaijuDBContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
    }
}
