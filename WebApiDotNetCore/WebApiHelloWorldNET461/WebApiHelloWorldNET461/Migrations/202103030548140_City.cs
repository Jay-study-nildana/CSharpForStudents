namespace WebApiHelloWorldNET461.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class City : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "CityOfAuthor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Authors", "CityOfAuthor");
        }
    }
}
