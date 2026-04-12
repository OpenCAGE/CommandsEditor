using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CommandsEditor
{
    public partial class ViewCone : UserControl
    {
        private string _name;

        public ViewCone()
        {
            InitializeComponent();
        }

        public void Populate(XmlElement viewcone)
        {
            _name = viewcone["ViewconeSettings_type"].InnerText;
            Length.Text = viewcone["Length"].InnerText;
            SmokeLengthModifier.Text = viewcone["SmokeLengthModifier"].InnerText;
            VerticalAngle.Text = viewcone["VerticalAngle"].InnerText;
            HorizontalAngle.Text = viewcone["HorizontalAngle"].InnerText;
            ExposureEffectLower.Text = viewcone["ExposureEffectLower"].InnerText;
            ExposureEffectUpper.Text = viewcone["ExposureEffectUpper"].InnerText;
            StanceEffectLower.Text = viewcone["StanceEffectLower"].InnerText;
            StanceEffectUpper.Text = viewcone["StanceEffectUpper"].InnerText;
            MovementEffectLower.Text = viewcone["MovementEffectLower"].InnerText;
            MovementEffectUpper.Text = viewcone["MovementEffectUpper"].InnerText;
            SmokeEffectLower.Text = viewcone["SmokeEffectLower"].InnerText;
            SmokeEffectUpper.Text = viewcone["SmokeEffectUpper"].InnerText;
            DistanceEffectLower.Text = viewcone["DistanceEffectLower"].InnerText;
            DistanceEffectUpper.Text = viewcone["DistanceEffectUpper"].InnerText;
            Light_meter_dark_level.Text = viewcone["Light_meter_dark_level"].InnerText;
            Light_meter_partially_lit_level.Text = viewcone["Light_meter_partially_lit_level"].InnerText;
            Light_meter_fully_lit_level.Text = viewcone["Light_meter_fully_lit_level"].InnerText;
        }

        public void Save(XmlElement viewcone)
        {
            EnsureChildElements(viewcone, "ViewconeSettings_type").InnerText = _name;
            EnsureChildElements(viewcone, "Length").InnerText = Length.Text;
            EnsureChildElements(viewcone, "SmokeLengthModifier").InnerText = SmokeLengthModifier.Text;
            EnsureChildElements(viewcone, "VerticalAngle").InnerText = VerticalAngle.Text;
            EnsureChildElements(viewcone, "HorizontalAngle").InnerText = HorizontalAngle.Text;
            EnsureChildElements(viewcone, "ExposureEffectLower").InnerText = ExposureEffectLower.Text;
            EnsureChildElements(viewcone, "ExposureEffectUpper").InnerText = ExposureEffectUpper.Text;
            EnsureChildElements(viewcone, "StanceEffectLower").InnerText = StanceEffectLower.Text;
            EnsureChildElements(viewcone, "StanceEffectUpper").InnerText = StanceEffectUpper.Text;
            EnsureChildElements(viewcone, "MovementEffectLower").InnerText = MovementEffectLower.Text;
            EnsureChildElements(viewcone, "MovementEffectUpper").InnerText = MovementEffectUpper.Text;
            EnsureChildElements(viewcone, "SmokeEffectLower").InnerText = SmokeEffectLower.Text;
            EnsureChildElements(viewcone, "SmokeEffectUpper").InnerText = SmokeEffectUpper.Text;
            EnsureChildElements(viewcone, "DistanceEffectLower").InnerText = DistanceEffectLower.Text;
            EnsureChildElements(viewcone, "DistanceEffectUpper").InnerText = DistanceEffectUpper.Text;
            EnsureChildElements(viewcone, "Light_meter_dark_level").InnerText = Light_meter_dark_level.Text;
            EnsureChildElements(viewcone, "Light_meter_partially_lit_level").InnerText = Light_meter_partially_lit_level.Text;
            EnsureChildElements(viewcone, "Light_meter_fully_lit_level").InnerText = Light_meter_fully_lit_level.Text;
        }

        private XmlElement EnsureChildElements(XmlNode parent, params string[] localNames)
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
    }
}
