namespace DotNet6APIEFCoreSQLite.Models
{
    public static class DbInitializer
    {
        public static void Initialize(KaijuDBContext context)
        {
            //check if DB is already created. 
            //if not, this will ensure DB is created.
            context.Database.EnsureCreated();

            // Look for any kaijus.
            if (context.Kaijus.Any())
            {
                return;   // DB has been seeded
            }

            //lets add some kaijus

            //create an array with some default kaijus
            var somekaijus = new Kaiju[]
            {
                new Kaiju{ Name = "Godzilla", Description = "Greatest Kaiju Ever", FoodChainLevel = "Alpha"},
                new Kaiju{ Name = "Kong", Description = "Second Greatest Kaiju Ever", FoodChainLevel = "Alpha"}
            };

            //add the kaiju objects to the table
            foreach(var x in somekaijus)
            {
                context.Kaijus.Add(x);
            }

            //save all db changes
            context.SaveChanges();

        }
    }
}
