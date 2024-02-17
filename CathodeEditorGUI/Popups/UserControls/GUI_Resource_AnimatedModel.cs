using CATHODE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor.Popups.UserControls
{
    public partial class GUI_Resource_AnimatedModel : ResourceUserControl
    {
        private EnvironmentAnimations.EnvironmentAnimation _envAnimInfo = null;

        public GUI_Resource_AnimatedModel() : base()
        {
            InitializeComponent();

            skeletonList.BeginUpdate();
            skeletonList.Items.Clear();
            foreach (var skeleton in Singleton.AllSkeletons)
            {
                skeletonList.Items.Add(skeleton);
            }
            skeletonList.EndUpdate();
        }

        //NOTE TO SELF: This writes to the original env anim obj, which goes against the thing of having a save button in the UI.

        public void PopulateUI(EnvironmentAnimations.EnvironmentAnimation animInfo)
        {
            _envAnimInfo = animInfo;

            if (animInfo.SkeletonName == "")
                skeletonList.SelectedIndex = 0;
            else
                skeletonList.SelectedItem = animInfo.SkeletonName;
        }

        private void animatedModelIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            _envAnimInfo.SkeletonName = skeletonList.SelectedItem.ToString();
        }
    }
}
