using FA_BaseTemplate.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FA_BaseTemplate
{
	public class Account_Function
	{
		private readonly ILogger logger;
		private readonly AccountInformationRepository accountRepo;

		public Account_Function(
			ILoggerFactory loggerFactory, 
			AccountInformationRepository accountRepo)
		{
			this.logger = loggerFactory.CreateLogger<Account_Function>();
			this.accountRepo = accountRepo;
		}

		[Function("AccountInfo_Function")]
		public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
		{
			try
			{
				logger.LogInformation($"Timer trigger function started at: {DateTime.UtcNow}");

				if (myTimer.ScheduleStatus is not null)
				{
					logger.LogInformation($"Next timer execution at: {myTimer.ScheduleStatus.Next}");
					await accountRepo.ProcessAccountInformation();
					logger.LogInformation("Account processing completed successfully.");
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Timer function failed.");
				throw;
			}
		}
	}
}
