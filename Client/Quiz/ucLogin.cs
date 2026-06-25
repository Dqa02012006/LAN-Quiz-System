using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz
{
    public partial class ucLogin : UserControl
    {
        public ucLogin()
        {
            InitializeComponent();
            btnLogin.Click += BtnLogin_Click;
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string ipAddress = txtIP.Text.Trim();
            string portStr = txtPort.Text.Trim();

            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Vui lòng nhập Họ và tên!", "Thông báo");
                return;
            }

            if (!IPAddress.TryParse(ipAddress, out IPAddress parsedIP))
            {
                MessageBox.Show("IP không hợp lệ!", "Thông báo");
                return;
            }

            if (!int.TryParse(portStr, out int port))
            {
                MessageBox.Show("Port không hợp lệ!", "Thông báo");
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Đang kết nối...";

            bool isConnected = false;
            TcpClient tempClient = null;
            StreamReader tempReader = null;
            StreamWriter tempWriter = null;

            await Task.Run(() =>
            {
                try
                {
                    tempClient = new TcpClient();
                    tempClient.Connect(parsedIP, port);

                    NetworkStream stream = tempClient.GetStream();
                    tempWriter = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                    tempReader = new StreamReader(stream, Encoding.UTF8);

                    tempWriter.WriteLine($"LOGIN|{fullName}");
                    isConnected = true;
                }
                catch
                {
                    isConnected = false;
                    tempWriter?.Close();
                    tempReader?.Close();
                    tempClient?.Close();
                }
            });

            btnLogin.Enabled = true;
            btnLogin.Text = "Đăng nhập";

            if (isConnected && tempClient != null)
            {
                MessageBox.Show("Kết nối server thành công!", "Thông báo");

                // Lưu dữ liệu vào kho trung tâm
                ClientManager.Client = tempClient;
                ClientManager.Reader = tempReader;
                ClientManager.Writer = tempWriter;
                ClientManager.StudentName = fullName;

                // Lệnh cho FormMain đổi sang màn hình chờ (ucWaiting)
                FormMain.Instance.SwitchControl(new ucWaitingRoom());
            }
            else
            {
                MessageBox.Show("Không kết nối được server!", "Lỗi");
            }
        }
    }
}