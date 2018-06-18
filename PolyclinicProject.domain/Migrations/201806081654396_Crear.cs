namespace PolyclinicProject.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Crear : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(maxLength: 150),
                        ClaimValue = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Role", t => t.RoleId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Personal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserInfoId = c.Int(nullable: false),
                        PolyclinicId = c.Int(nullable: false),
                        PositionId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Polyclinic", t => t.PolyclinicId)
                .ForeignKey("dbo.Position", t => t.PositionId)
                .ForeignKey("dbo.User", t => t.UserInfoId)
                .Index(t => t.UserInfoId)
                .Index(t => t.PolyclinicId)
                .Index(t => t.PositionId);
            
            CreateTable(
                "dbo.Polyclinic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Address = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Position",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Schedule",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonalId = c.Int(nullable: false),
                        Cabinet = c.Int(nullable: false),
                        Even = c.Boolean(nullable: false),
                        IsFirstShift = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Personal", t => t.PersonalId)
                .Index(t => t.PersonalId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 30),
                        LastName = c.String(nullable: false, maxLength: 30),
                        SurName = c.String(nullable: false, maxLength: 30),
                        Sex = c.Int(nullable: false),
                        Birthday = c.DateTime(nullable: false),
                        Password = c.String(maxLength: 20),
                        PhoneNumber = c.String(nullable: false, maxLength: 50),
                        RoleInfoId = c.Int(nullable: false),
                        Address = c.String(nullable: false, maxLength: 100),
                        DateRegistration = c.DateTime(),
                        DateLogIn = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(maxLength: 500),
                        SecurityStamp = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Role", t => t.RoleInfoId)
                .Index(t => t.RoleInfoId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.User", "RoleInfoId", "dbo.Role");
            DropForeignKey("dbo.Personal", "UserInfoId", "dbo.User");
            DropForeignKey("dbo.UserLogin", "UserId", "dbo.User");
            DropForeignKey("dbo.UserClaim", "UserId", "dbo.User");
            DropForeignKey("dbo.Schedule", "PersonalId", "dbo.Personal");
            DropForeignKey("dbo.Personal", "PositionId", "dbo.Position");
            DropForeignKey("dbo.Personal", "PolyclinicId", "dbo.Polyclinic");
            DropIndex("dbo.Role", "RoleNameIndex");
            DropIndex("dbo.User", "UserNameIndex");
            DropIndex("dbo.User", new[] { "RoleInfoId" });
            DropIndex("dbo.Schedule", new[] { "PersonalId" });
            DropIndex("dbo.Personal", new[] { "PositionId" });
            DropIndex("dbo.Personal", new[] { "PolyclinicId" });
            DropIndex("dbo.Personal", new[] { "UserInfoId" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.UserLogin", new[] { "UserId" });
            DropIndex("dbo.UserClaim", new[] { "UserId" });
            DropTable("dbo.Role");
            DropTable("dbo.User");
            DropTable("dbo.Schedule");
            DropTable("dbo.Position");
            DropTable("dbo.Polyclinic");
            DropTable("dbo.Personal");
            DropTable("dbo.UserRole");
            DropTable("dbo.UserLogin");
            DropTable("dbo.UserClaim");
        }
    }
}
