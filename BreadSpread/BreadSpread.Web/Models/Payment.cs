using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BreadSpread.Web.Models
{
	public class Payment
	{
		public long Id { get; set; }

		[Required]
		public DateTime Time { get; set; }

		[Required]
		public virtual ApplicationUser FromUser { get; set; }

		[Required]
		public virtual ApplicationUser ToUser { get; set; }

		public virtual ICollection<Memo> Memos { get; set; }

	}
}