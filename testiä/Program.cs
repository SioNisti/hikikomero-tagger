using System;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace testiä
{
    static class Program
    {
        public static EventHandler ShowMessage { get; private set; }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new imageMTDT());
        }
        public class PictureBoxWithInterpolationMode : PictureBox
        {
            public InterpolationMode InterpolationMode { get; set; }

            protected override void OnPaint(PaintEventArgs paintEventArgs)
            {
                paintEventArgs.Graphics.InterpolationMode = InterpolationMode;
                base.OnPaint(paintEventArgs);
            }
        }
    }
}
