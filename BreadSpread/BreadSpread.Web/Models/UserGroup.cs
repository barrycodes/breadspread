using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadSpread.Web.Models
{
	public class UserGroup
	{
		[Key]
		public string UserId { get; set; }

		[Key]
		public string GroupId { get; set; }
	}
}
