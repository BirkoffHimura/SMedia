namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserPostTableDefinition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserPosts",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Subject = c.String(maxLength: 25),
                        Post_Body = c.String(nullable: false, maxLength: 256),
                        Img_Extern = c.String(),
                        UserPicture_PictureID = c.Long(),
                        User_ID = c.Long(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserPictures", t => t.UserPicture_PictureID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.UserPicture_PictureID)
                .Index(t => t.User_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPosts", "User_ID", "dbo.Users");
            DropForeignKey("dbo.UserPosts", "UserPicture_PictureID", "dbo.UserPictures");
            DropIndex("dbo.UserPosts", new[] { "User_ID" });
            DropIndex("dbo.UserPosts", new[] { "UserPicture_PictureID" });
            DropTable("dbo.UserPosts");
        }
    }
}
