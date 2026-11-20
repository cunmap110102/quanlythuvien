using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;





namespace QLTV
{
    public partial class fSearch : Form
    {
        public fSearch()
        {
            InitializeComponent();
            InitializeDataGridViewColumns();
            this.FormClosing += fInterface_FormClosing;
            this.Load += fInterface_Load;
            dataGridView1.CellClick += dataGridView1_CellClick;
            Bitmap originalImage = Properties.Resources.DHBK;


            Bitmap blurredImage = ApplyGaussianBlur(originalImage);


            panel1.BackgroundImage = blurredImage;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void InitializeDataGridViewColumns()
        {
            dataGridView1.Columns.Clear(); // Clear old columns if any
            dataGridView1.Columns.Add("FileName", "File Name"); // File name column
            dataGridView1.Columns.Add("FileType", "File Type"); // File type column
            dataGridView1.Columns.Add("Uploader", "Uploader"); // Uploader name column
            dataGridView1.Columns.Add("UploadTime", "Upload Time"); // Upload time column
            dataGridView1.Columns.Add("PDFPath", "PDF Path"); // Column to store PDF path

            
           
        }
        private void SetDoubleBuffered(Control control)
        {
            control.GetType().InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });
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
        private void ConvertWordToPDF(string inputFilePath, string outputFilePath)
        {
            Word.Application wordApp = new Word.Application();
            Word.Document wordDoc = null;
            try
            {
                if (!File.Exists(inputFilePath))
                {
                    MessageBox.Show("File không tồn tại: " + inputFilePath);
                    return;
                }

                object readOnly = true;
                object isVisible = false;
                object missing = Type.Missing;
                wordDoc = wordApp.Documents.Open(inputFilePath, ref missing, ref readOnly, ref missing, ref missing, ref missing,
                                                 ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible,
                                                 ref missing, ref missing, ref missing, ref missing);

                if (Path.GetExtension(inputFilePath).Equals(".doc", StringComparison.OrdinalIgnoreCase))
                {
                    wordDoc.Convert();
                }

                wordDoc.ExportAsFixedFormat(outputFilePath, Word.WdExportFormat.wdExportFormatPDF);

                // Check if the PDF file was actually created
                if (File.Exists(outputFilePath))
                {
                    MessageBox.Show("Chuyển đổi thành công! File PDF đã được lưu tại: " + outputFilePath);
                }
                else
                {
                    MessageBox.Show("Chuyển đổi thất bại. Không tìm thấy file PDF tại: " + outputFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during conversion: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                wordDoc?.Close();
                wordApp.Quit();
            }
        }



        private void fInterface_Load(object sender, EventArgs e)
        {
            LoadFileList();
            // Tải danh sách file khi load form
        }

        private void fInterface_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveDataGridViewToFile();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void UploadFile()
{
    OpenFileDialog openFileDialog = new OpenFileDialog();
    openFileDialog.Filter = "Word Documents|*.docx;*.doc";

    if (openFileDialog.ShowDialog() == DialogResult.OK)
    {
        string fileName = Path.GetFileName(openFileDialog.FileName);
        string fileType = Path.GetExtension(openFileDialog.FileName).Replace(".", "").ToUpper();
        string uploader = "User"; // Giả sử "User" là người tải lên
        string uploadTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Cập nhật hoặc thêm dòng trong DataGridView
        if (dataGridView1.Rows.Count == 0)
        {
            dataGridView1.Rows.Add(fileName, fileType, uploader, uploadTime, ""); // Thêm hàng mới nếu chưa có
        }
        else
        {
            dataGridView1.Rows[0].Cells["FileName"].Value = fileName;
            dataGridView1.Rows[0].Cells["FileType"].Value = fileType;
            dataGridView1.Rows[0].Cells["Uploader"].Value = uploader;
            dataGridView1.Rows[0].Cells["UploadTime"].Value = uploadTime;
            dataGridView1.Rows[0].Cells["PDFPath"].Value = ""; // Reset PDF path cho lần upload mới
        }
    }
}
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = Path.GetFileName(dialog.FileName); // File name
                    string fileType = Path.GetExtension(dialog.FileName); // File type
                    string uploader = "User"; // Placeholder for uploader; replace with actual user if needed
                    string uploadTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Current time

                    // Save file to specified path
                    string path = Path.Combine(Application.StartupPath, "fileName");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, fileName); // Correctly combine paths
                    File.Copy(dialog.FileName, path, true);

                    // Add file details directly to DataGridView without reloading entire list
                    dataGridView1.Rows.Add(fileName, fileType, uploader, uploadTime);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void LoadFileList()
        {
            string folderPath = Path.Combine(Application.StartupPath, "fileName");
            if (Directory.Exists(folderPath))
            {
                dataGridView1.Rows.Clear();
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string fileType = Path.GetExtension(file);
                    string uploader = "User"; // Placeholder for uploader
                    string uploadTime = File.GetCreationTime(file).ToString("yyyy-MM-dd HH:mm:ss");
                    dataGridView1.Rows.Add(fileName, fileType, uploader, uploadTime);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string fileName = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string filePath = Path.Combine(Application.StartupPath, "fileName", fileName);

                if (File.Exists(filePath))
                {
                    try
                    {
                        File.Delete(filePath);  // Delete file from folder
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);  // Remove row from DataGridView
                        MessageBox.Show("File deleted successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to delete file: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("File does not exist.");
                }
            }
            else
            {
                MessageBox.Show("Please select a file to delete.");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            SearchFiles(textBox2.Text);
        }

        private void SearchFiles(string searchTerm)
        {
            String folderPath = Path.Combine(Application.StartupPath, "fileName");
            if (Directory.Exists(folderPath))
            {
                dataGridView1.Rows.Clear();
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    if (fileName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        string fileType = Path.GetExtension(file);
                        string uploader = "User";
                        string uploadTime = File.GetCreationTime(file).ToString("yyyy-MM-dd HH:mm:ss");
                        dataGridView1.Rows.Add(fileName, fileType, uploader, uploadTime);
                    }
                }
            }
        }
        private Size initialFormSize;
        private Dictionary<Control, Rectangle> initialControlBounds;
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string fileName = selectedRow.Cells["FileName"].Value?.ToString();
                string fileType = selectedRow.Cells["FileType"].Value?.ToString();

                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(fileType))
                {
                    string filePath = Path.Combine(Application.StartupPath, "fileName", fileName);
                    string pdfOutputPath = Path.Combine(Application.StartupPath, "fileName", Path.GetFileNameWithoutExtension(fileName) + ".pdf");

                    if (File.Exists(filePath))
                    {
                        try
                        {
                            ConvertFileToPDF(filePath, pdfOutputPath, fileType, selectedRow);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("The specified file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a valid file for conversion.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("No file selected. Please select a file first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Hàm mẫu chuyển đổi từ Word sang PDF và trả về đường dẫn file PDF
        private string ConvertWordToPdf(string wordFilePath)
        {
            string pdfFilePath = Path.ChangeExtension(wordFilePath, ".pdf");
            MessageBox.Show("PDF File Path: " + pdfFilePath); // Hiển thị đường dẫn file PDF để kiểm tra
                                                              // Thực hiện chuyển đổi ở đây
            return pdfFilePath;
        }
       
        private void ConvertFileToPDF(string filePath, string pdfOutputPath, string fileType, DataGridViewRow row)
        {
            if (fileType.Equals(".doc", StringComparison.OrdinalIgnoreCase) || fileType.Equals(".docx", StringComparison.OrdinalIgnoreCase))
            {
                ConvertWordToPDF(filePath, pdfOutputPath);

                // Cập nhật các thông tin vào hàng đã chọn sau khi chuyển đổi
                row.Cells["FileName"].Value = Path.GetFileName(pdfOutputPath); // Cập nhật tên file mới là PDF
                row.Cells["FileType"].Value = "PDF"; // Cập nhật loại file là PDF
                row.Cells["Uploader"].Value = "User"; // Cập nhật lại thông tin người tải lên nếu cần
                row.Cells["UploadTime"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Cập nhật lại thời gian
                row.Cells["PDFPath"].Value = pdfOutputPath; // Đường dẫn PDF sau khi chuyển đổi

                MessageBox.Show("File đã được chuyển đổi thành công sang PDF và thông tin đã được cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                

                // Lưu dữ liệu sau khi cập nhật và xóa mục gốc
                SaveDataGridViewToFile();
            }
            else
            {
                MessageBox.Show("Chỉ có thể chuyển đổi file Word (.doc, .docx) sang PDF.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void fSearch_Load(object sender, EventArgs e)
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
        private void fSearch_Resize(object sender, EventArgs e)
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
        private void fSearch_ResizeBegin(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void fSearch_ResizeEnd(object sender, EventArgs e)
        {
            float widthRatio = (float)this.Width / initialFormSize.Width;
            float heightRatio = (float)this.Height / initialFormSize.Height;

            this.SuspendLayout();
            ResizeControls(this, widthRatio, heightRatio);
            this.ResumeLayout();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "ConvertToPDF")
            {
                // Get the file name and type from the selected row
                string fileName = dataGridView1.Rows[e.RowIndex].Cells["FileName"].Value?.ToString();
                string fileType = dataGridView1.Rows[e.RowIndex].Cells["FileType"].Value?.ToString();

                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(fileType))
                {
                    // Get the full path of the file to be converted
                    string filePath = Path.Combine(Application.StartupPath, "fileName", fileName);

                    // Check if the file exists
                    if (File.Exists(filePath))
                    {
                        // Set the PDF output path
                        string pdfOutputPath = Path.Combine(Application.StartupPath, "fileName", Path.GetFileNameWithoutExtension(fileName) + ".pdf");

                        try
                        {
                            // Convert the file if it's a Word document
                            if (fileType.Equals(".doc", StringComparison.OrdinalIgnoreCase) || fileType.Equals(".docx", StringComparison.OrdinalIgnoreCase))
                            {
                                ConvertWordToPDF(filePath, pdfOutputPath);
                                dataGridView1.Rows[e.RowIndex].Cells["PDFPath"].Value = pdfOutputPath;

                                MessageBox.Show("File has been successfully converted to PDF!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Only Word files (.doc, .docx) can be converted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("The specified file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Unable to identify the file for conversion.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void SaveDataGridViewToFile()
        {
            string savePath = Path.Combine(Application.StartupPath, "fileData.txt");

            using (StreamWriter writer = new StreamWriter(savePath))
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string rowData = string.Join(",", row.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value?.ToString() ?? ""));
                        writer.WriteLine(rowData);
                    }
                }
            }

            MessageBox.Show("Data saved successfully!", "Save Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }
    }
}
