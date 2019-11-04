using System;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Spreadsheet;

namespace App.Core.ReportManager
{
    public static class CellExtension
    {
        public static string GetMyColumnName(this Cell cell)
        {
            return GetColumnName(cell.CellReference);
        }

        public static int GetValueLength(this Cell cell)
        {
            return cell.CellValue.Text.Length;
        }

        private static string GetColumnName(string cellName)
        {
            if (String.IsNullOrEmpty(cellName))
            {
                return "";
            }

            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellName);

            return match.Value;
        }
    }
}
