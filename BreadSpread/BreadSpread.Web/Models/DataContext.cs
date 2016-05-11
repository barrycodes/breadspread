using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;

namespace BreadSpread.Web.Models
{
	public class ApplicationIdentityDbContext : IdentityDbContext<User>
	{
		public DbSet<Group> Groups { get; set; }

		public DbSet<Invitation> Invitations { get; set; }

		static ApplicationIdentityDbContext()
		{
			// Uncomment below line to erase database upon application start
			//Database.SetInitializer(new DropCreateDatabaseAlways<Models.ApplicationIdentityDbContext>());
		}

		public ApplicationIdentityDbContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{
		}

		public static ApplicationIdentityDbContext Create()
		{
			return new ApplicationIdentityDbContext();
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>().ToTable("Users");
			modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
			modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
			modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
			modelBuilder.Entity<IdentityRole>().ToTable("Roles");

			//modelBuilder.Entity<Group>()
			//	.HasRequired(g => g.OwnerUser)
			//	.WithMany(u => u.Groups)
			//	.HasForeignKey(g => g.OwnerUserId);

			//modelBuilder.Entity<Group>()
			//	.HasMany(g => g.Users)
			//	.WithMany(u => u.Groups)
			//	.Map(m =>
			//	{
			//		m.ToTable("UserGroups");
			//		m.MapLeftKey("GroupId");
			//		m.MapRightKey("UserId");
			//	});

			modelBuilder.Entity<User>()
				.HasMany(u => u.OwnedGroups)
				.WithRequired(g => g.OwnerUser)
				.WillCascadeOnDelete(false);

			//modelBuilder.Entity<Group>()
			//	.HasRequired(g => g.OwnerUser)
			//	.WithMany(u => u.OwnedGroups);
		}
	}
}