namespace projectcrudresume.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResumeTablesAdded3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AddressTables", "UniqueGuid", c => c.String());
            AddColumn("dbo.EducationalDetails", "UniqueGuid", c => c.String());
            AddColumn("dbo.ExtraCurriculars", "UniqueGuid", c => c.String());
            AddColumn("dbo.OtherStuffs", "UniqueGuid", c => c.String());
            AddColumn("dbo.PhoneNumberTables", "UniqueGuid", c => c.String());
            AddColumn("dbo.ProjectDetails", "UniqueGuid", c => c.String());
            AddColumn("dbo.SkillsTables", "UniqueGuid", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SkillsTables", "UniqueGuid");
            DropColumn("dbo.ProjectDetails", "UniqueGuid");
            DropColumn("dbo.PhoneNumberTables", "UniqueGuid");
            DropColumn("dbo.OtherStuffs", "UniqueGuid");
            DropColumn("dbo.ExtraCurriculars", "UniqueGuid");
            DropColumn("dbo.EducationalDetails", "UniqueGuid");
            DropColumn("dbo.AddressTables", "UniqueGuid");
        }
    }
}
