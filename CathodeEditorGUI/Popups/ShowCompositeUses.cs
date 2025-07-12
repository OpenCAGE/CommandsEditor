using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups;
using CommandsEditor.Popups.Base;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CommandsEditor
{
    public partial class ShowCompositeUses : BaseWindow
    {
        public Action<Composite, Entity> OnEntitySelected;

        private Dictionary<Entity, Composite> _entityComposites = new Dictionary<Entity, Composite>();
        private string _baseText = "Function Uses";

        public ShowCompositeUses(Composite composite = null) : base(composite == null ? WindowClosesOn.COMMANDS_RELOAD : WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            this.FormClosing += ShowCompositeUses_FormClosing;

            if (composite != null)
            {
                _baseText = "Composite Uses";
                label.Text = "Entities that instance the composite '" + composite.name + "':";
                entityVariant.Visible = false;
                searchFunctionTypes.Visible = false;
                Search(composite.shortGUID);
            }
            else
            {
                List<string> functionsOrdered = new List<string>();
                foreach (FunctionType function in Enum.GetValues(typeof(FunctionType)))
                    functionsOrdered.Add(function.ToString());
                functionsOrdered.Sort();
                foreach (string function in functionsOrdered)
                    entityVariant.Items.Add(function);
                entityVariant.SelectedIndex = SettingsManager.GetInteger(Singleton.Settings.PrevFuncUsesSearch);
            }
        }

        private void ShowCompositeUses_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_funcSelector != null)
            {
                _funcSelector.OnTypeSelected -= OnFunctionTypeSelected;
                _funcSelector.Close();
            }
        }

        private void jumpToEntity_Click(object sender, EventArgs e)
        {
            if (entityList.SelectedItems.Count == 0) return;
            Entity selected = (Entity)entityList.SelectedItems[0].Tag;
            OnEntitySelected?.Invoke(_entityComposites[selected], selected);

            if (!SettingsManager.GetBool(Singleton.Settings.KeepUsesWindowOpen))
                this.Close();
        }

        private void entityVariant_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsManager.SetInteger(Singleton.Settings.PrevFuncUsesSearch, entityVariant.SelectedIndex);
            Search(ShortGuidUtils.Generate(entityVariant.Text));
        }

        private void Search(ShortGuid guid)
        {
            _entityComposites.Clear();

            entityList.BeginUpdate();
            entityList.Items.Clear();
            entityList.Groups.Clear();
            foreach (Composite comp in Content.commands.Entries)
            {
                List<FunctionEntity> funcs = comp.functions.FindAll(o => o.function == guid);
                if (funcs.Count == 0)
                    continue;
                entityList.Groups.Add(new ListViewGroup() { Header = comp.name });
                foreach (FunctionEntity ent in funcs)
                {
                    ListViewItem item = (ListViewItem)Content.GenerateListViewItem(ent, comp).Clone();
                    item.Group = entityList.Groups[entityList.Groups.Count - 1];
                    item.ImageIndex = ent.function.IsFunctionType ? 1 : 2;
                    entityList.Items.Add(item);
                    _entityComposites.Add(ent, comp);
                }
            }
            entityList.EndUpdate();

            Text = _baseText + " - " + (entityVariant.Text != "" ? entityVariant.Text + " " : "") + "(" + entityList.Items.Count + ")";
        }

        SelectFunctionType _funcSelector = null;
        private void searchFunctionTypes_Click(object sender, EventArgs e)
        {
            if (_funcSelector != null)
            {
                _funcSelector.OnTypeSelected -= OnFunctionTypeSelected;
                _funcSelector.Close();
            }
            _funcSelector = new SelectFunctionType();
            _funcSelector.OnTypeSelected += OnFunctionTypeSelected;
            _funcSelector.Show();
        }
        private void OnFunctionTypeSelected(FunctionType type)
        {
            entityVariant.SelectedItem = type.ToString();
        }
    }
}
