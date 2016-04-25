using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace BreadSpread.Web.Models
{
	public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationIdentityDbContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{
		}

		public static ApplicationIdentityDbContext Create()
		{
			return new ApplicationIdentityDbContext();
		}
	}
}