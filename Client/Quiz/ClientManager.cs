using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks; // Thư viện để chạy luồng ngầm
using System.Windows.Forms;    // Thư viện để dùng Invoke chuyển màn hình

namespace Quiz
{
    public static class ClientManager
    {
        // Khai báo các thuộc tính hệ thống
        public static TcpClient Client { get; set; }
        public static StreamReader Reader { get; set; }
        public static StreamWriter Writer { get; set; }
        public static string StudentName { get; set; }
        public static string IPAddress { get; set; }
        public static string Port { get; set; }

        // 🔥 ĐÂY LÀ HÀM CÒN THIẾU KHIẾN BẠN BỊ BÁO LỖI:
        public static void StartListening()
        {
            Task.Run(() =>
            {
                try
                {
                    while (Client != null && Client.Connected)
                    {
                        string message = Reader.ReadLine();
                        if (message == "START_EXAM")
                        {
                            // Ép giao diện FormMain chuyển sang màn hình làm bài ucQuiz
                            FormMain.Instance.Invoke((MethodInvoker)delegate {
                                FormMain.Instance.SwitchControl(new ucQuiz());
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    // Xử lý khi mất kết nối mạng nếu cần
                }
            });
        }

        // Hàm Disconnect ngắt kết nối
        public static void Disconnect()
        {
            try
            {
                Writer?.Close();
                Reader?.Close();
                Client?.Close();
            }
            catch (Exception)
            {
                // Bỏ trống hoặc xử lý lỗi
            }
        }
    }
}