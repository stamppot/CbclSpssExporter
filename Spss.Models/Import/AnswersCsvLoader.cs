using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using General.Helpers.DataConversion;

namespace Spss.Models.Import
{
    public class AnswersCsvLoader
    {
        //private List<string> answerFiles = new List<string> {
        //    @"..\..\files\eksport_svar_2012-03-06_1.csv",
        //    @"..\..\files\eksport_svar_2012-03-06_2.csv",
        //    @"..\..\files\eksport_svar_2012-03-06_3.csv",
        //    @"..\..\files\eksport_svar_2012-03-06_4.csv",
        //    @"..\..\files\eksport_svar_2012-03-06_5.csv"
        //};

        //public AnswersCsvLoader() {
        //}

        //public Dictionary<int, DataTable> ReadCsvAnswers() {
        //    return answerFiles.Select(f => ReadAnswer(f)).ToDictionary(k => k.First, k => k.Second);
        //}

        public Dictionary<int, DataTable> ReadCsvAnswers(IEnumerable<string> answerFiles) {
            return answerFiles.Select(f => ReadAnswer(f)).ToDictionary(k => k.First, k => k.Second);
        }

        public Pair<int,DataTable> ReadAnswer(string filename) {
            var reader = new StringReader(new StreamReader(filename).ReadToEnd());
            var csv = new CsvConverter(reader);
            var table = csv.ToDataTable();
            var surveyId = ReadSurveyId(filename);

            return new Pair<int, DataTable>(surveyId, table);
        }

        private int ReadSurveyId(string filename) {
            filename = filename.Replace(".csv", "");
            filename = filename.Remove(0, filename.Length - 1);
            var surveyId = 0;
            if(!int.TryParse(filename, out surveyId))
                throw new Exception(string.Format("Filename '{0}' must end with Id (1-5)", filename));
            return surveyId;
        }
    }
}
