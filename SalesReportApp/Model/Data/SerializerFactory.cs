namespace Model.Data
{
    public static class SerializerFactory
    {
        public static Serializer Create(string format)
        {
            if (format == "json")
            {
                return new JsonReportSerializer();
            }
            return new XmlReportSerializer();
        }
    }
}
