using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Quiz
{
    // =======================================================================
    // VỊ TRÍ BẮT BUỘC: Lớp UserControl phải nằm ĐẦU TIÊN để khôi phục View Design
    // =======================================================================
    public partial class ucQuiz : UserControl
    {
        private List<QuestionItem> _questions = new List<QuestionItem>();
        private int _currentIndex = 0;
        private int _timeLeft = 0; // Tính bằng giây
        private List<Button> _paletteButtons = new List<Button>();

        public ucQuiz()
        {
            InitializeComponent();

            // Đăng ký sự kiện tích chọn đáp án để cập nhật trạng thái ô lưới bên phải ngay lập tức
            rdoA.CheckedChanged += Radio_CheckedChanged;
            rdoB.CheckedChanged += Radio_CheckedChanged;
            rdoC.CheckedChanged += Radio_CheckedChanged;
            rdoD.CheckedChanged += Radio_CheckedChanged;
        }

        private void ucQuiz_Load(object sender, EventArgs e)
        {
            // 1. Hiển thị thông tin Họ tên sinh viên
            try
            {
                txtStudentInfo.Text = ClientManager.StudentName;
            }
            catch
            {
                txtStudentInfo.Text = "Nguyễn Văn A (Demo)";
            }
            txtStudentInfo.ReadOnly = true;

            // 2. Chạy hàm đọc file JSON thủ công không cần thư viện
            LoadQuestionsFromJson();

            if (_questions == null || _questions.Count == 0)
            {
                MessageBox.Show("Không tìm thấy dữ liệu đề thi hoặc file questions.json trống!", "Thông báo");
                return;
            }

            // 3. Tự động tính thời gian: Mỗi câu hỏi cho 60 giây (1 phút)
            _timeLeft = _questions.Count * 60;
            UpdateTimerDisplay();

            // Cấu hình và kích hoạt Timer chạy ngầm đếm ngược
            quizTimer.Interval = 1000;
            quizTimer.Tick += QuizTimer_Tick;
            quizTimer.Start();

            // 4. Quét danh sách câu hỏi để vẽ các ô số câu hỏi vào FlowLayoutPanel (flpQuestions)
            GenerateQuestionPalette();

            // 5. Đổ dữ liệu câu hỏi đầu tiên lên màn hình
            DisplayQuestion(0);
        }

        // ==========================================
        // 🛠️ THUẬT TOÁN ĐỌC & BÓC TÁCH JSON THỦ CÔNG
        // ==========================================
        private void LoadQuestionsFromJson()
        {
            try
            {
                string jsonPath = Path.Combine(Application.StartupPath, "questions.json");
                if (!File.Exists(jsonPath))
                {
                    MessageBox.Show("Không tìm thấy file questions.json tại thư mục: " + Application.StartupPath, "Lỗi thiếu file");
                    return;
                }

                string json = File.ReadAllText(jsonPath).Trim();

                if (json.StartsWith("[")) json = json.Substring(1);
                if (json.EndsWith("]")) json = json.Substring(0, json.Length - 1);
                json = json.Trim();

                _questions = new List<QuestionItem>();
                int pos = 0;

                while ((pos = json.IndexOf('{', pos)) != -1)
                {
                    int endPos = json.IndexOf('}', pos);
                    if (endPos == -1) break;

                    string objContent = json.Substring(pos + 1, endPos - pos - 1);

                    string idStr = GetJsonValue(objContent, "Id");
                    string text = GetJsonValue(objContent, "Question");
                    string a = GetJsonValue(objContent, "A");
                    string b = GetJsonValue(objContent, "B");
                    string c = GetJsonValue(objContent, "C");
                    string d = GetJsonValue(objContent, "D");
                    string correct = GetJsonValue(objContent, "Answer");

                    if (!string.IsNullOrEmpty(text))
                    {
                        _questions.Add(new QuestionItem
                        {
                            Id = string.IsNullOrEmpty(idStr) ? 0 : int.Parse(idStr),
                            Text = text,
                            A = a,
                            B = b,
                            C = c,
                            D = d,
                            CorrectAnswer = correct,
                            UserAnswer = ""
                        });
                    }

                    pos = endPos + 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phân rã chuỗi JSON thủ công: " + ex.Message, "Lỗi xử lý dữ liệu");
            }
        }

        private string GetJsonValue(string block, string key)
        {
            // Tìm kiếm các giá trị dạng chuỗi (VD: "Question": "Nội dung câu hỏi")
            // \s* có nghĩa là bỏ qua mọi khoảng trắng thừa
            string stringPattern = $"\"{key}\"\\s*:\\s*\"(.*?)\"";
            Match matchStr = Regex.Match(block, stringPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (matchStr.Success)
            {
                return matchStr.Groups[1].Value.Trim();
            }

            // Tìm kiếm các giá trị dạng số (VD: "Id": 1)
            string numPattern = $"\"{key}\"\\s*:\\s*([0-9]+)";
            Match matchNum = Regex.Match(block, numPattern, RegexOptions.IgnoreCase);
            if (matchNum.Success)
            {
                return matchNum.Groups[1].Value.Trim();
            }

            return ""; // Trả về rỗng nếu không tìm thấy
        }

        // ==========================================
        // 🎨 ĐIỀU KHIỂN GIAO DIỆN & ĐIỀU HƯỚNG QUIZ
        // ==========================================
        private void GenerateQuestionPalette()
        {
            flpQuestions.Controls.Clear();
            _paletteButtons.Clear();

            for (int i = 0; i < _questions.Count; i++)
            {
                Button btn = new Button
                {
                    Text = (i + 1).ToString(),
                    Width = 45,
                    Height = 45,
                    Tag = i,
                    BackColor = Color.Gainsboro,
                    FlatStyle = FlatStyle.Flat
                };

                btn.Click += PaletteButton_Click;
                flpQuestions.Controls.Add(btn);
                _paletteButtons.Add(btn);
            }
        }

        private void DisplayQuestion(int index)
        {
            _currentIndex = index;
            QuestionItem q = _questions[index];

            lblQuestionText.Text = $"Câu {index + 1}: {q.Text}";
            rdoA.Text = "A. " + q.A;
            rdoB.Text = "B. " + q.B;
            rdoC.Text = "C. " + q.C;
            rdoD.Text = "D. " + q.D;

            rdoA.Checked = rdoB.Checked = rdoC.Checked = rdoD.Checked = false;

            if (q.UserAnswer == "A") rdoA.Checked = true;
            else if (q.UserAnswer == "B") rdoB.Checked = true;
            else if (q.UserAnswer == "C") rdoC.Checked = true;
            else if (q.UserAnswer == "D") rdoD.Checked = true;

            if (_currentIndex == _questions.Count - 1)
            {
                btnNext.Text = "Câu cuối";
                btnNext.Enabled = false;
            }
            else
            {
                btnNext.Text = "Next >";
                btnNext.Enabled = true;
            }
        }

        private void Radio_CheckedChanged(object sender, EventArgs e)
        {
            if (_questions == null || _currentIndex < 0 || _currentIndex >= _questions.Count)
            {
                return;
            }
            RadioButton rdo = sender as RadioButton;
            if (rdo != null && rdo.Checked)
            {
                if (rdo == rdoA) _questions[_currentIndex].UserAnswer = "A";
                else if (rdo == rdoB) _questions[_currentIndex].UserAnswer = "B";
                else if (rdo == rdoC) _questions[_currentIndex].UserAnswer = "C";
                else if (rdo == rdoD) _questions[_currentIndex].UserAnswer = "D";

                _paletteButtons[_currentIndex].BackColor = Color.ForestGreen;
                _paletteButtons[_currentIndex].ForeColor = Color.White;
            }
        }

        private void PaletteButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                int targetIndex = (int)btn.Tag;
                DisplayQuestion(targetIndex);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_currentIndex < _questions.Count - 1)
            {
                DisplayQuestion(_currentIndex + 1);
            }
        }

        private void QuizTimer_Tick(object sender, EventArgs e)
        {
            _timeLeft--;
            UpdateTimerDisplay();

            if (_timeLeft <= 0)
            {
                quizTimer.Stop();
                MessageBox.Show("Đã hết thời gian làm bài thi! Hệ thống sẽ tự động nộp bài.", "Hết giờ");
                SubmitExam();
            }
        }

        private void UpdateTimerDisplay()
        {
            TimeSpan time = TimeSpan.FromSeconds(_timeLeft);
            txtTimer.Text = time.ToString(@"mm\:ss");
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn nộp bài thi ngay bây giờ không?", "Xác nhận nộp bài", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                quizTimer.Stop();
                SubmitExam();
            }
        }

        private void SubmitExam()
        {
            int correctCount = 0;
            foreach (var q in _questions)
            {
                if (q.UserAnswer == q.CorrectAnswer)
                {
                    correctCount++;
                }
            }

            double finalGrade = Math.Round(((double)correctCount / _questions.Count) * 10, 2);

            try
            {
                if (ClientManager.Writer != null)
                {
                    ClientManager.Writer.WriteLine($"SUBMIT_SCORE|{txtStudentInfo.Text}|{finalGrade}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối mạng: Không thể gửi điểm số về Server! Chi tiết: " + ex.Message, "Lỗi gửi điểm");
            }

            MessageBox.Show($"Bạn đã hoàn thành bài thi! \nSố câu đúng: {correctCount}/{_questions.Count} \nĐiểm số đạt được: {finalGrade}", "Kết quả chấm điểm");

            if (FormMain.Instance != null)
            {
                FormMain.Instance.SwitchControl(new ucLogin());
            }
        }
    }

    // =======================================================================
    // ĐÃ HẠ XUỐNG ĐÂY: Lớp bổ trợ được đưa xuống cuối file để không phá Designer
    // =======================================================================
    public class QuestionItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string CorrectAnswer { get; set; }
        public string UserAnswer { get; set; } = "";
    }
}