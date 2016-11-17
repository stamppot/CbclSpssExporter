using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spss.Models.Interfaces;

namespace Spss.Models.Data
{
    public class CodeAnswer : Code, IVariable
    {
        public String Value { get; set; }
        public int ColumnWidth { get; set; }

        public CodeAnswer(int id, int codeBookId, int questionId, int questionNumber, string item, string varName, string itemType, string datatype, string questionText, string value, int columnWidth)
            : base(id, codeBookId, questionId, questionNumber, item, varName, itemType, datatype, questionText, columnWidth) {
            Value = value;
            ColumnWidth = columnWidth;
        }

        public CodeAnswer(ICode c, string value) : base(c.Id, c.CodebookId, c.QuestionId, c.QuestionNumber, c.Item, c.Name, c.ItemType, c.Datatype, c.Label, c.ColumnWidth) {
            Value = value;
        }
    }
}
