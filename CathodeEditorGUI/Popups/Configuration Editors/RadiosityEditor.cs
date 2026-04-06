using CommandsEditor.Popups.Base;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor
{
    public partial class RadiosityEditor : BaseWindow
    {
        public RadiosityEditor() : base()
        {
            InitializeComponent();
        }

        private void RadiosityEditor_Load(object sender, EventArgs e)
        {
            string[] lightingData = File.ReadAllLines(Singleton.PathToAI + @"\DATA\RADIOSITY_SETTINGS.TXT");
            for (int i = 0; i < lightingData.Length; i++) if (lightingData[i] != "") lightingData[i] = lightingData[i].Split('=')[1];

            gRadiosityEmissiveSurfaceScale.Text = lightingData[1];
            gRadiosityFirstBounceScale.Text = lightingData[2];
            gRadiosityMultiBounceScale.Text = lightingData[3];
            gRadiosityAlbedoOverbrightAmount.Text = lightingData[4];
            gRadiosityAlbedoSaturationAmount.Text = lightingData[5];
            gRadiositySpecularGlossScale.Text = lightingData[6];
            gDeferredEmissiveSurfaceScale.Text = lightingData[7];
            gDeferredEmissiveSurfaceExponent.Text = lightingData[8];
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            File.WriteAllText(Singleton.PathToAI + @"\DATA\RADIOSITY_SETTINGS.TXT", 
                "settings_file_version=1" + Environment.NewLine +
                "gRadiosityEmissiveSurfaceScale=" + gRadiosityEmissiveSurfaceScale.Text + Environment.NewLine +
                "gRadiosityFirstBounceScale=" + gRadiosityFirstBounceScale.Text + Environment.NewLine +
                "gRadiosityMultiBounceScale=" + gRadiosityMultiBounceScale.Text + Environment.NewLine +
                "gRadiosityAlbedoOverbrightAmount=" + gRadiosityAlbedoOverbrightAmount.Text + Environment.NewLine +
                "gRadiosityAlbedoSaturationAmount=" + gRadiosityAlbedoSaturationAmount.Text + Environment.NewLine +
                "gRadiositySpecularGlossScale=" + gRadiositySpecularGlossScale.Text + Environment.NewLine +
                "gDeferredEmissiveSurfaceScale=" + gDeferredEmissiveSurfaceScale.Text + Environment.NewLine +
                "gDeferredEmissiveSurfaceExponent=" + gDeferredEmissiveSurfaceExponent.Text);
        }
    }
}
