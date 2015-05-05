using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BreadSpread.Web.Models
{
	public class UserGroup
	{
		public long Id { get; set; }
		
		[Required]
		[StringLength(60, MinimumLength=3)]
		public string Name { get; set; }
		
		public string PhotoFilePath { get; set; }

		public virtual ICollection<ApplicationUser> Users { get; set; }

		public virtual ICollection<Memo> Memos { get; set; }

		[Required]
		public virtual ApplicationUser Owner { get; set; }
	}

	public class CreateGroupViewModel
	{
		[Required]
		[StringLength(60, MinimumLength = 3)]
		public string Name { get; set; }

		[Required]
		public string OwnerId { get; set; }
	}
}