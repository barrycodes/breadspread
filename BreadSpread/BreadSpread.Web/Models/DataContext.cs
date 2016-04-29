using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;

namespace BreadSpread.Web.Models
{
	public class ApplicationIdentityDbContext : IdentityDbContext<User>
	{

		static ApplicationIdentityDbContext()
		{
			// Uncomment below line to erase database upon application start
			Database.SetInitializer(new DropCreateDatabaseAlways<Models.ApplicationIdentityDbContext>());
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
		}
	}
}