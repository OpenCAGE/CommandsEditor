using CommandsEditor.Popups.Base;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor.ConfigEditors
{
    public partial class RadiosityEditor : BaseWindow
    {
        public RadiosityEditor() : base()
        {
            InitializeComponent();
        }

        private void RadiosityEditor_Load(object sender, EventArgs e)
        {
            //note - HAIR_SHADING_SETTINGS.TXT and SKIN_SHADING_SETTINGS.TXT are also used

            string[] lightingData = File.ReadAllLines(Singleton.PathToAI + @"\DATA\RADIOSITY_SETTINGS.TXT");
            for (int i = 0; i < lightingData.Length; i++) if (lightingData[i] != "") lightingData[i] = lightingData[i].Split('=')[1];

            gRadiosityEmissiveSurfaceScale.Text = lightingData[1];
            gRadiosityFirstBounceScale.Text = lightingData[2];
            gRadiosityMultiBounceScale.Text = lightingData[3];
            gRadiosityAlbedoOverbrightAmount.Text = lightingData[4];
            gRadiosityAlbedoSaturationAmount.Text = lightingData[5];
            gRadiositySpecularGlossScale.Text = lightingData[6];

            ConfigEditorUtils.Subscribe(this.Controls, Save);
            this.FormClosing += RadiosityEditor_FormClosing;
        }

        private void RadiosityEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConfigEditorUtils.Unsubscribe(this.Controls, Save);
            this.FormClosing -= RadiosityEditor_FormClosing;
        }

        private void Save(object sender, EventArgs e)
        {
            File.WriteAllText(Singleton.PathToAI + @"\DATA\RADIOSITY_SETTINGS.TXT", 
                "settings_file_version=1" + Environment.NewLine +
                "gRadiosityEmissiveSurfaceScale=" + gRadiosityEmissiveSurfaceScale.Text + Environment.NewLine +
                "gRadiosityFirstBounceScale=" + gRadiosityFirstBounceScale.Text + Environment.NewLine +
                "gRadiosityMultiBounceScale=" + gRadiosityMultiBounceScale.Text + Environment.NewLine +
                "gRadiosityAlbedoOverbrightAmount=" + gRadiosityAlbedoOverbrightAmount.Text + Environment.NewLine +
                "gRadiosityAlbedoSaturationAmount=" + gRadiosityAlbedoSaturationAmount.Text + Environment.NewLine +
                "gRadiositySpecularGlossScale=" + gRadiositySpecularGlossScale.Text + Environment.NewLine +
                "gDeferredEmissiveSurfaceScale=1.487500" + Environment.NewLine +
                "gDeferredEmissiveSurfaceExponent=0.563500");
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://opencage.co.uk/docs/configs/radiosity");
        }
    }
}
