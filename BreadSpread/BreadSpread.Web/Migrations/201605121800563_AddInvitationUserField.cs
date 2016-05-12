namespace BreadSpread.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvitationUserField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invitations", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Invitations", "User_Id");
            AddForeignKey("dbo.Invitations", "User_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invitations", "User_Id", "dbo.Users");
            DropIndex("dbo.Invitations", new[] { "User_Id" });
            DropColumn("dbo.Invitations", "User_Id");
        }
    }
}
