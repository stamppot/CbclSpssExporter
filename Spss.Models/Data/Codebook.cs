using System;
using System.Collections.Generic;
using Spss.Models.Interfaces;

namespace Spss.Models.Data
{
    public class Codebook : ICodebook
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public SurveyType SurveyType { get; set; }

        public IEnumerable<ICode> Codes { get; set; }

        public Codebook() {
            Codes = new List<ICode>();
        }
    }
}
