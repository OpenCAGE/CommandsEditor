using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.Popups.Base;
using CommandsEditor.Popups.UserControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimelineFramework;

namespace CommandsEditor
{
    public partial class CAGEAnimationEditor : BaseWindow
    {
        public Action<CAGEAnimation> OnSaved;

        float anim_length = 0;
        CAGEAnimation animEntity = null; //this is a unique instance we can write to

        Dictionary<Keyframe, CAGEAnimation.Animation.Keyframe> keyframeHandlesAnim;
        Dictionary<Keyframe, CAGEAnimation.Event.Keyframe> keyframeHandlesEvent;
        CurrentDisplay currentDisplay = CurrentDisplay.ANIMATION_KEYFRAMES;

        public CAGEAnimationEditor(CAGEAnimation entity) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            animEntity = entity.Copy();
            InitializeComponent();

            displaySelection.SelectedIndex = 0;

#if DEBUG
            Parameter anim_length_param = animEntity.GetParameter("anim_length");
            if (anim_length_param != null && anim_length_param.content != null)
            {
                if (((cFloat)anim_length_param.content).value != anim_length)
                    Console.WriteLine("WARNING: CAGEAnimation 'anim_length' does not match calculated length based on keyframes!");
            }
#endif

            this.BringToFront();
            this.Focus();
        }

        private void displaySelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentDisplay = (CurrentDisplay)displaySelection.SelectedIndex;

