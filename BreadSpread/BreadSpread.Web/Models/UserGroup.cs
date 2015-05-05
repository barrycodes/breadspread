using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreadSpread.Web.Models
{
	public class UserGroup
	{
		public long Id { get; set; }
		
		public string Name { get; set; }
		
		public string PhotoFilePath { get; set; }

		public virtual ICollection<ApplicationUser> Users { get; set; }

		public virtual ICollection<Memo> Memos { get; set; }

	}
}