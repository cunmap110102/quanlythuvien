using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTV
{
    public partial class fAdmin : Form
    {
        public fAdmin()
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

            using (Graphics g = Graphics.FromImage(image))
            {

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;


                g.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), new Rectangle(0, 0, image.Width, image.Height));
            }

            return image;
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void SetDoubleBuffered(Control control)
        {
            control.GetType().InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedType = comboBox1.SelectedItem.ToString();
        }
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;

        private void fAdmin_Load(object sender, EventArgs e)
        {
            initialFormSize = this.Size;
            initialControlBounds = new Dictionary<Control, Rectangle>();
            SaveInitialBounds(this);
        }

        // Hàm đệ quy để lưu vị trí và kích thước ban đầu của tất cả các điều khiển
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

        private void fAdmin_Resize(object sender, EventArgs e)
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
        
        // Hàm đệ quy để thay đổi kích thước và vị trí của tất cả các điều khiển
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

        private void fAdmin_ResizeBegin(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void fAdmin_ResizeEnd(object sender, EventArgs e)
        {
            float widthRatio = (float)this.Width / initialFormSize.Width;
            float heightRatio = (float)this.Height / initialFormSize.Height;

            this.SuspendLayout();
            ResizeControls(this, widthRatio, heightRatio);
            this.ResumeLayout();
        }
    }
}

