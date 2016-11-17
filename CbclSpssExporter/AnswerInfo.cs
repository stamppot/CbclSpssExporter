using System;
using System.Collections.Generic;
using System.Data;
using Spss.Models.Data;

namespace CbclSpssExporter
{
    public class AnswerInfo : AnswerMetadata
    {
        public string Birthdate { get; set; }
        public string Center { get; set; }
        public DateTime? Besvarelsesdato_TRF { get; set; }
        public DateTime? Besvarelsesdato_CBCL { get; set; }
        public bool SPSS { get; set; }

        public AnswerInfo(AnswerMetadata answerMetadata)
        {
            Id = answerMetadata.Id;
            Pid = answerMetadata.Pid;
            this.Birthdate = answerMetadata.Pfoedt;
            this.Pfoedt = answerMetadata.Pfoedt;
            this.Palder = answerMetadata.Palder;
            this.Pkoen = answerMetadata.Pkoen;
            this.Pnation = answerMetadata.Pnation;
            this.Projekt = answerMetadata.Projekt;
            this.JournalId = answerMetadata.JournalId;
            this.Safdnavn = answerMetadata.Safdnavn;
            this.Ssghafd = answerMetadata.Ssghafd;
            this.Ssghnavn = answerMetadata.Ssghnavn;
            this.Besvarelsesdato = answerMetadata.Besvarelsesdato;
        }

       public static IEnumerable<AnswerInfo> GetJournalInfos(DataTable table, int surveyId)
       {
           var infos = new List<AnswerInfo>();
           foreach(DataRow row in table.Rows)
           {
               var answerInfo = new AnswerInfo(AnswerMetadata.GetJournalInfo(row));
               if(surveyId == 2)
                   answerInfo.Besvarelsesdato_CBCL = answerInfo.Besvarelsesdato;
               else
                   answerInfo.Besvarelsesdato_TRF = answerInfo.Besvarelsesdato;
               
               infos.Add(answerInfo);
           }

           return infos;
       }

       public string ToCsvLine()
       {
           return String.Join(";",
                              new string[]
                                  {
                                      Pfoedt, Pid, Projekt, Palder.ToString(), Pkoen.ToString(), Ssghnavn, Ssghafd, Safdnavn,
                                      Besvarelsesdato_CBCL.HasValue ? Besvarelsesdato_CBCL.Value.ToShortDateString() : "",
                                      Besvarelsesdato_TRF.HasValue ? Besvarelsesdato_TRF.Value.ToShortDateString() : "", SPSS.ToString()
                                  });
       }

       public static string CsvHeader()
       {
           return string.Join(";", new string[] {"Foedt", "lb nr", "Projekt", "Alder", "Koen", "Ssghnavn",
                              "ssghafd", "safdnavn", "CBCL Besvaret", "TRF Besvaret", "I SPSS"});
       }

    }
}
