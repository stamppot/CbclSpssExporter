using System;

namespace Spss.Models.Export
{
    public interface IDocExporter
    {
        void Write(String filename);
    }
}
