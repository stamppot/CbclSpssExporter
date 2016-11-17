using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spss.Models.Interfaces
{
    public interface IVariable
    {
        int Id { get; }
        String Name { get; }
        String Value { get; set; }
        String Label { get; }
        String Datatype { get; }
        int ColumnWidth { get; }  
        string MeasurementLevel { get; }
    }
}
