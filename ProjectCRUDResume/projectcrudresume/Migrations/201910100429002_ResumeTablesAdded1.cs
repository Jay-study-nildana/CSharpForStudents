namespace projectcrudresume.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResumeTablesAdded1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "AspNetUsersUniqueIdentifier", c => c.String());
            AddColumn("dbo.UserResumes", "AspNetUsersUniqueIdentifier", c => c.String());
            AddColumn("dbo.UserResumes", "ProjectDetails", c => c.Boolean(nullable: false));
            AlterColumn("dbo.UserResumes", "Address", c => c.Boolean(nullable: false));
            AlterColumn("dbo.UserResumes", "PhoneNumber", c => c.Boolean(nullable: false));
            AlterColumn("dbo.UserResumes", "SkillsSummary", c => c.Boolean(nullable: false));
            AlterColumn("dbo.UserResumes", "EducationalDetailsSummary", c => c.Boolean(nullable: false));
            AlterColumn("dbo.UserResumes", "ExtraCurricularActivitiesSummary", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserResumes", "ExtraCurricularActivitiesSummary", c => c.String());
            AlterColumn("dbo.UserResumes", "EducationalDetailsSummary", c => c.String());
            AlterColumn("dbo.UserResumes", "SkillsSummary", c => c.String());
            AlterColumn("dbo.UserResumes", "PhoneNumber", c => c.String());
            AlterColumn("dbo.UserResumes", "Address", c => c.String());
            DropColumn("dbo.UserResumes", "ProjectDetails");
            DropColumn("dbo.UserResumes", "AspNetUsersUniqueIdentifier");
            DropColumn("dbo.UserProfiles", "AspNetUsersUniqueIdentifier");
        }
    }
}
