using CATHODE;
using CATHODE.Enums;
using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor.ConfigEditors
{
    public partial class InventoryItemEditor : BaseWindow
    {
        private readonly BML _gblItem;
        private XmlElement _selectedElement;

        public InventoryItemEditor() : base()
        {
            InitializeComponent();

            _gblItem = new BML(Singleton.PathToAI + @"\DATA\GBL_ITEM.BML");

            weapon_type.BeginUpdate();
            foreach (WEAPON_TYPE type in Enum.GetValues(typeof(WEAPON_TYPE)))
            {
                if ((int)type == -1)
                    continue;
                weapon_type.Items.Add(type.ToString());
            }
            weapon_type.EndUpdate();

            ammo_type.BeginUpdate();
            foreach (AMMO_TYPE type in Enum.GetValues(typeof(AMMO_TYPE)))
            {
                if ((int)type == -1)
                    continue;
                ammo_type.Items.Add(type.ToString());
            }
            ammo_type.EndUpdate();

            Dictionary<string, ListViewGroup> groups = new Dictionary<string, ListViewGroup>();
            foreach (ListViewGroup group in listView.Groups)
            {
                groups.Add(group.Name, group);
            }

            listView.BeginUpdate();
            target_weapon.BeginUpdate();
            var objects = _gblItem.Content["item_database"]["objects"];
            foreach (XmlElement obj in objects)
            {
                listView.Items.Add(new ListViewItem() { Text = obj.GetAttribute("name"), Group = groups[obj.Name] });
                if (obj.Name == "weapon")
                    target_weapon.Items.Add(obj.GetAttribute("name")); // todo - this listbox needs updating if names change
            }
            listView.EndUpdate();
            target_weapon.EndUpdate();

            listView.Items[0].Selected = true;
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count != 1 || listView.SelectedItems[0].Group == null)
                return;

            string type = listView.SelectedItems[0].Group.Name;

            var objects = _gblItem.Content["item_database"]["objects"];
            foreach (XmlElement obj in objects)
            {
                if (obj.Name != type)
                    continue;
                if (obj.GetAttribute("name") != listView.SelectedItems[0].Text)
                    continue;
                _selectedElement = obj;
                break;
            }

            baseObject.Visible = false;
            weapon.Visible = false;
            ammo.Visible = false;
            held.Visible = false;
            medikit.Visible = false;

            SetObjectInfo();
            switch (type)
            {
                case "weapon":
                    weapon.Visible = true;
                    weapon_type.Text = _selectedElement.GetAttribute("weapon_type");
                    break;
                case "ammo":
                    ammo.Visible = true;
                    target_weapon.Text = _selectedElement.GetAttribute("target_weapon");
                    ammo_type.Text = ((AMMO_TYPE)Convert.ToInt32(_selectedElement.GetAttribute("ammo_type"))).ToString();
                    break;
                case "medikit":
                    SetHeldInfo();
                    medikit.Visible = true;
                    health_increase_percentage.Text = _selectedElement.GetAttribute("health_increase_percentage");
                    upgraded_health_increase_percentage.Text = _selectedElement.GetAttribute("upgraded_health_increase_percentage");
                    break;
                case "ied":
                case "light":
                    SetHeldInfo();
                    break;
            }
        }

        private void SetObjectInfo()
        {
            baseObject.Visible = true;
            name.Text = _selectedElement.GetAttribute("name");
            localisation_tag.Text = _selectedElement.GetAttribute("localisation_tag");
            if (localisation_tag.Text == "") localisation_tag.Text = name.Text;
            keyframe.Text = _selectedElement.GetAttribute("keyframe");
            if (keyframe.Text == "") keyframe.Text = name.Text;
            vanish_when_collected.Checked = !(_selectedElement.GetAttribute("vanish_when_collected") == "true");
            if (_selectedElement.GetAttribute("default_quantity") == "")
                default_quantity.Value = 1;
            else
                default_quantity.Text = _selectedElement.GetAttribute("default_quantity");
            if (_selectedElement.GetAttribute("stack_limit") == "")
                stack_limit.Value = 1;
            else
                stack_limit.Text = _selectedElement.GetAttribute("stack_limit");
            display_quantity.Checked = _selectedElement.GetAttribute("display_quantity") == "true";
            if (_selectedElement.GetAttribute("radial_menu_order_index") == "")
                radial_menu_order_index.Value = 0;
            else
                radial_menu_order_index.Text = _selectedElement.GetAttribute("radial_menu_order_index");
            crafting_resource.Checked = _selectedElement.GetAttribute("crafting_resource") == "true";
            if (_selectedElement.GetAttribute("composite") == "")
                composite.Text = "Required_Assets/Pickups/" + name.Text;
            else
                composite.Text = "Required_Assets/Pickups/" + _selectedElement.GetAttribute("composite");
            special_slot.Text = _selectedElement.GetAttribute("special_slot");
        }
        private void SetHeldInfo()
        {
            held.Visible = true;
            held_object_name.Text = "Required_Assets/Archetypes/Equipment/" + _selectedElement.GetAttribute("held_object_name");
            thrown_object_name.Text = "Required_Assets/Thrown/" + _selectedElement.GetAttribute("thrown_object_name");
            droppable_when_held.Checked = _selectedElement.GetAttribute("droppable_when_held") == "true";
            drop_when_consume.Checked = _selectedElement.GetAttribute("drop_when_consume") == "true";
            consume_when.Text = _selectedElement.GetAttribute("consume_when");
            activated_by.Text = _selectedElement.GetAttribute("activated_by");
            if (_selectedElement.GetAttribute("cancellable_duration_in_seconds") == "")
                cancellable_duration_in_seconds.Value = 0;
            else
                cancellable_duration_in_seconds.Text = _selectedElement.GetAttribute("cancellable_duration_in_seconds");
        }

        private void SetAttributeString(string attributeName, TextBox textbox, ComboBox combobox)
        {
            bool hasAttr = _selectedElement.HasAttribute(attributeName);

            if (textbox == null)
            {
                if (hasAttr)
                {
                    combobox.Text = _selectedElement.GetAttribute(attributeName);
                    combobox.Enabled = true;
                }
                else
                {
                    combobox.SelectedIndex = -1;
                    combobox.Text = "";
                    combobox.Enabled = false;
                }
            }
            else
            {
                if (hasAttr)
                {
                    textbox.Text = _selectedElement.GetAttribute(attributeName);
                    textbox.Enabled = true;
                }
                else
                {
                    textbox.Text = "";
                    textbox.Enabled = false;
                }
            }
        }

        private static void SetAttributeIfPresent(XmlElement element, string attributeName, string value)
        {
            if (element.HasAttribute(attributeName))
                element.SetAttribute(attributeName, value);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_selectedElement == null)
            {
                MessageBox.Show("Please load an inventory item first.");
                return;
            }

            var doc = _gblItem.Content;

            _selectedElement.SetAttribute("name", name.Text);
            SetAttributeIfPresent(_selectedElement, "thrown_object_name", thrown_object_name.Text);
            SetAttributeIfPresent(_selectedElement, "target_weapon", target_weapon.Text);
            SetAttributeIfPresent(_selectedElement, "ammo_type", ammo_type.Text);
            SetAttributeIfPresent(_selectedElement, "held_object_name", held_object_name.Text);
            SetAttributeIfPresent(_selectedElement, "keyframe", keyframe.Text);
            SetAttributeIfPresent(_selectedElement, "default_quantity", default_quantity.Text);
            SetAttributeIfPresent(_selectedElement, "stack_limit", stack_limit.Text);
            SetAttributeIfPresent(_selectedElement, "consume_when", consume_when.Text);
            SetAttributeIfPresent(_selectedElement, "composite", composite.Text);
            SetAttributeIfPresent(_selectedElement, "droppable_when_held", droppable_when_held.Text);
            SetAttributeIfPresent(_selectedElement, "special_slot", special_slot.Text);
            SetAttributeIfPresent(_selectedElement, "vanish_when_collected", vanish_when_collected.Text);
            SetAttributeIfPresent(_selectedElement, "display_quantity", display_quantity.Text);
            SetAttributeIfPresent(_selectedElement, "radial_menu_order_index", radial_menu_order_index.Text);
            SetAttributeIfPresent(_selectedElement, "crafting_resource", crafting_resource.Text);
            SetAttributeIfPresent(_selectedElement, "localisation_tag", localisation_tag.Text);
            SetAttributeIfPresent(_selectedElement, "activated_by", activated_by.Text);
            SetAttributeIfPresent(_selectedElement, "health_increase_percentage", health_increase_percentage.Text);
            SetAttributeIfPresent(_selectedElement, "upgraded_health_increase_percentage", upgraded_health_increase_percentage.Text);
            SetAttributeIfPresent(_selectedElement, "drop_when_consume", drop_when_consume.Text);
            SetAttributeIfPresent(_selectedElement, "cancellable_duration_in_seconds", cancellable_duration_in_seconds.Text);

            _gblItem.Content = doc;
            _gblItem.Save();


            MessageBox.Show("Saved new item configuration!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
