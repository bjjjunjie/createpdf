using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Font = iTextSharp.text.Font;
using Image = iTextSharp.text.Image;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SavePDF();
        }
        private void SavePDF()
        {
            Document document = new Document();
            //中文字体
            string chinese = @"D:\kaiu.ttf";
            BaseFont baseFont = BaseFont.CreateFont(chinese, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //文字大小12，文字样式
            Font cn = new Font(baseFont, 12, Font.Height);

            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(@"D:\temp.pdf", FileMode.Create));
            document.Open();
            //最后一个参数是颜色，这里可以是rgb格式，也可以是默认定义的
            var title = new Paragraph("\n 这是一条标题0.0  hello ", new Font(baseFont, 14, Font.Height, BaseColor.RED));
            //居中
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            Paragraph p = new Paragraph(" \n this is Second title \n ", cn);
            //Phrase p = new Phrase("这是一条标题0.0  hello", cn);
            p.Alignment = Element.ALIGN_CENTER;
            document.Add(p);

            //添加表格
            PdfPTable table = new PdfPTable(3);
            PdfPCell cell = new PdfPCell();

            table.AddCell("Row");
            cell = new PdfPCell(new Phrase("Cell"));
            cell.Colspan = 2;
            table.AddCell(cell);

            table.AddCell("row");
            cell = new PdfPCell(new Phrase("Cell"));
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Row"));
            cell.Rowspan = 2;
            table.AddCell(cell);
            table.AddCell("Cell");
            table.AddCell("Cell");
            table.AddCell("Cell");
            table.AddCell("Cell");

            table.HorizontalAlignment = Element.ALIGN_CENTER;
            document.Add(table);

            //新页面
            document.NewPage();
            document.Add(new Paragraph("Second page pic", cn));
           Image img = Image.GetInstance("F:/33.png");

            float percentage = 1;
            //这里都是图片最原始的宽度与高度  
            float resizedWidht = img.Width;
            float resizedHeight = img.Height;

            //这时判断图片宽度是否大于页面宽度减去也边距，如果是，那么缩小，如果还大，继续缩小，  
            //这样这个缩小的百分比percentage会越来越小  
            while (resizedWidht > (document.PageSize.Width - document.LeftMargin - document.RightMargin) * 0.8)
            {
                percentage = percentage * 0.9f;
                resizedHeight = img.Height * percentage;
                resizedWidht = img.Width * percentage;
            }

            while (resizedHeight > (document.PageSize.Height - document.TopMargin - document.BottomMargin) * 0.8)
            {
                percentage = percentage * 0.9f;
                resizedHeight = img.Height * percentage;
                resizedWidht = img.Width * percentage;
            }

            //这里用计算出来的百分比来缩小图片  
            img.ScalePercent(percentage * 100);
            //图片定位，页面总宽283，高416；这里设置0,0的话就是页面的左下角 让图片的中心点与页面的中心店进行重合  
            img.SetAbsolutePosition(document.PageSize.Width / 2 - resizedWidht / 2, document.PageSize.Height / 2 - resizedHeight / 2);
            writer.DirectContent.AddImage(img);

            document.Close();

        }

    }
}
