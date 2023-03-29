using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
    [STNode("/")]
    public class Checkpoint : STNode
    {
        private bool _is_first_checkpoint;
        [STNodeProperty("is_first_checkpoint", "is_first_checkpoint")]
        public bool is_first_checkpoint
        {
            get { return _is_first_checkpoint; }
            set { _is_first_checkpoint = value; this.Invalidate(); }
        }

        private bool _is_first_autorun_checkpoint;
        [STNodeProperty("is_first_autorun_checkpoint", "is_first_autorun_checkpoint")]
        public bool is_first_autorun_checkpoint
        {
            get { return _is_first_autorun_checkpoint; }
            set { _is_first_autorun_checkpoint = value; this.Invalidate(); }
        }

        private string _section;
        [STNodeProperty("section", "section")]
        public string section
        {
            get { return _section; }
            set { _section = value; this.Invalidate(); }
        }

        private float _mission_number;
        [STNodeProperty("mission_number", "mission_number")]
        public float mission_number
        {
            get { return _mission_number; }
            set { _mission_number = value; this.Invalidate(); }
        }

        private CHECKPOINT_TYPE _checkpoint_type;
        [STNodeProperty("checkpoint_type", "checkpoint_type")]
        public CHECKPOINT_TYPE checkpoint_type
        {
            get { return _checkpoint_type; }
            set { _checkpoint_type = value; this.Invalidate(); }
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            this.Title = "Checkpoint";

            this.OutputOptions.Add("on_checkpoint", typeof(void), false);
            this.OutputOptions.Add("on_captured", typeof(void), false);
            this.OutputOptions.Add("on_saved", typeof(void), false);
            this.OutputOptions.Add("finished_saving", typeof(void), false);
            this.OutputOptions.Add("finished_loading", typeof(void), false);
            this.OutputOptions.Add("cancelled_saving", typeof(void), false);
            this.OutputOptions.Add("finished_saving_to_hdd", typeof(void), false);

            this.InputOptions.Add("player_spawn_position", typeof(cTransform), false);

            this.InputOptions.Add("trigger", typeof(void), false);
            this.OutputOptions.Add("triggered", typeof(void), false);
        }
    }
}
