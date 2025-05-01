namespace BranchManagementApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchName = c.String(nullable: false),
                        PersonName = c.String(nullable: false),
                        Age = c.Int(nullable: false),
                        MobileNumber = c.String(nullable: false),
                        CompleteAddress = c.String(nullable: false),
                        Profession = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Branches");
        }
    }
}
