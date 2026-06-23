using System;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Gán sự kiện Click cho nút Đăng nhập khi Form khởi tạo
            btnLogin.Click += BtnLogin_Click;
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            // 1. Lấy dữ liệu từ giao diện và xóa khoảng trắng thừa
            string fullName = txtFullName.Text.Trim();
            string ipAddress = txtIP.Text.Trim();
            string portStr = txtPort.Text.Trim();

            // 2. Kiểm tra dữ liệu đầu vào (Validation)
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Vui lòng nhập Họ và tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (!IPAddress.TryParse(ipAddress, out IPAddress parsedIP))
            {
                MessageBox.Show("Địa chỉ IP Server không hợp lệ! (Ví dụ đúng: 192.168.1.100)", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIP.Focus();
                return;
            }

            if (!int.TryParse(portStr, out int port) || port < 1 || port > 65535)
            {
                MessageBox.Show("Cổng Port phải là số nguyên từ 1 đến 65535!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPort.Focus();
                return;
            }

            // 3. Trạng thái đang kết nối (Vô hiệu hóa nút để tránh bấm liên tục)
            btnLogin.Enabled = false;
            btnLogin.Text = "Đang kết nối...";
            btnLogin.BackColor = Color.Gray;

            // 4. Mô phỏng tiến trình kết nối bất đồng bộ tới Server (Không gây đơ UI)
            bool isConnected = await Task.Run(() =>
            {
                try
                {
                    // Giả lập thời gian phản hồi từ Socket mạng (2 giây)
                    Task.Delay(2000).Wait();

                    // THỰC TẾ: Bạn sẽ viết code Socket hoặc HttpClient ở đây
                    // ví dụ: TcpClient client = new TcpClient();
                    // client.Connect(parsedIP, port);

                    return true; // Trả về true nếu kết nối thành công
                }
                catch
                {
                    return false; // Trả về false nếu lỗi (Timeout, sai IP/Port...)
                }
            });

            // 5. Khôi phục lại trạng thái ban đầu của nút Đăng nhập
            btnLogin.Enabled = true;
            btnLogin.Text = "Đăng nhập  →";
            btnLogin.BackColor = Color.FromArgb(110, 0, 255);

            // 6. Xử lý kết quả trả về
            if (isConnected)
            {
                MessageBox.Show($"Kết nối thành công!\n\nThí sinh: {fullName}\nServer: {ipAddress}:{port}",
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mở Form làm bài thi tại đây và ẩn Form đăng nhập này đi:
                // FormExam frmExam = new FormExam(fullName);
                // frmExam.Show();
                // this.Hide();
            }
            else
            {
                MessageBox.Show("Không thể kết nối tới Server phòng thi. Vui lòng kiểm tra lại đường truyền hoặc địa chỉ IP/Port!",
                                "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}