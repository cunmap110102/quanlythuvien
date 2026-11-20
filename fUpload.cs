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
using System.IO;

namespace QLTV
{
    public partial class fUpload : Form
    {
        public fUpload()
        {
            InitializeComponent();
            this.FormClosing += fInterface_FormClosing;
            this.Load += fInterface_Load;

            // Đường dẫn tới file ảnh
            Bitmap originalImage = Properties.Resources.DHBK;
            Bitmap blurredImage = ApplyGaussianBlur(originalImage);
            panel1.BackgroundImage = blurredImage;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
        }

        public Bitmap ApplyGaussianBlur(Bitmap image)
        {
            // Tạo hiệu ứng Gaussian Blur
            using (Graphics g = Graphics.FromImage(image))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Tạo Rectangle để áp dụng độ mờ
                g.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), new Rectangle(0, 0, image.Width, image.Height));
            }
            return image;
        }

        private void fInterface_Load(object sender, EventArgs e)
        {
            LoadFileList(); // Tải danh sách file khi load form
        }

        private void fInterface_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = Path.GetFileName(dialog.FileName); // Lấy tên tệp
                    textBox1.Text = fileName; // Hiển thị tên tệp trong textBox1

                    string path = Path.Combine(Application.StartupPath, "fileName");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, fileName); // Đúng cách nối đường dẫn
                    File.Copy(dialog.FileName, path);

                    // Thêm tên tệp vào DataGridView
                    dataGridView1.Rows.Add(fileName);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        private void LoadFileList()
        {
            string folderPath = Path.Combine(Application.StartupPath, "fileName");
            if (Directory.Exists(folderPath))
            {
                // Xóa các dòng hiện tại trong DataGridView
                dataGridView1.Rows.Clear();

                // Lấy danh sách tất cả các file trong thư mục và thêm vào DataGridView
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    dataGridView1.Rows.Add(fileName);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
