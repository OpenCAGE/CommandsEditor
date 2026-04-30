using Assimp;
using CathodeLib;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor
{
    static class Program
    {
        static Dictionary<string, string> _args;
        static Stopwatch _timer = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            _args = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            {
                var arguments = Environment.GetCommandLineArgs();
                for (int i = 0; i < arguments.Length; i++)
                {
                    var match = Regex.Match(arguments[i], "-([^=]+)=(.*)");
                    if (!match.Success) continue;
                    var vName = match.Groups[1].Value;
                    var vValue = match.Groups[2].Value;
                    _args[vName] = vValue;

                    if (_args[vName].Substring(_args[vName].Length - 1) == "\"")
                        _args[vName] = _args[vName].Substring(0, _args[vName].Length - 1);
                }
            }

            //Set path to AI
            if (GetArgument("pathToAI") != null)
                Singleton.PathToAI = GetArgument("pathToAI");
            else
                Singleton.PathToAI = Environment.CurrentDirectory;

#if !DEBUG
            //Verify location
            //if (!File.Exists(Singleton.PathToAI + "/AI.exe")) 
            //    throw new Exception("This tool was launched incorrectly, or was not placed within the Alien: Isolation directory.");
#endif

            Singleton.Platform = PatchManager.GetPlatform(Singleton.PathToAI);

            //Make sure we're using the UK culture to format our numbers correctly
            CultureInfo newCulture = CultureInfo.CreateSpecificCulture("en-GB");
            Thread.CurrentThread.CurrentUICulture = newCulture;
            Thread.CurrentThread.CurrentCulture = newCulture;

#if !DEBUG
            //Advanced error handlers for silent exceptions
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            _timer = Stopwatch.StartNew();
#endif

            //DLL needs to be copied out for Assimp to work
            string dllPath = "runtimes\\win-x64\\native\\assimp.dll";
            if (!File.Exists(dllPath))
            {
                using (MemoryStream stream = new MemoryStream())
                using (GZipStream compressedStream = new GZipStream(new MemoryStream(Properties.Resources.assimp), CompressionMode.Decompress))
                {
                    compressedStream.CopyTo(stream);
                    Directory.CreateDirectory(Path.GetDirectoryName(dllPath));
                    File.WriteAllBytes(dllPath, stream.ToArray());
                }
            }

#if DEBUG
            //Assimp logging
            LogStream logstream = new LogStream(delegate (String msg, String userData) {
                Console.WriteLine(msg);
            });
            logstream.Attach();
#endif

            //Run app
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CommandsEditor(GetArgument("level")));
        }

        public static string GetArgument(string name)
        {
            if (_args.TryGetValue(name, out string arg))
                return arg;
            return null;
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            HandleError("Application_ThreadException\n" + e.Exception.ToString());
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleError("CurrentDomain_UnhandledException\n" + ((Exception)e.ExceptionObject).ToString());
        }
        static void HandleError(string error)
        {
            string logPath = Singleton.PathToAI + "/DATA/MODTOOLS/LOGS/CECrash_" + DateTime.Now.ToString("ddMMyy-HHmmss") + ".log";
            Directory.CreateDirectory(Singleton.PathToAI + "/DATA/MODTOOLS/LOGS");

            MessageBox.Show("A critical error occurred.\nPlease wait while a log is generated.", "OpenCAGE Error Handler", MessageBoxButtons.OK, MessageBoxIcon.Error);

            try
            {
                Task.Run(async () =>
                {
                    await UploadCrashLog(error, logPath);
                }).Wait();

                MessageBox.Show("Thanks, a log has been generated and auto-submitted.\nYou can find your logs within DATA/MODTOOLS/LOGS.", "OpenCAGE Error Handler", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("A log has been generated.\nYou can find it within DATA/MODTOOLS/LOGS, please submit it to GitHub!", "OpenCAGE Error Handler", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            Application.Exit();
        }
        static async Task UploadCrashLog(string error, string logPath)
        {
            try
            {
                var client = new HttpClient();
                var content = new MultipartFormDataContent();

                content.Add(new StringContent(error), "error_log");

                error += "\n **** ";

                string version = SettingsManager.GetString("OpenCAGE_Version");
                if (version == "")
                    version = "Standalone: " + Application.ProductVersion;
                string platform = SettingsManager.GetString("META_GameVersion");
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string uptime = _timer == null ? "" : _timer.Elapsed.ToString(@"dd\.hh\:mm\:ss");
                content.Add(new StringContent(version), "application_version");
                error += "\n Application Version: " + version;
                content.Add(new StringContent(platform), "game_version");
                error += "\n Game Version: " + platform;
                content.Add(new StringContent(time), "datetime");
                error += "\n Crash Time: " + time;
                content.Add(new StringContent(uptime), "uptime");
                error += "\n Uptime: " + uptime;

                error += "\n **** ";

                string level = Singleton.Editor?.CommandsDisplay?.Content?.Level?.Name;
                CATHODE.Scripting.Composite composite = Singleton.Editor?.CommandsDisplay?.CompositeDisplay?.Composite;
                CATHODE.Scripting.Internal.Entity entity = Singleton.Editor?.CommandsDisplay?.CompositeDisplay?.EntityDisplay?.Entity;
                content.Add(new StringContent(level == null ? "Unknown/None" : level), "current_level");
                error += "\n Current Level: " + level;
                content.Add(new StringContent(composite == null ? "Unknown/None" : composite.name), "current_composite");
                error += "\n Current Composite: " + composite.name;
                content.Add(new StringContent(entity == null ? "Unknown/None" : entity.shortGUID.ToByteString()), "current_entity");
                error += "\n Current Entity: " + entity.shortGUID.ToByteString();

                error += "\n **** ";

                string os = SystemInfo.GetOsName();
                string cpu = SystemInfo.GetCpuName();
                string ram = SystemInfo.GetTotalPhysicalMemory();
                content.Add(new StringContent(os), "os_name");
                error += "\n OS: " + os;
                content.Add(new StringContent(cpu), "cpu_name");
                error += "\n CPU: " + cpu;
                content.Add(new StringContent(ram), "ram_total");
                error += "\n RAM: " + ram;

                var response = await client.PostAsync("http://opencage.mattfiler.co.uk/crashes/crash_handler.php", content);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Failed to upload crash log [" + response.StatusCode + "]: " + response.RequestMessage);
                }
                else
                {
                    Console.WriteLine("Uploaded crash log successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to create crash log to send: " + ex.Message);
            }

            File.WriteAllText(logPath, error);
        }
    }
}
