namespace BootCampApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDepartmentCoursesInstructors : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Course", "Instructor_ID", "dbo.Instructor");
            DropIndex("dbo.Course", new[] { "Instructor_ID" });
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        InstructorID = c.Int(),
                    })
                .PrimaryKey(t => t.DepartmentID)
                .ForeignKey("dbo.Instructor", t => t.InstructorID)
                .Index(t => t.InstructorID);
            
            CreateTable(
                "dbo.CourseInstructor",
                c => new
                    {
                        CourseID = c.Int(nullable: false),
                        InstructorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CourseID, t.InstructorID })
                .ForeignKey("dbo.Course", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Instructor", t => t.InstructorID, cascadeDelete: true)
                .Index(t => t.CourseID)
                .Index(t => t.InstructorID);

            //eskase to update database edw
            //create a default department
            Sql("INSERT INTO dbo.Department (Name, Budget, StartDate) VALUES ('Default', 0, GETDATE())");
            //ADD Default value for non nullable Column
            AddColumn("dbo.Course", "DepartmentID", c => c.Int(nullable: false, defaultValue: 1));
            AlterColumn("dbo.Course", "Title", c => c.String(maxLength: 50));
            CreateIndex("dbo.Course", "DepartmentID");
            AddForeignKey("dbo.Course", "DepartmentID", "dbo.Department", "DepartmentID", cascadeDelete: true);
            DropColumn("dbo.Course", "Instructor_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Course", "Instructor_ID", c => c.Int());
            DropForeignKey("dbo.CourseInstructor", "InstructorID", "dbo.Instructor");
            DropForeignKey("dbo.CourseInstructor", "CourseID", "dbo.Course");
            DropForeignKey("dbo.Course", "DepartmentID", "dbo.Department");
            DropForeignKey("dbo.Department", "InstructorID", "dbo.Instructor");
            DropIndex("dbo.CourseInstructor", new[] { "InstructorID" });
            DropIndex("dbo.CourseInstructor", new[] { "CourseID" });
            DropIndex("dbo.Department", new[] { "InstructorID" });
            DropIndex("dbo.Course", new[] { "DepartmentID" });
            AlterColumn("dbo.Course", "Title", c => c.String());
            DropColumn("dbo.Course", "DepartmentID");
            DropTable("dbo.CourseInstructor");
            DropTable("dbo.Department");
            CreateIndex("dbo.Course", "Instructor_ID");
            AddForeignKey("dbo.Course", "Instructor_ID", "dbo.Instructor", "ID");
        }
    }
}
