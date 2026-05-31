using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Model.Core;

namespace Model.Data
{
    public class XmlReportSerializer : Serializer
    {
        public override void Save(List<Report> reports, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Report>));
            StreamWriter writer = new StreamWriter(path);
            serializer.Serialize(writer, reports);
            writer.Close();
        }

        public override List<Report> Load(string path)
        {
            if (File.Exists(path) == false)
            {
                return new List<Report>();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<Report>));
            StreamReader reader = new StreamReader(path);
            List<Report> result = (List<Report>)serializer.Deserialize(reader);
            reader.Close();

            if (result == null)
            {
                return new List<Report>();
            }
            return result;
        }
    }
}
