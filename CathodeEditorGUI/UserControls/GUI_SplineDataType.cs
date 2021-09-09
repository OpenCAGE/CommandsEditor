using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE.Commands;
using CATHODE;
using CathodeLib;

namespace CathodeEditorGUI.UserControls
{
    public partial class GUI_SplineDataType : UserControl
    {
        public GUI_SplineDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI(CathodeSpline cSpline, cGUID paramID)
        {
            UNIMPLEMENTED_VARIABLE_TYPE.Text = NodeDB.GetCathodeName(paramID) + " (" + paramID.ToString() + ")";
            //todo: dynamically populate UI
        }
    }
}
