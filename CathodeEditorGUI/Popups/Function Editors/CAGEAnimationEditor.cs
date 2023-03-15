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

        List<EntityHierarchy> entityListToHierarchies;

        public CAGEAnimationEditor(CAGEAnimation entity) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            animEntity = entity.Copy();
            InitializeComponent();

            anim_length = CalculateAnimLength();
            Parameter anim_length_param = animEntity.GetParameter("anim_length");
            if (anim_length_param != null && anim_length_param.content != null)
            {
                float animLengthParam = ((cFloat)anim_length_param.content).value;
                if (animLengthParam > anim_length) anim_length = animLengthParam;
            }
            animLength.Text = anim_length.ToString();

            UpdateEntityList();

            SetupAnimTimeline();
            SetupEventTimeline();

            this.BringToFront();
            this.Focus();
        }

        private void UpdateEntityList()
        {
            entityListToHierarchies = new List<EntityHierarchy>();
            entityList.Items.Clear();
            foreach (CAGEAnimation.Connection connection in animEntity.connections)
            {
                string connectionLink = connection.connectedEntity.GetHierarchyAsString(Editor.commands, Editor.selected.composite);
                if (!entityList.Items.Contains(connectionLink))
                {
                    entityList.Items.Add(connectionLink);
                    entityListToHierarchies.Add(connection.connectedEntity);
                }
            }
            entityList.SelectedIndex = (entityList.Items.Count < 1) ? -1 : 0;
        }

        public float CalculateAnimLength()
        {
            float animLength = 0.0f;
            for (int i = 0; i < animEntity.animations.Count; i++)
            {
                for (int x = 0; x < animEntity.animations[i].keyframes.Count; x++)
                {
                    if (animLength < animEntity.animations[i].keyframes[x].secondsSinceStart)
                        animLength = animEntity.animations[i].keyframes[x].secondsSinceStart;
                }
            }
            for (int i = 0; i < animEntity.events.Count; i++)
            {
                for (int x = 0; x < animEntity.events[i].keyframes.Count; x++)
                {
                    if (animLength < animEntity.events[i].keyframes[x].secondsSinceStart)
                        animLength = animEntity.events[i].keyframes[x].secondsSinceStart;
                }
            }
            return animLength;
        }

        private void SetupAnimTimeline()
        {
            keyframeHandlesAnim = new Dictionary<Keyframe, CAGEAnimation.Animation.Keyframe>();
            tracksAnim = new Dictionary<Track, CAGEAnimation.Animation>();

            activeAnimKeyframe = null;
            activeAnimHandle = null;
            animHost.Child = null;
            animKeyframeData.Visible = false;

            //Filter down anims to the selected entity in the dropdown
            if (entityList.SelectedIndex == -1) return;
            List<CAGEAnimation.Animation> filteredAnims = new List<CAGEAnimation.Animation>();
            List<CAGEAnimation.Connection> filteredConnections = animEntity.connections.FindAll(o => o.connectedEntity == entityListToHierarchies[entityList.SelectedIndex]);
            for (int i = 0; i < filteredConnections.Count; i++)
            {
                CAGEAnimation.Animation anim = animEntity.animations.FirstOrDefault(o => o.shortGUID == filteredConnections[i].keyframeID);
                if (anim != null) filteredAnims.Add(anim);
            }

            float anim_step = anim_length < 10.0f ? 1.0f : anim_length / 10.0f;

            Timeline animTimeline = new Timeline(animHost.Width, animHost.Height);
            animTimeline.OnNewKeyframe += OnKeyframeAddedToTrack_Anim;
            animTimeline.Setup(0, anim_length, anim_step, 150);
            for (int i = 0; i < filteredAnims.Count; i++)
            {
                for (int x = 0; x < filteredAnims[i].keyframes.Count; x++)
                {
                    CAGEAnimation.Animation.Keyframe keyframeData = filteredAnims[i].keyframes[x];
                    Keyframe keyframeUI = animTimeline.AddKeyframe(keyframeData.secondsSinceStart, (i + 1).ToString());
                    keyframeUI.OnMoved += OnHandleMoved;
                    keyframeUI.HandleText = keyframeData.paramValue.ToString("0.00");
                    keyframeHandlesAnim.Add(keyframeUI, keyframeData);
                    if (!tracksAnim.ContainsKey(keyframeUI.Track)) tracksAnim.Add(keyframeUI.Track, filteredAnims[i]);
                }
            }
            animHost.Child = animTimeline;
        }

        private void SetupEventTimeline()
        {
            keyframeHandlesEvent = new Dictionary<Keyframe, CAGEAnimation.Event.Keyframe>();
            tracksEvent = new Dictionary<Track, CAGEAnimation.Event>();

            activeEventKeyframe = null;
            activeEventHandle = null;
            eventHost.Child = null;
            eventKeyframeData.Visible = false;

            float anim_step = anim_length < 10.0f ? 1.0f : anim_length / 10.0f;

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
        }
        private void OnKeyframeAddedToTrack_Event(Keyframe key)
        {
            CAGEAnimation.Event e = tracksEvent[key.Track];
            CAGEAnimation.Event.Keyframe keyData = new CAGEAnimation.Event.Keyframe();
            keyData.secondsSinceStart = key.Seconds;
            e.keyframes.Add(keyData);
            keyframeHandlesEvent.Add(key, keyData);
            key.OnMoved += OnHandleMoved;
        }

        CAGEAnimation.Animation.Keyframe activeAnimKeyframe = null;
        CAGEAnimation.Event.Keyframe activeEventKeyframe = null;
        Keyframe activeAnimHandle = null;
        Keyframe activeEventHandle = null;
        private void OnHandleMoved(Keyframe handle, float time)
        {
            if (keyframeHandlesAnim.ContainsKey(handle))
            {
                if (activeAnimHandle != null) activeAnimHandle.Highlight(false);
                handle.Highlight();
                activeAnimHandle = handle;

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
                if (activeEventHandle != null) activeEventHandle.Highlight(false);
                handle.Highlight();
                activeEventHandle = handle;

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

        private void deleteAnimKeyframe_Click(object sender, EventArgs e)
        {
            tracksAnim[activeAnimHandle.Track].keyframes.Remove(activeAnimKeyframe);
            ((Timeline)animHost.Child).RemoveKeyframe(activeAnimHandle);
            activeAnimHandle = null;
            animKeyframeData.Visible = false;
        }
        private void deleteEventKeyframe_Click(object sender, EventArgs e)
        {
            tracksEvent[activeEventHandle.Track].keyframes.Remove(activeEventKeyframe);
            ((Timeline)eventHost.Child).RemoveKeyframe(activeEventHandle);
            activeEventHandle = null;
            eventKeyframeData.Visible = false;
        }

        private void addNewEntityRef_Click(object sender, EventArgs e)
        {
            EditHierarchy hierarchyEditor = new EditHierarchy(Editor.selected.composite, false);
            hierarchyEditor.Show();
            hierarchyEditor.OnHierarchyGenerated += HierarchyEditor_HierarchyGenerated;
        }
        private void HierarchyEditor_HierarchyGenerated(List<ShortGuid> generatedHierarchy)
        {
            //TODO: creating a placeholder here that points to nothing so that the dropdown will pick it up - not ideal, but shouldn't affect in-game
            CAGEAnimation.Connection connection = new CAGEAnimation.Connection();
            connection.connectedEntity.hierarchy = generatedHierarchy;
            connection.objectType = ObjectType.ENTITY;
            animEntity.connections.Add(connection);

            UpdateEntityList();
            entityList.SelectedIndex = entityList.Items.Count - 1;
        }

        private void addAnimationTrack_Click(object sender, EventArgs e)
        {

        }
        private void deleteAnimationTrack_Click(object sender, EventArgs e)
        {

        }

        private void addEventTrack_Click(object sender, EventArgs e)
        {

        }
        private void deleteEventTrack_Click(object sender, EventArgs e)
        {

        }

        private void SaveEntity_Click(object sender, EventArgs e)
        {
            animEntity.AddParameter("anim_length", new cFloat(anim_length));
            OnSaved?.Invoke(animEntity);
            this.Close();
        }

        private void animKeyframeValue_TextChanged(object sender, EventArgs e)
        {
            animKeyframeValue.Text = EditorUtils.ForceStringNumeric(animKeyframeValue.Text, true);
            activeAnimKeyframe.paramValue = Convert.ToSingle(animKeyframeValue.Text);
            activeAnimHandle.HandleText = activeAnimKeyframe.paramValue.ToString("0.00");
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

        private void entityList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupAnimTimeline();
        }

        private void animLength_TextChanged(object sender, EventArgs e)
        {
            animLength.Text = EditorUtils.ForceStringNumeric(animLength.Text, true);
        }
        private void editAnimLength_Click(object sender, EventArgs e)
        {
            animLength.Text = EditorUtils.ForceStringNumeric(animLength.Text, true);
            float newAnimLength = Convert.ToSingle(animLength.Text);

            //Validate no keyframes are below the new length
            float actualAnimLength = CalculateAnimLength();
            if (actualAnimLength > newAnimLength)
            {
                MessageBox.Show("There are keyframes that are placed beyond the new animation length.\nPlease move these keyframes within range before updating the length!", "Actual animation exceeds requested length!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                animLength.Text = anim_length.ToString();
                return;
            }

            anim_length = newAnimLength;
            SetupAnimTimeline();
            SetupEventTimeline();
        }
    }
}
