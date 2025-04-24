using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor
{
    public partial class LevelViewerSetup: Form
    {
        public LevelViewerSetup()
        {
            InitializeComponent();
        }

        public static bool Installed
        {
            get
            {
                string editorPath = GetUnityInstallPath();
                return editorPath != "" && File.Exists(editorPath);
            }
        }

        private void LevelViewerSetup_Load(object sender, EventArgs e)
        {
            if (InstallUnity())
            {
                if (MessageBox.Show("Successfully set-up the Level Viewer!\nWould you like to launch it now?", "Setup Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //launch
                }
            }
        }

        public bool InstallUnity()
        {
            if (Installed)
                return true;

            label1.Text = "Downloading Unity...";
            label1.Refresh();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                    | SecurityProtocolType.Tls11
                    | SecurityProtocolType.Tls12
                    | SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            try
            {
                string installerPath = Path.Combine(Path.GetTempPath(), "UnitySetup.exe");

                WebClient client = new WebClient();
                client.DownloadProgressChanged += (s, progress) =>
                {
                    progressBar1.Value = progress.ProgressPercentage - 20 < 0 ? 0 : progress.ProgressPercentage - 20;
                    this.Refresh();
                };
                client.DownloadFileCompleted += (s, progress) =>
                {
                    if (progress.Error != null)
                    {
                        MessageBox.Show("Encountered an error while downloading Unity!\n" + progress.Error.Message, "Unity Download Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        label1.Text = "Installing Unity...";
                        label1.Refresh();

                        progressBar1.Value = 80;
                        this.Refresh();

                        var process = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = installerPath,
                                Arguments = "/S",
                                UseShellExecute = true,
                                Verb = "runas"
                            }
                        };
                        process.Start();
                        process.WaitForExit();

                        label1.Text = "Finished!";
                        label1.Refresh();

                        progressBar1.Value = 100;
                        this.Refresh();
                    }
                };
                client.DownloadFileAsync(new Uri("https://download.unity3d.com/download_unity/ea401c316338/Windows64EditorInstaller/UnitySetup64-2022.3.9f1.exe"), installerPath);
            }
            catch
            {
                MessageBox.Show("Unity download failed!\nPlease ensure you are connected to the internet.", "Unity Download Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return Installed;
        }

        private static string GetUnityInstallPath()
        {
            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Unity Technologies\\Installer\\Unity 2022.3.9f1"))
            {
                if (regKey != null)
                {
                    var location = regKey.GetValue("Location x64") as string;
                    if (!string.IsNullOrEmpty(location))
                        return location + "\\Editor\\Unity.exe";
                }
            }

            return null;
        }
    }
}
