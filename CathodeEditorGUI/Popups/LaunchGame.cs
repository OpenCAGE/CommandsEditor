using CATHODE;
using CathodeLib;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//Imported from LaunchGame tool

namespace CommandsEditor
{
    public partial class LaunchGame : Form
    {
        string _cinematicToolDLL = "";
        string _utilPath = "";

        PatchManager.Platform _platform;
        string _gamePath;

        /* On init, if we are trying to launch to a map, skip GUI */
        public LaunchGame(string level = null, string launchDirectly = null)
        {
            if (level != null && launchDirectly != null)
            {
                LaunchToMap(level);
                this.Close();
                return;
            }

            InitializeComponent();

            //Get the game version
            switch (OpenCAGE.SettingsManager.GetString("META_GameVersion"))
            {
                case "STEAM":
                    _platform = PatchManager.Platform.STEAM;
                    break;
                case "EPIC_GAMES_STORE":
                    _platform = PatchManager.Platform.EPIC_GAMES_STORE;
                    break;
                case "GOG":
                    _platform = PatchManager.Platform.GOG;
                    break;
                default:
                    MessageBox.Show("Your game version is unsupported!", "Unsupported", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
            }
            _gamePath = SettingsManager.GetString("PATH_GameRoot");

            //Close the game down before we do anything
            List<Process> allProcesses = new List<Process>(Process.GetProcessesByName("AI"));
            for (int x = 0; x < allProcesses.Count; x++)
            {
                try
                {
                    allProcesses[x].Kill();
                    allProcesses[x].WaitForExit();
                }
                catch { }
            }

            PatchManager.PerformRecommendedPatches(_platform, _gamePath);

            _cinematicToolDLL = _gamePath + "/DATA/MODTOOLS/REMOTE_ASSETS/cinematictools/CT_AlienIsolation.dll";
            _utilPath = _gamePath + "/DATA/MODTOOLS/REMOTE_ASSETS/runtimeutils";

            enableCinematicTools.Checked = SettingsManager.GetBool("OPT_CinematicTools");
            enableCinematicTools.Enabled = _platform == PatchManager.Platform.STEAM && File.Exists(_cinematicToolDLL);

            enableRuntimeUtils.Checked = SettingsManager.GetBool("OPT_Runtime_Utils");
            enableRuntimeUtils.Enabled = _platform == PatchManager.Platform.STEAM && Directory.Exists(_utilPath);

            disableUI.Checked = SettingsManager.GetBool("OPT_HudDisabled");
            disableUI.Enabled = _platform != PatchManager.Platform.WINDOWS_STORE;

            skipFrontend.Checked = SettingsManager.GetBool("OPT_SkipFE");
            skipFrontend.Enabled = _platform != PatchManager.Platform.WINDOWS_STORE;

            enableUIPerf.Checked = SettingsManager.GetBool("OPT_cUIEnabled_UIPerf");
            enableUIPerf.Enabled = _platform != PatchManager.Platform.WINDOWS_STORE;

            enableMemReplayLogs.Checked = SettingsManager.GetBool("OPT_Mem_Replay_Logs");
            enableMemReplayLogs.Enabled = _platform != PatchManager.Platform.WINDOWS_STORE;

            patchCurrentGen.Checked = SettingsManager.GetBool("OPT_PatchCurrentGen");
            patchCurrentGen.Enabled = _platform != PatchManager.Platform.WINDOWS_STORE;

            UIMOD_DebugCheckpoints.Checked = SettingsManager.GetBool("UIOPT_PAUSEMENU");
            UIMOD_MapName.Checked = SettingsManager.GetBool("UIOPT_LOADINGSCREEN");
            UIMOD_MapSelection.Checked = SettingsManager.GetBool("UIOPT_NEWFRONTENDMENU");
            UIMOD_ReturnFrontend.Checked = SettingsManager.GetBool("UIOPT_GAMEOVERMENU");

            if (SettingsManager.GetString("OPT_LoadToMap") == "") 
                SettingsManager.SetString("OPT_LoadToMap", "Production/Frontend");

            string loadedLevel = Singleton.Editor?.CommandsDisplay?.Content?.Level?.Name;
            if (loadedLevel == null) loadedLevel = SettingsManager.GetString("OPT_LoadToMap");

            levelList.Items.AddRange(Level.GetLevels(_gamePath).ToArray());
            levelList.SelectedItem = loadedLevel;
            if (levelList.SelectedIndex == -1)
            {
                if (levelList.Items.Contains("PRODUCTION/FRONTEND")) levelList.SelectedItem = "PRODUCTION/FRONTEND";
                else levelList.SelectedIndex = 0;
            }
            if (level != null)
            {
                levelList.SelectedItem = level;
            }
            levelList.Enabled = _platform != PatchManager.Platform.WINDOWS_STORE;
        }

        /* Load game with given map name */
        private bool LaunchToMap(string MapName)
        {
            if (MapName.Length > 32)
            {
                MessageBox.Show("The name of the selected level is too long!\nPlease rename it.", "Level name too long.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            bool patchLaunch = PatchManager.PatchLaunchMode(_platform, _gamePath, MapName);
            bool patchIntegrity = PatchManager.PatchFileIntegrityCheck(_platform, _gamePath);
            bool patchMsg = PatchManager.PatchPopupMessage(_platform, _gamePath);
            if (!patchLaunch || !patchIntegrity || !patchMsg)
                MessageBox.Show("Failed to set level loading values in AI.exe!\nIs the game already open?", "Failed to patch binary.", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            PatchManager.UpdateLevelListInPackages(_platform, _gamePath);

            //Start game process 
            if (_platform == PatchManager.Platform.STEAM)
            {
                Process.Start("steam://rungameid/214490");
            }
            else
            {
                ProcessStartInfo alienProcess = new ProcessStartInfo();
                alienProcess.WorkingDirectory = _gamePath;
                alienProcess.FileName = _gamePath + "/AI.exe";
                Process.Start(alienProcess);
            }
            return true;
        }

        /* Load game from GUI map selection */
        Task cinematicToolInjectTask = null;
        private void LaunchGame_Click(object sender, EventArgs e)
        {
            //Copy/delete runtime utils as requested
            string rtUtilASI = _gamePath + "OpenCAGE_Utils.asi";
            string rtUtilDLL = _gamePath + "d3d11.dll";
            if (SettingsManager.GetBool("OPT_Runtime_Utils"))
            {
                try
                {
                    File.Copy(_utilPath + "/OpenCAGE_Utils.asi", rtUtilASI, true);
                    File.Copy(_utilPath + "/winmm.dll", rtUtilDLL, true);
                }
                catch
                {
                    if (!File.Exists(rtUtilASI) && !File.Exists(rtUtilDLL))
                        MessageBox.Show("Failed to enable hot reloading.", "Hot reload error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    if (File.Exists(rtUtilASI)) File.Delete(rtUtilASI);
                    //if (File.Exists(rtUtilDLL)) File.Delete(rtUtilDLL);
                }
                catch { }
            }

            //Work out what option was selected and launch to it
            if (!LaunchToMap(levelList.Items[levelList.SelectedIndex].ToString()))
                return;

            //Enable Cinematic Tools if requested
            if (SettingsManager.GetBool("OPT_CinematicTools"))
            {
                if (cinematicToolInjectTask != null) cinematicToolInjectTask.Dispose();
                cinematicToolInjectTask = Task.Factory.StartNew(() => InjectCinematicTools(this));
                this.Visible = false;
            }
            else
            {
                this.Close();
            }
        }
        public void OnInjectComplete(bool success)
        {
            this.Close();
        }

        /* Remember selected level */
        private void levelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsManager.SetString("OPT_LoadToMap", levelList.Items[levelList.SelectedIndex].ToString());
        }

        /* Enable/disable the Cinematic Tools */
        private void enableCinematicTools_CheckedChanged(object sender, EventArgs e)
        {
            SettingsManager.SetBool("OPT_CinematicTools", enableCinematicTools.Checked);
        }

        /* Enable/disable cUI rendering for UI perf stats (Cathode debug render) */ 
        private void enableUIPerf_CheckedChanged(object sender, EventArgs e)
        {
            SettingsManager.SetBool("OPT_cUIEnabled_UIPerf", enableUIPerf.Checked);
            if (!PatchManager.PatchUIPerfFlag(_platform, _gamePath, enableUIPerf.Checked))
                MessageBox.Show("Failed to set cUI UI perf option.\nIs Alien: Isolation open?", "Couldn't write!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /* Enable/disable Mem_Replay_Logs */
        private void enableMemReplayLogs_CheckedChanged(object sender, EventArgs e)
        {
            SettingsManager.SetBool("OPT_Mem_Replay_Logs", enableMemReplayLogs.Checked);
            if (!PatchManager.PatchMemReplayLogFlag(_platform, _gamePath, enableMemReplayLogs.Checked))
                MessageBox.Show("Failed to set memory logging option.\nIs Alien: Isolation open?", "Couldn't write!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /* Enable/disable runtime utils */
        private void enableRuntimeUtils_CheckedChanged(object sender, EventArgs e)
        {
            SettingsManager.SetBool("OPT_Runtime_Utils", enableRuntimeUtils.Checked);
        }

        /* Enable/disable in-game HUD */
        private void disableUI_CheckedChanged(object sender, EventArgs e)
        {
            SettingsManager.SetBool("OPT_HudDisabled", disableUI.Checked);
            if (!PatchManager.PatchNoUIFlag(_platform, _gamePath, disableUI.Checked))
                MessageBox.Show("Failed to set HUD disabled option.\nIs Alien: Isolation open?", "Couldn't write!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /* Skip Frontend (WARNING: Causes issues when returning to main menu - duh) */
        private void skipFrontend_CheckedChanged(object sender, EventArgs e)
        {
            SettingsManager.SetBool("OPT_SkipFE", skipFrontend.Checked);
            if (!PatchManager.PatchSkipFrontendFlag(_platform, _gamePath, skipFrontend.Checked))
                MessageBox.Show("Failed to set skip frontend option.\nIs Alien: Isolation open?", "Couldn't write!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /* Current-gen script optimiser patch */
        private void patchCurrentGen_CheckedChanged(object sender, EventArgs e)
        {
            SettingsManager.SetBool("OPT_PatchCurrentGen", patchCurrentGen.Checked);
            if (!PatchManager.DisableCurrentGenOptimisations(_platform, _gamePath, patchCurrentGen.Checked))
                MessageBox.Show("Failed to set optimisation patch option.\nIs Alien: Isolation open?", "Couldn't write!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /* UI Modifications */
        PAK2 uiPAK = null;
        private void UIMOD_DebugCheckpoints_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI("PAUSEMENU", UIMOD_DebugCheckpoints.Checked);
        }
        private void UIMOD_MapName_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI("LOADINGSCREEN", UIMOD_MapName.Checked);
        }
        private void UIMOD_MapSelection_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI("NEWFRONTENDMENU", UIMOD_MapSelection.Checked);
        }
        private void UIMOD_ReturnFrontend_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI("GAMEOVERMENU", UIMOD_ReturnFrontend.Checked);
        }
        private void UpdateUI(string file, bool modded)
        {
            if (uiPAK == null)
                uiPAK = new PAK2(_gamePath + "/DATA/UI.PAK");

            using (MemoryStream stream = new MemoryStream())
            using (BinaryReader reader = new BinaryReader(stream))
            {
                GetResourceStream((modded) ? "UI_Mods/" + file + "_MOD.GFX" : "UI_Mods/" + file + ".GFX").CopyTo(stream);
                reader.BaseStream.Position = 0;
                PAK2.File pakFile = uiPAK.Entries.FirstOrDefault(o => o.Filename == "DATA/UI/" + file + ".GFX");
                if (pakFile != null)
                    pakFile.Content = reader.ReadBytes((int)reader.BaseStream.Length);
            }

            uiPAK.Save();
            SettingsManager.SetBool("UIOPT_" + file, modded);
        }
        protected static Stream GetResourceStream(string resourcePath)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            List<string> resourceNames = new List<string>(assembly.GetManifestResourceNames());

            resourcePath = resourcePath.Replace(@"/", ".");
            resourcePath = resourceNames.FirstOrDefault(r => r.Contains(resourcePath));

            if (resourcePath == null)
                throw new FileNotFoundException("Resource not found");

            return assembly.GetManifestResourceStream(resourcePath);
        }

        /* Inject the cinematic tools */
        private void InjectCinematicTools(LaunchGame mainInst)
        {
            Process[] processes = null;
            while (processes == null || processes.Length == 0)
            {
                Thread.Sleep(2500);
                processes = Process.GetProcessesByName("AI");
            }

            try
            {
                Thread.Sleep(2500);
                Process alienProcess = processes.FirstOrDefault(o => o.MainWindowTitle.ToLower().Contains("alien") && o.MainWindowTitle.ToLower().Contains("isolation"));
                IntPtr Size = (IntPtr)_cinematicToolDLL.Length;
                IntPtr DllSpace = VirtualAllocEx(alienProcess.Handle, IntPtr.Zero, Size, AllocationType.Reserve | AllocationType.Commit, MemoryProtection.ExecuteReadWrite);
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(_cinematicToolDLL);
                bool DllWrite = WriteProcessMemory(alienProcess.Handle, DllSpace, bytes, (int)bytes.Length, out var bytesread);
                IntPtr Kernel32Handle = GetModuleHandle("Kernel32.dll");
                IntPtr LoadLibraryAAddress = GetProcAddress(Kernel32Handle, "LoadLibraryA");
                Thread.Sleep(1000);
                IntPtr RemoteThreadHandle = CreateRemoteThread(alienProcess.Handle, IntPtr.Zero, 0, LoadLibraryAAddress, DllSpace, 0, IntPtr.Zero);
                Thread.Sleep(1000);
                bool FreeDllSpace = VirtualFreeEx(alienProcess.Handle, DllSpace, 0, AllocationType.Release);
                Thread.Sleep(1000);
                CloseHandle(RemoteThreadHandle);
                CloseHandle(alienProcess.Handle);
                mainInst.OnInjectComplete(true);
            }
            catch (Exception e)
            {
                mainInst.OnInjectComplete(false);
            }
        }

        //Everything below is thanks to: https://github.com/ihack4falafel/DLL-Injection/blob/master/DllInjection/DllInjection/Program.cs

        // OpenProcess signture https://www.pinvoke.net/default.aspx/kernel32.openprocess
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
        ProcessAccessFlags processAccess,
        bool bInheritHandle,
        int processId);
        public static IntPtr OpenProcess(Process proc, ProcessAccessFlags flags)
        {
            return OpenProcess(flags, false, proc.Id);
        }

        // VirtualAllocEx signture https://www.pinvoke.net/default.aspx/kernel32.virtualallocex
        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        // VirtualFreeEx signture  https://www.pinvoke.net/default.aspx/kernel32.virtualfreeex
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
        int dwSize, AllocationType dwFreeType);

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            IntPtr dwSize,
            AllocationType flAllocationType,
            MemoryProtection flProtect);

        // WriteProcessMemory signture https://www.pinvoke.net/default.aspx/kernel32/WriteProcessMemory.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        [MarshalAs(UnmanagedType.AsAny)] object lpBuffer,
        int dwSize,
        out IntPtr lpNumberOfBytesWritten);

        // GetProcAddress signture https://www.pinvoke.net/default.aspx/kernel32.getprocaddress
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        // GetModuleHandle signture http://pinvoke.net/default.aspx/kernel32.GetModuleHandle
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        // CreateRemoteThread signture https://www.pinvoke.net/default.aspx/kernel32.createremotethread
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(
        IntPtr hProcess,
        IntPtr lpThreadAttributes,
        uint dwStackSize,
        IntPtr lpStartAddress,
        IntPtr lpParameter,
        uint dwCreationFlags,
        IntPtr lpThreadId);

        // CloseHandle signture https://www.pinvoke.net/default.aspx/kernel32.closehandle
        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);
    }
}
