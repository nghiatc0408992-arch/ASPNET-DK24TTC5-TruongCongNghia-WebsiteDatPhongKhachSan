namespace QLDatPhong.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccoun : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.AccountID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Accounts");
        }
    }
}
