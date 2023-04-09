using CATHODE;
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

namespace CommandsEditor.Popups.Base
{
    public partial class BaseWindow : Form
    {
        protected CommandsEditor _editor;
        protected Editor Editor { get { return _editor.Loaded; } } //hotfix for old Editor. static

        private WindowClosesOn _closesOn;

        private Commands _startCommands;
        private Entity _startEntity;
        private Composite _startComposite;

        [Obsolete("Designer only", true)]
        public BaseWindow()
        {
            InitializeComponent();
        }

        public BaseWindow(WindowClosesOn config, CommandsEditor editor)
        {
            InitializeComponent();

            _closesOn = config;
            _editor = editor;

            _startCommands = Editor.commands;
            _startEntity = Editor.selected.entity;
            _startComposite = Editor.selected.composite;

            if (_closesOn.HasFlag(WindowClosesOn.COMMANDS_RELOAD))
                Editor.OnCommandsSelected += OnCommandsSelected;
            if (_closesOn.HasFlag(WindowClosesOn.NEW_ENTITY_SELECTION))
                Editor.OnEntitySelected += OnEntitySelected;
            if (_closesOn.HasFlag(WindowClosesOn.NEW_COMPOSITE_SELECTION))
                Editor.OnCompositeSelected += OnCompositeSelected;
            if (_closesOn.HasFlag(WindowClosesOn.NEW_CAGEANIM_EDITOR_OPENED))
                Editor.OnCAGEAnimationEditorOpened += OnCAGEAnimationEditorOpened;

            this.BringToFront();
            this.Focus();
        }

        private void OnFormClosed(Object sender, FormClosedEventArgs e)
        {
            if (_closesOn.HasFlag(WindowClosesOn.COMMANDS_RELOAD))
                Editor.OnCommandsSelected -= OnCommandsSelected;
            if (_closesOn.HasFlag(WindowClosesOn.NEW_ENTITY_SELECTION))
                Editor.OnEntitySelected -= OnEntitySelected;
            if (_closesOn.HasFlag(WindowClosesOn.NEW_COMPOSITE_SELECTION))
                Editor.OnCompositeSelected -= OnCompositeSelected;
            if (_closesOn.HasFlag(WindowClosesOn.NEW_CAGEANIM_EDITOR_OPENED))
                Editor.OnCAGEAnimationEditorOpened -= OnCAGEAnimationEditorOpened;
        }

        private void OnCommandsSelected(Commands commands)
        {
            if ((_startCommands == null && commands != null) ||
                (_startCommands != null && commands == null) ||
                (_startCommands.Filepath != commands.Filepath))
                this.Close();
        }

        private void OnEntitySelected(Entity entity)
        {
            if ((_startEntity == null && entity != null) ||
                (_startEntity != null && entity == null) ||
                (_startEntity.shortGUID != entity.shortGUID))
                this.Close();
        }

        private void OnCompositeSelected(Composite composite)
        {
            if ((_startComposite == null && composite != null) ||
                (_startComposite != null && composite == null) ||
                (_startComposite.shortGUID != composite.shortGUID))
                this.Close();
        }

        private void OnCAGEAnimationEditorOpened()
        {
            this.Close();
        }
    }

    [Flags]
    public enum WindowClosesOn
    {
        COMMANDS_RELOAD = 1,
        NEW_ENTITY_SELECTION = 2,
        NEW_COMPOSITE_SELECTION = 4,

        NEW_CAGEANIM_EDITOR_OPENED = 8,

        NONE = 16,
    }
}
