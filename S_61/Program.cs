using System;
using System.Windows.Forms;
using S_61.Basic;

namespace S_61
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Common.EditTime = false;
            Application.Run(new MainForm());
        }
    }
}
