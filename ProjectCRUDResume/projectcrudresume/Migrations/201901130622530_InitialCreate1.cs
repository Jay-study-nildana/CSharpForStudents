namespace projectcrudresume.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message1 = c.String(),
                        Message2 = c.String(),
                        Number1 = c.Int(nullable: false),
                        Number2 = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TestModels");
        }
    }
}
