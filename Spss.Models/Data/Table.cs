using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spss.Models {
  public class Table {

    public Table() {
      Headers = new List<Variable>();
    }

    public List<Variable> Headers { get; set; }
  }
}
