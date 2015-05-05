using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace BreadSpread.Web.Models
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ICollection<Expense> Expenses { get; set; }

		public ICollection<Memo> Memos { get; set; }

		public ICollection<Payment> Payments { get; set; }

		public ICollection<UserGroup> Groups { get; set; }

		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{
		}

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}
	}
}