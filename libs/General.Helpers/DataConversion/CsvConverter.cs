using System;
using System.Collections.Generic;
using System.Linq;
using GenericParsing;
using System.Data;
using System.IO;

namespace General.Helpers.DataConversion {

    public class CsvConverter {

        private Char _columnSeparator = '\t';
        //private TextReader _reader;
        private string _csv;

        public CsvConverter(StringReader strReader) {
            //_reader = strReader;
            _csv = strReader.ReadToEnd();
            _columnSeparator = FindColumnDelimiter();
        }

        public CsvConverter WithColumnDelimiter(Char columnDelimiter) {
            _columnSeparator = columnDelimiter;
            return this;
        }

        public DataTable ToDataTable() {
            DataTable output;
            var lines = _csv.Split('\n');
            var columnNums = lines.AsEnumerable().Select(l => l.Split(';')).ToList();
            var header = lines.First();
            var columnCount = header.Split(';').Count();
            Console.Out.WriteLine(header);
            
            var wrongLines = new List<string>();
            for(int i = 0; i < columnNums.Count(); i++)
            {
                if (columnNums[i].Count() < columnCount && lines[i].Length > 0)
                {
                    wrongLines.Add(string.Format("{0}: line misses fields: {1}", i, lines[i]));
                }
            }
            if(wrongLines.Any())
                throw new Exception(wrongLines.JoinStr("\n"));

            using (GenericParserAdapter parserAdapter = new GenericParserAdapter(new StringReader(_csv))) {
                parserAdapter.ColumnDelimiter = _columnSeparator;
                parserAdapter.FirstRowHasHeader = true;
                parserAdapter.FirstRowSetsExpectedColumnCount = true;
                parserAdapter.StripControlChars = true;
                parserAdapter.SkipEmptyRows = true;
                parserAdapter.TextQualifier = '\"';
                parserAdapter.MaxBufferSize = 1048500;
                //parserAdapter.MaxRows = 10000;
                output = parserAdapter.GetDataTable();
            }
            return output;
        }

        private char FindColumnDelimiter() {
            char tab = '\t';
            char semicolon = ';';
            String line = new StringReader(_csv).ReadLine();
            int countSemicolons = line.Where(c => c == semicolon).Count();
            int countTabs = line.Where(c => c == tab).Count();
            
            return (countTabs > 6 && countTabs > countSemicolons) ? tab : semicolon;
        }
    }
}
