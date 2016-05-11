namespace BreadSpread.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvitationEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invitations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Group_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .Index(t => t.Group_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invitations", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Invitations", new[] { "Group_Id" });
            DropTable("dbo.Invitations");
        }
    }
}
