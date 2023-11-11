using FA_BaseTemplate.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FA_BaseTemplate
{
	public class Account_Function
	{
		private readonly ILogger logger;
		private readonly AccountInformationRepository accountRepo;

		public Account_Function(ILoggerFactory loggerFactory, AccountInformationRepository accountRepo)
		{
			this.logger = loggerFactory.CreateLogger<Account_Function>();
			this.accountRepo = accountRepo;
		}

		[Function("AccountInfo_Function")]
		public async Task Run([TimerTrigger("* * * * *")] TimerInfo myTimer)
		{
			logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

			if (myTimer.ScheduleStatus is not null)
			{
				logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

				await accountRepo.ProcessAccountInformation();
			}
		}

	}
}
