using System.Collections.Generic;
using Spss.Models.Data;
using Spss.Models.Interfaces;

namespace Spss.Models 
{
    public class Answer : IAnswer
    {

        public Answer()
        {
            Variables = new List<IVariable>();
        }

        // TODO: journal info
        // TODO: spoergeskema info

        //public int Id { get; set; }
        public int Id { get; set; } // journalId
        public int ExportJournalInfoId { get; set; }
        public string JournalId { get; private set; }
        public FollowUp FollowUp { get; private set; }
        
        public string Projekt { get; set; }
        public int SurveyAnswerId { get; set; }
        public int SurveyId { get; set; }

        public AnswerMetadata AnswerMetadata { get; set; }
        public List<IVariable> Variables { get; set; }

        public string Key => JournalId + "_" + FollowUp;

        public void SetKey(string journalId, string followUp)
        {
            JournalId = journalId;
            FollowUp = FollowUpHelper.Read(followUp);
        }

    }
}
