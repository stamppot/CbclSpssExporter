//using System;
//using System.Data;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml;
//using Spss;
//using System.IO;
//using Spss.Models.Data;
//using Spss.Models.Export;
//using Spss.Models.Import;
//using General.Helpers;
//using General.Helpers.Datastructures;

//namespace Spss.Models
//{
//  class Program
//  {
//    public static void Main(string[] args) {
//      var filename = "TEST1.sav";
//      var fileInfo = new FileInfo(filename);
//      if (fileInfo.Exists)
//        fileInfo.Delete();

//      Console.WriteLine("Reading codebooks");
//      // codebooks
//      var codebookFiles = new List<String> { 
//            @"..\..\codebooks\codebook_cc.xml", @"..\..\codebooks\codebook_ccy.xml", 
//            @"..\..\codebooks\codebook_ct.xml", @"..\..\codebooks\codebook_tt.xml", @"..\..\codebooks\codebook_ycy.xml" };

//      var codebookLoader = new CodeBookXmlLoader(codebookFiles);
//      var codebooks = codebookLoader.Read();

//      Console.WriteLine("Importing answers data from CSV files");

//      // read answers
//      var answersLoader = new AnswersCsvLoader();
//      var answers2 = answersLoader.ReadCsvAnswers();

//      var journalInfoColumns = JournalInfo.JournalInfoColumns();

//      var mixer = new CodebookAnswerMixer(codebooks.Values);
//      var answersPerSurvey = new Dictionary<int, List<IAnswer>>();
     
//      foreach (var pair in codebooks) {
//          var codebookId = pair.Key; //.ToString();
//          var codebook = pair.Value;
//          var answersTable = answers2[codebookId]; // get answer table and codebook with same surveyId. Just for trying out
//          var answersForSurvey = mixer.ExtractAnswers(codebook, answersTable, journalInfoColumns);
//          answersPerSurvey.Add(answersForSurvey.First, answersForSurvey.Second);
//      }


//      var answersByJournal = mixer.CollectAnswersByJournal(answersPerSurvey);
//      //var allAnswersInbooks = mixer.GetAnswersByJournal(answers2, codebooks);

//      var columnCodes = GetColumnDefinitions(codebooks);

//      var saveFile = "hello123.sav";

//      Console.WriteLine("Exporting document...");
//      var exporter = new SpssExporter(answersByJournal, columnCodes);

//      exporter.Write(saveFile);

//      Console.Write("Press any key to continue . . . ");
//      Console.ReadKey(true);
//    }

//    private static Dictionary<int,Codebook>  ReadCodeBooks(IEnumerable<string> filenames) {
//        if(!filenames.Any())
//            filenames = new List<String> { @"..\..\files\codebook_cc.xml", @"..\..\files\codebook_ccy.xml", @"..\..\files\codebook_ct.xml", @"..\..\files\codebook_tt.xml", @"..\..\files\codebook_ycy.xml" };

//        var codebookLoader = new CodeBookXmlLoader(filenames);
//        var codebooks = codebookLoader.Read();

//        return codebooks;
//    }

//    private static IEnumerable<Code> GetColumnDefinitions(Dictionary<int, Codebook> codebooks) {
//        var columns = new List<Code>();

//        foreach(var colPair in JournalInfo.JournalInfoColumns()) {
//            var columnName = colPair.Key;
//            var datatypePair = colPair.Value;
//            var columnType = datatypePair.First;
//            var columnWidth = datatypePair.Second;
//            var col = new Code(0, 0, 0, 0, "", columnName, "", columnType, "", columnWidth);
//            columns.Add(col);
//        }

//        columns.AddRange(codebooks.Values.SelectMany(cb => cb.Codes));
//        return columns;
//    }

//    private static IEnumerable<JournalInfo> ReadJournalInfos(String filename) {
//      var parser = new JournalInfoParser(filename);
//      return parser.Parse();
//    }

//    //private static Dictionary<String,IEnumerable<IAnswer>> ReadAnswers(List<String> filenames) {
//    //  var loader = new AnswersXmlLoader(filenames, JournalInfo.JournalInfoColumns());
//    //  var headers = loader.GetHeaders();
//    //  var answers = loader.GetAnswers();
//    //  return answers;
//    //}

//    private static IEnumerable<IAnswerBook> CombineData(IEnumerable<IAnswer> allAnswers, IEnumerable<JournalInfo> journalInfos) {
//      var converter = new AnswerJuggler();
//      return converter.ToJournalAnswers(allAnswers, journalInfos);
//    }
//  }
//}
