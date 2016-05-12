using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadSpread.Web.Models
{
	public class InvitationViewModel
	{
		public string InvitationId { get; set; }

		public string GroupId { get; set; }

		public string GroupName { get; set; }

		public string OwnerName { get; set; }
	}
}
