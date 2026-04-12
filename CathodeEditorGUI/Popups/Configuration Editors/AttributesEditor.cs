using CATHODE;
using CommandsEditor.Popups.Base;
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
using System.Xml;

namespace CommandsEditor.ConfigEditors
{
    public partial class AttributesEditor : BaseWindow
    {
        public AttributesEditor() : base()
        {
            InitializeComponent();

            BML attributeTypes = new BML(Singleton.PathToAI + "\\DATA\\CHR_INFO\\ATTRIBUTES\\ATTRIBUTES.BML");
            var attributes = attributeTypes.Content["Attributes"];
            characters.BeginUpdate();
            foreach (XmlElement attribute in attributes)
            {
                characters.Items.Add(attribute["Name"].InnerText);
            }
            characters.EndUpdate();
            characters.SelectedIndex = 0;

            BML behaviourTrees = new BML(Singleton.PathToAI + "\\DATA\\BINARY_BEHAVIOR\\_DIRECTORY_CONTENTS.BML");
            var behaviours = behaviourTrees.Content["DIR"];
            Behavior_Tree.BeginUpdate();
            foreach (XmlElement behaviour in behaviours)
            {
                Behavior_Tree.Items.Add(Path.GetFileNameWithoutExtension(behaviour["File"].GetAttribute("name")));
            }
            Behavior_Tree.EndUpdate();

            Character_Sound.BeginUpdate();
            Character_Sound.Items.Add("PLAYER1");
            Character_Sound.Items.Add("PLAYER2");
            Character_Sound.Items.Add("ALIEN");
            Character_Sound.Items.Add("FACEHUGGER");
            Character_Sound.Items.Add("ANDROID");
            Character_Sound.Items.Add("CIVILIAN");
            Character_Sound.Items.Add("SECURITY_GUARD");
            Character_Sound.EndUpdate();

            TargetingSystem.BeginUpdate();
            TargetingSystem.Items.Add("TM_NONE");
            TargetingSystem.Items.Add("TM_ALIEN");
            TargetingSystem.Items.Add("TM_FACEHUGGER");
            TargetingSystem.Items.Add("TM_ANDROID");
            TargetingSystem.Items.Add("TM_HUMAN");
            TargetingSystem.Items.Add("TM_PLAYER");
            TargetingSystem.EndUpdate();

            ATTACK_GROUP.BeginUpdate();
            ATTACK_GROUP.Items.Add("AT_NONE");
            ATTACK_GROUP.Items.Add("AT_FHUGGER");
            ATTACK_GROUP.Items.Add("AT_ANDROID");
            ATTACK_GROUP.Items.Add("AT_HUMAN");
            ATTACK_GROUP.Items.Add("AT_ALIEN");
            ATTACK_GROUP.EndUpdate();
        }

        private void characters_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://opencage.co.uk/docs/configs/blueprint-recipes");
        }
    }
}
