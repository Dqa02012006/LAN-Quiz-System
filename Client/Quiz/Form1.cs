using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using wr;

namespace Quiz
{
    public partial class Form1 : Form
    {
        public Form1()
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
                MessageBox.Show("Vui lòng nhập Họ và tên!");
                return;
            }

            if (!IPAddress.TryParse(ipAddress, out IPAddress parsedIP))
            {
                MessageBox.Show("IP không hợp lệ!");
                return;
            }

            if (!int.TryParse(portStr, out int port))
            {
                MessageBox.Show("Port không hợp lệ!");
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Đang kết nối...";

            bool isConnected = false;

            await Task.Run(() =>
            {
                try
                {
                    TcpClient client = new TcpClient();
                    client.Connect(parsedIP, port);

                    string msg = $"LOGIN|{fullName}";
                    byte[] data = Encoding.UTF8.GetBytes(msg);

                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);

                    isConnected = true;

                    // Giữ kết nối ngắn trước khi đóng
                    Task.Delay(500).Wait();
                    client.Close();
                }
                catch
                {
                    isConnected = false;
                }
            });

            btnLogin.Enabled = true;
            btnLogin.Text = "Đăng nhập";

            if (isConnected)
            {
                MessageBox.Show("Kết nối server thành công!");

                // TRUYỀN ĐẦY ĐỦ 3 THAM SỐ: Giữ nguyên chuỗi fullName thô để tránh mất họ tên
                wr.Form1 waitingRoom = new wr.Form1(fullName, ipAddress, portStr);
                waitingRoom.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Không kết nối được server!");
            }
        }
    }
}