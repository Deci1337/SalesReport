namespace Model.Data
{
    public class SerializerFactory
    {
        public static Serializer Create(string format)
        {
            if (format == "json")
            {
                return new JsonReportSerializer();
            }
            if (format == "xml")
            {
                return new XmlReportSerializer();
            }
            return new JsonReportSerializer();
        }
    }
}
