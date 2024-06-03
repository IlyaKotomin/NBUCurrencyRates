using System.Text;
using NBUCurrencyRatesService.API;

namespace NBUCurrencyRatesService.DataWriter;

public class CsvDataWriterStrategy(ILogger logger) : IDataWriterStrategy
{
    /// <summary>
    ///     Saves currency rates to a file in the .csv file.
    /// </summary>
    /// <param name="rates">Collection of currency rates to save.</param>
    /// <param name="path">Path where the output file will be saved.</param>
    /// <param name="fileName">Name of the output file.</param>
    public void Save(IEnumerable<CurrencyRate> rates, string path, string fileName)
    {
        var fullPath = Path.Combine(path, fileName + ".csv");
        var csv = new StringBuilder();
        csv.AppendLine("R030,FullName,CurrencyCode,Rate,ExchangeDate");
        foreach (var rate in rates)
            csv.AppendLine($"{rate.R030},{rate.FullName},{rate.CurrencyCode},{rate.Rate},{rate.ExchangeDate}");

        File.WriteAllText(fullPath, csv.ToString());
        logger.LogInformation("Saved to: {path}", fullPath);
    }
}