using Newtonsoft.Json;

namespace NBUCurrencyRatesService.API;

// ReSharper disable once InconsistentNaming
public class NBUGrabber(ILogger logger)
{
    private const string ApiUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";

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