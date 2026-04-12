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

        public static void SetCheckbox(List<BML> configs, CheckBox checkbox, string parentVal, string val)
        {
            for (int i = 0; i < configs.Count; i++)
            {
                if (configs[i].Content["Ammo"][parentVal]?[val]?.InnerText == null)
                    continue;
                checkbox.Checked = configs[i].Content["Ammo"][parentVal][val].InnerText.ToUpper() == "TRUE";

                if (i != 0)
                    Console.WriteLine("Inherited " + parentVal + " " + val + " value of " + checkbox.Checked + " from " + configs[i].Filepath);
                break;
            }
        }

        public static void SetNumber(List<BML> configs, NumericUpDown updown, string parentVal, string val)
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

        public static void SetText(List<BML> configs, TextBox textbox, string parentVal, string val)
        {
            for (int i = 0; i < configs.Count; i++)
            {
                if (configs[i].Content["Ammo"][parentVal]?[val]?.InnerText == null)
                    continue;
                textbox.Text = configs[i].Content["Ammo"][parentVal][val].InnerText;

                if (i != 0)
                    Console.WriteLine("Inherited " + parentVal + " " + val + " value of " + textbox.Text + " from " + configs[i].Filepath);
                break;
            }
        }
    }
}
