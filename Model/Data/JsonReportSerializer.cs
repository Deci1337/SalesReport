using System.Collections.Generic;
using System.IO;
using Model.Core;
using Newtonsoft.Json;

namespace Model.Data
{
    public class JsonReportSerializer : Serializer
    {
        private JsonSerializerSettings jsonSettings = new JsonSerializerSettings();

        public JsonReportSerializer()
        {
            jsonSettings.TypeNameHandling = TypeNameHandling.All;
        }

        public override void Save(List<Report> reports, string path)
        {
            string json = JsonConvert.SerializeObject(reports, Formatting.Indented, jsonSettings);
            File.WriteAllText(path, json);
        }

        public override List<Report> Load(string path)
        {
            if (File.Exists(path) == false)
            {
                return new List<Report>();
            }

            string json = File.ReadAllText(path);
            List<Report> reports = JsonConvert.DeserializeObject<List<Report>>(json, jsonSettings);

            if (reports == null)
            {
                return new List<Report>();
            }
            return reports;
        }
    }
}
