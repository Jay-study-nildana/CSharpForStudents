namespace projectcrudresume.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addgetotherstuffflag1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserResumes", "GetOtherStuff", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserResumes", "GetOtherStuff");
        }
    }
}
