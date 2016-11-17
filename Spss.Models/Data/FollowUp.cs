using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spss.Models.Data
{
    public class FollowUpHelper
    {
        public static FollowUp Read(string followUp)
        {
            if (followUp == "Diagnose")
                return FollowUp.Diagnose;
            else
            {
                if (followUp.StartsWith("1"))
                    return FollowUp.FirstFollowUp;
                if (followUp.StartsWith("2"))
                    return FollowUp.SecondFollowUp;
                if (followUp.StartsWith("3"))
                    return FollowUp.ThirdFollowUp;
                if (followUp.StartsWith("4"))
                    return FollowUp.FourthFollowUp;
                if (followUp.StartsWith("5"))
                    return FollowUp.FifthFollowUp;
                
                return FollowUp.Unknown;
            }

        }
    }

    public enum FollowUp
    {
        Diagnose = 0,
        FirstFollowUp = 1,
        SecondFollowUp = 2,
        ThirdFollowUp = 3,
        FourthFollowUp = 4,
        FifthFollowUp = 5,
        Unknown = 99
    }
}
