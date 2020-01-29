namespace UserPortalWebService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePasswordModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangePasswords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OldPassword = c.String(nullable: false),
                        NewPassword = c.String(nullable: false),
                        ConfirmNewPassword = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChangePasswords");
        }
    }
}
