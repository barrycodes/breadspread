using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadSpread.Web.Models
{
	public class Invitation
	{
		public string Id { get; set; }

		public DateTime CreatedTime { get; set; }

		public virtual Group Group { get; set; }
	}
}
