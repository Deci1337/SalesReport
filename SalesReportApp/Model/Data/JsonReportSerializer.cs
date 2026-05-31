using System.Collections.Generic;
using System.IO;
using Model.Core;
using Newtonsoft.Json;

namespace Model.Data
{
    public class JsonReportSerializer : Serializer
    {
        public override void Save(List<Report> reports, string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;

            for (int i = 0; i < reports.Count; i++)
            {
                string json = JsonConvert.SerializeObject(reports[i], Formatting.Indented, settings);
                string filePath = Path.Combine(folderPath, "report_" + reports[i].Id + ".json");
                File.WriteAllText(filePath, json);
            }
        }

        public override List<Report> Load(string folderPath)
        {
            List<Report> result = new List<Report>();
            if (!Directory.Exists(folderPath))
            {
                return result;
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;

            string[] files = Directory.GetFiles(folderPath, "*.json");
            for (int i = 0; i < files.Length; i++)
            {
                string json = File.ReadAllText(files[i]);
                Report report = JsonConvert.DeserializeObject<Report>(json, settings);
                if (report != null)
                {
                    result.Add(report);
                }
            }
            return result;
        }
    }
}
