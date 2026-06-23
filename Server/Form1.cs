using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        // Thêm ? và = null để sửa triệt để cảnh báo CS8618 (vàng)
        private TcpListener? _server = null;
        private Thread? _serverThread = null;
        private bool _serverRunning = false;
        private const int Port = 9999;
        private int clientCount = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtStatus.Text = "ĐANG DỪNG";
            txtStatus.BackColor = Color.LightCoral;
            SetupGrids();
            LoadQuestionsFromFile();
        }

        private void SetupGrids()
        {
            dgvStudents.Columns.Clear();
            dgvStudents.Columns.Add("STT", "STT");
            dgvStudents.Columns.Add("Name", "Tên");
            dgvStudents.Columns.Add("ID", "MSSV");
            dgvStudents.Columns.Add("IP", "IP");
            dgvStudents.Columns.Add("Score", "Điểm");
            dgvStudents.Columns.Add("Status", "Trạng thái");
            dgvStudents.Columns.Add("Time", "Thời gian");
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadQuestionsFromFile()
        {
            string path = "questions.txt";
            if (File.Exists(path))
            {
                dgvQuestions.Rows.Clear();
                var lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    var p = line.Split('|');
                    if (p.Length >= 4) dgvQuestions.Rows.Add(p[0], p[1], p[2], p[3]);
                }
                WriteLog("Đã load câu hỏi từ file.");
            }
        }

        // Đã thay thế hàm btnStart_Click cũ bằng hàm Toggle này
        private void btnToggle_Click(object sender, EventArgs e)
        {
            if (!_serverRunning)
            {
                // --- BẬT SERVER ---
                try
                {
                    _server = new TcpListener(System.Net.IPAddress.Any, Port);
                    _server.Start();
                    _serverRunning = true;

                    _serverThread = new Thread(ListenForClients);
                    _serverThread.IsBackground = true;
                    _serverThread.Start();

                    // Đổi giao diện sang trạng thái CHẠY
                    btnToggle.Text = "Stop Server";
                    btnToggle.BackColor = Color.Red;
                    txtStatus.Text = "ĐANG CHẠY";
                    txtStatus.BackColor = Color.LightGreen;

                    WriteLog("Server đã khởi động tại cổng " + Port);
                }
                catch (Exception ex) { WriteLog("Lỗi bật server: " + ex.Message); }
            }
            else
            {
                // --- TẮT SERVER ---
                try
                {
                    _serverRunning = false;
                    if (_server != null)
                    {
                        _server.Stop();
                        _server = null;
                    }

                    // Đổi giao diện về trạng thái DỪNG
                    btnToggle.Text = "Start Server";
                    btnToggle.BackColor = Color.LimeGreen;
                    txtStatus.Text = "ĐANG DỪNG";
                    txtStatus.BackColor = Color.LightCoral;

                    WriteLog("Server đã dừng.");
                }
                catch (Exception ex) { WriteLog("Lỗi tắt server: " + ex.Message); }
            }
        }

        private void ListenForClients()
        {
            while (_serverRunning)
            {
                try
                {
                    if (_server == null) break;
                    TcpClient client = _server.AcceptTcpClient();
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
                catch { break; }
            }
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                string ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                clientCount++;

                this.Invoke(new Action(() =>
                {
                    dgvStudents.Rows.Add(clientCount, "Client " + clientCount, "ID_" + clientCount, ip, "0", "Đang thi", DateTime.Now.ToString("HH:mm:ss"));
                }));
                WriteLog("Client kết nối: " + ip);
            }
            catch { /* Bỏ qua nếu quá trình lấy IP lỗi do client rớt mạng đột ngột */ }
        }

        private void WriteLog(string message)
        {
            if (InvokeRequired) { Invoke(new Action<string>(WriteLog), message); return; }
            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            rtbLog.ScrollToCaret();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _serverRunning = false;
            if (_server != null) _server.Stop();
            base.OnFormClosing(e);
        }

        private void btnToggle_CommandCanExecuteChanged(object sender, EventArgs e)
        {

        }
    }
}