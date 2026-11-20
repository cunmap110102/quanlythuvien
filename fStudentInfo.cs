using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D; // Thư viện vẽ
using System.Drawing.Imaging;

namespace QLTV
{
    public partial class fStudentInfo : Form
    {
        public fStudentInfo()
        {
            InitializeComponent();

            // Đường dẫn tới file ảnh


            Bitmap originalImage = Properties.Resources.DHBK;


            Bitmap blurredImage = ApplyGaussianBlur(originalImage);


            panel1.BackgroundImage = blurredImage;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Bạn có thể tùy chỉnh thêm ở đây
        }

        // Hàm Gaussian Blur đơn giản
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

                // Tạo Rectangle để áp dụng độ mờ
                g.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), new Rectangle(0, 0, image.Width, image.Height));
            }

            return image;
        
    }

        private void fStudentInfo_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            fInterface f = new fInterface();
            this.Show();               
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            {
                this.Close();
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnUpImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"; // Filter for image files

            // Show the dialog and check if the user selects a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Load the image into the PictureBox
                pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; // Make sure image fits the box
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
