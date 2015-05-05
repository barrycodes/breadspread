namespace BreadSpread.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeContextName : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 60),
                        PhotoFilePath = c.String(),
                        Owner_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id, cascadeDelete: true)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Memos",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Message = c.String(),
                        PhotoFilePath = c.String(),
                        Author_Id = c.String(nullable: false, maxLength: 128),
                        UserGroup_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserGroups", t => t.UserGroup_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.UserGroup_Id);
            
            AddColumn("dbo.AspNetUsers", "UserGroup_Id", c => c.Long());
            CreateIndex("dbo.AspNetUsers", "UserGroup_Id");
            AddForeignKey("dbo.AspNetUsers", "UserGroup_Id", "dbo.UserGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.UserGroups", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Memos", "UserGroup_Id", "dbo.UserGroups");
            DropForeignKey("dbo.Memos", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "UserGroup_Id" });
            DropIndex("dbo.Memos", new[] { "UserGroup_Id" });
            DropIndex("dbo.Memos", new[] { "Author_Id" });
            DropIndex("dbo.UserGroups", new[] { "Owner_Id" });
            DropColumn("dbo.AspNetUsers", "UserGroup_Id");
            DropTable("dbo.Memos");
            DropTable("dbo.UserGroups");
        }
    }
}
