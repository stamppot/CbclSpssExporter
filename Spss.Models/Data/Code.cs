using System;
using System.Collections.Generic;
using System.Linq;
using Spss.Models.Interfaces;

namespace Spss.Models.Data
{
    public class Code : ICode
    {
        public string Name { get; set; }
        public string Datatype { get; set; }
        public string Label { get; set; } // Question text
        public int ColumnWidth { get; set; } 

        public int Id { get; set; }
        public int CodebookId { get; set; }
        public int QuestionId { get; set; }
        public int QuestionNumber { get; set; }
        public IVariable Variable { get; set; }
        public string Item { get; set; }
        public string ItemType { get; set; }
        public string MeasurementLevel { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int ItemChoiceId { get; set; }
        public ICodebook Codebook { get; set; }
                                   
        public Code(int id, int codeBookId, int questionId, int questionNumber, string item, string varName, string itemType, string datatype, string questionText, int columnWidth) {
            Id = id;
            CodebookId = codeBookId;
            QuestionId = questionId;
            QuestionNumber = questionNumber;
            Item = item;
            Name = varName;
            ItemType = itemType;
            Datatype = datatype;
            Label = questionText;
            ColumnWidth = columnWidth;
        }

    }
}
