using System;
using System.Collections.Generic;

namespace Spss.Models.Interfaces
{
    public interface ICodebook
    {
        int Id { get; }
        string Description { get; }
        SurveyType SurveyType { get; }
        string Title { get; }
        IEnumerable<ICode> Codes { get;  }
    }
}
