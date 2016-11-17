using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Spss.Models.Data;

namespace Spss.Models 
{
  public class JournalInfoParser {

    XDocument _xdoc;
    XmlHelper _xmlHelper;

    public JournalInfoParser(String filename) {
      _xdoc = XDocument.Load(filename);
      _xmlHelper = new XmlHelper();
    }

    public IEnumerable<AnswerMetadata> Parse() {
      var db = _xdoc.Element("mysqldump").Element("database");
      var table = db.Element("table_data");
      var rows = table.Elements("row");
      return ParseRows(rows);
    }

    private AnswerMetadata ParseRow(XElement row) {
      var fields = row.Elements("field");

      var id = _xmlHelper.GetIntAttributeValue(fields, "id");
      var journalId = _xmlHelper.GetIntAttributeValue(fields, "journal_id");
      var ssghafd = fields.Single(e => e.HasAttributeValue("ssghafd")).Value;
      var ssghnavn = fields.Single(e => e.HasAttributeValue("ssghnavn")).Value;
      var safdnavn = fields.Single(e => e.HasAttributeValue("safdnavn")).Value;
      var pid = fields.Single(e => e.HasAttributeValue("pid")).Value;
      var projekt = fields.Single(e => e.HasAttributeValue("projekt")).Value;
      var pkoen = _xmlHelper.GetAttributeGender(fields, "pkoen");
      var palder = _xmlHelper.GetIntAttributeValue(fields, "palder");
      var pkoen_datatype = _xmlHelper.GetAttributeDatatype(fields, "pkoen_datatype");
      var palder_datatype = _xmlHelper.GetAttributeDatatype(fields, "palder_datatype");
      var pnation = _xmlHelper.GetAttributeValue(fields, "pnation");
      var pnation_datatype = _xmlHelper.GetAttributeDatatype(fields, "pnation_datatype");
      var besvarelsesdato = _xmlHelper.GetAttributeDateTime(fields, "besvarelsesdato");
      var pfoedt = _xmlHelper.GetAttributeDateTime(fields, "pfoedt");
      var besvarelsesdato_datatype = _xmlHelper.GetAttributeDatatype(fields, "besvarelsesdato_datatype");
      var pfoedt_datatype = _xmlHelper.GetAttributeDatatype(fields, "pfoedt_datatype");

      var journalInfo = new AnswerMetadata() {
        Id = id,
        JournalId = journalId,
        Ssghafd = ssghafd,
        Ssghnavn = ssghnavn,
        Safdnavn = safdnavn,
        Pid = pid,
        Projekt = projekt,
        Pkoen = pkoen,
        Palder = palder,
        Pnation = pnation,
        Pfoedt = pfoedt.ToString(),
      };
      return journalInfo;
    }

    private IEnumerable<AnswerMetadata> ParseRows(IEnumerable<XElement> rows) {
      return rows.Select(row => ParseRow(row));
    }
  }
}
