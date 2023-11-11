using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FA_BaseTemplate.Data
{
	public class UserAddress
	{
		[Key]
		public int AddressID { get; set; }
		public int ProfileID { get; set; }
		[MaxLength(50)]
		public string? Address { get; set; }
		[MaxLength(50)]
		public string? City { get; set; }
		[MaxLength(25)]
		public string? PostalCode { get; set; }
		[MaxLength(25)]
		public string? State { get; set; }
		[MaxLength(25)]
		public string? Country { get; set; }
		public bool Primary { get; set; }

		[ForeignKey("ProfileID")]
		public virtual UserProfile? UserProfile { get; set; }

	}
}
