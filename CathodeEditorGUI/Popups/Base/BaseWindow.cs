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
        protected LevelContent _content;
        protected LevelContent Content { get { return _content; } }

        private WindowClosesOn _closesOn;

        [Obsolete("Designer only", true)]
        public BaseWindow()
        {
            InitializeComponent();

            this.Disposed += BaseWindow_Disposed;
        }

        private void BaseWindow_Disposed(object sender, EventArgs e)
        {
            _content = null;
        }

        public BaseWindow(WindowClosesOn config, LevelContent content)
        {
            InitializeComponent();

            _closesOn = config;
            _content = content;

            if (_closesOn.HasFlag(WindowClosesOn.COMMANDS_RELOAD))
                Singleton.OnLevelLoaded += OnCommandsSelected;
            if (_closesOn.HasFlag(WindowClosesOn.NEW_ENTITY_SELECTION))
                Singleton.OnEntitySelected += OnEntitySelected;
            if (_closesOn.HasFlag(WindowClosesOn.NEW_COMPOSITE_SELECTION))
                Singleton.OnCompositeSelected += OnCompositeSelected;
            if (_closesOn.HasFlag(WindowClosesOn.NEW_CAGEANIM_EDITOR_OPENED))
                Singleton.OnCAGEAnimationEditorOpened += OnCAGEAnimationEditorOpened;

            this.BringToFront();
            this.Focus();
        }

        private void OnFormClosed(Object sender, FormClosedEventArgs e)
        {
            if (_closesOn.HasFlag(WindowClosesOn.COMMANDS_RELOAD))
                Singleton.OnLevelLoaded -= OnCommandsSelected;
            if (_closesOn.HasFlag(WindowClosesOn.NEW_ENTITY_SELECTION))
                Singleton.OnEntitySelected -= OnEntitySelected;
            if (_closesOn.HasFlag(WindowClosesOn.NEW_COMPOSITE_SELECTION))
                Singleton.OnCompositeSelected -= OnCompositeSelected;
            if (_closesOn.HasFlag(WindowClosesOn.NEW_CAGEANIM_EDITOR_OPENED))
                Singleton.OnCAGEAnimationEditorOpened -= OnCAGEAnimationEditorOpened;
        }

        private void OnCommandsSelected(LevelContent content)
        {
            this.Close();
        }

        private void OnEntitySelected(Entity entity)
        {
            this.Close();
        }

        private void OnCompositeSelected(Composite composite)
        {
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
