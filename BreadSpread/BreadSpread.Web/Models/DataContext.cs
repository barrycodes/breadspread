using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace BreadSpread.Web.Models
{
	public class DataContext : IdentityDbContext<ApplicationUser>
	{
		public ICollection<Expense> Expenses { get; set; }

		public ICollection<Memo> Memos { get; set; }

		public ICollection<Payment> Payments { get; set; }

		public ICollection<UserGroup> Groups { get; set; }

		public ICollection<Invitation> Invitations { get; set; }

		public DataContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{
		}

		public static DataContext Create()
		{
			return new DataContext();
		}

		public System.Data.Entity.DbSet<BreadSpread.Web.Models.UserGroup> UserGroups { get; set; }
	}
}