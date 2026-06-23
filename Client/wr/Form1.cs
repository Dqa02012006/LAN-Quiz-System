using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace wr
{
    public partial class Form1 : Form
    {
        // Góc bắt đầu của vòng tròn xoay hoạt họa
        private float startAngle = 0;

        public Form1()
        {
            InitializeComponent();

            // Bật bộ đệm đôi tránh nhấp nháy màn hình khi vẽ lại liên tục
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        // 1. ĐÂY LÀ HÀM SỰ KIỆN LOAD CỦA FORM
        private void Form1_Load(object sender, EventArgs e)
        {
            // Ủy quyền các hàm tự vẽ đồ họa nâng cao
            pnlLoadingContainer.Paint += PnlLoadingContainer_Paint;
            pnlCard.Paint += PnlCard_Paint;
        }

        // 2. ĐÂY LÀ HÀM SỰ KIỆN TICK CỦA TIMER
        private void tmrLoading_Tick(object sender, EventArgs e)
        {
            startAngle += 6;
            if (startAngle >= 360)
            {
                startAngle = 0;
            }
            pnlLoadingContainer.Invalidate(); // Yêu cầu panel vẽ lại
        }

        // Vẽ đồ họa vòng tròn loading mượt mà không răng cưa
        private void PnlLoadingContainer_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int diameter = Math.Min(pnlLoadingContainer.Width, pnlLoadingContainer.Height) - 10;
            Rectangle rect = new Rectangle(5, 5, diameter, diameter);

            // Vẽ vòng tròn mờ phía dưới làm nền
            using (Pen trackPen = new Pen(Color.FromArgb(235, 232, 249), 6))
            {
                g.DrawEllipse(trackPen, rect);
            }

            // Vẽ vòng cung màu tím chuyển động phía trên
            using (Pen indicatorPen = new Pen(Color.FromArgb(138, 43, 226), 6))
            {
                indicatorPen.StartCap = LineCap.Round;
                indicatorPen.EndCap = LineCap.Round;
                g.DrawArc(indicatorPen, rect, startAngle, 110);
            }
        }

        // Tạo bo góc mịn cho Panel thông tin màu trắng và vẽ các đường chia hàng
        private void PnlCard_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int radius = 20; // Độ bo tròn góc
            Rectangle rect = new Rectangle(0, 0, pnlCard.Width - 1, pnlCard.Height - 1);

            using (GraphicsPath cardPath = new GraphicsPath())
            {
                cardPath.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                cardPath.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
                cardPath.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
                cardPath.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
                cardPath.CloseAllFigures();

                pnlCard.Region = new Region(cardPath);

                // Vẽ viền nhạt bao xung quanh card để tạo chiều sâu
                using (Pen borderPen = new Pen(Color.FromArgb(228, 230, 235), 1))
                {
                    g.DrawPath(borderPen, cardPath);
                }
            }

            // Vẽ 3 đường kẻ ngang màu xám nhạt để chia rõ thông tin
            using (Pen linePen = new Pen(Color.FromArgb(242, 243, 245), 1.5f))
            {
                g.DrawLine(linePen, 30, 225, pnlCard.Width - 30, 225);
                g.DrawLine(linePen, 30, 272, pnlCard.Width - 30, 272);
                g.DrawLine(linePen, 30, 318, pnlCard.Width - 30, 318);
            }
        }
    }
}