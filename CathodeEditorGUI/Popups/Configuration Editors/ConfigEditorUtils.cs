using CATHODE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CommandsEditor.ConfigEditors
{
    static class ConfigEditorUtils
    {
        public static XmlElement EnsureChildElements(XmlNode parent, params string[] localNames)
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

        public static void SetCheckbox(List<BML> configs, CheckBox checkbox, params string[] elementPath)
        {
            if (elementPath == null || elementPath.Length == 0)
                return;
            string pathLabel = string.Join("/", elementPath);
            for (int i = 0; i < configs.Count; i++)
            {
                XmlElement leaf = TryGetDescendant(configs[i].Content, elementPath);
                if (leaf?.InnerText == null)
                    continue;
                checkbox.Checked = leaf.InnerText.ToUpper() == "TRUE";

                if (i != 0)
                    Console.WriteLine("Inherited " + pathLabel + " value of " + checkbox.Checked + " from " + configs[i].Filepath);
                break;
            }
        }

        public static void SetNumber(List<BML> configs, NumericUpDown updown, params string[] elementPath)
        {
            if (elementPath == null || elementPath.Length == 0)
                return;
            string pathLabel = string.Join("/", elementPath);
            for (int i = 0; i < configs.Count; i++)
            {
                XmlElement leaf = TryGetDescendant(configs[i].Content, elementPath);
                if (leaf?.InnerText == null)
                    continue;
                updown.Value = Convert.ToDecimal(leaf.InnerText);

                if (i != 0)
                    Console.WriteLine("Inherited " + pathLabel + " value of " + updown.Value + " from " + configs[i].Filepath);
                break;
            }
        }

        public static void SetText(List<BML> configs, TextBox textbox, params string[] elementPath)
        {
            if (elementPath == null || elementPath.Length == 0)
                return;
            string pathLabel = string.Join("/", elementPath);
            for (int i = 0; i < configs.Count; i++)
            {
                XmlElement leaf = TryGetDescendant(configs[i].Content, elementPath);
                if (leaf?.InnerText == null)
                    continue;
                textbox.Text = leaf.InnerText;

                if (i != 0)
                    Console.WriteLine("Inherited " + pathLabel + " value of " + textbox.Text + " from " + configs[i].Filepath);
                break;
            }
        }

        private static XmlElement TryGetDescendant(XmlNode root, params string[] localNames)
        {
            if (localNames == null || localNames.Length == 0)
                return null;
            XmlNode current = root;
            foreach (string name in localNames)
            {
                if (current == null)
                    return null;
                current = current[name];
            }
            return current as XmlElement;
        }
    }
}
