using CATHODE;
using CATHODE.Enums;
using CATHODE.Scripting;
using CommandsEditor.Popups.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor.ConfigEditors
{
    public partial class InventoryItemEditor : BaseWindow
    {
        private readonly BML _gblItem;
        private string _ogName;

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
                    target_weapon.Items.Add(obj.GetAttribute("name"));
            }
            listView.EndUpdate();
            target_weapon.EndUpdate();

            listView.Items[0].Selected = true;

            this.FormClosing += InventoryItemEditor_FormClosing;
        }

        private void InventoryItemEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.FormClosing -= InventoryItemEditor_FormClosing;

            ConfigEditorUtils.Unsubscribe(baseObject.Controls, Save);
            ConfigEditorUtils.Unsubscribe(weapon.Controls, Save);
            ConfigEditorUtils.Unsubscribe(ammo.Controls, Save);
            ConfigEditorUtils.Unsubscribe(held.Controls, Save);
            ConfigEditorUtils.Unsubscribe(medikit.Controls, Save);
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count != 1 || listView.SelectedItems[0].Group == null)
                return;

            string type = listView.SelectedItems[0].Group.Name;
            XmlElement selectedElement = null;
            {
                var objects = _gblItem.Content["item_database"]["objects"];
                foreach (XmlElement obj in objects)
                {
                    if (obj.Name != type)
                        continue;
                    if (obj.GetAttribute("name") != listView.SelectedItems[0].Text)
                        continue;
                    selectedElement = obj;
                    break;
                }
            }

            ConfigEditorUtils.Unsubscribe(baseObject.Controls, Save);
            ConfigEditorUtils.Unsubscribe(weapon.Controls, Save);
            ConfigEditorUtils.Unsubscribe(ammo.Controls, Save);
            ConfigEditorUtils.Unsubscribe(held.Controls, Save);
            ConfigEditorUtils.Unsubscribe(medikit.Controls, Save);

            baseObject.Visible = false;
            weapon.Visible = false;
            ammo.Visible = false;
            held.Visible = false;
            medikit.Visible = false;

            SetObjectInfo(selectedElement);
            switch (type)
            {
                case "weapon":
                    weapon.Visible = true;
                    weapon_type.Text = selectedElement.GetAttribute("weapon_type");
                    ConfigEditorUtils.Subscribe(weapon.Controls, Save);
                    break;
                case "ammo":
                    ammo.Visible = true;
                    target_weapon.Text = selectedElement.GetAttribute("target_weapon");
                    ammo_type.Text = ((AMMO_TYPE)Convert.ToInt32(selectedElement.GetAttribute("ammo_type"))).ToString();
                    ConfigEditorUtils.Subscribe(ammo.Controls, Save);
                    break;
                case "medikit":
                    SetHeldInfo(selectedElement);
                    medikit.Visible = true;
                    health_increase_percentage.Text = selectedElement.GetAttribute("health_increase_percentage");
                    upgraded_health_increase_percentage.Text = selectedElement.GetAttribute("upgraded_health_increase_percentage");
                    ConfigEditorUtils.Subscribe(medikit.Controls, Save);
                    break;
                case "ied":
                case "light":
                    SetHeldInfo(selectedElement);
                    break;
            }

            _ogName = name.Text;
        }

        private void SetObjectInfo(XmlElement selectedElement)
        {
            baseObject.Visible = true;
            name.Text = selectedElement.GetAttribute("name");
            localisation_tag.Text = selectedElement.GetAttribute("localisation_tag");
            if (localisation_tag.Text == "") localisation_tag.Text = name.Text;
            keyframe.Text = selectedElement.GetAttribute("keyframe");
            if (keyframe.Text == "") keyframe.Text = name.Text;
            vanish_when_collected.Checked = !(selectedElement.GetAttribute("vanish_when_collected") == "true");
            if (selectedElement.GetAttribute("default_quantity") == "")
                default_quantity.Value = 1;
            else
                default_quantity.Text = selectedElement.GetAttribute("default_quantity");
            if (selectedElement.GetAttribute("stack_limit") == "")
                stack_limit.Value = 1;
            else
                stack_limit.Text = selectedElement.GetAttribute("stack_limit");
            display_quantity.Checked = selectedElement.GetAttribute("display_quantity") == "true";
            if (selectedElement.GetAttribute("radial_menu_order_index") == "")
                radial_menu_order_index.Value = 0;
            else
                radial_menu_order_index.Text = selectedElement.GetAttribute("radial_menu_order_index");
            crafting_resource.Checked = selectedElement.GetAttribute("crafting_resource") == "true";
            if (selectedElement.GetAttribute("composite") == "")
                composite.Text = "Required_Assets/Pickups/" + name.Text;
            else
                composite.Text = "Required_Assets/Pickups/" + selectedElement.GetAttribute("composite");
            special_slot.Text = selectedElement.GetAttribute("special_slot");
            ConfigEditorUtils.Subscribe(baseObject.Controls, Save);
        }
        private void SetHeldInfo(XmlElement selectedElement)
        {
            held.Visible = true;
            held_object_name.Text = "Required_Assets/Archetypes/Equipment/" + selectedElement.GetAttribute("held_object_name");
            thrown_object_name.Text = "Required_Assets/Thrown/" + selectedElement.GetAttribute("thrown_object_name");
            droppable_when_held.Checked = selectedElement.GetAttribute("droppable_when_held") == "true";
            drop_when_consume.Checked = selectedElement.GetAttribute("drop_when_consume") == "true";
            consume_when.Text = selectedElement.GetAttribute("consume_when");
            activated_by.Text = selectedElement.GetAttribute("activated_by");
            if (selectedElement.GetAttribute("cancellable_duration_in_seconds") == "")
                cancellable_duration_in_seconds.Value = 0;
            else
                cancellable_duration_in_seconds.Text = selectedElement.GetAttribute("cancellable_duration_in_seconds");
            ConfigEditorUtils.Subscribe(held.Controls, Save);
        }

        private void Save(object sender, EventArgs e)
        {
            var doc = _gblItem.Content;

            string type = listView.SelectedItems[0].Group.Name;
            XmlElement selectedElement = null;
            {
                var objects = doc["item_database"]["objects"];
                foreach (XmlElement obj in objects)
                {
                    if (obj.Name != type)
                        continue;
                    if (obj.GetAttribute("name") != _ogName)
                        continue;
                    selectedElement = obj;
                    break;
                }
            }

            if (baseObject.Visible)
            {
                if (!composite.Text.StartsWith("Required_Assets/Pickups/"))
                    composite.Text = "Required_Assets/Pickups/";

                var specialSlots = doc["item_database"]["special_slots"];
                foreach (XmlElement specialSlot in specialSlots)
                {
                    if (specialSlot.GetAttribute("name") != selectedElement.GetAttribute("special_slot"))
                        continue;
                    specialSlot.SetAttribute("name", special_slot.Text);
                    break;
                }

                selectedElement.SetAttribute("name", name.Text); 
                selectedElement.SetAttribute("localisation_tag", localisation_tag.Text);
                selectedElement.SetAttribute("keyframe", keyframe.Text);
                selectedElement.SetAttribute("vanish_when_collected", (!vanish_when_collected.Checked).ToString().ToLower());
                selectedElement.SetAttribute("default_quantity", default_quantity.Text);
                selectedElement.SetAttribute("stack_limit", stack_limit.Text);
                selectedElement.SetAttribute("display_quantity", display_quantity.Checked.ToString().ToLower());
                selectedElement.SetAttribute("radial_menu_order_index", radial_menu_order_index.Text);
                selectedElement.SetAttribute("crafting_resource", crafting_resource.Checked.ToString().ToLower());
                selectedElement.SetAttribute("composite", composite.Text.Substring(("Required_Assets/Pickups/").Length));
                selectedElement.SetAttribute("special_slot", special_slot.Text);
            }
            if (weapon.Visible)
            {
                selectedElement.SetAttribute("weapon_type", weapon_type.Text);
            }
            if (ammo.Visible)
            {
                selectedElement.SetAttribute("target_weapon", target_weapon.Text);
                selectedElement.SetAttribute("ammo_type", ((int)(AMMO_TYPE)Enum.Parse(typeof(AMMO_TYPE), ammo_type.Text)).ToString());
            }
            if (held.Visible)
            {
                if (!held_object_name.Text.StartsWith("Required_Assets/Archetypes/Equipment/"))
                    held_object_name.Text = "Required_Assets/Archetypes/Equipment/";
                if (!thrown_object_name.Text.StartsWith("Required_Assets/Thrown/"))
                    thrown_object_name.Text = "Required_Assets/Thrown/";

                selectedElement.SetAttribute("held_object_name", held_object_name.Text.Substring(("Required_Assets/Archetypes/Equipment/").Length));
                selectedElement.SetAttribute("thrown_object_name", thrown_object_name.Text.Substring(("Required_Assets/Thrown/").Length));
                selectedElement.SetAttribute("droppable_when_held", droppable_when_held.Checked.ToString().ToLower());
                selectedElement.SetAttribute("drop_when_consume", drop_when_consume.Checked.ToString().ToLower());
                selectedElement.SetAttribute("consume_when", consume_when.Text);
                selectedElement.SetAttribute("activated_by", activated_by.Text);
                selectedElement.SetAttribute("cancellable_duration_in_seconds", cancellable_duration_in_seconds.Text);
            }
            if (medikit.Visible)
            {
                selectedElement.SetAttribute("health_increase_percentage", health_increase_percentage.Text);
                selectedElement.SetAttribute("upgraded_health_increase_percentage", upgraded_health_increase_percentage.Text);
            }

            if (name.Text != _ogName)
            {
                target_weapon.BeginUpdate();
                target_weapon.Items.Clear();
                var objects = doc["item_database"]["objects"];
                foreach (XmlElement obj in objects)
                {
                    if (obj.Name == "weapon")
                        target_weapon.Items.Add(obj.GetAttribute("name"));
                    if (obj.Name == "ammo" && obj.GetAttribute("target_weapon") == _ogName)
                        obj.SetAttribute("target_weapon", name.Text);
                }
                target_weapon.EndUpdate();

                for (int i = 0; i < listView.Items.Count; i++)
                {
                    if (!listView.Items[i].Selected)
                        continue;
                    listView.Items[i].Text = name.Text;
                    break;
                }

                _ogName = name.Text;
            }

            _gblItem.Content = doc;
            _gblItem.Save();
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://opencage.co.uk/docs/configs/inventory-items");
        }
    }
}
