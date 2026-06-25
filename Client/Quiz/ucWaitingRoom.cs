using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz
{
    public partial class ucWaitingRoom : UserControl
    {
        private bool _isListening = true;

        public ucWaitingRoom()
        {
            InitializeComponent();
        }

        // Sự kiện chạy ngay khi màn hình Phòng chờ được nạp lên Khung chính
        private void ucWaitingRoom_Load(object sender, EventArgs e)
        {
            // 1. Hiển thị thông tin cá nhân lấy từ kho lưu trữ trung tâm
            lblWelcome.Text = $"Xin chào thí sinh: {ClientManager.StudentName}!";
            lblStatus.Text = "Kết nối thành công! Vui lòng đợi giáo viên phát lệnh khai mạc phòng thi...";

            // 2. Kích hoạt luồng ngầm liên tục đọc dữ liệu từ Server gửi xuống
            Task.Run(() => ListenFromServer());
        }

        private void ListenFromServer()
        {
            while (_isListening)
            {
                try
                {
                    if (ClientManager.Reader != null)
                    {
                        // Đọc từng dòng lệnh từ Server
                        string msg = ClientManager.Reader.ReadLine();

                        // Nếu nhận được đúng mật lệnh bắt đầu thi từ Server
                        if (msg == "START_EXAM")
                        {
                            _isListening = false; // Tắt cờ lắng nghe của màn hình này để chuyển giao

                            // Ép luồng chính (UI Thread) thực hiện đổi màn hình sang ucQuiz
                            FormMain.Instance.Invoke(new Action(() =>
                            {
                                FormMain.Instance.SwitchControl(new ucQuiz());
                            }));

                            break; // Thoát hẳn vòng lặp lắng nghe của phòng chờ
                        }
                    }
                }
                catch
                {
                    // Trường hợp Server đột ngột sập hoặc ngắt kết nối giữa chừng
                    _isListening = false;
                    MessageBox.Show("Mất kết nối với máy chủ thi!", "Lỗi mạng", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Đá thí sinh quay trở về màn hình đăng nhập ban đầu
                    FormMain.Instance.Invoke(new Action(() =>
                    {
                        FormMain.Instance.SwitchControl(new ucLogin());
                    }));

                    break;
                }
            }
        }
    }
}