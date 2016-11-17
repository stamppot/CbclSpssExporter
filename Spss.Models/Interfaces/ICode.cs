using System;

namespace Spss.Models.Interfaces
{
    public interface ICode
    {
        //ICodebook Codebook { get; set; }
        int CodebookId { get; }
        int Column { get; }
        int ColumnWidth { get; }
        string Datatype { get; }
        int Id { get; }
        string Item { get; }
        int ItemChoiceId { get; }
        string ItemType { get; }
        string Label { get; }
        string MeasurementLevel { get; }
        string Name { get; }
        int QuestionId { get; }
        int QuestionNumber { get; }
        int Row { get; }
        IVariable Variable { get; }
    }
}
