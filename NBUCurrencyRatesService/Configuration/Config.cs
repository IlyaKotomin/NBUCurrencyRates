namespace NBUCurrencyRatesService.Configuration;

/// <summary>
///     Class responsible for managing service configuration settings.
/// </summary>
public class Config
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    ///     Initializes a new instance of the Config class with the specified configuration and logger.
    /// </summary>
    /// <param name="configuration">Configuration instance for managing service settings.</param>
    /// <param name="logger">Logger instance for logging messages.</param>
    public Config(IConfiguration configuration, ILogger logger)
    {
        _logger = logger;
        Initialize(configuration);
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    #region Fields

    /// <summary>
    ///     Logger instance for logging messages.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    ///     Configuration instance for managing application settings.
    /// </summary>
    private IConfiguration _configuration;

    /// <summary>
    ///     Frequency at which currency rates are fetched, in milliseconds.
    /// </summary>
    public int FetchFrequency;

    /// <summary>
    ///     Type of output file for saving currency rates.
    /// </summary>
    public OutputType FileType;

    /// <summary>
    ///     Name of the output file for saving currency rates.
    /// </summary>
    public string OutputFileName;

    /// <summary>
    ///     Path where the output file for saving currency rates is located.
    /// </summary>
    public string OutputPath;

    #endregion


    #region Methods

    /// <summary>
    ///     Reloads the configuration settings.
    /// </summary>
    public void Reload()
    {
        _logger.LogInformation("Reloading config...");
        Initialize(GetDefaultConfiguration());
    }

    /// <summary>
    ///     Gets the default configuration settings.
    /// </summary>
    /// <returns>Default configuration settings.</returns>
    public static IConfiguration GetDefaultConfiguration()
    {
        return new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("config.json")
            .Build();
    }

    /// <summary>
    ///     Initializes the configuration settings.
    /// </summary>
    /// <param name="configuration">Configuration instance for service application settings.</param>
    private void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
        FetchFrequency = int.Parse(_configuration["FetchFrequency"] ?? string.Empty);

        OutputPath = string.IsNullOrEmpty(_configuration["OutputPath"])
            ? AppContext.BaseDirectory
            : _configuration["OutputPath"]!;
        OutputFileName = string.IsNullOrEmpty(_configuration["OutputFileName"])
            ? "Rates"
            : _configuration["OutputFileName"]!;

        Enum.TryParse(_configuration["FileType"], out FileType);
    }

    #endregion
}