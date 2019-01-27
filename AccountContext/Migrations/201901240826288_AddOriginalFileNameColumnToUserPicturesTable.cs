namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOriginalFileNameColumnToUserPicturesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserPictures", "OriginalFileName", c => c.String(maxLength: 60));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserPictures", "OriginalFileName");
        }
    }
}
