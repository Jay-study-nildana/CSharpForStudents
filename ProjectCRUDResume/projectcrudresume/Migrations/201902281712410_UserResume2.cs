namespace projectcrudresume.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserResume2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserResumes", "UserUniqueKey", c => c.String());
            AddColumn("dbo.UserResumes", "UserEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserResumes", "UserEmail");
            DropColumn("dbo.UserResumes", "UserUniqueKey");
        }
    }
}
