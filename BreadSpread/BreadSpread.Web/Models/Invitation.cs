using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BreadSpread.Web.Models
{
	public class Invitation
	{
		public long Id { get; set; }

		public Guid Token { get; set; }

		public virtual UserGroup Group { get; set; }

		public virtual ApplicationUser User { get; set; }
	}

	public class CreateInviteViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name="Email address of the user to invite")]
		public string Email { get; set; }

		public UserGroup Group { get; set; }

	}
}