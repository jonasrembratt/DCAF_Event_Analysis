using System;

namespace DCAF.Inspection
{
    public class CsvFormatException : FormatException
    {
        public CsvFormatException(string message, int lineNo, Exception? inner = null)
        : base($"{message} @ line {lineNo.ToString()}", inner)
        {
        }
    }
}