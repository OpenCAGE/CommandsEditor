using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using CATHODE;
using CATHODE.Scripting;
using static CommandsEditor.SelectSpecialString;

namespace CommandsEditor.UserControls
{
    public partial class GUI_StringVariant_AssetDropdown : ParameterUserControl
    {
        cString _stringVal = null;
        AssetList.Type _assetType;
        string _args;

        public GUI_StringVariant_AssetDropdown() : base()
        {
            InitializeComponent();
            this.ContextMenuStrip = contextMenuStrip1;
            this.deleteToolStripMenuItem.Click += new EventHandler(deleteToolStripMenuItem_Click);
        }

        public void PopulateUI(cString cString, string paramID, AssetList.Type assets, string args = "")
        {
            _stringVal = cString;
            _assetType = assets;
            _args = args;

            label1.Text = paramID;
            textBox1.Text = cString.value;
            this.deleteToolStripMenuItem.Text = "Delete '" + paramID + "'";

            _hasDoneSetup = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _stringVal.value = textBox1.Text;
            HighlightAsModified();
        }

        SelectSpecialString _popup = null;
        private void SelectStr_Click(object sender, EventArgs e)
        {
            if (_popup != null)
            {
                _popup.OnSelected -= OnStringSelected;
                _popup.Close();
            }

            _popup = new SelectSpecialString(label1.Text, _stringVal.value, _assetType, _args);
            _popup.OnSelected += OnStringSelected;
            _popup.Show();
        }
        private void OnStringSelected(string str)
        {
            textBox1.Text = str;
            _stringVal.value = str;
            HighlightAsModified();
        }

        public override void HighlightAsModified(bool updateDatabase = true, Control fontToUpdate = null)
        {
            base.HighlightAsModified(updateDatabase, label1);
        }
    }
}
