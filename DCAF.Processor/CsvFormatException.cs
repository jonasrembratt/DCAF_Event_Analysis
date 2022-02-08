using System;

namespace DCAF.Inspection
{
    public class CsvFormatException : FormatException
    {
        public CsvFormatException(string message, int line, Exception? inner = null)
            : base($"{message} @ line {line.ToString()}")
        {
        }
    }
}