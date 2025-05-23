using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using OpenCAGE;
using ST.Library.UI.NodeEditor;
using WebSocketSharp;
using static System.Net.Mime.MediaTypeNames;
using static CATHODE.SkeleDB;
using static CommandsEditor.EditorUtils;

namespace CommandsEditor
{
    public partial class ModifyPinsOrParameters : BaseWindow
    {
        public Action OnSaved;

        private List<ListViewItem> _items = new List<ListViewItem>();
        private ListViewColumnSorter _sorter = new ListViewColumnSorter();

        private EntityInspector _inspector = null;
        private STNode _node;
        private Mode _mode;

        public enum Mode
        {
            LINK_IN,
            LINK_OUT,

            PARAMETER,
        }

        public ModifyPinsOrParameters() : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();
        }

        public ModifyPinsOrParameters(EntityInspector inspector) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            _inspector = inspector;
            _mode = Mode.PARAMETER;

            SetupUI(inspector.Entity, inspector.Composite);
        }

        public ModifyPinsOrParameters(STNode node, Mode mode) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            _node = node;
            _mode = mode;

            SetupUI(Singleton.Editor.CommandsDisplay.CompositeDisplay.Composite.GetEntityByID(node.ShortGUID), Singleton.Editor.CommandsDisplay.CompositeDisplay.Composite);
        }

        private void SetupUI(Entity ent, Composite comp)
        { 
            InitializeComponent();
            param_name.ListViewItemSorter = _sorter;

            switch (_mode)
            {
                case Mode.LINK_IN:
                    this.Text = "Modify Pins In [" + _node.Title + "]";
                    break;
                case Mode.LINK_OUT:
                    this.Text = "Modify Pins Out [" + _node.Title + "]";
                    label2.Text = "Pins Out";
                    createParams.Text = "Set Checked As Pins Out";
                    break;
                case Mode.PARAMETER:
                    this.Text = "Modify Parameters";
                    label2.Text = "Parameters";
                    createParams.Text = "Set Parameters";
                    break;
            }

            List<ListViewItem> options = Singleton.Editor.CommandsDisplay.Content.editor_utils.GenerateParameterListAsListViewItem(ent, comp);
            switch (_mode)
            {
                case Mode.LINK_IN:
                case Mode.LINK_OUT:
                    STNodeOption[] nodeOptions = _mode == Mode.LINK_IN ? _node.GetInputOptions() : _node.GetOutputOptions();
                    //Add all base-game ones
                    for (int i = 0; i < options.Count; i++)
                    {
                        var metadata = ParameterUtils.GetParameterMetadata(ent, options[i].Text, comp);

                        if (metadata.Item1.Value == ParameterVariant.METHOD_FUNCTION)
                            continue;

                        options[i].Checked = nodeOptions.FirstOrDefault(o => o.ShortGUID == ((ParameterListViewItemTag)options[i].Tag).ShortGUID) != null;
                        options[i].SubItems[1].Text = metadata.Item2.Value.ToString();
                        options[i].Group = GetGroupFromVariant(metadata.Item1.Value);
                        options[i].ImageIndex = 0;
                        _items.Add(options[i]);
                    }
                    //Add any additional custom ones
                    for (int i = 0; i < nodeOptions.Length; i++)
                    {
                        if (options.FirstOrDefault(o => ((ParameterListViewItemTag)o.Tag).ShortGUID == nodeOptions[i].ShortGUID) != null)
                            continue;
                        AddCustomEntry(nodeOptions[i].ShortGUID, DataType.FLOAT, _mode == Mode.LINK_IN ? ParameterVariant.INPUT_PIN : ParameterVariant.OUTPUT_PIN); //TODO: Should maybe allow selection of variant?
                    }
                    break;

                case Mode.PARAMETER:
                    //Add all base-game ones
                    for (int i = 0; i < options.Count; i++)
                    {
                        var metadata = ParameterUtils.GetParameterMetadata(ent, options[i].Text, comp);

                        //TODO: Maybe we don't want to show other things here too?
                        if (metadata.Item1.Value == ParameterVariant.METHOD_FUNCTION || metadata.Item1.Value == ParameterVariant.TARGET_PIN)
                            continue;

                        options[i].Checked = ent.GetParameter(options[i].Text) != null;
                        options[i].SubItems[1].Text = metadata.Item2.Value.ToString();
                        options[i].Group = GetGroupFromVariant(metadata.Item1.Value);
                        options[i].ImageIndex = 0;
                        _items.Add(options[i]);
                    }
                    //Add any additional custom ones
                    for (int i = 0; i < ent.parameters.Count; i++)
                    {
                        if (options.FirstOrDefault(o => ((ParameterListViewItemTag)o.Tag).ShortGUID == ent.parameters[i].name && o.SubItems[1].Text == ent.parameters[i].content.dataType.ToString()) != null)
                            continue;
                        AddCustomEntry(ent.parameters[i].name, ent.parameters[i].content.dataType);
                    }
                    break;
            }

            AddCustom.Visible = ent.variant != EntityVariant.VARIABLE;

            Search();
        }

        private ListViewGroup GetGroupFromVariant(ParameterVariant variant)
        {
            foreach (ListViewGroup group in param_name.Groups)
                if (group.Name == variant.ToString())
                    return group;

            return null;
        }

        private void clearSearchBtn_Click(object sender, EventArgs e)
        {
            searchText.Text = "";
            Search();
        }

        private void searchText_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            param_name.BeginUpdate();
            param_name.Items.Clear();
            ListViewItem[] items = _items.Where(o => o.Text.ToUpper().Contains(searchText.Text.ToUpper())).ToList().ToArray();
            foreach (ListViewItem item in items)
            {
                ParameterListViewItemTag tag = (ParameterListViewItemTag)item.Tag;
                item.Group = GetGroupFromVariant(tag.Usage);
                param_name.Items.Add(item);
            }
            param_name.EndUpdate();

            typesCount.Text = "Showing " + param_name.Items.Count;
        }

        private void createParams_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in _items)
            {
                ParameterListViewItemTag tag = (ParameterListViewItemTag)item.Tag;
                switch (_mode)
                {
                    case Mode.LINK_IN:
                        if (item.Checked)
                        {
                            //NOTE: Hijacking this to add relays as well. Maybe we should make this optional?
                            if (item?.Group != null && item.Group.Name == ParameterVariant.METHOD_PIN.ToString())
                            {
                                ShortGuid relay = ParameterUtils.GetRelay(tag.ShortGUID);
                                if (relay != ShortGuid.Invalid)
                                    _node.AddOutputOption(relay);
                            }
                            _node.AddInputOption(tag.ShortGUID);
                        }
                        else
                            _node.RemoveInputOption(tag.ShortGUID);
                        break;
                    case Mode.LINK_OUT:
                        if (item.Checked)
                            _node.AddOutputOption(tag.ShortGUID);
                        else
                            _node.RemoveOutputOption(tag.ShortGUID);
                        break;

                    case Mode.PARAMETER:
                        if (item.Checked)
                        {
                            Parameter existing = _inspector.Entity.GetParameter(tag.ShortGUID);
                            DataType type = (DataType)Enum.Parse(typeof(DataType), item.SubItems[1].Text);
                            if (existing == null || existing.content.dataType != type)
                            {
                                ParameterData data = ParameterUtils.CreateDefaultParameterData(_inspector.Entity, _inspector.Composite, item.Text);
                                if (data != null)
                                {
                                    _inspector.Entity.AddParameter(ShortGuidUtils.Generate(item.Text), data);
                                }
                                else
                                {
                                    //Data can be null if this is a custom parameter (e.g. CAGEAnimation) - use the type info from the list here instead.
                                    _inspector.Entity.AddParameter(ShortGuidUtils.Generate(item.Text), (DataType)Enum.Parse(typeof(DataType), item.SubItems[1].Text));
                                }
                                Singleton.OnParameterModified?.Invoke();
                                switch (type)
                                {
                                    case DataType.RESOURCE:
                                        Singleton.OnResourceModified?.Invoke();
                                        break;
                                    case DataType.TRANSFORM:
                                        if (data != null && item.Text == "position")
                                        {
                                            cTransform transformVal = (cTransform)data;
                                            Singleton.OnEntityMoved?.Invoke(transformVal, _inspector.Entity);
                                        }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (_inspector.Entity.RemoveParameter(tag.ShortGUID))
                            {
                                DataType type = (DataType)Enum.Parse(typeof(DataType), item.SubItems[1].Text);
                                Singleton.OnParameterModified?.Invoke();
                                switch (type)
                                {
                                    case DataType.RESOURCE:
                                        Singleton.OnResourceModified?.Invoke();
                                        break;
                                    case DataType.TRANSFORM:
                                        if (item.Text == "position")
                                            Singleton.OnEntityMoved?.Invoke(null, _inspector.Entity);
                                        break;
                                }
                            }
                        }
                        break;
                }
            }

            if (_node != null)
                _node.Recompute();

            OnSaved?.Invoke();
            this.Close();
        }

        private void selectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in param_name.Items)
            {
                item.Selected = true;
                item.Checked = true;
            }
        }

        private void deSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in param_name.Items)
            {
                item.Selected = false;
                item.Checked = false;
            }
        }

        private void ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            // Determine if the clicked column is already the column that is being sorted.
            if (e.Column == _sorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_sorter.Order == SortOrder.Ascending)
                {
                    _sorter.Order = SortOrder.Descending;
                }
                else
                {
                    _sorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _sorter.SortColumn = e.Column;
                _sorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.param_name.Sort();
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            if (_inspector.Entity.variant == EntityVariant.FUNCTION)
            {
                FunctionEntity funcEnt = (FunctionEntity)_inspector.Entity;
                if (CommandsUtils.FunctionTypeExists(funcEnt.function))
                    Process.Start("https://opencage.co.uk/docs/cathode-entities/#" + ((FunctionType)((FunctionEntity)_inspector.Entity).function.ToUInt32()).ToString());
                else
                    Process.Start("https://opencage.co.uk/docs/cathode-entities/#" + FunctionType.CompositeInterface.ToString());
                return;
            }
            else if (_inspector.Entity.variant == EntityVariant.PROXY)
                Process.Start("https://opencage.co.uk/docs/cathode-entities/#" + FunctionType.ProxyInterface.ToString());
            else
                Process.Start("https://opencage.co.uk/docs/cathode-entities/#entities");
        }

        RenameGeneric _customPin = null;
        AddCustomParameter _customParam = null;
        private void AddCustom_Click(object sender, EventArgs e)
        {
            switch (_mode)
            {
                case Mode.LINK_IN:
                case Mode.LINK_OUT:
                    if (_customPin != null)
                    {
                        _customPin.OnRenamed -= OnAddedCustomPin;
                        _customPin.Close();
                    }

                    _customPin = new RenameGeneric("", new RenameGeneric.RenameGenericContent() { Title = "Add Custom Pin", Description = "Pin Name", ButtonText = "Add Custom Pin" });
                    _customPin.Show();
                    _customPin.OnRenamed += OnAddedCustomPin;
                    break;

                case Mode.PARAMETER:
                    if (_customParam != null)
                    {
                        _customParam.OnSelected -= OnAddedCustomParam;
                        _customParam.Close();
                    }

                    _customParam = new AddCustomParameter(_inspector);
                    _customParam.Show();
                    _customParam.OnSelected += OnAddedCustomParam;
                    break;
            }
        }

        private bool IsNameValid(string name)
        {
            foreach (ListViewItem existingItem in _items)
            {
                if (existingItem.Text == name)
                {
                    MessageBox.Show("The parameter '" + name + "' is already available on this Entity!\nPlease pick a new name.", "Parameter exists", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            return true;
        }

        private void OnAddedCustomPin(string text)
        {
            if (IsNameValid(text)) 
                AddCustomEntry(ShortGuidUtils.Generate(text), DataType.FLOAT, _mode == Mode.LINK_IN ? ParameterVariant.INPUT_PIN : ParameterVariant.OUTPUT_PIN);
        }
        private void OnAddedCustomParam(string name, DataType datatype)
        {
            if (IsNameValid(name))
                AddCustomEntry(ShortGuidUtils.Generate(name), datatype);
        }

        private void AddCustomEntry(ShortGuid guid, DataType datatype, ParameterVariant variant = ParameterVariant.PARAMETER)
        {
            string paramName = guid.ToString();

            ListViewItem item = new ListViewItem(paramName);
            item.SubItems.Add(datatype.ToString());
            item.Tag = new ParameterListViewItemTag() { ShortGUID = guid, Usage = variant };
            item.Checked = true;
            item.SubItems[1].Text = datatype.ToString();
            item.Group = GetGroupFromVariant(variant);
            item.ImageIndex = 0;
            item.Selected = true;
            _items.Add(item);

            Search();
        }
    }
}
