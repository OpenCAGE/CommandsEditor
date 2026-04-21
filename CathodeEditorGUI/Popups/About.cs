using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor.Popups
{
    public partial class About : BaseWindow
    {
        public About() : base()
        {
            InitializeComponent();

            AboutWPF about = new AboutWPF();
            about.SetVersion(ProductVersion);
            aboutHost.Child = about;
        }
    }
}
