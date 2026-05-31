using System;
using System.Collections.Generic;

namespace Model.Core
{
    public interface IReportable
    {
        void Sort(bool ascending);
        List<ITProduct> Select(Type type);
    }
}
