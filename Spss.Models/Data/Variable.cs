using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spss.Models.Data;
using Spss.Models.Interfaces;

namespace Spss.Models 
{
  public class Variable : IVariable {

    public String Name { get; set; }
    public String Value { get; set; }
    public String Label { get; set; } // to be used?  or same as name?
    public int ColumnWidth { get; set; } 
    public String Datatype { get; set; }
    public String MeasurementLevel { get; set; }
      //public String TableName { get; set; }

    // table info
    public int Id { get; set; }
    public int ExportJournalInfoId { get; set; }
    public int JournalId { get; set; }
    public int SurveyAnswerId { get; set; }
  }
}
