using System;
using System.Collections.Generic;
//using System.Text;
using System.Data;
using System.IO;
using Spss;

namespace Spss.Models {
  class MyProgram {
    static void Main(string[] args) {
      Console.WriteLine("SPSS file writing demo:");
      if (File.Exists("example.sav"))
        File.Delete("example.sav");
      CreateExampleDocument();

      Console.WriteLine("Exporting a DataTable demo... (the source code is interesting)");
      DataTable dt = SpssConvert.ToDataTable(GetFileName());
      if (File.Exists("example2.sav"))
        File.Delete("example2.sav");
      SpssConvert.ToFile(dt, "example2.sav", MetaDataCallback);

      Console.WriteLine("SPSS dictionary copying demo:");
      if (File.Exists("example3.sav"))
        File.Delete("example3.sav");
      using (SpssDataDocument doc = SpssDataDocument.Create("example3.sav", GetFileName())) {
        PrintMetaData(doc);
      }

      Console.WriteLine("Demo concluded.  Press any key to end.");
      Console.ReadKey();
    }

    private static void CreateExampleDocument() {
      using (SpssDataDocument doc = SpssDataDocument.Create("example.sav")) {
        CreateMetaData(doc);
        CreateData(doc);
      }
      Console.WriteLine("Examine example.sav for the results.");
      Console.WriteLine("SPSS file reading demo:");
      using (SpssDataDocument doc = SpssDataDocument.Open(GetFileName(), SpssFileAccess.Read)) {
        PrintMetaData(doc);
        PrintData(doc);
      }
    }


    public static string GetFileName() {
      if (File.Exists("..\\..\\demo.sav"))
        return "..\\..\\demo.sav";
      if (File.Exists("..\\..\\..\\demo.sav"))
        return "..\\..\\..\\demo.sav";
      if (File.Exists("..\\..\\..\\..\\demo.sav"))
        return "..\\..\\..\\..\\demo.sav";
      if (File.Exists("demo.sav"))
        return "demo.sav";
      throw new ApplicationException("Cannot find demo.sav file.");
    }

    public static void CreateMetaData(SpssDataDocument doc) {
      // Define dictionary
      SpssStringVariable v1 = new SpssStringVariable();
      v1.Name = "v1";
      v1.Label = "What is your name?";
      doc.Variables.Add(v1);
      SpssNumericVariable v2 = new SpssNumericVariable();
      v2.Name = "v2";
      v2.Label = "How old are you?";
      doc.Variables.Add(v2);
      SpssNumericVariable v3 = new SpssNumericVariable();
      v3.Name = "v3";
      v3.Label = "What is your gender?";
      v3.ValueLabels.Add(1, "Male");
      v3.ValueLabels.Add(2, "Female");
      doc.Variables.Add(v3);
      SpssDateVariable v4 = new SpssDateVariable();
      v4.Name = "v4";
      v4.Label = "What is your birthdate?";
      doc.Variables.Add(v4);
      // Add some data
      doc.CommitDictionary();
    }

    public static void CreateData(SpssDataDocument doc) {
      SpssCase case1 = doc.Cases.New();
      case1.SetDBValue("v1", "Andrew");
      case1.SetDBValue("v2", 24);
      case1.SetDBValue("v3", 1);
      case1.SetDBValue("v4", DateTime.Parse("1/1/1982 7:32 PM"));
      case1.Commit();
      SpssCase case2 = doc.Cases.New();
      case2.SetDBValue("v1", "Cindy");
      case2.SetDBValue("v2", 21);
      case2.SetDBValue("v3", 2);
      case2.SetDBValue("v4", DateTime.Parse("12/31/2002"));
      case2.Commit();
    }

    public static void MetaDataCallback(SpssVariable var) {
      // In a real application, you would probably draw this metadata out of 
      // some repository of your own rather than hard-coding the labels.
      switch (var.Name) {
        case "v1":
          var.Label = "What is your name?";
          break;
        case "v2":
          var.Label = "How old are you?";
          break;
        case "v3":
          var.Label = "What is your gender?";
          // Set the value labels
          SpssNumericVariable numericVar = (SpssNumericVariable)var;
          numericVar.ValueLabels[1] = "Male";
          numericVar.ValueLabels[2] = "Female";
          break;
        case "v4":
          var.Label = "What is your birthdate?";
          break;
      }
    }

    public static void PrintMetaData(SpssDataDocument doc) {
      Console.WriteLine("Variables:");
      foreach (SpssVariable var in doc.Variables) {
        Console.WriteLine("{0}" + Environment.NewLine + "{1}", var.Name, var.Label);
        if (var is SpssNumericVariable) {
          SpssNumericVariable varNum = (SpssNumericVariable)var;
          foreach (KeyValuePair<double, string> label in varNum.ValueLabels) {
            Console.WriteLine(Environment.NewLine + label.Key.ToString() + Environment.NewLine + label.Value.ToString());
          }
        }
      }
    }

    public static void PrintData(SpssDataDocument doc) {
      foreach (SpssVariable var in doc.Variables) {
        Console.Write(var.Name + Environment.NewLine);
      }
      Console.WriteLine();

      foreach (SpssCase row in doc.Cases) {
        foreach (SpssVariable var in doc.Variables) {
          if ((row[var.Name] == null)) {
            Console.Write("<SYSMISS>");
          } else {
            Console.Write(row[var.Name]);
          }
          Console.Write(Environment.NewLine);
        }
        Console.WriteLine();
      }
    }
  }
}
