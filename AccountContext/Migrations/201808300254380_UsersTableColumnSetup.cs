namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsersTableColumnSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 50, nullable: false),
                        UserName = c.String(maxLength: 256, nullable:false),
                        Password = c.String(maxLength: 25, nullable: false),
                        AddressLine1 = c.String(maxLength: 50),
                        AddressLine2 = c.String(maxLength: 50),
                        Country = c.String(maxLength: 55),
                        City = c.String(),
                        State = c.String(maxLength: 2),
                        ZipCode = c.String(maxLength: 5),
                        BirthDate = c.DateTime(nullable: false),
                        SignupDate = c.DateTime(nullable: false),
                        SmallBio = c.String(maxLength:256),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
