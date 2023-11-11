using Microsoft.EntityFrameworkCore;

namespace FA_BaseTemplate.Data
{
	public partial class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		public virtual DbSet<UserProfile> UserProfile { get; set; }
		public virtual DbSet<UserAddress> UserAddress { get; set; }

	}
}
