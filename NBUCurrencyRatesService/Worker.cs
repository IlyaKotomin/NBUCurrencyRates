using System.Globalization;
using NBUCurrencyRatesService.API;
using NBUCurrencyRatesService.Configuration;

namespace NBUCurrencyRatesService;

/// <summary>
///     Worker class responsible for continuously fetching currency rates from NBU (National Bank of Ukraine) API,
///     and saving them to a specified file location.
/// </summary>
/// <param name="logger">Logger instance for logging messages.</param>
/// <param name="configuration">Configuration instance for managing application settings.</param>

// ReSharper disable once SuggestBaseTypeForParameterInConstructor
public class Worker(ILogger<Worker> logger, IConfiguration configuration) : BackgroundService
{
    
    #region Fields

    /// <summary>
    ///     Configuration instance used to manage application configuration settings.
    /// </summary>
    private readonly Config _config = new(configuration, logger);

    /// <summary>
    ///     Class responsible for grabbing currency rates from the NBU API.
    /// </summary>
    private readonly NBUGrabber _grabber = new(logger);

    /// <summary>
    ///     Class responsible for writing currency rates to a file.
    /// </summary>
    private readonly API.DataWriter _writer = new(logger);

    #endregion
    
    #region Methods

    /// <summary>
    ///     Executes the worker task asynchronously.
    /// </summary>
    /// <param name="stoppingToken">Cancellation token to stop the task.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _config.Reload();

            var rates = await _grabber.FetchRates();

            if (rates != null) 
                _writer.Save(rates, _config.OutputPath, _config.OutputFileName, _config.FileType);

            logger.LogInformation("Loop done: {Date}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            await Task.Delay(_config.FetchFrequency, stoppingToken);
        }
    }

    #endregion
}