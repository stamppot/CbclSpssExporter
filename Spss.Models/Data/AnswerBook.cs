using System.Collections.Generic;
using Spss.Models.Interfaces;

namespace Spss.Models.Data
{
    public class AnswerBook : IAnswerBook
    {
        public string Code { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public SurveyType SurveyType { get; set; }
        //public Codebook Codebook { get; set; }
        public List<AnswerMetadata> AnswerMetadataList { get; set; }

        //public Dictionary<int, List<CodeAnswer>> CodeAnswers { get; set; }

        public List<IAnswer> Answers { get; set; }

        public AnswerBook() {
            Answers = new List<IAnswer>();
        }
    }
}