            CalculateAnimLength();
            SetupTimeline();
        }

        public void CalculateAnimLength()
        {
            for (int i = 0; i < animEntity.animations.Count; i++)
            {
                for (int x = 0; x < animEntity.animations[i].keyframes.Count; x++)
                {
                    if (anim_length < animEntity.animations[i].keyframes[x].secondsSinceStart)
                        anim_length = animEntity.animations[i].keyframes[x].secondsSinceStart;
                }
            }
            for (int i = 0; i < animEntity.events.Count; i++)
            {
                for (int x = 0; x < animEntity.events[i].keyframes.Count; x++)
                {
                    if (anim_length < animEntity.events[i].keyframes[x].secondsSinceStart)
                        anim_length = animEntity.events[i].keyframes[x].secondsSinceStart;
                }
            }
        }

        private void SetupTimeline()
        {
            keyframeHandlesAnim = new Dictionary<Keyframe, CAGEAnimation.Animation.Keyframe>();
            keyframeHandlesEvent = new Dictionary<Keyframe, CAGEAnimation.Event.Keyframe>();

            activeAnimKeyframe = null;
            activeEventKeyframe = null;
            previousKeyframeUI = null;

            Timeline timeline = new Timeline(animHost.Width, animHost.Height);
            timeline.Setup(0, anim_length, anim_length / 10.0f, 150);
            switch (currentDisplay)
            {
                case CurrentDisplay.ANIMATION_KEYFRAMES:
                    for (int i = 0; i < animEntity.animations.Count; i++)
                    {
                        for (int x = 0; x < animEntity.animations[i].keyframes.Count; x++)
                        {
                            CAGEAnimation.Animation.Keyframe keyframeData = animEntity.animations[i].keyframes[x];
                            Keyframe keyframeUI = timeline.AddKeyframe(keyframeData.secondsSinceStart, i + 1);
                            keyframeUI.OnMoved += OnHandleMoved;
                            keyframeHandlesAnim.Add(keyframeUI, keyframeData);
                        }
                    }
                    break;
                case CurrentDisplay.EVENT_KEYFRAMES:
                    for (int i = 0; i < animEntity.events.Count; i++)
                    {
                        for (int x = 0; x < animEntity.events[i].keyframes.Count; x++)
                        {
                            CAGEAnimation.Event.Keyframe keyframeData = animEntity.events[i].keyframes[x];
                            Keyframe keyframeUI = timeline.AddKeyframe(keyframeData.secondsSinceStart, i + 1);
                            keyframeUI.OnMoved += OnHandleMoved;
                            keyframeHandlesEvent.Add(keyframeUI, keyframeData);
                        }
                    }
                    break;
            }
            animHost.Child = timeline;
        }

        CAGEAnimation.Animation.Keyframe activeAnimKeyframe = null;
        CAGEAnimation.Event.Keyframe activeEventKeyframe = null;
        Keyframe previousKeyframeUI = null;
        private void OnHandleMoved(Keyframe handle, float time)
        {
            animKeyframeData.Visible = currentDisplay == CurrentDisplay.ANIMATION_KEYFRAMES;
            eventKeyframeData.Visible = currentDisplay == CurrentDisplay.EVENT_KEYFRAMES;

            if (previousKeyframeUI != null) previousKeyframeUI.Highlight(false);
            handle.Highlight();
            previousKeyframeUI = handle;

            switch (currentDisplay)
            {
                case CurrentDisplay.ANIMATION_KEYFRAMES:
                    activeAnimKeyframe = keyframeHandlesAnim[handle];
                    activeAnimKeyframe.secondsSinceStart = time;
                    animKeyframeValue.Text = activeAnimKeyframe.paramValue.ToString();
                    startVelX.Text = activeAnimKeyframe.startVelocity.X.ToString();
                    startVelY.Text = activeAnimKeyframe.startVelocity.Y.ToString();
                    endVelX.Text = activeAnimKeyframe.endVelocity.X.ToString();
                    endVelY.Text = activeAnimKeyframe.endVelocity.Y.ToString();
                    break;
                case CurrentDisplay.EVENT_KEYFRAMES:
                    activeEventKeyframe = keyframeHandlesEvent[handle];
                    activeEventKeyframe.secondsSinceStart = time;
                    eventParam1.Text = activeEventKeyframe.start.ToString();
                    eventParam2.Text = activeEventKeyframe.unk3.ToString();
                    break;
            }
        }

        private void SaveEntity_Click(object sender, EventArgs e)
        {
            CalculateAnimLength();
            animEntity.AddParameter("anim_length", new cFloat(anim_length));
            OnSaved?.Invoke(animEntity);
            this.Close();
        }

        private enum CurrentDisplay
        {
            ANIMATION_KEYFRAMES,
            EVENT_KEYFRAMES,
        }

        private void animKeyframeValue_TextChanged(object sender, EventArgs e)
        {
            animKeyframeValue.Text = EditorUtils.ForceStringNumeric(animKeyframeValue.Text, true);
            activeAnimKeyframe.paramValue = Convert.ToSingle(animKeyframeValue.Text);
        }
        private void startVelX_TextChanged(object sender, EventArgs e)
        {
            startVelX.Text = EditorUtils.ForceStringNumeric(startVelX.Text, true);
            activeAnimKeyframe.startVelocity.X = Convert.ToSingle(startVelX.Text);
        }
        private void startVelY_TextChanged(object sender, EventArgs e)
        {
            startVelY.Text = EditorUtils.ForceStringNumeric(startVelY.Text, true);
            activeAnimKeyframe.startVelocity.Y = Convert.ToSingle(startVelY.Text);
        }
        private void endVelX_TextChanged(object sender, EventArgs e)
        {
            endVelX.Text = EditorUtils.ForceStringNumeric(endVelX.Text, true);
            activeAnimKeyframe.endVelocity.X = Convert.ToSingle(endVelX.Text);
        }
        private void endVelY_TextChanged(object sender, EventArgs e)
        {
            endVelY.Text = EditorUtils.ForceStringNumeric(endVelY.Text, true);
            activeAnimKeyframe.endVelocity.Y = Convert.ToSingle(endVelY.Text);
        }

        private void eventParam1_TextChanged(object sender, EventArgs e)
        {
            //Handle non-convertable param names
            if (activeEventKeyframe.start.ToByteString() == eventParam1.Text) return;
            activeEventKeyframe.start = ShortGuidUtils.Generate(eventParam1.Text);
        }
        private void eventParam2_TextChanged(object sender, EventArgs e)
        {
            //Handle non-convertable param names
            if (activeEventKeyframe.unk3.ToByteString() == eventParam2.Text) return;
            activeEventKeyframe.unk3 = ShortGuidUtils.Generate(eventParam2.Text);
        }
    }
}
