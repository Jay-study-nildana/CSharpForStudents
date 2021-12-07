namespace projectcrudresume.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserResume1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserResumes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        SkillsSummary = c.String(),
                        EducationalDetailsSummary = c.String(),
                        ExtraCurricularActivitiesSummary = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserResumes");
        }
    }
}
