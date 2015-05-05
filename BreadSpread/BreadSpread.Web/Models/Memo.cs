using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreadSpread.Web.Models
{
	public class Memo
	{
		public long Id { get; set; }

		public string Message { get; set; }

		public string PhotoFilePath { get; set; }

		public virtual ApplicationUser Author { get; set; }
	}
}