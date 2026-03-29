using FA_BaseTemplate.Configuration;
using FA_BaseTemplate.Data;
using FA_BaseTemplate.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var host = new HostBuilder()
	.ConfigureAppConfiguration(builder =>
	{
		builder.AddAzureAppConfiguration(options =>
		{
			var connectionString =
				options.Connect(Environment.GetEnvironmentVariable("AZURE_APP_CONFIG_CONNECTION_STRING"))
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

		// Add Logging
		services.AddLogging();

		// Register AppSettings options
		var build = services.BuildServiceProvider();
		var config = build.GetService<IConfiguration>();
		
		var appSettings = new AppSettings
		{
			DatabaseSettings = new DatabaseSettings
			{
				ConnectionString = config?["DATABASE"]
			},
			ApplicationSettings = new ApplicationSettings
			{
				Key = config?["ApplicationSettings:KEY"]
			}
		};

		if (string.IsNullOrEmpty(appSettings.DatabaseSettings?.ConnectionString))
		{
			throw new InvalidOperationException("DATABASE configuration not found in App Configuration.");
		}

		if (string.IsNullOrEmpty(config?["AZURE_APP_CONFIG_CONNECTION_STRING"]))
		{
			throw new InvalidOperationException("CONNECTION_STRING environment variable not set.");
		}

		services.Configure<AppSettings>(options =>
		{
			options.DatabaseSettings = appSettings.DatabaseSettings;
			options.ApplicationSettings = appSettings.ApplicationSettings;
		});

		// Add Repositories
		services.AddScoped<AccountInformationRepository>();

		// Add DB Connection
		services.AddDbContext<DataContext>(options => options.UseSqlServer(appSettings.DatabaseSettings?.ConnectionString));

	})
	.ConfigureFunctionsWorkerDefaults(app =>
	{
		// Use Azure App Configuration middleware for data refresh.
		app.UseAzureAppConfiguration();
	})
	.Build();

host.Run();
