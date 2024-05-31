namespace NBUCurrencyRatesService.Configuration;

public class Config
{
    private readonly ILogger _logger;
    private IConfiguration _configuration;
    public int FetchFrequency;
    public OutputType FileType;
    public string OutputFileName;
    public string OutputPath;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Config(IConfiguration configuration, ILogger logger)
    {
        _logger = logger;
        Initialize(configuration);
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public void Reload()
    {
        _logger.LogInformation("Reloading config...");
        Initialize(GetDefaultConfiguration());
    }

    public static IConfiguration GetDefaultConfiguration()
    {
        return new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("config.json")
            .Build();
    }

    private void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
        FetchFrequency = int.Parse(_configuration["FetchFrequency"] ?? string.Empty);

        OutputPath = string.IsNullOrEmpty(_configuration["OutputPath"]) 
            ? AppContext.BaseDirectory : _configuration["OutputPath"]!;
        OutputFileName = string.IsNullOrEmpty(_configuration["OutputFileName"]) 
            ? "Rates" : _configuration["OutputFileName"]!;
        
        Enum.TryParse(_configuration["FileType"], out FileType);
    }
}