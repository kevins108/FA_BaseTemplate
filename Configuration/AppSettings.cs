namespace FA_BaseTemplate.Configuration
{
	public class AppSettings
	{
		public DatabaseSettings? DatabaseSettings { get; set; }
		public ApplicationSettings? ApplicationSettings { get; set; }
	}

	public class DatabaseSettings
	{
		public string? ConnectionString { get; set; }
	}

	public class ApplicationSettings
	{
		public string? Key { get; set; }
	}
}
