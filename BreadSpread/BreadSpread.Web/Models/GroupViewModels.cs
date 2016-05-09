using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadSpread.Web.Models
{
	public class GroupViewModel
	{
		public string Id { get; set; }

		[Display(Name="Group Name")]
		public string Name { get; set; }

		public string PhotoId { get; set; }

		[Display(Name="Created")]
		[DisplayFormat(DataFormatString = "{0:MMM dd yyyy}")]
		public DateTime CreatedTime { get; set; }

		[Display(Name="Owner")]
		public string OwnerName { get; set; }

		[Display(Name="Users")]
		public int UserCount { get; set; }

		public bool IsOwner { get; set; }
	}

	public class GroupUserViewModel
	{
		public string Name { get; set; }

		public bool IsOwner { get; set; }
	}

	public class GroupDetailViewModel : GroupViewModel
	{
		public GroupUserViewModel[] Users { get; set; }
	}
}
