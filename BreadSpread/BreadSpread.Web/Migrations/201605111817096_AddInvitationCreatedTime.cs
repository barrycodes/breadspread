namespace BreadSpread.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvitationCreatedTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invitations", "CreatedTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invitations", "CreatedTime");
        }
    }
}
