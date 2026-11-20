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
    public partial class fUpdateInfo : Form
    {
        public fUpdateInfo()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            SetDoubleBuffered(this);
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
        private void SetDoubleBuffered(Control control)
        {
            control.GetType().InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblFullName_Click(object sender, EventArgs e)
        {

        }

        private void lblPhoneNumber_Click(object sender, EventArgs e)
        {

        }

        private void lblSutdentID_Click(object sender, EventArgs e)
        {

        }

        private void btnUploadImgage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"; // Filter for image files

            // Show the dialog and check if the user selects a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Load the image into the PictureBox
                ptbImage.Image = Image.FromFile(openFileDialog.FileName);
                ptbImage.SizeMode = PictureBoxSizeMode.StretchImage; // Make sure image fits the box
            }

        }

        private void ptbImage_Click(object sender, EventArgs e)
        {

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            fInformation infoForm = new fInformation();

            // Gán dữ liệu từ các TextBox cho thuộc tính của fInformation
            infoForm.FullName = txtFullName.Text;
            infoForm.StudentID = txtStudentID.Text;
            infoForm.Class = txtClass.Text;
            infoForm.Faculty = txtFaculty.Text;
            infoForm.DateOfBirth = txtDateOfBirth.Text;
            infoForm.Email = txtEmail.Text;
            infoForm.PhoneNumber = txtPhoneNumber.Text;

            // Kiểm tra nếu người dùng đã tải ảnh lên
            if (ptbImage.Image != null)
            {
                // Lưu ảnh vào một biến Bitmap
                infoForm.UserImage = new Bitmap(ptbImage.Image);
            }

            // Ẩn form hiện tại (fUpdateInfo)
            this.Hide();

            // Hiển thị form fInformation
            infoForm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;
        private void txtFullName_TextChanged(object sender, EventArgs e)
        {

        }

        private void fUpdateInfo_Load(object sender, EventArgs e)
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
        private void fUpdateInfo_Resize(object sender, EventArgs e)
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
        private void fUpdateInfo_ResizeBegin(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void fUpdateInfo_ResizeEnd(object sender, EventArgs e)
        {
            float widthRatio = (float)this.Width / initialFormSize.Width;
            float heightRatio = (float)this.Height / initialFormSize.Height;

            this.SuspendLayout();
            ResizeControls(this, widthRatio, heightRatio);
            this.ResumeLayout();
        }
    }
}