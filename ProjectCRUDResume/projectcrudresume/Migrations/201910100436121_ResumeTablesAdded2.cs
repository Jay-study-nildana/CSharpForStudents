namespace projectcrudresume.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResumeTablesAdded2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AddressTables",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AspNetUsersUniqueIdentifier = c.String(),
                        AddressLineOne = c.String(),
                        AddressLineTwo = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Pincode = c.String(),
                        Landmark = c.String(),
                        AddressExtraNotes = c.String(),
                        PrimaryAddress = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.EducationalDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AspNetUsersUniqueIdentifier = c.String(),
                        EducationTitle = c.String(),
                        InstituationName = c.String(),
                        YearOfGraduation = c.Int(nullable: false),
                        PassGrade = c.String(),
                        EducationOtherNotes1 = c.String(),
                        EducationOtherNotes2 = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ExtraCurriculars",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AspNetUsersUniqueIdentifier = c.String(),
                        ExtraCurricularOtherNotes1 = c.String(),
                        ExtraCurricularNotes2 = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OtherStuffs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AspNetUsersUniqueIdentifier = c.String(),
                        OtherStuffNotes1 = c.String(),
                        OtherStuffNotes2 = c.String(),
                        OtherStuffNotes3 = c.String(),
                        OtherStuffNotes4 = c.String(),
                        OtherStuffNotes5 = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PhoneNumberTables",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AspNetUsersUniqueIdentifier = c.String(),
                        PhoneNumber = c.String(),
                        CountryCode = c.Boolean(nullable: false),
                        PrimaryAddress = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProjectDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AspNetUsersUniqueIdentifier = c.String(),
                        ProjectTitle = c.String(),
                        ProjectDescription = c.String(),
                        YearOfProject = c.Int(nullable: false),
                        ProjectNotes1 = c.String(),
                        ProjectNotes2 = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SkillsTables",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AspNetUsersUniqueIdentifier = c.String(),
                        SkillTitle = c.String(),
                        SkillDescription = c.String(),
                        SkillExperience = c.Int(nullable: false),
                        SkillOtherNotes1 = c.String(),
                        SkillOtherNotes2 = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SkillsTables");
            DropTable("dbo.ProjectDetails");
            DropTable("dbo.PhoneNumberTables");
            DropTable("dbo.OtherStuffs");
            DropTable("dbo.ExtraCurriculars");
            DropTable("dbo.EducationalDetails");
            DropTable("dbo.AddressTables");
        }
    }
}
