using System.IO;
using System.Net.Sockets;

namespace Quiz
{
    public static class ClientManager
    {
        // Biến lưu trữ kết nối mạng
        public static TcpClient Client { get; set; }
        public static StreamReader Reader { get; set; }
        public static StreamWriter Writer { get; set; }

        // Biến lưu trữ thông tin thí sinh
        public static string StudentName { get; set; }
        public static string ServerIP { get; set; }
        public static int ServerPort { get; set; }

        // Hàm hỗ trợ dọn dẹp kết nối
        public static void Disconnect()
        {
            Reader?.Close();
            Writer?.Close();
            Client?.Close();
        }
    }
}