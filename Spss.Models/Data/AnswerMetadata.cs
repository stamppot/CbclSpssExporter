using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using General.Helpers;

namespace Spss.Models.Data {
  public class AnswerMetadata {

      public static IEnumerable<ColumnInfo> JournalInfoColumns()
      {
          var journalColumns = new List<ColumnInfo>();
          //journalColumns.Add("Id", "Numeric");
          //journalColumns.Add("JournalId", "Numeric");
          journalColumns.Add(new ColumnInfo {Column = "Ssghnavn", Datatype = "String", ColumnWidth = 80});
          journalColumns.Add(new ColumnInfo {Column = "Ssghafd", Datatype = "String", ColumnWidth = 20});
          journalColumns.Add(new ColumnInfo {Column = "Safdnavn", Datatype = "String", ColumnWidth = 80});
          journalColumns.Add(new ColumnInfo {Column = "Pid", Datatype = "String", ColumnWidth = 20});
          journalColumns.Add(new ColumnInfo {Column = "Projekt", Datatype = "String", ColumnWidth = 23});
          journalColumns.Add(new ColumnInfo {Column = "Pkoen", Datatype = "Numeric", ColumnWidth = 2});
          journalColumns.Add(new ColumnInfo {Column = "Palder", Datatype = "Numeric", ColumnWidth = 2});
          journalColumns.Add(new ColumnInfo {Column = "Pnation", Datatype = "String", ColumnWidth = 20});
          journalColumns.Add(new ColumnInfo {Column = "Besvarelsesdato", Datatype = "Date", ColumnWidth = 30});
          journalColumns.Add(new ColumnInfo {Column = "Pfoedt", Datatype = "Date", ColumnWidth = 30});
          journalColumns.Add(new ColumnInfo {Column = "follow_up", Datatype = "String", ColumnWidth = 30});
          return journalColumns;
      }

      public static AnswerMetadata GetJournalInfo(DataRow row)
      {
          var journalInfo = new AnswerMetadata();
          journalInfo.Ssghafd = row["ssghafd"].ToString();
          journalInfo.Ssghnavn = row["ssghnavn"].ToString();
          journalInfo.Safdnavn = row["safdnavn"].ToString();
          journalInfo.Pid = row.ToString("pid");
          journalInfo.Projekt = row.ToString("projekt");
          journalInfo.Pkoen = (Gender) row.ToInt("pkoen");
          journalInfo.Palder = row.ToInt("palder");
          journalInfo.Pnation = row.ToString("pnation");
          journalInfo.Besvarelsesdato = StringHelperExtensions.ToDate(row["besvarelsesdato"].ToString());
          journalInfo.Pfoedt = row["pfoedt"].ToString();
            journalInfo.FollowUp = row["follow_up"].ToString();
            return journalInfo;
      }

      public AnswerMetadata() {
      Values = new OrderedDictionary(750);
    }

    public int Id { get; set; }
    public int JournalId { get; set; }
    public string Ssghafd { get; set; }
    public string Ssghnavn { get; set; }
    public string Safdnavn { get; set; }
    public string Pid { get; set; } // journal code
    public string Projekt { get; set; }
    public Gender Pkoen { get; set; }
    public int Palder { get; set; }
    public string Pnation { get; set; }
    public DateTime Besvarelsesdato { get; set; }
        public string Pfoedt { get; set; }
        public string FollowUp { get; set; }

        public OrderedDictionary Values { get; private set; } // values for all columns in single row
  }
}
