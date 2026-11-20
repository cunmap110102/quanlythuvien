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
    public partial class fInterface : Form
    {
        public fInterface()
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

        private void informationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void studentPersonalInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fInformation f = new fInformation();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           
    }
        
        
        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Hiển thị một thông báo xác nhận đăng xuất
            var result = MessageBox.Show("Do you want to log out?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();  // Đóng form chính (fInterface)
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fChangePass  f = new fChangePass();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void uploadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fUpload  f = new fUpload ();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void convertPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fConvert  f = new fConvert();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

       

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fUpdateInfo f = new fUpdateInfo();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void adminToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fSearch f = new fSearch();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void signInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                fLogin loginForm = new fLogin();
                
                if (loginForm.ShowDialog() == DialogResult.OK) // Giả sử DialogResult.OK là khi đăng nhập thành công
                {
                    // Ẩn mục Sign In và bật các mục khác
                    signInToolStripMenuItem.Visible = false;
                    foreach (ToolStripMenuItem item in menuStrip1.Items)
                    {
                        item.Enabled = true;
                    }
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void fInterface_Load(object sender, EventArgs e)
        {
            {
                // Vô hiệu hóa tất cả các mục trên MenuStrip ngoại trừ Sign In
                foreach (ToolStripMenuItem item in menuStrip1.Items)
                {
                    if (item.Name != "signInToolStripMenuItem") // Đặt tên chính xác của mục Sign In
                    {
                        item.Enabled = false;
                    }
                }
            }
        }

        private void fInterface_Resize(object sender, EventArgs e)
        {

        }
    }
}
