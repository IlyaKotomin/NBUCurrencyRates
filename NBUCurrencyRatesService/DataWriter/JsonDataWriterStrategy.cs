using NBUCurrencyRatesService.API;
using Newtonsoft.Json;

namespace NBUCurrencyRatesService.DataWriter;

public class JsonDataWriterStrategy(ILogger logger) : IDataWriterStrategy
{
    /// <summary>
    ///     Saves currency rates to a file in the .json file.
    /// </summary>
    /// <param name="rates">Collection of currency rates to save.</param>
    /// <param name="path">Path where the output file will be saved.</param>
    /// <param name="fileName">Name of the output file.</param>
    public void Save(IEnumerable<CurrencyRate> rates, string path, string fileName)
    {
        var fullPath = Path.Combine(path, fileName + ".json");
        var json = JsonConvert.SerializeObject(rates, Formatting.Indented);
        File.WriteAllText(fullPath, json);
        logger.LogInformation("Saved to: {path}", fullPath);
    }
}