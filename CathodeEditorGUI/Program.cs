using System;
using System.Windows.Forms;

namespace CathodeEditorGUI
{
    public static class SharedData
    {
        public static string pathToAI = "";
    }

    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] test = Environment.GetCommandLineArgs();
            if (test.Length > 1) for (int i = 1; i < test.Length; i++) SharedData.pathToAI += test[i] + " ";
            else SharedData.pathToAI = Environment.CurrentDirectory + " ";
            SharedData.pathToAI = SharedData.pathToAI.Substring(0, SharedData.pathToAI.Length - 1);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CathodeEditorGUI());
        }
    }
}
