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
    // CODE LOGIC GIAO DIỆN CHÍNH SERVER - ĐÃ ĐƯỢC FIX LỖI ĐỒNG BỘ UI & JSON
    // =========================================================================
    public partial class Form1 : Form
    {
        private TcpListener _server;
        private bool _isRunning = false;
        private List<ClientPlayer> _playersList = new List<ClientPlayer>();
        private int _studentCount = 0;

        // SỬA LỖI 2: Sửa lại Key tìm kiếm JSON cho khớp với file questions.json ("Question" và "Answer")
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

                dgvQuestions.Rows.Clear();

                int pos = 0;
                while ((pos = json.IndexOf('{', pos)) != -1)
                {
                    int endPos = json.IndexOf('}', pos);
                    if (endPos == -1) break;

                    string objContent = json.Substring(pos + 1, endPos - pos - 1);

                    string idStr = GetServerJsonValue(objContent, "Id");
                    string text = GetServerJsonValue(objContent, "Question"); // FIX: Đổi từ "Text" thành "Question"
                    string correct = GetServerJsonValue(objContent, "Answer");   // FIX: Đổi từ "CorrectAnswer" thành "Answer"

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

                    // SỬA LỖI 1: Sử dụng Invoke để ép luồng giao diện chính vẽ lại bảng sinh viên, tránh phân mảnh luồng
                    this.Invoke((MethodInvoker)delegate
                    {
                        dgvStudents.Rows.Add(_studentCount, player.FullName, player.ClientID, player.IPAddress, player.Grade, player.Status, DateTime.Now.ToString("HH:mm:ss"));
                    });
                }
            }
            catch
            {
                client?.Close();
            }
        }

        // =========================================================================
        // NÚT PHÁT LỆNH START THI
        // =========================================================================
        private void btnStart_Click(object sender, EventArgs e)
        {
            // SỬA LỖI 3: Quét trực tiếp từ danh sách code dữ liệu gốc (_playersList) thay vì quét trên giao diện ô Grid để chống lỗi đặt tên cột
            bool hasWaitingStudent = false;

            lock (_playersList)
            {
                foreach (var player in _playersList)
                {
                    if (player.Status == "WAITING")
                    {
                        hasWaitingStudent = true;
                        break;
                    }
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

            // Cập nhật lại cột trạng thái (Cột thứ 6 - chỉ số index là 5) hiển thị trên màn hình thành EXAMING
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                if (row.Cells[5].Value != null && row.Cells[5].Value.ToString() == "WAITING")
                {
                    row.Cells[5].Value = "EXAMING";
                }
            }

            LogAction("Tất cả phòng chờ máy trạm đã được chuyển sang màn hình làm bài.");
        }

        // =========================================================================
        // CÁC HÀM PHỤ TRỢ ĐỒNG BỘ UI
        // =========================================================================
        private void LogAction(string message)
        {
            // Sử dụng Invoke để hiển thị nhật ký Log an toàn từ mọi luồng mạng
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke((MethodInvoker)delegate { LogAction(message); });
            }
            else
            {
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
            }
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