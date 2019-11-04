using System;
using System.Collections;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace App.Core.ReportManager
{
    public class GeneratedClassDoc: GeneratedClassBase
    {
        public byte[] CreatePackageAsBytes(Object list)
        {
            using (var mstm = new MemoryStream())
            {
                using (WordprocessingDocument package = WordprocessingDocument.Create(mstm, WordprocessingDocumentType.Document))
                {
                    CreateParts(package, list);
                }
                mstm.Flush();
                mstm.Close();
                return mstm.ToArray();
            }
        }

        // Creates a WordprocessingDocument.
        public void CreatePackage(string filePath, Object list)
        {
            using (WordprocessingDocument package = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                CreateParts(package, list);
            }
        }

        // Adds child parts and generates content of the specified part.
        private void CreateParts(WordprocessingDocument document, Object list)
        {
            MainDocumentPart mainDocumentPart = document.AddMainDocumentPart();
            var doc = document.MainDocumentPart.Document;
            //Styles
            SetStyles(mainDocumentPart);

            FontTablePart fontTablePart = mainDocumentPart.AddNewPart<FontTablePart>("rId4");
            SetFontTablePart(fontTablePart);

            GenerateMainDocumentPartContent(mainDocumentPart, list);

            //doc.Body.Append();
            document.Save();
        }

        private void SetStyles(MainDocumentPart mainDocumentPart)
        {
            StyleDefinitionsPart styleDefinitionsPart1 = mainDocumentPart.AddNewPart<StyleDefinitionsPart>("rId1");
            Styles styles1 = new Styles();// { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15" } };
                                          //styles1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
                                          //styles1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
                                          //styles1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
                                          //styles1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
                                          //styles1.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");

            styles1.Append(SetRunPropertyDefault());

            Style style1 = new Style() { Type = StyleValues.Paragraph, StyleId = "Normal", Default = true };
            StyleName styleName1 = new StyleName() { Val = "Normal" };
            PrimaryStyle primaryStyle1 = new PrimaryStyle();

            style1.Append(styleName1);
            style1.Append(primaryStyle1);

            Style style2 = new Style() { Type = StyleValues.Character, StyleId = "DefaultParagraphFont", Default = true };
            StyleName styleName2 = new StyleName() { Val = "Default Paragraph Font" };
            UIPriority uIPriority1 = new UIPriority() { Val = 1 };
            SemiHidden semiHidden1 = new SemiHidden();
            UnhideWhenUsed unhideWhenUsed1 = new UnhideWhenUsed();

            style2.Append(styleName2);
            style2.Append(uIPriority1);
            style2.Append(semiHidden1);
            style2.Append(unhideWhenUsed1);

            Style style3 = new Style() { Type = StyleValues.Table, StyleId = "TableNormal", Default = true };
            StyleName styleName3 = new StyleName() { Val = "Normal Table" };
            UIPriority uIPriority2 = new UIPriority() { Val = 99 };
            SemiHidden semiHidden2 = new SemiHidden();
            UnhideWhenUsed unhideWhenUsed2 = new UnhideWhenUsed();

            StyleTableProperties styleTableProperties1 = new StyleTableProperties();
            TableIndentation tableIndentation1 = new TableIndentation() { Width = 0, Type = TableWidthUnitValues.Dxa };

            TableCellMarginDefault tableCellMarginDefault1 = new TableCellMarginDefault();
            TopMargin topMargin1 = new TopMargin() { Width = "0", Type = TableWidthUnitValues.Dxa };
            TableCellLeftMargin tableCellLeftMargin1 = new TableCellLeftMargin() { Width = 108, Type = TableWidthValues.Dxa };
            BottomMargin bottomMargin1 = new BottomMargin() { Width = "0", Type = TableWidthUnitValues.Dxa };
            TableCellRightMargin tableCellRightMargin1 = new TableCellRightMargin() { Width = 108, Type = TableWidthValues.Dxa };

            tableCellMarginDefault1.Append(topMargin1);
            tableCellMarginDefault1.Append(tableCellLeftMargin1);
            tableCellMarginDefault1.Append(bottomMargin1);
            tableCellMarginDefault1.Append(tableCellRightMargin1);

            styleTableProperties1.Append(tableIndentation1);
            styleTableProperties1.Append(tableCellMarginDefault1);

            style3.Append(styleName3);
            style3.Append(uIPriority2);
            style3.Append(semiHidden2);
            style3.Append(unhideWhenUsed2);
            style3.Append(styleTableProperties1);

            Style style4 = new Style() { Type = StyleValues.Numbering, StyleId = "NoList", Default = true };
            StyleName styleName4 = new StyleName() { Val = "No List" };
            UIPriority uIPriority3 = new UIPriority() { Val = 99 };
            SemiHidden semiHidden3 = new SemiHidden();
            UnhideWhenUsed unhideWhenUsed3 = new UnhideWhenUsed();

            style4.Append(styleName4);
            style4.Append(uIPriority3);
            style4.Append(semiHidden3);
            style4.Append(unhideWhenUsed3);

            styles1.Append(style1);
            styles1.Append(style2);
            styles1.Append(style3);
            styles1.Append(style4);

            styleDefinitionsPart1.Styles = styles1;
        }

        private DocDefaults SetRunPropertyDefault()
        {
            DocDefaults docDefaults1 = new DocDefaults();

            RunPropertiesDefault runPropertiesDefault1 = new RunPropertiesDefault();

            RunPropertiesBaseStyle runPropertiesBaseStyle1 = new RunPropertiesBaseStyle();
            RunFonts runFonts33 = new RunFonts() { AsciiTheme = ThemeFontValues.MinorHighAnsi, HighAnsiTheme = ThemeFontValues.MinorHighAnsi, EastAsiaTheme = ThemeFontValues.MinorHighAnsi, ComplexScriptTheme = ThemeFontValues.MinorBidi };
            FontSize fontSize4 = new FontSize() { Val = "22" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "22" };
            Languages languages8 = new Languages() { Val = "en-US", EastAsia = "en-US", Bidi = "ar-SA" };

            runPropertiesBaseStyle1.Append(runFonts33);
            runPropertiesBaseStyle1.Append(fontSize4);
            runPropertiesBaseStyle1.Append(fontSizeComplexScript1);
            runPropertiesBaseStyle1.Append(languages8);

            runPropertiesDefault1.Append(runPropertiesBaseStyle1);

            ParagraphPropertiesDefault paragraphPropertiesDefault1 = new ParagraphPropertiesDefault();

            ParagraphPropertiesBaseStyle paragraphPropertiesBaseStyle1 = new ParagraphPropertiesBaseStyle();
            SpacingBetweenLines spacingBetweenLines13 = new SpacingBetweenLines() { After = "160", Line = "259", LineRule = LineSpacingRuleValues.Auto };

            paragraphPropertiesBaseStyle1.Append(spacingBetweenLines13);

            paragraphPropertiesDefault1.Append(paragraphPropertiesBaseStyle1);

            docDefaults1.Append(runPropertiesDefault1);
            docDefaults1.Append(paragraphPropertiesDefault1);

            return docDefaults1;
        }

        private void SetFontTablePart(FontTablePart fontTablePart1)
        {
            Fonts fonts1 = new Fonts() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "w14 w15" } };
            fonts1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            fonts1.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            fonts1.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            fonts1.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
            fonts1.AddNamespaceDeclaration("w15", "http://schemas.microsoft.com/office/word/2012/wordml");

            Font font1 = new Font() { Name = "Calibri" };
            Panose1Number panose1Number1 = new Panose1Number() { Val = "020F0502020204030204" };
            FontCharSet fontCharSet1 = new FontCharSet() { Val = "00" };
            FontFamily fontFamily1 = new FontFamily() { Val = FontFamilyValues.Swiss };
            Pitch pitch1 = new Pitch() { Val = FontPitchValues.Variable };
            FontSignature fontSignature1 = new FontSignature() { UnicodeSignature0 = "E0002AFF", UnicodeSignature1 = "C000247B", UnicodeSignature2 = "00000009", UnicodeSignature3 = "00000000", CodePageSignature0 = "000001FF", CodePageSignature1 = "00000000" };

            font1.Append(panose1Number1);
            font1.Append(fontCharSet1);
            font1.Append(fontFamily1);
            font1.Append(pitch1);
            font1.Append(fontSignature1);

            Font font2 = new Font() { Name = "Times New Roman" };
            Panose1Number panose1Number2 = new Panose1Number() { Val = "02020603050405020304" };
            FontCharSet fontCharSet2 = new FontCharSet() { Val = "00" };
            FontFamily fontFamily2 = new FontFamily() { Val = FontFamilyValues.Roman };
            Pitch pitch2 = new Pitch() { Val = FontPitchValues.Variable };
            FontSignature fontSignature2 = new FontSignature() { UnicodeSignature0 = "E0002EFF", UnicodeSignature1 = "C000785B", UnicodeSignature2 = "00000009", UnicodeSignature3 = "00000000", CodePageSignature0 = "000001FF", CodePageSignature1 = "00000000" };

            font2.Append(panose1Number2);
            font2.Append(fontCharSet2);
            font2.Append(fontFamily2);
            font2.Append(pitch2);
            font2.Append(fontSignature2);

            Font font3 = new Font() { Name = "Calibri Light" };
            Panose1Number panose1Number3 = new Panose1Number() { Val = "020F0302020204030204" };
            FontCharSet fontCharSet3 = new FontCharSet() { Val = "00" };
            FontFamily fontFamily3 = new FontFamily() { Val = FontFamilyValues.Swiss };
            Pitch pitch3 = new Pitch() { Val = FontPitchValues.Variable };
            FontSignature fontSignature3 = new FontSignature() { UnicodeSignature0 = "E0002AFF", UnicodeSignature1 = "C000247B", UnicodeSignature2 = "00000009", UnicodeSignature3 = "00000000", CodePageSignature0 = "000001FF", CodePageSignature1 = "00000000" };

            font3.Append(panose1Number3);
            font3.Append(fontCharSet3);
            font3.Append(fontFamily3);
            font3.Append(pitch3);
            font3.Append(fontSignature3);

            fonts1.Append(font1);
            fonts1.Append(font2);
            fonts1.Append(font3);

            fontTablePart1.Fonts = fonts1;
        }

        private void GenerateMainDocumentPartContent(MainDocumentPart mainDocumentPart1, object list)
        {
            Document document1 = new Document();

            Body body1 = new Body();
            body1.AppendChild(SetParagraph("Список карт клиента Иванова В.В.", 28, true));
            body1.AppendChild(SetParagraph());   //_

            #region List data section
            //Cell Data region
            var realType = list.GetType();
            var convertedObject = new object();
            var reallList = new object();

            if (IsList(list))
            {
                reallList = Convert.ChangeType(list, realType);
                convertedObject = (reallList is IList) && (reallList as IList).Count > 0 ? (reallList as IList)[0] : null;
            }
            else
            {
                convertedObject = Convert.ChangeType(list, realType);
            }

            var props = convertedObject.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(ExportFilePropAttribute), false).Count() == 1);
            #endregion


            Table table1 = new Table();
            TableRow tableRow1 = new TableRow();
            foreach (var item in props)
            {
                string value = ((ExportFilePropAttribute)Attribute.GetCustomAttribute(item, typeof(ExportFilePropAttribute))).GetHeader();
                tableRow1.Append(SetTableCell(value, isBold: true));
            }
            table1.Append(tableRow1);

            if (IsList(list))
            {
                foreach (var listItem in reallList as IList)
                {
                    TableRow newRow = new TableRow();
                    foreach (var item in props)
                    {
                        string value = item.GetValue(listItem, null).ToString();
                        newRow.Append(SetTableCell(value));
                    }
                    table1.Append(newRow);
                }
            }
            else
            {
                TableRow newRow = new TableRow();
                foreach (var item in props)
                {
                    string value = item.GetValue(convertedObject, null).ToString();
                    newRow.Append(SetTableCell(value));
                }
                table1.Append(newRow);
            }

            body1.AppendChild(table1);
            body1.AppendChild(SetParagraph());   //_
            body1.AppendChild(SetParagraph());   //_
            body1.AppendChild(SetParagraph("Подписано:"));

 
            document1.Append(body1);
            mainDocumentPart1.Document = document1;
        }

        private Style SetStyle()
        {
            Style style2 = new Style() { Type = StyleValues.Character, StyleId = "DefaultParagraphFont", Default = true };
            StyleName styleName2 = new StyleName() { Val = "Default Paragraph Font" };
            UIPriority uIPriority1 = new UIPriority() { Val = 1 };
            SemiHidden semiHidden1 = new SemiHidden();
            UnhideWhenUsed unhideWhenUsed1 = new UnhideWhenUsed();

            style2.Append(styleName2);
            style2.Append(uIPriority1);
            style2.Append(semiHidden1);
            style2.Append(unhideWhenUsed1);

            return style2;
        }

        private Paragraph SetParagraph(string textVal = null, int fontsize = 22, bool isBold = false)
        {
            Languages languages = new Languages() { Val = "ru-RU" };
            Paragraph paragraph = new Paragraph();// { RsidParagraphAddition = "00A341BE", RsidRunAdditionDefault = "00C92E31" };
            ParagraphProperties paragraphProperties1 = new ParagraphProperties();
            ParagraphMarkRunProperties paragraphMarkRunProperties1 = new ParagraphMarkRunProperties();

            if (isBold)
            {
                Bold bold1 = new Bold() { Val = true };
                paragraphMarkRunProperties1.Append(bold1);
            }

            FontSize fontSize1 = new FontSize() { Val = fontsize.ToString() };
            paragraphMarkRunProperties1.Append(fontSize1);

            paragraphMarkRunProperties1.Append(languages);
            paragraphProperties1.Append(paragraphMarkRunProperties1);

            if (textVal != null)
            {
                Languages languages2 = new Languages() { Val = "ru-RU" };
                Run run1 = new Run();// { RsidRunProperties = "00C92E31" };

                RunProperties runProperties1 = new RunProperties();
                if (isBold)
                {
                    Bold bold = new Bold() { Val = isBold };
                    runProperties1.Append(bold);
                    BoldComplexScript boldComplexScript = new BoldComplexScript();
                    runProperties1.Append(boldComplexScript);
                }
                
                FontSize fontSize = new FontSize() { Val = fontsize.ToString() };
                runProperties1.Append(fontSize);
                runProperties1.Append(languages2);

                Text text1 = new Text();
                text1.Text = textVal;

                run1.Append(runProperties1);
                run1.Append(text1);
                paragraph.Append(paragraphProperties1);
                paragraph.Append(run1);
            }
            else
            {
                paragraph.Append(paragraphProperties1);
            }

            return paragraph;
        }

        private TableCell SetTableCell(string textVal = null, int fontsize = 22, bool isBold = false)
        {
            TableCell tableCell = new TableCell();
            TableCellProperties tableCellProperties1 = new TableCellProperties();
            //TableCellWidth tableCellWidth1 = new TableCellWidth() { Width = "1175", Type = TableWidthUnitValues.Dxa };
            //tableCellProperties1.Append(tableCellWidth1);
            tableCellProperties1.Append(SetTableCellBorders());
            tableCell.AppendChild(tableCellProperties1);
            tableCell.AppendChild(SetParagraph(textVal, fontsize, isBold));
            
            return tableCell;
        }

        private TableCellBorders SetTableCellBorders(string color = "auto", UInt32 size = 4, UInt32 space = 0)
        {
            TableCellBorders tableCellBorders1 = new TableCellBorders();
            TopBorder topBorder1 = new TopBorder() { Val = BorderValues.Single, Color = color, Size = size, Space = space };
            LeftBorder leftBorder1 = new LeftBorder() { Val = BorderValues.Single, Color = color, Size = size, Space = space };
            BottomBorder bottomBorder1 = new BottomBorder() { Val = BorderValues.Single, Color = color, Size = size, Space = space };
            RightBorder rightBorder1 = new RightBorder() { Val = BorderValues.Single, Color = color, Size = size, Space = space };

            tableCellBorders1.Append(topBorder1);
            tableCellBorders1.Append(leftBorder1);
            tableCellBorders1.Append(bottomBorder1);
            tableCellBorders1.Append(rightBorder1);

            return tableCellBorders1;
        }
    }
}

