using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D; 
using System.Drawing.Imaging;   

namespace QLTV
{
    public partial class fLogin : Form
    {
        public fLogin()
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
            
        }

        
        public Bitmap ApplyGaussianBlur(Bitmap image)
        {
            
            using (Graphics g = Graphics.FromImage(image))
            {
                
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                
                g.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), new Rectangle(0, 0, image.Width, image.Height));
            }

            return image;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }



        private void button1_Click(object sender, EventArgs e)
        {
            string username = txbUsername.Text;
            string password = txbPassWord.Text;

            string correctUsername = "admin";
            string correctPassword = "1";

            if (username == correctUsername && password == correctPassword)
            {
                // Mở form chính (fInterface)
                fInterface f = new fInterface();
                this.Hide();  // Ẩn form đăng nhập hiện tại

                f.Show();  // Hiển thị form chính

                // Đặt DialogResult là OK để đánh dấu đăng nhập thành công và đóng form đăng nhập
                this.DialogResult = DialogResult.OK;
           
            }
            else
            {
                // Thông báo lỗi nếu thông tin đăng nhập không chính xác
                MessageBox.Show("Username or password is incorrect, please try again.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txbUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txbPassWord_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
            
        }
        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void fLogin_Load(object sender, EventArgs e)
        {

        }
    }

}
