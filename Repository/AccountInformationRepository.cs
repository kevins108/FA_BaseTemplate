using FA_BaseTemplate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FA_BaseTemplate.Repository
{
	public class AccountInformationRepository
	{
		private readonly IConfiguration configs;
		private readonly DataContext context;

		public AccountInformationRepository(IConfiguration configs, DataContext context)
		{
			this.configs = configs;
			this.context = context;
		}

		public async Task ProcessAccountInformation()
		{
			// Get Key value from Azure
			var key = configs["ApplicationSettings:KEY"];

			try
			{
				var users = await (from up in context.UserProfile
								   join ua in context.UserAddress on up.ProfileID equals ua.ProfileID into x
								   from upa in x.DefaultIfEmpty()
								   select new
								   {
									   up,
									   upa
								   }).ToListAsync();

				foreach (var user in users)
				{
					// execute something here
				}

			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}


		public void LogError(Exception ex)
		{
			throw new Exception($"Exception Message: {ex}");
		}
	}
}
