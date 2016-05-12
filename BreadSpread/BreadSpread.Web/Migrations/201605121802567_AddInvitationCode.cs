namespace BreadSpread.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvitationCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invitations", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invitations", "Code");
        }
    }
}
