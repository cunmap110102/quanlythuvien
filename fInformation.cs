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
    public partial class fInformation : Form
    {
        // Các thuộc tính để nhận dữ liệu từ form khác
        public string FullName { get; set; }
        public string StudentID { get; set; }
        public string Class { get; set; }
        public string Faculty { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Thêm thuộc tính để nhận hình ảnh
        public Bitmap UserImage { get; set; }

        public fInformation()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            SetDoubleBuffered(this);
            // Đường dẫn tới file ảnh nền
            Bitmap originalImage = Properties.Resources.DHBK;
            Bitmap blurredImage = ApplyGaussianBlur(originalImage);

            panel1.BackgroundImage = blurredImage;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;

            // Gán sự kiện Load cho form
            this.Load += new System.EventHandler(this.fInformation_Load);
        }

        private void fInformation_Load(object sender, EventArgs e)
        {
            // Gán giá trị cho các Label hoặc TextBox khi form được load
            lblFullName.Text = FullName;
            lblStudentID.Text = StudentID;
            lblClass.Text = Class;
            lblFaculty.Text = Faculty;
            lblDateOfBirth.Text = DateOfBirth;
            lblEmail.Text = Email;
            lblPhoneNumber.Text = PhoneNumber;

            // Kiểm tra xem có hình ảnh người dùng hay không
            if (UserImage != null)
            {
                ptbImage.Image = UserImage;
                ptbImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            Console.WriteLine("Full Name: " + FullName); // Debugging
            Console.WriteLine("Student ID: " + StudentID);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        // Hàm làm mờ Gaussian Blur
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
        private void SetDoubleBuffered(Control control)
        {
            control.GetType().InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
        }
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;
        private void fInformation_Load_1(object sender, EventArgs e)
        {
            initialFormSize = this.Size;
            initialControlBounds = new Dictionary<Control, Rectangle>();
            SaveInitialBounds(this);
        }
        private void SaveInitialBounds(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                initialControlBounds[ctrl] = ctrl.Bounds;

                // Bật Double Buffering cho các control con
                SetDoubleBuffered(ctrl);

                if (ctrl.HasChildren)
                {
                    SaveInitialBounds(ctrl);
                }
            }
        }
        private void fInformation_Resize(object sender, EventArgs e)
        {
            if (initialControlBounds == null || initialControlBounds.Count == 0)
                return;

            // Ẩn form trong lúc thay đổi kích thước để giảm độ trễ hiển thị
            this.Visible = false;

            // Tính toán tỷ lệ thay đổi kích thước một lần
            float widthRatio = (float)this.Width / initialFormSize.Width;
            float heightRatio = (float)this.Height / initialFormSize.Height;

            // Tạm ngừng cập nhật giao diện để tăng tốc
            this.SuspendLayout();
            ResizeControls(this, widthRatio, heightRatio);
            this.ResumeLayout();

            // Hiển thị lại form sau khi thay đổi kích thước xong
            this.Visible = true;
        }
        private void ResizeControls(Control parent, float widthRatio, float heightRatio)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (initialControlBounds.ContainsKey(ctrl))
                {
                    Rectangle initialBounds = initialControlBounds[ctrl];
                    ctrl.Left = (int)(initialBounds.Left * widthRatio);
                    ctrl.Top = (int)(initialBounds.Top * heightRatio);
                    ctrl.Width = (int)(initialBounds.Width * widthRatio);
                    ctrl.Height = (int)(initialBounds.Height * heightRatio);
                }

                if (ctrl.HasChildren)
                {
                    ResizeControls(ctrl, widthRatio, heightRatio);
                }
            }
        }
        private void fInformation_ResizeBegin(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void fInformation_ResizeEnd(object sender, EventArgs e)
        {
            float widthRatio = (float)this.Width / initialFormSize.Width;
            float heightRatio = (float)this.Height / initialFormSize.Height;

            this.SuspendLayout();
            ResizeControls(this, widthRatio, heightRatio);
            this.ResumeLayout();
        }

        private void ptbImage_Click(object sender, EventArgs e)
        {

        }
    }
}
