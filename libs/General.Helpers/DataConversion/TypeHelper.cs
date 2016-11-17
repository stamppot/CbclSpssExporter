using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Helpers.DataConversion {
    public class TypeHelper {

        public Type FieldDataTypeToExcelDataType(String dataType) {
            Type result;
            switch (dataType) {
                case "NUMERIC": result = typeof(int); break;
                case "CHECKBOX": result = typeof(short); break;
                case "COMBO": result = typeof(string); break;
                case "TEXT": result = typeof(string); break;
                case "TEXTAREA": result = typeof(string); break;
                case "DATE": result = typeof(string); break;
                case "FILE": result = typeof(string); break;
                case "SAMPLELINK": result = typeof(string); break;
                case "PROJECT": result = typeof(string); break;
                default: result = typeof(string); break;
            }
            return result;
        }
    }
}
