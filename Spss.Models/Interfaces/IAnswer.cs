using System.Collections.Generic;
using Spss.Models.Data;

namespace Spss.Models.Interfaces
{
    public interface IAnswer
    {
        int Id { get; }
        AnswerMetadata AnswerMetadata { get; } 
        List<IVariable> Variables { get; }
        int SurveyId { get; }
        string Projekt { get; }

        string JournalId { get; }
        FollowUp FollowUp { get; }

        /// <summary>
        /// Combination of journalId and followUp to group answers by answer moment
        /// </summary>
        /// <param name="journalId"></param>
        /// <param name="followUp"></param>
        void SetKey(string journalId, string followUp);

        /// <summary>
        /// Combination of journalId and followUp to group answers by answer moment
        /// </summary>
        string Key { get; }
    }
}
