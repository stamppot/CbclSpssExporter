using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spss.Models {
  public class DataVariable {

    public String Name { get; set; }
    public String Value { get; set; }
    public String Label { get; set; }
    public Datatype Datatype { get; set; }
  }
}
