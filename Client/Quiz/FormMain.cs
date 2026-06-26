using System;
using System.Windows.Forms;
using Quiz;

namespace Quiz
{
    public partial class FormMain : Form
    {
        // Biến Singleton để các UserControl khác có thể gọi FormMain thực hiện chuyển màn hình
        public static FormMain Instance { get; private set; }

        public FormMain()
        {
            InitializeComponent();
            Instance = this;
            CheckForIllegalCrossThreadCalls = false;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // Khi vừa mở app lên, nạp màn hình Đăng Nhập (ucLogin) vào khung Panel
            SwitchControl(new ucLogin());
        }

        // ================================================================
        // HÀM QUAN TRỌNG NHẤT: CHUYỂN ĐỔI GIỮA CÁC USER CONTROL
        // ================================================================
        public void SwitchControl(UserControl uc)
        {
            // Xóa màn hình cũ đang hiển thị
            pnlContent.Controls.Clear();

            // Cài đặt màn hình mới lấp đầy khung
            uc.Dock = DockStyle.Fill;

            // Thêm màn hình mới vào khung Panel
            pnlContent.Controls.Add(uc);
        }

        // Đảm bảo ngắt mạng khi tắt cửa sổ X màu đỏ
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClientManager.Disconnect();
        }
    }
}