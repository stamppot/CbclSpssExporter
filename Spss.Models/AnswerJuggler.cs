//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Spss.Models.Data;
//using System.Diagnostics;

//namespace Spss.Models {
  //public class AnswerJuggler { // juggles data

    //public IEnumerable<IAnswerBook> ToJournalAnswers(IEnumerable<IAnswer> allAnswers, IEnumerable<JournalInfo> journalInfos) {
    //  var indexedByJournalInfo = GroupAnswersByJournal(allAnswers, journalInfos);
    //  return ToJournalAnswers(indexedByJournalInfo);
    //}

    //private Dictionary<JournalInfo, List<IAnswer>> GroupAnswersByJournal(IEnumerable<IAnswer> allAnswers, IEnumerable<JournalInfo> journalInfos) {
    //  var indexedJournalAnswers = new Dictionary<JournalInfo, List<IAnswer>>();

    //  var indexedJournalInfos = IndexJournalInfos(journalInfos);

    //  foreach (var answer in allAnswers) {
    //    if (!indexedJournalInfos.ContainsKey(answer.Id))
    //      Debug.WriteLine(String.Format("GroupAnswersByJournal: journalInfo not found {0}", answer.Id));

    //    var journalInfo = indexedJournalInfos[answer.Id];
    //    if (!indexedJournalAnswers.ContainsKey(journalInfo)) // add empty if no answers are added already
    //      indexedJournalAnswers.Add(journalInfo, new List<IAnswer> { answer });
    //    else {  // existing journalAnswers object, add answer to it
    //      List<IAnswer> answerList = indexedJournalAnswers[journalInfo];
    //      answerList.Add(answer);
    //    }
    //  }
    //  return indexedJournalAnswers;
    //}

    //private IEnumerable<IAnswerBook> ToJournalAnswers(Dictionary<JournalInfo, List<IAnswer>> indexedAnswers) {
    //  var results = new List<IAnswerBook>();
    //  foreach (KeyValuePair<JournalInfo, List<IAnswer>> tuple in indexedAnswers) {
    //    var journalAnswers = new JournalAnswers(tuple.Key) { Answers = tuple.Value };
    //    results.Add(journalAnswers);
    //  }
    //  return results;
    //}

    //public Dictionary<int, JournalInfo> IndexJournalInfos(IEnumerable<JournalInfo> journalInfos) {
    //  var indexedJournalInfos = new Dictionary<int, JournalInfo>();
    //  foreach (var journalInfo in journalInfos) {
    //    if (!indexedJournalInfos.ContainsKey(journalInfo.JournalId))
    //      indexedJournalInfos.Add(journalInfo.JournalId, journalInfo);
    //  }
    //  return indexedJournalInfos;
    //}
  //}
//}