using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
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

namespace CommandsEditor
{
    public partial class ShowInstanceInfo : BaseWindow
    {
        private CompositeDisplay _display;

        public ShowInstanceInfo(CompositeDisplay display) : base(WindowClosesOn.NEW_COMPOSITE_SELECTION | WindowClosesOn.COMMANDS_RELOAD, display.Content)
        {
            _display = display;
            InitializeComponent();

            cTransform globalTransform = new cTransform();
            foreach (Entity entity in display.Path.AllEntities)
            {
                Parameter position = entity.GetParameter("position");
                if (position == null) continue;
                if (position.content == null || position.content.dataType != DataType.TRANSFORM) continue;
                cTransform localTransform = (cTransform)position.content;
                globalTransform += localTransform;
            }

            bool isFromRoot = 
                (display.Path.PreviousComposite == null && display.Composite == Content.commands.EntryPoints[0]) ||         //Current composite is root
                (display.Path.AllComposites.Count > 0 && display.Path.AllComposites[0] == Content.commands.EntryPoints[0]); //First composite in path is root

            guI_TransformDataType1.PopulateUI(globalTransform, isFromRoot ? "Global Position" : "Relative Position", true);
        }
    }
}
