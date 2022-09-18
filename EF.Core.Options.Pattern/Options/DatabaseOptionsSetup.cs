using Microsoft.Extensions.Options;

namespace EF.Core.Options.Pattern.Options
{
    public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
    {
        private const string ConfigurationSectionName = "DatabaseOptions";
        private readonly IConfiguration _configuration;

        public DatabaseOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(DatabaseOptions options)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            options.ConnectionString = connectionString;
            _configuration.GetSection(ConfigurationSectionName).Bind(options);
        }
    }
}
