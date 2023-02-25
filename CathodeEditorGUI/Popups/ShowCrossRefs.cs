using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor
{
    public partial class ShowCrossRefs : Form
    {
        public Action<ShortGuid, Composite> OnEntitySelected;

        private List<EntityRef> proxies = new List<EntityRef>();
        private List<EntityRef> overrides = new List<EntityRef>();

        public ShowCrossRefs()
        {
            InitializeComponent();

            foreach (Composite comp in Editor.commands.Entries)
            {
                foreach (OverrideEntity ovr in comp.overrides)
                {
                    Entity ent = CommandsUtils.ResolveHierarchy(Editor.commands, comp, ovr.hierarchy, out Composite compRef, out string str);
                    if (ent != Editor.selected.entity) continue;
                    overrides.Add(new EntityRef() { composite = comp, entity = ovr.shortGUID });
                    overridesUI.Items.Add(EditorUtils.GenerateEntityName(ovr, comp));
                }
                foreach (ProxyEntity prox in comp.proxies)
                {
                    Entity ent = CommandsUtils.ResolveHierarchy(Editor.commands, comp, prox.hierarchy, out Composite compRef, out string str);
                    if (ent != Editor.selected.entity) continue;
                    proxies.Add(new EntityRef() { composite = comp, entity = prox.shortGUID });
                    proxiesUI.Items.Add(EditorUtils.GenerateEntityName(prox, comp));
                }
            }
        }

        private void jumpToProxy_Click(object sender, EventArgs e)
        {
            if (proxiesUI.SelectedIndex == -1) return;
            OnEntitySelected?.Invoke(proxies[proxiesUI.SelectedIndex].entity, proxies[proxiesUI.SelectedIndex].composite);
            this.Close();
        }

        private void jumpToOverride_Click(object sender, EventArgs e)
        {
            if (overridesUI.SelectedIndex == -1) return;
            OnEntitySelected?.Invoke(overrides[overridesUI.SelectedIndex].entity, overrides[overridesUI.SelectedIndex].composite);
            this.Close();
        }

        private struct EntityRef
        {
            public ShortGuid entity;
            public Composite composite;
        }
    }
}
