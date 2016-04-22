using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BreadSpread.Web.Models
{
	public class Expense
	{
		public long Id { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime Time { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string Title { get; set; }

		[Required]
		public virtual UserGroup Group { get; set; }

		[Required]
		public virtual ICollection<ApplicationUser> Participants { get; set; }

		[Required]
		public virtual ApplicationUser Spender { get; set; }
	}
}