using System.Text.Json.Serialization; // Bắt buộc phải thêm dòng này ở trên cùng

namespace Server
{
    public class Question // Giữ nguyên tên lớp là Question để các file khác không bị báo lỗi
    {
        public string Id { get; set; }

        // Mẹo ở đây: Nhãn này giúp map đúng chữ "Question" trong file JSON vào biến Content
        [JsonPropertyName("Question")]
        public string CauHoi { get; set; }

        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string Answer { get; set; }
    }
}