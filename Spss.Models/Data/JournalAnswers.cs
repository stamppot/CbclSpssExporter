using System.Collections.Generic;
using Spss.Models.Interfaces;

namespace Spss.Models.Data 
{
  public class JournalAnswers : IAnswerBook {

      public JournalAnswers(AnswerMetadata answerMetadata)
      {
          AnswerMetadataList = new List<AnswerMetadata> {answerMetadata};
          Answers = new List<IAnswer>();
      }

      public string Code { get; set; }
    public List<AnswerMetadata> AnswerMetadataList { get; set; }
    public List<IAnswer> Answers { get; set; }
  }
}
