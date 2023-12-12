using FA_BaseTemplate.Data;
using FA_BaseTemplate.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
	.ConfigureAppConfiguration(builder =>
	{
		builder.AddAzureAppConfiguration(options =>
		{
			var connectionString =
				options.Connect(Environment.GetEnvironmentVariable("CONNECTION_STRING"))
						.Select("*")
						.ConfigureRefresh(refresh =>
						{
							refresh.Register("~REFRESH_ALL", refreshAll: true).SetCacheExpiration(TimeSpan.FromSeconds(30));
						})
						.UseFeatureFlags(flags => flags.CacheExpirationInterval = TimeSpan.FromSeconds(30));
		});
	})
	.ConfigureServices(services =>
	{
		services.AddAzureAppConfiguration();

		// Add Repositories
		services.AddScoped<AccountInformationRepository>();

		// Add DB Connection
		var build = services.BuildServiceProvider();
		var config = build.GetService<IConfiguration>();
		services.AddDbContext<DataContext>(options => options.UseSqlServer(config?["DATABASE"]));

	})
	.ConfigureFunctionsWorkerDefaults(app =>
	{
		// Use Azure App Configuration middleware for data refresh.
		app.UseAzureAppConfiguration();
	})
	.Build();

host.Run();
