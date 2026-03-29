using FA_BaseTemplate.Configuration;
using FA_BaseTemplate.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FA_BaseTemplate.Repository
{
	public class AccountInformationRepository
	{
		private readonly IOptions<AppSettings> settings;
		private readonly DataContext context;
		private readonly ILogger<AccountInformationRepository> logger;

		public AccountInformationRepository(IOptions<AppSettings> settings, DataContext context, ILogger<AccountInformationRepository> logger)
		{
			this.settings = settings;
			this.context = context;
			this.logger = logger;
		}

		public async Task ProcessAccountInformation()
		{
			// Get Key value from AppSettings
			var key = settings.Value.ApplicationSettings?.Key;

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
				logger.LogError(ex, "An error occurred while processing account information.");
			}
		}
	}
}
