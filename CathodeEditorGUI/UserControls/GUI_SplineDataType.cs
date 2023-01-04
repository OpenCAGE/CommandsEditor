using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE.Scripting;
using CATHODE;
using CathodeLib;

namespace CathodeEditorGUI.UserControls
{
    public partial class GUI_SplineDataType : UserControl
    {
        //TODO TODO TODO TODO !!!!!!!!!!!!!

        public GUI_SplineDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI(cSpline cSpline, ShortGuid paramID)
        {
            UNIMPLEMENTED_VARIABLE_TYPE.Text = ShortGuidUtils.FindString(paramID);

            //todo: dynamically populate UI
        }
    }
}
