using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTV
{
    public partial class fRegister : Form
    {
        public fRegister()
        {
            InitializeComponent();

            // Đường dẫn tới file ảnh


            Bitmap originalImage = Properties.Resources.DHBK;


            Bitmap blurredImage = ApplyGaussianBlur(originalImage);


            panel1.BackgroundImage = blurredImage;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
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

                // Tạo Rectangle để áp dụng độ mờ
                g.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), new Rectangle(0, 0, image.Width, image.Height));
            }

            return image;
        }
            private void panel1_Paint(object sender, PaintEventArgs e)
            {

            }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter complete information!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Kiểm tra xem Password và ConfirmPassword có khớp hay không
                if (textBox2.Text != textBox3.Text)
                {
                    MessageBox.Show("Confirmation password does not match!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // Nếu mọi thứ ổn, chuyển sang form đăng nhập
                    MessageBox.Show("Registered successfully! Go to the login page.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở form đăng nhập
                    fLogin f = new fLogin();
                    this.Show();
                    this.Hide(); // Ẩn form hiện tại
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fLogin f = new fLogin();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
