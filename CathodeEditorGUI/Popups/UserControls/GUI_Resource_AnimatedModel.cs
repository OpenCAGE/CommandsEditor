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
        public int EnvironmentAnimIndex = -1;

        public GUI_Resource_AnimatedModel(LevelContent editor) : base(editor)
        {
            InitializeComponent();

            animatedModelIndex.BeginUpdate();
            animatedModelIndex.Items.Clear();
            List<int> indexes = new List<int>();
            foreach (var anim in Content.resource.env_animations.Entries)
            {
                if (indexes.Contains(anim.ResourceIndex)) continue;
                indexes.Add(anim.ResourceIndex);
            }
            for (int i = 0; i < indexes.Count; i++)
            {
                animatedModelIndex.Items.Add(i.ToString());
            }
            animatedModelIndex.EndUpdate();
        }

        public void PopulateUI(int index)
        {
            animatedModelIndex.SelectedItem = index.ToString();
            EnvironmentAnimIndex = index;
        }

        private void animatedModelIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnvironmentAnimIndex = Convert.ToInt32(animatedModelIndex.SelectedItem.ToString());
        }
    }
}
