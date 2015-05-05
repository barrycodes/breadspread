using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreadSpread.Web.Models
{
	public class Payment
	{
		public long Id { get; set; }

		public DateTime Time { get; set; }

		public virtual ApplicationUser FromUser { get; set; }

		public virtual ApplicationUser ToUser { get; set; }

		public virtual ICollection<Memo> Memos { get; set; }

	}
}