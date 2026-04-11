using CATHODE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor
{
    public partial class GlobalConstantsEditor : DockContent
    {
        private BML _globalConstants;

        public GlobalConstantsEditor()
        {
            InitializeComponent();

            _globalConstants = new BML(Singleton.PathToAI + @"\DATA\GLOBALCONSTANTS.BML");

            stealth_light_meter_full_dark_threshold.Text = _globalConstants.Content["GlobalConstants"]["StealthLightMeter"]["stealth_light_meter_full_dark_threshold"].InnerText;
            stealth_light_meter_full_light_threshold.Text = _globalConstants.Content["GlobalConstants"]["StealthLightMeter"]["stealth_light_meter_full_light_threshold"].InnerText;
            stealth_light_meter_timeout_when_detected.Text = _globalConstants.Content["GlobalConstants"]["StealthLightMeter"]["stealth_light_meter_timeout_when_detected"].InnerText;

            interaction_distance_threshold.Text = _globalConstants.Content["GlobalConstants"]["Interaction"]["interaction_distance_threshold"].InnerText;

            min_time_between_squad_shots_lower_bound.Text = _globalConstants.Content["GlobalConstants"]["squad_shots"]["min_time_between_squad_shots_lower_bound"].InnerText;
            min_time_between_squad_shots_upper_bound.Text = _globalConstants.Content["GlobalConstants"]["squad_shots"]["min_time_between_squad_shots_upper_bound"].InnerText;

            min_time_suspicious_reaction_loop.Text = _globalConstants.Content["GlobalConstants"]["suspicious_item_reaction"]["min_time_suspicious_reaction_loop"].InnerText;
            max_time_suspicious_reaction_loop.Text = _globalConstants.Content["GlobalConstants"]["suspicious_item_reaction"]["max_time_suspicious_reaction_loop"].InnerText;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var doc = _globalConstants.Content;

            doc["GlobalConstants"]["StealthLightMeter"]["stealth_light_meter_full_dark_threshold"].InnerText = stealth_light_meter_full_dark_threshold.Text;
            doc["GlobalConstants"]["StealthLightMeter"]["stealth_light_meter_full_light_threshold"].InnerText = stealth_light_meter_full_light_threshold.Text;
            doc["GlobalConstants"]["StealthLightMeter"]["stealth_light_meter_timeout_when_detected"].InnerText = stealth_light_meter_timeout_when_detected.Text;

            doc["GlobalConstants"]["Interaction"]["interaction_distance_threshold"].InnerText = interaction_distance_threshold.Text;

            doc["GlobalConstants"]["squad_shots"]["min_time_between_squad_shots_lower_bound"].InnerText = min_time_between_squad_shots_lower_bound.Text;
            doc["GlobalConstants"]["squad_shots"]["min_time_between_squad_shots_upper_bound"].InnerText = min_time_between_squad_shots_upper_bound.Text;

            doc["GlobalConstants"]["suspicious_item_reaction"]["min_time_suspicious_reaction_loop"].InnerText = min_time_suspicious_reaction_loop.Text;
            doc["GlobalConstants"]["suspicious_item_reaction"]["max_time_suspicious_reaction_loop"].InnerText = max_time_suspicious_reaction_loop.Text;

            _globalConstants.Content = doc;
            _globalConstants.Save();
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://opencage.co.uk/docs/configs/global-constants");
        }
    }
}
