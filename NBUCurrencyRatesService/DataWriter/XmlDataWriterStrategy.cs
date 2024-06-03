using System.Xml;
using System.Xml.Serialization;
using NBUCurrencyRatesService.API;

namespace NBUCurrencyRatesService.DataWriter;

public class XmlDataWriterStrategy(ILogger logger) : IDataWriterStrategy
{
    /// <summary>
    ///     Saves currency rates to a file in the .xml file.
    /// </summary>
    /// <param name="rates">Collection of currency rates to save.</param>
    /// <param name="path">Path where the output file will be saved.</param>
    /// <param name="fileName">Name of the output file.</param>
    public void Save(IEnumerable<CurrencyRate> rates, string path, string fileName)
    {
        var fullPath = Path.Combine(path, fileName + ".xml");
        var xmlSerializer = new XmlSerializer(typeof(List<CurrencyRate>));

        using (var writer = XmlWriter.Create(fullPath, new XmlWriterSettings { Indent = true }))
        {
            xmlSerializer.Serialize(writer, rates);
        }

        logger.LogInformation("Saved to: {path}", fullPath);
    }
}