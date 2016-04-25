namespace BreadSpread.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateApplicationUser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "PhotoFilePath");
            DropColumn("dbo.AspNetUsers", "DisplayName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "DisplayName", c => c.String());
            AddColumn("dbo.AspNetUsers", "PhotoFilePath", c => c.String());
        }
    }
}
