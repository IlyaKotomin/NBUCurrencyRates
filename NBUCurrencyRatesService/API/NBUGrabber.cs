using Newtonsoft.Json;

namespace NBUCurrencyRatesService.API;

/// <summary>
///     Class responsible for fetching currency rates from the NBU (National Bank of Ukraine) API.
/// </summary>
/// <param name="logger">Logger instance for logging messages.</param>
// ReSharper disable once InconsistentNaming
public class NBUGrabber(ILogger logger)
{
    /// <summary>
    ///     URL of the NBU API for fetching currency rates.
    /// </summary>
    private const string ApiUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";


    /// <summary>
    ///     Fetches currency rates from the NBU API.
    /// </summary>
    /// <returns>List of currency rates fetched from the NBU API, or null if an error occurs.</returns>
    public async Task<List<CurrencyRate>?> FetchRates()
    {
        logger.LogInformation("Fetching data from: {url}", ApiUrl);
        try
        {
            using var httpClient = new HttpClient();
            var rawData = await httpClient.GetStringAsync(ApiUrl);
            return JsonConvert.DeserializeObject<List<CurrencyRate>>(rawData);
        }
        catch (Exception e)
        {
            logger.LogError("Error! Can not get data from {url}\nError: {exception}", ApiUrl, e);
            return null;
        }
    }
}