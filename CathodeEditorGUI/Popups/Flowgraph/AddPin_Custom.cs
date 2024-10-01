using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CATHODE.Scripting;
using CommandsEditor.Popups.Base;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor
{
    public partial class AddPin_Custom : AddPin
    {
        public AddPin_Custom(STNode node, Mode mode) : base(node, mode)
        {
            _node = node;
            _mode = mode;

            InitializeComponent();

            switch (_mode)
            {
                case Mode.ADD_IN:
                    this.Text = "Add Pin In [" + _node.Title + "]";
                    label3.Text = "Pin In";
                    break;
                case Mode.REMOVE_IN:
                    this.Text = "Remove Pin In [" + _node.Title + "]";
                    label3.Text = "Pin In";
                    save_pin.Text = "Remove";
                    break;
                case Mode.ADD_OUT:
                    this.Text = "Add Pin Out [" + _node.Title + "]";
                    label3.Text = "Pin Out";
                    break;
                case Mode.REMOVE_OUT:
                    this.Text = "Remove Pin Out [" + _node.Title + "]";
                    label3.Text = "Pin Out";
                    save_pin.Text = "Remove";
                    break;
            }

            parameterList.BeginUpdate();
            parameterList.Items.Clear();
            //TODO: need to do some filtering on this list ideally so that it's showing things that would be expected for in/out
            List<string> items = Singleton.Editor?.CommandsDisplay?.Content.editor_utils.GenerateParameterListAsString(_node.Entity, _node.Entity.GetContainedComposite()); //TODO: idk if this is the most reliable way. should probably pass composite in
            for (int i = 0; i < items.Count; i++)
                parameterList.Items.Add(items[i]);
            parameterList.EndUpdate();
            parameterList.AutoSelectOff();
        }

        private void save_pin_Click(object sender, EventArgs e)
        {
            if (parameterList.Text == "")
            {
                MessageBox.Show("Please enter a parameter name!", "Incomplete information.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ShortGuid id = ShortGuidUtils.Generate(parameterList.Text);
            switch (_mode)
            {
                case Mode.ADD_IN:
                    _node.AddInputOption(id);
                    break;
                case Mode.REMOVE_IN:
                    _node.RemoveInputOption(id);
                    break;
                case Mode.ADD_OUT:
                    _node.AddOutputOption(id);
                    break;
                case Mode.REMOVE_OUT:
                    _node.RemoveOutputOption(id);
                    break;
            }
            _node.Recompute();

            OnSaved?.Invoke();
            this.Close();
        }
    }
}
