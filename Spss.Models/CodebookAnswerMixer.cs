using System;
using System.Collections.Generic;
using System.Linq;
using Spss.Models.Data;
using System.Data;
using General.Helpers;
using Spss.Models.Interfaces;

namespace Spss.Models
{
    public class CodebookAnswerMixer
    {
        private Dictionary<int, ICodebook> _codebooks;

        public CodebookAnswerMixer(IEnumerable<ICodebook> codebooks) {
            _codebooks = codebooks.ToDictionary(c => c.Id);
        }

        public IEnumerable<IAnswerBook> CollectAnswersByJournal(Dictionary<int, List<IAnswer>> answers) {
            var allAnswers = answers.Values
                .SelectMany(l => l);

            // group answers by journalId and answer moment (Pid, FollowUp) 
            var answerBooks =
            from groupedByJournal in
                (from answer in allAnswers
                 group answer by answer.Key)
            select new AnswerBook() { Id = groupedByJournal.Key, Answers = groupedByJournal.Select(a => a).ToList(), AnswerMetadataList = groupedByJournal.Select(g => g.AnswerMetadata).ToList() };

              return answerBooks.Cast<IAnswerBook>();
        }

        public Pair<int, List<IAnswer>> ExtractAnswers(ICodebook codebook, DataTable answers, IEnumerable<ColumnInfo> journalInfoColumns) {
            var results = new List<IAnswer>();

            var missingVars = new List<string>();
            foreach(DataRow row in answers.Rows) {
                Answer answer = new Answer();
                var variables = new List<IVariable>();

                // add journalInfo
                foreach(var info in journalInfoColumns) {
                    var col = info.Column;
                    var columnType = info.Datatype;
                    var columnWidth = info.ColumnWidth;
                    if(answers.Columns[col] != null) {
                        var value = row[col].ToString();
                        variables.Add(new CodeAnswer(new Code(0, 0, 0, 0, col, col, "", columnType, col, columnWidth), value));
                    }
                }

                // add answers
                foreach(var code in codebook.Codes) { // read each column in row
                    if(answers.Columns[code.Name] != null) {
                        var value = row[code.Name].ToString();
                        IVariable codeAnswer = new CodeAnswer(code, value);
                        variables.Add(codeAnswer);
                    } else {
                        var miss = "Missing var: {0} - survey: {1}  row,col,sur,qId: {2},{3},{1},{4}  Q:{5}".F(code.Name, codebook.Id, code.Row, code.Column, code.QuestionId, code.QuestionNumber);
                        if(!missingVars.Contains(miss))
                            missingVars.Add(miss);
                    }
                }
                answer.Variables = variables;
                answer.Id = codebook.Id;
                answer.AnswerMetadata = AnswerMetadata.GetJournalInfo(row);
                // Key to group by (pid, follow_up)
                answer.SetKey(answer.AnswerMetadata.Pid, answer.AnswerMetadata.FollowUp);
                answer.Projekt = answer.AnswerMetadata.Projekt;
                results.Add(answer);
            }
            foreach(var miss in missingVars)
                Console.Out.WriteLine(miss);

            results = results.OrderBy(r => r.Key).ToList();
            return new Pair<int, List<IAnswer>>(codebook.Id, results);
        }

        private AnswerMetadata PopulateJournalInfo(DataRow row) {
            var journalInfo = new AnswerMetadata();
            journalInfo.Ssghafd = row["ssghafd"].ToString();
            journalInfo.Ssghnavn = row["ssghnavn"].ToString();
            journalInfo.Safdnavn = row["safdnavn"].ToString();
            journalInfo.Pid = row.ToString("pid");
            journalInfo.Projekt = row.ToString("projekt");
            journalInfo.Pkoen = (Gender)row.ToInt("pkoen");
            journalInfo.Palder = row.ToInt("palder");
            journalInfo.Pnation = row.ToString("pnation");
            journalInfo.Besvarelsesdato = StringHelperExtensions.ToDate(row["besvarelsesdato"].ToString());
            journalInfo.Pfoedt = row["pfoedt"].ToString();
            journalInfo.FollowUp = row["follow_up"].ToString();
            return journalInfo;
        }
    }
}
