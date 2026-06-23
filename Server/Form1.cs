using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Text.Json;

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
            LoadQuestionsFromFile();
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
        public class QuestionItem
        {
            public string ID { get; set; } = "";
            public string Question { get; set; } = "";
            public List<string> Options { get; set; } = new List<string>();
            public string Answer { get; set; } = "";
        }

        // 2. Hàm đọc file JSON và nạp vào bảng hiển thị
        private void LoadQuestionsFromFile()
        {
            string path = "question.json";
            if (File.Exists(path))
            {
                try
                {
                    dgvQuestions.Rows.Clear();
                    string jsonString = File.ReadAllText(path);

                    var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var questions = System.Text.Json.JsonSerializer.Deserialize<List<QuestionItem>>(jsonString, options);

                    if (questions != null)
                    {
                        foreach (var q in questions)
                        {
                            // Nạp vào bảng DataGridView của bạn: Cột ID, Cột Nội dung câu hỏi, Cột Đáp án đúng
                            dgvQuestions.Rows.Add(q.ID, q.Question, q.Answer);
                        }
                        WriteLog($"Đã load thành công {questions.Count} câu hỏi từ file JSON.");
                    }
                }
                catch (Exception ex)
                {
                    WriteLog("Lỗi đọc file JSON: " + ex.Message);
                }
            }
            else
            {
                WriteLog("Không tìm thấy file question.json ở thư mục chạy ứng dụng!");
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