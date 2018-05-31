using System;
using iText.IO.Util;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Test;

namespace iText.Layout {
    public class InlineBlockTest : ExtendedITextTest {
        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/layout/InlineBlockTest/";

        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/layout/InlineBlockTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            CreateDestinationFolder(destinationFolder);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void InlineTableTest01() {
            // TODO DEVSIX-1967
            String name = "inlineTableTest01.pdf";
            String outFileName = destinationFolder + name;
            String cmpFileName = sourceFolder + "cmp_" + name;
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outFileName));
            Document doc = new Document(pdfDoc);
            Paragraph p = new Paragraph().SetMultipliedLeading(0);
            p.Add(new Paragraph("This is inline table: ").SetBorder(new SolidBorder(1)).SetMultipliedLeading(0));
            Table inlineTable = new Table(1);
            int commonPadding = 10;
            Cell cell = new Cell();
            Paragraph paragraph = new Paragraph("Cell 1");
            inlineTable.AddCell(cell.Add(paragraph.SetMultipliedLeading(0)).SetPadding(commonPadding).SetWidth(33));
            Div div = new Div();
            p.Add(div.Add(inlineTable).SetPadding(commonPadding)).Add(new Paragraph(". Was it fun?").SetBorder(new SolidBorder
                (1)).SetMultipliedLeading(0));
            SolidBorder border = new SolidBorder(1);
            doc.Add(p);
            Paragraph p1 = new Paragraph().Add(p).SetBorder(border);
            doc.Add(p1);
            Paragraph p2 = new Paragraph().Add(p1).SetBorder(border);
            doc.Add(p2);
            Paragraph p3 = new Paragraph().Add(p2).SetBorder(border);
            doc.Add(p3);
            Paragraph p4 = new Paragraph().Add(p3).SetBorder(border);
            doc.Add(p4);
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void DeepNestingInlineBlocksTest01() {
            // TODO DEVSIX-1963
            String name = "deepNestingInlineBlocksTest01.pdf";
            String outFileName = destinationFolder + name;
            String cmpFileName = sourceFolder + "cmp_" + name;
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outFileName));
            Document doc = new Document(pdfDoc);
            Color[] colors = new Color[] { ColorConstants.BLUE, ColorConstants.RED, ColorConstants.LIGHT_GRAY, ColorConstants
                .ORANGE };
            int w = 60;
            int n = 6;
            Paragraph p = new Paragraph("hello world").SetWidth(w);
            for (int i = 0; i < n; ++i) {
                Paragraph currP = new Paragraph().SetWidth(i == 0 ? w * 1.1f * 3 : 450 + 5 * i);
                currP.Add(p).Add(p).Add(p).SetBorder(new DashedBorder(colors[i % colors.Length], 0.5f));
                p = currP;
            }
            long start = SystemUtil.GetRelativeTimeMillis();
            doc.Add(p);
            System.Console.Out.WriteLine(SystemUtil.GetRelativeTimeMillis() - start);
            // 606 on local machine (including jvm warming up)
            p = new Paragraph("hello world");
            for (int i = 0; i < n; ++i) {
                Paragraph currP = new Paragraph();
                currP.Add(p).Add(p).Add(p).SetBorder(new DashedBorder(colors[i % colors.Length], 0.5f));
                p = currP;
            }
            start = SystemUtil.GetRelativeTimeMillis();
            doc.Add(p);
            System.Console.Out.WriteLine(SystemUtil.GetRelativeTimeMillis() - start);
            // 4656 on local machine
            doc.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(outFileName, cmpFileName, destinationFolder
                , "diff"));
        }
    }
}