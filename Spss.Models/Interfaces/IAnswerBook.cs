using System.Collections.Generic;
using Spss.Models.Data;

namespace Spss.Models.Interfaces
{
    public interface IAnswerBook
    {
        string Code { get; }
        List<AnswerMetadata> AnswerMetadataList { get; }
        List<IAnswer> Answers { get; }
    }
}
