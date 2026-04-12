using CATHODE;
using CATHODE.Enums;
using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor
{
    public partial class LocomotionEditor : BaseWindow
    {
        List<BML> _selectedCharacter;

        public LocomotionEditor() : base()
        {
            InitializeComponent();

            BML ammoTypes = new BML(Singleton.PathToAI + "\\DATA\\CHR_INFO\\ATTRIBUTES\\ATTRIBUTES.BML");
            var attributes = ammoTypes.Content["Attributes"];
            characters.BeginUpdate();
            foreach (XmlElement attribute in attributes)
            {
                characters.Items.Add(attribute["Name"].InnerText);
            }
            characters.EndUpdate();
            characters.SelectedIndex = 0;
        }

        private void characters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (characters.Text == "")
            {
                MessageBox.Show("Please select a character class.");
                return;
            }

            _selectedCharacter = new List<BML>();
            _selectedCharacter.Add(new BML(Singleton.PathToAI + "\\DATA\\CHR_INFO\\ATTRIBUTES\\" + characters.Text + ".BML"));
            while (true)
            {
                string template = _selectedCharacter[_selectedCharacter.Count - 1].Content["Attribute"]["Template_Name"]?.InnerText;
                if (template == null || template == "") break;
                _selectedCharacter.Add(new BML(Singleton.PathToAI + "\\DATA\\CHR_INFO\\ATTRIBUTES\\" + template + ".BML"));
            }

            SetNumber(_selectedCharacter, capsuleRadius, "Locomotion", "capsuleRadius");
            SetNumber(_selectedCharacter, capsuleHeight, "Locomotion", "capsuleHeight");
            SetNumber(_selectedCharacter, permittedLocomotionModulation, "Locomotion", "permittedLocomotionModulation");

            var boundaries = _selectedCharacter[0].Content["Attribute"]["Locomotion"]["SteeringControls"];
            int i = 0;
            foreach (XmlElement boundary in boundaries)
            {
                tabControl1.TabPages[i].Controls.OfType<SteeringBoundarySet>().FirstOrDefault().Populate(boundary);
                i++;
            }
            tabControl1.SelectedIndex = 0;
        }

        private void SetNumber(List<BML> configs, NumericUpDown updown, string parentVal, string val)
        {
            for (int i = 0; i < configs.Count; i++)
            {
                if (configs[i].Content["Attribute"][parentVal]?[val]?.InnerText == null)
                    continue;
                updown.Value = Convert.ToDecimal(configs[i].Content["Attribute"][parentVal][val].InnerText);

                if (i != 0)
                    Console.WriteLine("Inherited " + parentVal + " " + val + " value of " + updown.Value + " from " + configs[i].Filepath);
                break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var doc = _selectedCharacter[0].Content;

            EnsureChildElements(doc, "Attribute", "Locomotion", "capsuleRadius").InnerText = capsuleRadius.Text;
            EnsureChildElements(doc, "Attribute", "Locomotion", "capsuleHeight").InnerText = capsuleHeight.Text;
            EnsureChildElements(doc, "Attribute", "Locomotion", "permittedLocomotionModulation").InnerText = permittedLocomotionModulation.Text;

            foreach (TabPage page in tabControl1.TabPages)
            {
                page.Controls.OfType<SteeringBoundarySet>().FirstOrDefault().Save(doc, doc["Attribute"]["Locomotion"]["SteeringControls"]);
            }

            _selectedCharacter[0].Content = doc;
            _selectedCharacter[0].Save();
        }

        private XmlElement EnsureChildElements(XmlNode parent, params string[] localNames)
        {
            XmlNode current = parent;
            XmlDocument document = parent as XmlDocument ?? parent.OwnerDocument;
            foreach (string name in localNames)
            {
                XmlElement match = null;
                foreach (XmlNode child in current.ChildNodes)
                {
                    if (child is XmlElement el && el.LocalName == name)
                    {
                        match = el;
                        break;
                    }
                }
                if (match == null)
                {
                    match = document.CreateElement(name);
                    current.AppendChild(match);
                }
                current = match;
            }
            return (XmlElement)current;
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://opencage.co.uk/docs/configs/locomotion");
        }
    }
}
