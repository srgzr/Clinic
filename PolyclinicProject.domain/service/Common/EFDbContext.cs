using Microsoft.AspNet.Identity.EntityFramework;
using PolyclinicProject.Domain.Abstract;
using PolyclinicProject.Domain.Entities;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PolyclinicProject.Domain.Service.Common
{
    public class EFDbContext : IdentityDbContext<UserInfo, RoleInfo, int,
        AppUserLogin, AppUserRole, AppUserClaim>, IDbContext
    {
        public EFDbContext() : base("name=PoloclinicDBEntities")
        {
            Database.SetInitializer<EFDbContext>(null);
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
        #region

       
        public static EFDbContext Create()
        {
            return new EFDbContext();
        }
        public override IDbSet<UserInfo> Users { get; set; }
        public DbSet<AppUserLogin> ApplicationUserLogins { get; set; }
        public DbSet<AppUserRole> ApplicationUserRoles { get; set; }
        public DbSet<AppUserClaim> AppApplicationUserClaims { get; set; }
        public override IDbSet<RoleInfo> Roles { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Personal> Personals { get; set; }
        public DbSet<Polyclinic> Polyclinics { get; set; }

        public DbQuery<T> Query<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            // Configure Asp Net Identity Tables
            modelBuilder.Entity<UserInfo>().ToTable("User");
            modelBuilder.Entity<UserInfo>().Property(u => u.PasswordHash).HasMaxLength(500);
            modelBuilder.Entity<UserInfo>().Property(u => u.PhoneNumber).HasMaxLength(50);
            modelBuilder.Entity<RoleInfo>().ToTable("Role");
            modelBuilder.Entity<AppUserRole>().ToTable("UserRole");
            modelBuilder.Entity<AppUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<AppUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<AppUserClaim>().Property(u => u.ClaimType).HasMaxLength(150);
            modelBuilder.Entity<AppUserClaim>().Property(u => u.ClaimValue).HasMaxLength(500);

            modelBuilder.Entity<Polyclinic>()
                .HasMany(e => e.Personals)
                .WithRequired(e => e.Polyclinic)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Position>()
                .HasMany(e => e.Personals)
                .WithRequired(e => e.Position)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.Personals)
                .WithRequired(e => e.UserInfo)
                .HasForeignKey(e => e.UserInfoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Personal>()
                .HasMany(e => e.Schedules)
                .WithRequired(e => e.Personal)
                .HasForeignKey(e => e.PersonalId)
                .WillCascadeOnDelete(false);
        }

        #endregion
    }
}