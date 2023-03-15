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

        Dictionary<Track, CAGEAnimation.Animation> tracksAnim;
        Dictionary<Track, CAGEAnimation.Event> tracksEvent;

        public CAGEAnimationEditor(CAGEAnimation entity) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            animEntity = entity.Copy();
            InitializeComponent();

            CalculateAnimLength();
            SetupTimeline();

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
            tracksAnim = new Dictionary<Track, CAGEAnimation.Animation>();
            tracksEvent = new Dictionary<Track, CAGEAnimation.Event>();

            activeAnimKeyframe = null;
            activeEventKeyframe = null;
            previousAnimHandle = null;
            previousEventHandle = null;

            animKeyframeData.Visible = false;
            eventKeyframeData.Visible = false;

            float anim_step = anim_length < 10.0f ? 1.0f : anim_length / 10.0f;

            Timeline animTimeline = new Timeline(animHost.Width, animHost.Height);
            animTimeline.OnNewKeyframe += OnKeyframeAddedToTrack_Anim;
            animTimeline.Setup(0, anim_length, anim_step, 150);
            for (int i = 0; i < animEntity.animations.Count; i++)
            {
                for (int x = 0; x < animEntity.animations[i].keyframes.Count; x++)
                {
                    CAGEAnimation.Animation.Keyframe keyframeData = animEntity.animations[i].keyframes[x];
                    Keyframe keyframeUI = animTimeline.AddKeyframe(keyframeData.secondsSinceStart, (i + 1).ToString());
                    keyframeUI.OnMoved += OnHandleMoved;
                    keyframeHandlesAnim.Add(keyframeUI, keyframeData);
                    if (!tracksAnim.ContainsKey(keyframeUI.Track)) tracksAnim.Add(keyframeUI.Track, animEntity.animations[i]);
                }
            }
            animHost.Child = animTimeline;

            Timeline eventTimeline = new Timeline(eventHost.Width, eventHost.Height);
            eventTimeline.OnNewKeyframe += OnKeyframeAddedToTrack_Event;
            eventTimeline.Setup(0, anim_length, anim_step, 150);
            for (int i = 0; i < animEntity.events.Count; i++)
            {
                for (int x = 0; x < animEntity.events[i].keyframes.Count; x++)
                {
                    CAGEAnimation.Event.Keyframe keyframeData = animEntity.events[i].keyframes[x];
                    Keyframe keyframeUI = eventTimeline.AddKeyframe(keyframeData.secondsSinceStart, (i + 1).ToString());
                    keyframeUI.OnMoved += OnHandleMoved;
                    keyframeHandlesEvent.Add(keyframeUI, keyframeData);
                    if (!tracksEvent.ContainsKey(keyframeUI.Track)) tracksEvent.Add(keyframeUI.Track, animEntity.events[i]);
                }
            }
            eventHost.Child = eventTimeline;
        }

        private void OnKeyframeAddedToTrack_Anim(Keyframe key)
        {
            CAGEAnimation.Animation e = tracksAnim[key.Track];
            CAGEAnimation.Animation.Keyframe keyData = new CAGEAnimation.Animation.Keyframe();
            keyData.secondsSinceStart = key.Seconds;
            e.keyframes.Add(keyData);
            keyframeHandlesAnim.Add(key, keyData);
            key.OnMoved += OnHandleMoved;
            Console.WriteLine("Added new entity anim keyframe");
        }
        private void OnKeyframeAddedToTrack_Event(Keyframe key)
        {
            CAGEAnimation.Event e = tracksEvent[key.Track];
            CAGEAnimation.Event.Keyframe keyData = new CAGEAnimation.Event.Keyframe();
            keyData.secondsSinceStart = key.Seconds;
            e.keyframes.Add(keyData);
            keyframeHandlesEvent.Add(key, keyData);
            key.OnMoved += OnHandleMoved;
            Console.WriteLine("Added new event keyframe");
        }

        CAGEAnimation.Animation.Keyframe activeAnimKeyframe = null;
        CAGEAnimation.Event.Keyframe activeEventKeyframe = null;
        Keyframe previousAnimHandle = null;
        Keyframe previousEventHandle = null;
        private void OnHandleMoved(Keyframe handle, float time)
        {
            if (keyframeHandlesAnim.ContainsKey(handle))
            {
                if (previousAnimHandle != null) previousAnimHandle.Highlight(false);
                handle.Highlight();
                previousAnimHandle = handle;

                activeAnimKeyframe = keyframeHandlesAnim[handle];
                activeAnimKeyframe.secondsSinceStart = time;
                animKeyframeData.Visible = true;
                animKeyframeValue.Text = activeAnimKeyframe.paramValue.ToString();
                startVelX.Text = activeAnimKeyframe.startVelocity.X.ToString();
                startVelY.Text = activeAnimKeyframe.startVelocity.Y.ToString();
                endVelX.Text = activeAnimKeyframe.endVelocity.X.ToString();
                endVelY.Text = activeAnimKeyframe.endVelocity.Y.ToString();
            }
            else if (keyframeHandlesEvent.ContainsKey(handle))
            {
                if (previousEventHandle != null) previousEventHandle.Highlight(false);
                handle.Highlight();
                previousEventHandle = handle;

                activeEventKeyframe = keyframeHandlesEvent[handle];
                activeEventKeyframe.secondsSinceStart = time;
                eventKeyframeData.Visible = true;
                eventParam1.Text = activeEventKeyframe.start.ToString();
                eventParam2.Text = activeEventKeyframe.unk3.ToString();
            }
            else
            {
                //WARNING: Invalid logic!
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
