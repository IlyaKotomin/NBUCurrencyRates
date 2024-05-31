using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using NBUCurrencyRatesService;
using NBUCurrencyRatesService.Configuration;

var config = Config.GetDefaultConfiguration();

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options => options.ServiceName = "NBU Currency Rates");

LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);

builder.Configuration.AddConfiguration(config);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();