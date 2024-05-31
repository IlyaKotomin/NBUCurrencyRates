using System.Globalization;
using NBUCurrencyRatesService.API;
using NBUCurrencyRatesService.Configuration;

namespace NBUCurrencyRatesService;

// ReSharper disable once SuggestBaseTypeForParameterInConstructor
public class Worker(ILogger<Worker> logger, IConfiguration configuration) : BackgroundService
{
    private readonly Config _config = new(configuration, logger);
    private readonly NBUGrabber _grabber = new(logger);
    private readonly RatesWriter _writer = new(logger);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _config.Reload();

            var rates = await _grabber.FetchRates();

            if (rates != null) _writer.Save(rates, _config.OutputPath, _config.OutputFileName, _config.FileType);

            logger.LogInformation("Loop done: {Date}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            await Task.Delay(_config.FetchFrequency, stoppingToken);
        }
    }
}