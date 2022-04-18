using CATHODE.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CathodeEditorGUI
{
    public partial class TriggerSequenceEditor : Form
    {
        TriggerSequence node = null;
        public TriggerSequenceEditor(TriggerSequence _node)
        {
            InitializeComponent();
            node = _node;

            this.Text = "TriggerSequence Editor: " + CurrentInstance.compositeLookup.GetEntityName(CurrentInstance.selectedComposite.shortGUID, _node.shortGUID);

            for (int i = 0; i < node.triggers.Count; i++)
            {
                string thisHierarchy;
                EditorUtils.ResolveHierarchy(node.triggers[i].hierarchy, out CathodeComposite comp, out thisHierarchy);

                string toAdd = "[" + node.triggers[i].timing + "s] " + thisHierarchy;
                trigger_list.Items.Add(toAdd);
            }

            for (int i = 0; i < node.events.Count; i++)
            {
                event_list.Items.Add(ShortGuidUtils.FindString(node.events[i].EventID) + "\n  - StartedID: " + ShortGuidUtils.FindString(node.events[i].StartedID) + "\n  - FinishedID: " + ShortGuidUtils.FindString(node.events[i].FinishedID));
            }


            //node.triggers
        }
    }
}
