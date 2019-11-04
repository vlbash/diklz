using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace App.Core.ReportManager
{
    public abstract class GeneratedClassBase
    {
        public bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        protected Cell ConstructCell(string value, CellValues dataType = CellValues.String, string reference = null, UInt32 styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                CellReference = reference,
                StyleIndex = styleIndex
            };
        }

        private static string GetColumnName(string cellName)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellName);

            return match.Value;
        }

        protected static uint GetRowIndex(string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(cellName);

            return uint.Parse(match.Value);
        }

        public static string IncrementColumn(string Address)
        {
            var parts = Regex.Matches(Address, @"([A-Z]+)|(\d+)");
            if (parts.Count != 2) return null;
            return incCol(parts[0].Value) + parts[1].Value;
        }

        public static string IncrementRow(string Address)
        {
            var parts = Regex.Matches(Address, @"([A-Z]+)|(\d+)");
            if (parts.Count != 2) return null;
            Int32 rowIndex;
            Int32.TryParse(parts[1].Value, out rowIndex);
            return parts[0].Value + (rowIndex + 1);
        }

        private static string incCol(string col)
        {
            if (col == "") return "A";
            string fPart = col.Substring(0, col.Length - 1);
            char lChar = col[col.Length - 1];
            if (lChar == 'Z') return incCol(fPart) + "A";
            return fPart + ++lChar;
        }

        private static void MergeCell(string cell1Name, string cell2Name)
        {
            // Create the merged cell and append it to the MergeCells collection.
            //       MergeCell mergeCell = new MergeCell() { Reference = new StringValue(cell1Name + ":" + cell2Name) };
            //    MergeCells mergeCells.Append(mergeCell);
        }

        protected Font GetFont(double fontsize = 12, bool isBold = false, string fontname = "Calibri")
        {
            Font font = new Font();

            FontSize fontSize = new FontSize() { Val = fontsize };
            FontName fontName = new FontName() { Val = fontname };
            if (isBold)
            {
                font.Append(new Bold() { Val = true });
            }

            font.Append(fontSize);
            font.Append(fontName);

            return font;
        }

        protected Border GetBorder(BorderStyleValues borderStyle = BorderStyleValues.None, double indexed = 64)
        {
            Border border = new Border();

            LeftBorder leftBorder = new LeftBorder() { Style = borderStyle };
            RightBorder rightBorder = new RightBorder() { Style = borderStyle };
            TopBorder topBorder = new TopBorder() { Style = borderStyle };
            BottomBorder bottomBorder = new BottomBorder() { Style = borderStyle };
            DiagonalBorder diagonalBorder = new DiagonalBorder();

            if (borderStyle != BorderStyleValues.None)
            {
                //Color color = new Color() { Indexed = (UInt32Value)indexed };
                leftBorder.Append(new Color() { Indexed = (UInt32Value)indexed });
                rightBorder.Append(new Color() { Indexed = (UInt32Value)indexed });
                topBorder.Append(new Color() { Indexed = (UInt32Value)indexed });
                bottomBorder.Append(new Color() { Indexed = (UInt32Value)indexed });
            }

            border.Append(leftBorder);
            border.Append(rightBorder);
            border.Append(topBorder);
            border.Append(bottomBorder);
            border.Append(diagonalBorder);

            return border;
        }

        protected CellFormat GetCellFormat(UInt32 numberFormatId = 0U, UInt32 fontId = 0U, UInt32 borderId = 0U, UInt32 formatId = 0U, bool applyFont = false, bool applyAlignment = false, bool applyBorder = false)
        {
            var cellFormat = new CellFormat() { NumberFormatId = numberFormatId, FontId = fontId, BorderId = borderId, FormatId = formatId, ApplyFont = applyFont, ApplyAlignment = applyAlignment, ApplyBorder = applyBorder };
            if (applyAlignment)
            {
                cellFormat.Append(new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center });
            }
            return cellFormat;
        }

        private static Worksheet GetWorksheet(SpreadsheetDocument document, string worksheetName)
        {
            IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == worksheetName);
            WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheets.First().Id);
            if (sheets.Count() == 0)
                return null;
            else
                return worksheetPart.Worksheet;
        }

        public static int ColumnLetterToColumnIndex(string columnLetter)
        {
            columnLetter = columnLetter.ToUpper();
            int sum = 0;

            for (int i = 0; i < columnLetter.Length; i++)
            {
                sum *= 26;
                sum += (columnLetter[i] - 'A' + 1);
            }
            return sum;
        }
    }
}