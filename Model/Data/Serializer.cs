using System.Collections.Generic;
using Model.Core;

namespace Model.Data
{
    public abstract class Serializer
    {
        public abstract void Save(List<Report> reports, string path);
        public abstract List<Report> Load(string path);
    }
}
