using System.Xml.Linq;

namespace BookstoreApi.Data
{
    public static class XmlHelper
    {
        public static async Task<XDocument> LoadDocumentAsync(string path)
        {
            if (!File.Exists(path))
            {
                var newDoc = new XDocument(new XElement("bookstore"));
                await using var createStream = File.Create(path);
                await newDoc.SaveAsync(createStream, SaveOptions.None, CancellationToken.None);
            }

            await using var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
        }

        public static async Task SaveDocumentAsync(XDocument document, string path)
        {
            using var stream = File.Create(path);
            await document.SaveAsync(stream, SaveOptions.None, CancellationToken.None);
        }
        public static void SetElementIfNotNull<T>(this XElement parent, string elementName, T? value)
        {
            if (value != null)
                parent.Element(elementName)?.SetValue(value);
        }
        public static void SetElementsFromListIfNotNull(this XElement parent, string elementName, List<string>? values)
        {
            if (values == null || !values.Any())
                return;

            parent.Elements(elementName).Remove();

            foreach (var value in values.Where(v => !string.IsNullOrWhiteSpace(v)))
            {
                parent.Add(new XElement(elementName, value));
            }
        }
    }
}
