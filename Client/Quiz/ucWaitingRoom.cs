using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Quiz
{
    public partial class ucWaitingRoom : UserControl
    {
        // Biến để lưu góc xoay của vòng tròn loading
        private int _spinAngle = 0;

        public ucWaitingRoom()
        {
            InitializeComponent();

            // Bật tính năng vẽ mượt mà (Double Buffer) để vòng tròn xoay không bị giật/nháy
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, pnlLoadingContainer, new object[] { true });
        }

        // 1. Sự kiện khi Form vừa load lên
        private void ucWaitingRoom_Load(object sender, EventArgs e)
        {
            // Điền dữ liệu vào 3 Label
            lblStudent.Text = ClientManager.StudentName; // Tên sinh viên
            lblIp.Text = ClientManager.IPAddress;        // IP Server
            lblPort.Text = ClientManager.Port;
            lblWelcome.Text = "Xin chào, " + ClientManager.StudentName;// Port

            // Cài đặt và bật Timer để bắt đầu xoay vòng tròn
            tmrLoading.Interval = 30; // Tốc độ xoay (để 30 mili-giây là mượt nhất)
            tmrLoading.Start();
        }

        // 2. Sự kiện Tick của Timer (Chạy liên tục mỗi 30ms)
        private void tmrLoading_Tick(object sender, EventArgs e)
        {
            // Tăng góc xoay lên 15 độ
            _spinAngle += 15;
            if (_spinAngle >= 360) _spinAngle = 0;

            // Bắt pnlLoadingContainer phải vẽ lại (sẽ gọi xuống hàm Paint bên dưới)
            pnlLoadingContainer.Invalidate();
        }

        // 3. Sự kiện Vẽ vòng tròn lên Panel
        private void pnlLoadingContainer_Paint(object sender, PaintEventArgs e)
        {
            // Bật chế độ khử răng cưa để vòng tròn tròn trịa, không bị rỗ
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Tạo bút vẽ màu Tím (giống màu nút Đăng nhập), độ dày = 6
            Pen pen = new Pen(Color.FromArgb(102, 0, 255), 6);

            // Bo tròn 2 đầu của nét vẽ
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;

            // Tạo khung hình chữ nhật để chứa vòng tròn (cách lề 5px)
            Rectangle rect = new Rectangle(5, 5, pnlLoadingContainer.Width - 10, pnlLoadingContainer.Height - 10);

            // Vẽ một đường cung (arc) từ góc hiện tại, dài 120 độ
            e.Graphics.DrawArc(pen, rect, _spinAngle, 120);

            // Dọn dẹp bút vẽ khỏi bộ nhớ
            pen.Dispose();
        }
    }
}