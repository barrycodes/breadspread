namespace BreadSpread.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGroupOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "OwnerUser_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Groups", "OwnerUser_Id");
            AddForeignKey("dbo.Groups", "OwnerUser_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groups", "OwnerUser_Id", "dbo.Users");
            DropIndex("dbo.Groups", new[] { "OwnerUser_Id" });
            DropColumn("dbo.Groups", "OwnerUser_Id");
        }
    }
}
