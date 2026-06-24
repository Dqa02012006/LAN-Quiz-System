using System;
using System.Windows.Forms;

namespace wr
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Truyền 3 tham số mặc định để sửa lỗi biên dịch
            Application.Run(new Form1("Thí sinh", "127.0.0.1", "9999"));
        }
    }
}