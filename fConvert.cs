using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Office.Interop.Word;
using System.Xml.Linq;
using System.Reflection;
using System.Drawing.Drawing2D;


namespace QLTV
{
    public partial class fConvert : Form
    {
        private OpenFileDialog openFileDialog1 = new OpenFileDialog();
        private TextBox txtPath = new TextBox();
        public fConvert()
        {
            InitializeComponent();
            Bitmap originalImage = Properties.Resources.DHBK;


            Bitmap blurredImage = ApplyGaussianBlur(originalImage);


            panel1.BackgroundImage = blurredImage;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public Bitmap ApplyGaussianBlur(Bitmap image)
        {
            // Tạo hiệu ứng Gaussian Blur
            // Thay thế hoặc tích hợp thư viện hỗ trợ như OpenCv hoặc ImageProcessor để đạt hiệu quả cao hơn
            using (Graphics g = Graphics.FromImage(image))
            {
                // Sử dụng Graphics để vẽ mờ
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                
            }

            return image;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = @"All Files | *.*";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txt.Text = System.IO.Path.GetFullPath(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            object oMissing = System.Reflection.Missing.Value;
            FileInfo wordFile = new FileInfo(txtPath.Text);
            word.Visible = false;
            word.ScreenUpdating = false;
            Object filename = (Object)wordFile.FullName;
            Document doc = word.Documents.Open(ref filename, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            doc.Activate();
            object outputFilename = wordFile.FullName.Replace(".docx", ".pdf");
            object fileformat = WdSaveFormat.wdFormatPDF;
            doc.SaveAs2(ref outputFilename,
            ref fileformat, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            object savechanges = WdSaveOptions.wdSaveChanges;
            ((_Document)doc).Close(ref savechanges, ref oMissing, ref oMissing);
            doc = null;
            ((_Application)word).Quit(ref oMissing, ref oMissing, ref oMissing);
            word = null;
            MessageBox.Show("The File has been converted to PDF");
        }

    }
}
