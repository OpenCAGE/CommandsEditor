using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Set paths
            if (args.Length > 0 && args[0] == "-opencage") for (int i = 1; i < args.Length; i++) SharedData.pathToAI += args[i] + " ";
            else SharedData.pathToAI = Environment.CurrentDirectory + " ";
            SharedData.pathToAI = SharedData.pathToAI.Substring(0, SharedData.pathToAI.Length - 1);

            //Verify location
            if (!File.Exists(SharedData.pathToAI + "/AI.exe")) throw new Exception("This tool was launched incorrectly, or was not placed within the Alien: Isolation directory.");

            //Run app
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CathodeEditorGUI());
        }
    }
}
