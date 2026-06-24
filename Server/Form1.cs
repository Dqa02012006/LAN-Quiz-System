using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        private TcpListener _server;
        private Thread _serverThread;
        private bool _running = false;

        private const int PORT = 9999;
        private int clientCount = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnToggle_Click(object sender, EventArgs e)
        {
            if (!_running)
            {
                try
                {
                    // Dùng System.Net.IPAddress để tránh xung đột với tên cột DataGridView
                    _server = new TcpListener(System.Net.IPAddress.Any, PORT);
                    _server.Start();
                    _running = true;

                    _serverThread = new Thread(Listen);
                    _serverThread.IsBackground = true;
                    _serverThread.Start();

                    txtStatus.Text = "ĐANG CHẠY";
                    txtStatus.BackColor = Color.LightGreen;

                    WriteLog("Server started on port " + PORT + "...");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể khởi động server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                StopServer();
                txtStatus.Text = "STOPPED";
                txtStatus.BackColor = Color.LightCoral;
                WriteLog("Server stopped...");
            }
        }

        // =========================================================================
        // HÀM SỬA LỖI: Thêm hàm này để xử lý lỗi "does not exist in the current context"
        // =========================================================================
        private void btnToggle_CommandCanExecuteChanged(object sender, EventArgs e)
        {
            // Để trống, không làm gì cả
        }

        private void Listen()
        {
            while (_running)
            {
                try
                {
                    if (_server == null) break;

                    TcpClient client = _server.AcceptTcpClient();
                    Thread t = new Thread(() => HandleClient(client));
                    t.IsBackground = true;
                    t.Start();
                }
                catch
                {
                    if (!_running) break;
                }
            }
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                string ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];

                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) return;

                string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                string name = "Unknown";

                if (msg.StartsWith("LOGIN|"))
                {
                    string[] parts = msg.Split('|');
                    if (parts.Length > 1)
                    {
                        name = parts[1];
                    }
                }

                Interlocked.Increment(ref clientCount);

                Invoke(new Action(() =>
                {
                    dgvStudents.Rows.Add(
                        clientCount,
                        name,
                        "ID_" + clientCount,
                        ip,
                        "0",
                        "WAITING",
                        DateTime.Now.ToString("HH:mm:ss")
                    );
                }));

                WriteLog($"LOGIN: {name} ({ip})");
            }
            catch (Exception ex)
            {
                WriteLog("Error handling client: " + ex.Message);
            }
            finally
            {
                client.Close();
            }
        }

        private void WriteLog(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(WriteLog), msg);
                return;
            }

            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\n");
            rtbLog.ScrollToCaret();
        }

        private void StopServer()
        {
            _running = false;
            _server?.Stop();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopServer();
            base.OnFormClosing(e);
        }
    }
}