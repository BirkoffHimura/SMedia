namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserPicturesTableDefinition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserPictures",
                c => new
                    {
                        PictureID = c.Long(nullable: false, identity: true),
                        FileName = c.String(nullable: false, maxLength: 25),
                        ProfilePicture = c.Boolean(nullable: false),
                        User_ID = c.Long(),
                    })
                .PrimaryKey(t => t.PictureID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.User_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPictures", "User_ID", "dbo.Users");
            DropIndex("dbo.UserPictures", new[] { "User_ID" });
            DropTable("dbo.UserPictures");
        }
    }
}
