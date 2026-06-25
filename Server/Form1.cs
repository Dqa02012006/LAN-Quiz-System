using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

namespace Server
{ 
    // =========================================================================
    // 2. CODE LOGIC GIAO DIỆN CHÍNH SERVER
    // =========================================================================
    public partial class Form1 : Form
    {
        private TcpListener _server;
        private bool _isRunning = false;
        private List<ClientPlayer> _playersList = new List<ClientPlayer>();
        private int _studentCount = 0;
        // Hàm tự động đọc file câu hỏi và nạp vào bảng hiển thị của Server
        private void LoadQuestionsToAdminGrid()
        {
            try
            {
                string jsonPath = Path.Combine(Application.StartupPath, "questions.json");
                if (!File.Exists(jsonPath)) return;

                string json = File.ReadAllText(jsonPath).Trim();
                if (json.StartsWith("[")) json = json.Substring(1);
                if (json.EndsWith("]")) json = json.Substring(0, json.Length - 1);
                json = json.Trim();

                // ⚠️ LƯU Ý: Thay 'dataGridView2' bằng đúng tên cái bảng bạn vừa xem ở Bước 1 nhé!
                dgvQuestions.Rows.Clear();

                int pos = 0;
                while ((pos = json.IndexOf('{', pos)) != -1)
                {
                    int endPos = json.IndexOf('}', pos);
                    if (endPos == -1) break;

                    string objContent = json.Substring(pos + 1, endPos - pos - 1);

                    string idStr = GetServerJsonValue(objContent, "Id");
                    string text = GetServerJsonValue(objContent, "Text");
                    string correct = GetServerJsonValue(objContent, "CorrectAnswer");

                    if (!string.IsNullOrEmpty(text))
                    {
                        // Thêm một dòng mới vào bảng theo thứ tự cột: ID | Question | Answer
                        dgvQuestions.Rows.Add(idStr, text, correct);
                    }

                    pos = endPos + 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách câu hỏi lên giao diện Server: " + ex.Message);
            }
        }

        // Hàm bổ trợ bóc tách chuỗi JSON thủ công an toàn
        private string GetServerJsonValue(string block, string key)
        {
            string searchKey = "\"" + key + "\":";
            int index = block.IndexOf(searchKey);
            if (index == -1) return "";

            int start = index + searchKey.Length;
            string sub = block.Substring(start).Trim();

            if (sub.StartsWith("\""))
            {
                int end = sub.IndexOf("\"", 1);
                if (end == -1) return "";
                return sub.Substring(1, end - 1);
            }
            else
            {
                int end = sub.IndexOf(",");
                if (end == -1) end = sub.Length;
                return sub.Substring(0, end).Replace("}", "").Trim();
            }
        }
        public Form1()
        {
            InitializeComponent();
            LoadQuestionsToAdminGrid();
        }

        // =========================================================================
        // NÚT BẬT / TẮT SERVER (MÀU XANH LÁ)
        // =========================================================================
        private void btnToggleServer_Click(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                try
                {
                    int port = int.Parse(txtPort.Text.Trim());
                    _server = new TcpListener(System.Net.IPAddress.Any, port);
                    _server.Start();
                    _isRunning = true;

                    lblStatus.Text = "ĐANG CHẠY";
                    lblStatus.BackColor = System.Drawing.Color.LightGreen;

                    Task.Run(() => ListenForClients());
                    LogAction($"Server đã mở thành công tại Port: {port}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khởi động Server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                _isRunning = false;
                _server?.Stop();
                lock (_playersList)
                {
                    foreach (var p in _playersList) p.Socket?.Close();
                    _playersList.Clear();
                }
                lblStatus.Text = "ĐÃ DỪNG";
                lblStatus.BackColor = System.Drawing.Color.Tomato;
                LogAction("Server đã dừng hoạt động.");
            }
        }

        private void ListenForClients()
        {
            while (_isRunning)
            {
                try
                {
                    TcpClient client = _server.AcceptTcpClient();
                    Task.Run(() => HandleClientLogin(client));
                }
                catch { break; }
            }
        }

        // =========================================================================
        // XỬ LÝ ĐĂNG NHẬP CỦA THÍ SINH
        // =========================================================================
        private void HandleClientLogin(TcpClient client)
        {
            try
            {
                StreamReader reader = new StreamReader(client.GetStream(), Encoding.UTF8);
                string msg = reader.ReadLine();

                if (msg != null && msg.StartsWith("LOGIN|"))
                {
                    string name = msg.Split('|')[1];
                    string ip = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                    _studentCount++;
                    string id = "ID_" + _studentCount;

                    ClientPlayer player = new ClientPlayer(client, name, id, ip);

                    lock (_playersList)
                    {
                        _playersList.Add(player);
                    }

                    LogAction($"LOGIN SUCCESS: {name} ({ip})");

                    // Đưa thông tin thí sinh lên bảng hiển thị dgvStudents đúng cấu trúc cột
                    dgvStudents.Rows.Add(_studentCount, player.FullName, player.ClientID, player.IPAddress, player.Grade, player.Status, DateTime.Now.ToString("HH:mm:ss"));
                }
            }
            catch
            {
                client?.Close();
            }
        }

        // =========================================================================
        // NÚT PHÁT LỆNH START THI (THAY THẾ HOÀN TOÀN .ANY BẰNG FOREACH TRUYỀN THỐNG)
        // =========================================================================
        private void btnStart_Click(object sender, EventArgs e)
        {
            // Kiểm tra thủ công bằng vòng lặp tránh hoàn toàn lỗi thư viện LINQ .Any()
            bool hasWaitingStudent = false;

            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                if (row.Cells["Status"].Value != null && row.Cells["Status"].Value.ToString() == "WAITING")
                {
                    hasWaitingStudent = true;
                    break; // Tìm thấy rồi thì thoát vòng lặp luôn
                }
            }

            if (!hasWaitingStudent)
            {
                MessageBox.Show("Không có thí sinh nào ở trạng thái WAITING phòng chờ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LogAction("--- ĐANG PHÁT LỆNH KÍCH HOẠT LÀM BÀI THI ---");

            // Phát lệnh thi đồng loạt qua socket mạng
            lock (_playersList)
            {
                foreach (var player in _playersList)
                {
                    if (player.Socket != null && player.Socket.Connected)
                    {
                        player.SendMessage("START_EXAM");
                        player.Status = "EXAMING";
                    }
                }
            }

            // Đồng bộ cột Trạng thái hiển thị trên giao diện của Server thành EXAMING
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                if (row.Cells["Status"].Value != null && row.Cells["Status"].Value.ToString() == "WAITING")
                {
                    row.Cells["Status"].Value = "EXAMING";
                }
            }

            LogAction("Tất cả phòng chờ máy trạm đã được chuyển sang màn hình làm bài.");
        }

        // =========================================================================
        // CÁC HÀM PHỤ TRỢ ĐỒNG BỘ UI
        // =========================================================================
        private void LogAction(string message)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }
    }
    public class ClientPlayer
    {
        public TcpClient Socket { get; set; }
        public string FullName { get; set; }
        public string ClientID { get; set; }
        public string IPAddress { get; set; }
        public int Grade { get; set; }
        public string Status { get; set; }

        private StreamWriter _writer;

        public ClientPlayer(TcpClient socket, string fullName, string clientId, string ipAddress)
        {
            this.Socket = socket;
            this.FullName = fullName;
            this.ClientID = clientId;
            this.IPAddress = ipAddress;
            this.Grade = 0;
            this.Status = "WAITING";
            _writer = new StreamWriter(socket.GetStream(), Encoding.UTF8) { AutoFlush = true };
        }

        public void SendMessage(string message)
        {
            try
            {
                if (Socket != null && Socket.Connected)
                {
                    _writer.WriteLine(message);
                }
            }
            catch { }
        }
    }
}