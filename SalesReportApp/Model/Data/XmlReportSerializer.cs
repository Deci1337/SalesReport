using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Model.Core;

namespace Model.Data
{
    public class XmlReportSerializer : Serializer
    {
        public override void Save(List<Report> reports, string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(Report));

            for (int i = 0; i < reports.Count; i++)
            {
                string filePath = Path.Combine(folderPath, "report_" + reports[i].Id + ".xml");
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, reports[i]);
                }
            }
        }

        public override List<Report> Load(string folderPath)
        {
            List<Report> result = new List<Report>();
            if (!Directory.Exists(folderPath))
            {
                return result;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(Report));

            string[] files = Directory.GetFiles(folderPath, "*.xml");
            for (int i = 0; i < files.Length; i++)
            {
                using (StreamReader reader = new StreamReader(files[i]))
                {
                    Report report = (Report)serializer.Deserialize(reader);
                    result.Add(report);
                }
            }
            return result;
        }
    }
}
