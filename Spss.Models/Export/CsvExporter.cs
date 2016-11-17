using System;
using System.Collections.Generic;
using System.Linq;
using General.Helpers;
using Spss.Models.Interfaces;

namespace Spss.Models.Export
{
    public class CsvExporter : IDocExporter
    {

    private readonly IEnumerable<ICode> _columnCodes;
    private readonly IEnumerable<IAnswerBook> _journalAnswersCollection;
    private SpssDataDocument _doc;

    public CsvExporter(IEnumerable<IAnswerBook> journalAnswersCollection, IEnumerable<ICode> columnCodes) {
        _journalAnswersCollection = journalAnswersCollection;
        _columnCodes = columnCodes;
    }

    public SpssDataDocument Result { get; set; }

    public void Write(String filename) {
      _doc = SpssDataDocument.Create(filename);
        
      CreateColumnMetaData(_doc, _columnCodes); //_columnDefinitions);
      CreateRows(_doc, _journalAnswersCollection);
      Result = _doc;
    }

    private void CreateColumnMetaData(SpssDataDocument doc, IEnumerable<ICode> columnDefinitions) {
        var addedVariables = new Dictionary<String, bool>() { { "Id", true } };

        foreach (var code in columnDefinitions) {
            var varName = code.Name;
            var label = string.IsNullOrEmpty(code.Label) ? code.Name : code.Label;
            var varDatatype = code.Datatype;
            if (!addedVariables.ContainsKey(varName)) {
                var v = CreateSpssVariable(varDatatype, varName, label, code.ColumnWidth);
                doc.Variables.Add(v);
                addedVariables.Add(varName, true);
            }
        }
        doc.CommitDictionary();
    }

    private void CreateRows(SpssDataDocument doc, IEnumerable<IAnswerBook> journalAnswersCollection) {
        Console.Out.WriteLine("journalsAnswers: {0}", journalAnswersCollection.Count());
        foreach(var journalAnswers in journalAnswersCollection) {
            Console.Out.WriteLine("Pid key: {0} CreateRows: {1}", journalAnswers.Answers.First().Key, journalAnswers.Answers.Count);
            CreateJournalRows(doc, journalAnswers);
        }
    }

    private void CreateJournalRows(SpssDataDocument doc, IAnswerBook journalAnswers) {
      var journalInfo = journalAnswers.AnswerMetadataList;

      var answers = journalAnswers.Answers.OrderBy(a => a.AnswerMetadata.Besvarelsesdato);
        // check if any of the answers is from the same survey

      //var lastAnswerDate = answers.First().JournalInfo.Besvarelsesdato;

      SpssCase case1 = doc.Cases.New(); // row. Can contain multiple answers. TODO: if more answers to the same survey, split answers into multiple rows
      bool isSetJournalData = false;

      foreach(var answer in answers) {
          //var answerDateDiff = answer.JournalInfo.Besvarelsesdato.Subtract(lastAnswerDate);
          //if(answerDateDiff.Days > 120) { // if more than 120 days difference between answers, we assume it's not the same followup, and thus a new row
          //    case1.Commit();
          //    case1 = doc.Cases.New();
          //    isSetJournalData = false;
          //}

          int varIndex = 0;
          foreach(var variable in answer.Variables) {

              if(variable.Datatype == "Date") {
                  var d = variable.Value.ToDate();
                  //variable.Value = string.Format("{0,2}{1,2}{2}", d.Day, d.Month, d.Year.ToString().Replace("20", "").Replace("19", "").ToInt()).Replace(" ", "0");
                  case1.SetDBValue(variable.Name, d); //variable.Value);
                  continue;
              }
              if(variable.Name == "Pid") {
                  //variable.Datatype = "String";
                  case1.SetDBValue(variable.Name, variable.Value);
              }
              if(variable.Datatype == "String" && variable.Value == "#NULL!") {
                  variable.Value = "";
                  case1.SetDBValue(variable.Name, variable.Value);
              }
              if(variable.Datatype == "Numeric" && variable.Value == "" || variable.Value == "#NULL!") {
                  variable.Value = "9";
                  case1.SetDBValue(variable.Name, variable.Value);
              }
              if(variable.Datatype != "Date" && variable.Value.StartsWith("0")) {
                  variable.Value = "0.0";
              }
              try {
                  if(!isSetJournalData || varIndex > 9) { // first 9 vars are journalInfo vars
                      case1.SetDBValue(variable.Name, variable.Value);
                  }
              } catch(FormatException ex) {
                  var message = string.Format("Variable name '{0}' could not set value: {1}. Label: {2} Answer: {3}. journalId: {4}",
                      variable.Name, variable.Value, variable.Label, answer.Id, answer.Key);
                  throw new FormatException(message, ex);
              }
              varIndex++;
          }
          isSetJournalData = true;
      }
      case1.Commit();

    }

    private SpssVariable CreateSpssVariable(String datatype, String name, String label, int columnWidth) {
      SpssVariable v = null;

      switch (datatype.ToLower()) {
        case "string":
          v = new SpssStringVariable();
          v.ColumnWidth = columnWidth;
          break;
        case "numeric":
          v = new SpssNumericVariable();
          v.ColumnWidth = columnWidth;
          break;
        case "date":
          //v = new SpssStringVariable();
          v = new SpssDateVariable();
          break;
        default:
          v = new SpssNumericVariable();
          break;
      }
      if (name.EndsWith("hv") && datatype != "String") {
        v = new SpssStringVariable();
        v.ColumnWidth = 255;
      }
      v.Name = name;
      v.Label = label;
      return v;
    }
  }
}
