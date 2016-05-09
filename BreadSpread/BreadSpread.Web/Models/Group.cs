using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadSpread.Web.Models
{
	public class Group
	{
		[Key]
		public string Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string PhotoId { get; set; }

		[Required]
		public DateTime CreatedTime { get; set; }

		public virtual User OwnerUser { get; set; }

		public virtual ICollection<User> Users { get; set; }
	}
}
