using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace wr
{
    public partial class Form1 : Form
    {
        private float startAngle = 0;

        // Các biến lưu trữ thông tin đăng nhập phục vụ vẽ đồ họa
        private string studentName = "Thí sinh";
        private string serverIP = "127.0.0.1";
        private string serverPort = "9999";

        // Hàm khởi tạo nhận chuẩn 3 tham số thô từ Form Login truyền sang
        public Form1(string loginName, string ip, string port)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(loginName)) this.studentName = loginName;
            if (!string.IsNullOrEmpty(ip)) this.serverIP = ip;
            if (!string.IsNullOrEmpty(port)) this.serverPort = port;

            // Bật Double Buffered để vòng xoay loading không bị nhấp nháy
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pnlLoadingContainer.Paint += PnlLoadingContainer_Paint;
            pnlCard.Paint += PnlCard_Paint;

            lblStudentVal.Text = studentName;
            lblIpVal.Text = serverIP;
            lblPortVal.Text = serverPort;
            // Đổi chữ "Xin chào..." ở tiêu đề tĩnh phía trên (nếu có dùng Label)
            if (this.Controls.Find("lblWelcome", true).Length > 0)
            {
                this.Controls.Find("lblWelcome", true)[0].Text = $"Xin chào, {this.studentName}";
            }
        }

        private void tmrLoading_Tick(object sender, EventArgs e)
        {
            startAngle += 6;
            if (startAngle >= 360)
            {
                startAngle = 0;
            }
            pnlLoadingContainer.Invalidate();
        }

        private void PnlLoadingContainer_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int diameter = Math.Min(pnlLoadingContainer.Width, pnlLoadingContainer.Height) - 10;
            Rectangle rect = new Rectangle(5, 5, diameter, diameter);

            using (Pen trackPen = new Pen(Color.FromArgb(235, 232, 249), 6))
            {
                g.DrawEllipse(trackPen, rect);
            }

            using (Pen indicatorPen = new Pen(Color.FromArgb(138, 43, 226), 6))
            {
                indicatorPen.StartCap = LineCap.Round;
                indicatorPen.EndCap = LineCap.Round;
                g.DrawArc(indicatorPen, rect, startAngle, 110);
            }
        }

        // Vẽ cấu trúc bo góc khung Card và vẽ đè nội dung thông tin đăng nhập lên
        private void PnlCard_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int radius = 20;
            Rectangle rect = new Rectangle(0, 0, pnlCard.Width - 1, pnlCard.Height - 1);

            using (GraphicsPath cardPath = new GraphicsPath())
            {
                cardPath.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                cardPath.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
                cardPath.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
                cardPath.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
                cardPath.CloseAllFigures();



                using (Pen borderPen = new Pen(Color.FromArgb(228, 230, 235), 1))
                {
                    g.DrawPath(borderPen, cardPath);
                }
            }

            // Vẽ 3 đường kẻ ngang màu xám nhạt làm nền chia thông tin
            using (Pen linePen = new Pen(Color.FromArgb(242, 243, 245), 1.5f))
            {
                g.DrawLine(linePen, 30, 220, pnlCard.Width - 30, 220);
                g.DrawLine(linePen, 30, 268, pnlCard.Width - 30, 268);
                g.DrawLine(linePen, 30, 316, pnlCard.Width - 30, 316);
            }

            // VẼ CHỮ ĐỘNG: Định vị tọa độ X xuất phát từ 135 để thẳng hàng tuyệt đối với chữ tĩnh của bạn
            // VẼ CHỮ ĐỘNG: Cấu hình chuẩn để hiển thị trọn vẹn họ tên tiếng Việt dài
        }

        private void pnlCard_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}