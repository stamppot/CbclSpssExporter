using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Spss.Models.Data;
using General.Helpers;
using System.IO;
using Spss.Models.Interfaces;

namespace Spss.Models.Import
{
    public class CodeBookXmlLoader
    {
        private readonly IEnumerable<String> _filenames;
        private Dictionary<String, String> _headers = new Dictionary<string, string>();
        public static Dictionary<string, int> CodeBookIdNames = new Dictionary<string, int>();

        public CodeBookXmlLoader(IEnumerable<String> filenames) {
            _filenames = filenames;
            CodeBookIdNames = new Dictionary<string, int>
                                  {
                                      { "cc", 1}, 
                                      { "ccy", 2},
                                      { "ct", 3},
                                      { "tt", 4},
                                      { "ycy", 5}
                                  };
        }

        public Dictionary<int, ICodebook> Read() {
            return ReadCodebooks(_filenames);
        }

        private Dictionary<int, ICodebook> ReadCodebooks(IEnumerable<String> filenames) {
            var codebooks = new Dictionary<int, ICodebook>();
            foreach (var filename in filenames) {
                var xdoc = XDocument.Load(filename);
                var tableStructure = xdoc.Element("mysqldump").Element("database").Element("table_structure");
                var tableName = Path.GetFileNameWithoutExtension(filename).Replace("codebook_", "");

                int codebookId = CodeBookIdNames[tableName];
                var codebook = ReadCodebook(xdoc, codebookId);
                codebooks.Add(codebookId, codebook);
            }
            return codebooks;
        }

        private Codebook ReadCodebook(XDocument doc, int codebookId) {
            var codebook = GetCodebook(codebookId);
            var rows = doc.Descendants("row");
            var codes = rows.Select(r => (ICode) ParseCodeRow(r));
            //codes.ForEach(c => c.Codebook = codebook);
            codebook.Codes = codes;
            return codebook;
        }

        private Codebook GetCodebook(int codebookId) {
            switch (codebookId) {
                case 1:
                    return new Codebook() { Id = codebookId, SurveyType = SurveyType.Parent, Title = "CBCL: 1 1/2-5 Parent" };
                case 2:
                    return new Codebook() { Id = codebookId, SurveyType = SurveyType.Parent, Title = "CBCL: 6-16 Parent" };
                case 3:
                    return new Codebook() { Id = codebookId, SurveyType = SurveyType.Teacher, Title = "C-TRF: 1 1/2-5 Teacher" };
                case 4:
                    return new Codebook() { Id = codebookId, SurveyType = SurveyType.Teacher, Title = "TRF: 1 1/2-5 Teacher" };
                case 5:
                    return new Codebook() { Id = codebookId, SurveyType = SurveyType.Youth, Title = "YSR: 11-16 Youth" };
                default:
                    return new Codebook();
            }
        }

        private Code ParseCodeRow(XElement xElem) {
            var fields = xElem.Elements("field");
            var id = Get(fields, "id").ToInt();
            var codeBookId = Get(fields, "code_book_id").ToInt();
            var questionId = Get(fields, "question_id").ToInt();
            var questionNumber = Get(fields, "question_number").ToInt();
            var varName = Get(fields, "variable");
            var itemType = Get(fields, "item_type");
            var item = Get(fields, "item");
            var datatype = Get(fields, "datatype");
            var questionText = Get(fields, "description");
            var measLevel = Get(fields, "measurement_level");
            var row = Get(fields, "row").ToInt();
            var col = Get(fields, "col").ToInt();
            var itemChoiceId = Get(fields, "item_choice_id").ToInt();
            return new Code(id, codeBookId, questionId, questionNumber, item, varName, itemType, datatype, questionText, 25) {
                Row = row,
                Column = col,
                MeasurementLevel = measLevel,
                ItemChoiceId = itemChoiceId
            };
        }

        private string Get(IEnumerable<XElement> fields, string attributeName) {
            var field = fields.FirstOrDefault(f => f.HasAttributeValue(attributeName));
            if (field == null) return "";
            return field.ReadTextNode(); //attributeName);
        }
    }
}
