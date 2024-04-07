using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WorkspaceForHomework
{
    public class Utils
    {
        const string prettySeparator = "+---------------------------------------------------------------------------------";
        const string mainPath = "../../../XML/ConsumAiguaRecords";

        public static List<Consum> ParseConsumCSV(string filepath)
        {
            List<Consum> resultList = new List<Consum>();
            using StreamReader reader = new StreamReader(filepath);
            using CsvReader csvreader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvreader.Read();
            csvreader.ReadHeader();
            while (csvreader.Read())
            {
                var record = new Consum
                {
                    Any = csvreader.GetField<int>("Any"),
                    CodComarca = csvreader.GetField<int>("Codi comarca"),
                    Comarca = csvreader.GetField<string>("Comarca"),
                    Poblacio = csvreader.GetField<int>("Població"),
                    XarxaDomestica = csvreader.GetField<int>("Domèstic xarxa"),
                    ActivitatsEconomiques = csvreader.GetField<int>("Activitats econòmiques i fonts pròpies"),
                    Total = csvreader.GetField<int>("Total"),
                    ConsumPerCapita = csvreader.GetField<float>("Consum domèstic per càpita")
                };
                resultList.Add(record);
            }
            return resultList;
        }

        public static string RecordsToXMLFile(List<Consum> recordsList)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            DateTime dateNow = DateTime.Now;
            string formattedDate = dateNow.ToString("ddMMyyyy_hhmmss");
            string filepath = mainPath + $"{formattedDate}.xml";
            using (XmlWriter output = XmlWriter.Create(filepath, settings))
            {
                output.WriteStartDocument();

                output.WriteStartElement("Records");
                foreach (Consum record in recordsList)
                {
                    output.WriteStartElement("Record");
                    output.WriteElementString("Any", record.Any.ToString());
                    output.WriteElementString("CodComarca", record.CodComarca.ToString());
                    output.WriteElementString("Comarca", record.Comarca.ToString());
                    output.WriteElementString("Poblacio", record.Poblacio.ToString());
                    output.WriteElementString("XarxaDomestica", record.XarxaDomestica.ToString());
                    output.WriteElementString("ActivitatsEconomiques", record.ActivitatsEconomiques.ToString());
                    output.WriteElementString("Total", record.Total.ToString());
                    output.WriteElementString("ConsumPerCapita", record.ConsumPerCapita.ToString());
                    output.WriteEndElement();
                }
                output.WriteEndElement();
            }
            return filepath;
        }

        public static void PrettyPrint(string text)
        {
            Console.WriteLine(prettySeparator);
            Console.WriteLine("| -> " + text);
            Console.WriteLine(prettySeparator);
        }

        public static List<int> GetComarcas(List<Consum> recordsList)
        {
            List<int> Comarcas = new List<int>();
            foreach (Consum current in recordsList)
            {
                if (Comarcas.Find(x => current.CodComarca == x) == 0)
                {
                    Comarcas.Add(current.CodComarca);
                }
            }
            return Comarcas;
        }

    }
}
