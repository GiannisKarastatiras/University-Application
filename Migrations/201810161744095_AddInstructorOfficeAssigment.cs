namespace BootCampApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInstructorOfficeAssigment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Instructor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        HireDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OfficeAssignment",
                c => new
                    {
                        InstructorID = c.Int(nullable: false),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.InstructorID)
                .ForeignKey("dbo.Instructor", t => t.InstructorID)
                .Index(t => t.InstructorID);
            
            AddColumn("dbo.Course", "Instructor_ID", c => c.Int());
            CreateIndex("dbo.Course", "Instructor_ID");
            AddForeignKey("dbo.Course", "Instructor_ID", "dbo.Instructor", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OfficeAssignment", "InstructorID", "dbo.Instructor");
            DropForeignKey("dbo.Course", "Instructor_ID", "dbo.Instructor");
            DropIndex("dbo.OfficeAssignment", new[] { "InstructorID" });
            DropIndex("dbo.Course", new[] { "Instructor_ID" });
            DropColumn("dbo.Course", "Instructor_ID");
            DropTable("dbo.OfficeAssignment");
            DropTable("dbo.Instructor");
        }
    }
}
