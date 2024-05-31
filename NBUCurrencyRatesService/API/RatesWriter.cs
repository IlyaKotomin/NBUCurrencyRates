using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NBUCurrencyRatesService.Configuration;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace NBUCurrencyRatesService.API;

public class RatesWriter(ILogger logger)
{
    public void Save(IEnumerable<CurrencyRate> rates, string path, string fileName, OutputType fileType)
    {
        if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(fileName))
        {
            logger.LogError("Config exception!\nPlease, configure config.json in {appPath}", AppContext.BaseDirectory);
            return;
        }

        switch (fileType)
        {
            case OutputType.json:
                SaveJson(rates, path, fileName);
                break;
            case OutputType.csv:
                SaveCsv(rates, path, fileName);
                break;
            case OutputType.xml:
                SaveXml(rates, path, fileName);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
        }
    }

    private void SaveXml(IEnumerable rates, string path, string fileName)
    {
        var fullPath = Path.Combine(path, fileName + ".xml");
        var xmlSerializer = new XmlSerializer(typeof(List<CurrencyRate>));

        using (var writer = XmlWriter.Create(fullPath, new XmlWriterSettings { Indent = true }))
            xmlSerializer.Serialize(writer, rates);

        logger.LogInformation("Saved to: {path}", fullPath);
    }

    private void SaveCsv(IEnumerable<CurrencyRate> rates, string path, string fileName)
    {
        var fullPath = Path.Combine(path, fileName + ".csv");
        var csv = new StringBuilder();
        csv.AppendLine("R030,FullName,CurrencyCode,Rate,ExchangeDate");
        foreach (var rate in rates)
            csv.AppendLine($"{rate.R030},{rate.FullName},{rate.CurrencyCode},{rate.Rate},{rate.ExchangeDate}");

        File.WriteAllText(fullPath, csv.ToString());
        logger.LogInformation("Saved to: {path}", fullPath);
    }

    private void SaveJson(IEnumerable rates, string path, string fileName)
    {
        var fullPath = Path.Combine(path, fileName + ".json");
        var json = JsonConvert.SerializeObject(rates, Formatting.Indented);
        File.WriteAllText(fullPath, json);
        logger.LogInformation("Saved to: {path}", fullPath);
    }
}