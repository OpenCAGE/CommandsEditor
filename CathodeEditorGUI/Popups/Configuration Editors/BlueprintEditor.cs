using CATHODE;
using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using WeifenLuo.WinFormsUI.Docking;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CommandsEditor
{
    public partial class BlueprintEditor : BaseWindow
    {
        private readonly BML _gblItem;

        public BlueprintEditor() : base()
        {
            InitializeComponent();

            _gblItem = new BML(Singleton.PathToAI + @"\DATA\GBL_ITEM.BML");

            var recipes = _gblItem.Content["item_database"]["recipes"];
            foreach (XmlElement recipe in recipes)
            {
                blueprints.Items.Add(recipe.GetAttribute("name"));
            }
            blueprints.SelectedIndex = 0;
        }

        private void blueprints_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (blueprints.Text == "")
            {
                MessageBox.Show("Please select a blueprint first.");
                return;
            }

            var recipes = _gblItem.Content["item_database"]["recipes"];
            foreach (XmlElement recipe in recipes)
            {
                if (recipe.GetAttribute("name") != blueprints.Text)
                    continue;

                craft_itemname.Items.Clear();
                craft_quantity.Items.Clear();
                output_itemname.Items.Clear();
                output_quantity.Items.Clear();

                var inputs = recipe["input"];
                foreach (XmlElement input in inputs)
                {
                    craft_itemname.Items.Add(input.GetAttribute("name"));
                    craft_quantity.Items.Add(input.GetAttribute("quantity"));
                }

                var outputs = recipe["output"];
                foreach (XmlElement output in outputs)
                {
                    output_itemname.Items.Add(output.GetAttribute("name"));
                    output_quantity.Items.Add(output.GetAttribute("quantity"));
                }
            }
        }

        //Add new required item
        private void addNewItemRequired_Click(object sender, EventArgs e)
        {
            BlueprintEditorPopup editorPopup = new BlueprintEditorPopup(1);
            editorPopup.Show();
        }

        //add new output item
        private void button3_Click(object sender, EventArgs e)
        {
            BlueprintEditorPopup editorPopup = new BlueprintEditorPopup(2);
            editorPopup.Show();
        }

        //Get data from other forms and add to listboxes
        public void getDataFromPopup(string NEW_QUANTITY, string NEW_ITEM, int DATA_TYPE)
        {
            if (DATA_TYPE == 1)
            {
                craft_itemname.Items.Add(NEW_ITEM);
                craft_quantity.Items.Add(NEW_QUANTITY);
            }
            else
            {
                output_itemname.Items.Add(NEW_ITEM);
                output_quantity.Items.Add(NEW_QUANTITY);
            }
        }

        //remove data from listbox OUTPUT
        private void removeOutputItem_Click(object sender, EventArgs e)
        {
            if (output_itemname.SelectedIndex == -1)
                return;

            output_quantity.Items.RemoveAt(output_itemname.SelectedIndex);
            output_itemname.Items.RemoveAt(output_itemname.SelectedIndex);
        }

        //remove data from listbox INPUT
        private void removeInputItem_Click(object sender, EventArgs e)
        {
            if (craft_itemname.SelectedIndex == -1)
                return;

            craft_quantity.Items.RemoveAt(craft_itemname.SelectedIndex);
            craft_itemname.Items.RemoveAt(craft_itemname.SelectedIndex);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (blueprints.Text == "")
            {
                MessageBox.Show("Please select a blueprint first.");
                return;
            }

            var doc = _gblItem.Content;

            var recipes = doc["item_database"]["recipes"];
            foreach (XmlElement recipe in recipes)
            {
                if (recipe.GetAttribute("name") != blueprints.Text)
                    continue;

                recipe.RemoveAll();
                recipe.SetAttribute("name", blueprints.Text);

                List<Tuple<string, string>> inputItems = new List<Tuple<string, string>>();
                for (int i = 0; i < craft_itemname.Items.Count; i++)
                    inputItems.Add(new Tuple<string, string>(craft_itemname.Items[i].ToString(), craft_quantity.Items[i].ToString()));
                AddRecipeParts(doc, "input", inputItems, recipe);

                List<Tuple<string, string>> outputItems = new List<Tuple<string, string>>();
                for (int i = 0; i < output_itemname.Items.Count; i++)
                    outputItems.Add(new Tuple<string, string>(output_itemname.Items[i].ToString(), output_quantity.Items[i].ToString()));
                AddRecipeParts(doc, "output", outputItems, recipe);
            }

            _gblItem.Content = doc;
            _gblItem.Save();
        }

        private void AddRecipeParts(XmlDocument doc, string itemType, List<Tuple<string, string>> items, XmlElement parent)
        {
            XmlElement type = doc.CreateElement(itemType);
            foreach (Tuple<string, string> item in items)
            {
                XmlElement itemElement = doc.CreateElement("item");
                itemElement.SetAttribute("name", item.Item1);
                itemElement.SetAttribute("quantity", item.Item2);
                type.AppendChild(itemElement);
            }
            parent.AppendChild(type);
        }
    }
}
