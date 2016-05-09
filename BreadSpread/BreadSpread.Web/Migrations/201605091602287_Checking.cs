namespace BreadSpread.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Checking : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserGroups", newName: "GroupUsers");
            RenameColumn(table: "dbo.GroupUsers", name: "GroupId", newName: "Group_Id");
            RenameColumn(table: "dbo.GroupUsers", name: "UserId", newName: "User_Id");
            RenameIndex(table: "dbo.GroupUsers", name: "IX_GroupId", newName: "IX_Group_Id");
            RenameIndex(table: "dbo.GroupUsers", name: "IX_UserId", newName: "IX_User_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.GroupUsers", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.GroupUsers", name: "IX_Group_Id", newName: "IX_GroupId");
            RenameColumn(table: "dbo.GroupUsers", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.GroupUsers", name: "Group_Id", newName: "GroupId");
            RenameTable(name: "dbo.GroupUsers", newName: "UserGroups");
        }
    }
}
