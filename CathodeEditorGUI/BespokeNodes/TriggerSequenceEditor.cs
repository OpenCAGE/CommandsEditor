﻿using CATHODE.Commands;
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

            for (int i = 0; i < node.triggers.Count; i++)
            {
                string toAdd = "[TIMING: " + node.triggers[i].timing + "] Hierarchy: ";
                for (int x = 0; x < node.triggers[i].hierarchy.Count - 1; x++)
                {
                    //TODO: how can i get the composite containing the node if we are chasing a hierarchy?
                    //toAdd += "[" + EntityDBEx.GetEntityName(node.triggers[i].hierarchy[x]) + "]";
                    if (x != node.triggers[i].hierarchy.Count - 2) toAdd += "->";
                }
                listBox1.Items.Add(toAdd);
            }

            for (int i = 0; i < node.events.Count; i++)
            {
                listBox2.Items.Add(ShortGuidUtils.FindString(node.events[i].EventID) + "\n  - StartedID: " + ShortGuidUtils.FindString(node.events[i].StartedID) + "\n  - FinishedID: " + ShortGuidUtils.FindString(node.events[i].FinishedID));
            }


            //node.triggers
        }
    }
}
