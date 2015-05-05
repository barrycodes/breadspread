using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreadSpread.Web.Models
{
	public class Expense
	{
		public long Id { get; set; }

		public DateTime Time { get; set; }

		public string Title { get; set; }

		public virtual UserGroup Group { get; set; }

		public virtual ICollection<ApplicationUser> Participants { get; set; }

		public virtual ApplicationUser Spender { get; set; }
	}
}