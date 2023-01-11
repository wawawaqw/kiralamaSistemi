using kiralamaSistemi.Entities.Abstract;
using System.Text;
using System.Xml.Linq;
using System.Xml;

namespace kiralamaSistemi.DataAccess.Sevices
{

    public class XmlLogger
    {
        private string FileName { get; set; }

        public XmlLogger()
        {
            FileName = Global.Files.xmlErrorFileUrl;
        }

        // XML Olustur
        private static void CreateXml(string fileName)
        {
            try
            {
                XmlTextWriter dosya = new(fileName, Encoding.UTF8)
                {
                    Formatting = Formatting.Indented,
                };
                // Write
                dosya.WriteStartDocument();
                dosya.WriteStartElement("Logs");
                dosya.WriteEndElement();
                dosya.WriteEndDocument();
                dosya.Flush();
                dosya.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void WriteXDocument(List<XmlLogModel> listModel, int counter = 0, bool fault = false)
        {
            try
            {
                if (!listModel.Any())
                {
                    return;
                }

                var fileName = $"{FileName} {counter}.xml";
                if (!File.Exists(fileName))
                {
                    if (fault)
                    {
                        throw new OzelException(new Error("cant write faults please recheck project Files"));
                    }
                    fault = true;
                    CreateXml(fileName);
                }
                // Load existing clients and add new 
                // şunu daha sonra aç
                XDocument xdosya = XDocument.Load(fileName);
                XElement root = xdosya.Root ?? new XElement("Logs");
                XElement[] element = new XElement[listModel.Count];

                for (int i = 0; i < listModel.Count; i++)
                {
                    element[i] = new XElement("Log"); // new log
                    XElement message = new("Message", listModel[i].Message);
                    XElement lineNumber = new("LineNumber", listModel[i].LineNumber);
                    XElement source = new("Source", listModel[i].Source);
                    XElement formatter = new("Formatter", listModel[i].Formatter);
                    XElement stacktrace = new("StackTrace", listModel[i].StackTrace);
                    XElement date = new("Date", listModel[i].Date);
                    element[i].Add(message, lineNumber, source, formatter, stacktrace, date);
                }

                root.Add(element);
                xdosya.Save(fileName);

            }
            catch (OzelException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                WriteXDocument(listModel, ++counter, fault);
            }
        }

    }

    // XML Log Model --
    public class XmlLogModel
    {
        public XmlLogModel()
        {
            Date = DateTime.Now;
        }
        public string? Message { get; set; }
        public string? LineNumber { get; set; }
        public string? EventId { get; set; }
        public string? Source { get; set; }
        public string? Formatter { get; set; }
        public string? StackTrace { get; set; }
        public DateTime Date { get; set; }

    }
}
