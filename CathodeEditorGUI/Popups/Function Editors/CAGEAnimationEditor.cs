using CATHODE.Scripting;
using CommandsEditor.Popups.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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

        public CAGEAnimationEditor(CommandsEditor editor, CAGEAnimation entity) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_CAGEANIM_EDITOR_OPENED | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor)
        {
            animEntity = entity.Copy();
            //File.WriteAllText("out.json", JsonConvert.SerializeObject(animEntity, Formatting.Indented));
            InitializeComponent();

            anim_length = CalculateAnimLength();
            Parameter anim_length_param = animEntity.GetParameter("anim_length");
            if (anim_length_param != null && anim_length_param.content != null)
            {
                float animLengthParam = ((cFloat)anim_length_param.content).value;
                if (animLengthParam > anim_length) anim_length = animLengthParam;
            }
            animLength.Text = anim_length.ToString();

            //TODO: if we don't already have an event track that is not in connections (e.g. triggered on us) we should make one

            UpdateEntityList();

            SetupAnimTimeline();
            SetupEventTimeline();

            this.BringToFront();
            this.Focus();
        }

        private void UpdateEntityList()
        {
            entityListToHierarchies = new List<EntityHierarchy>();
            entityList.BeginUpdate();
            entityList.Items.Clear();
            List<CAGEAnimation.Connection> connections = animEntity.connections.FindAll(o => o.objectType == ObjectType.ENTITY);
            foreach (CAGEAnimation.Connection connection in connections)
            {
                string connectionLink = connection.connectedEntity.GetHierarchyAsString(Editor.commands, Editor.selected.composite);
                if (!entityList.Items.Contains(connectionLink))
                {
                    entityList.Items.Add(connectionLink);
                    entityListToHierarchies.Add(connection.connectedEntity);
                }
            }
            entityList.EndUpdate();
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

        List<string> animTracks = new List<string>();
        private void SetupAnimTimeline()
        {
            keyframeHandlesAnim = new Dictionary<Keyframe, CAGEAnimation.Animation.Keyframe>();
            tracksAnim = new Dictionary<Track, CAGEAnimation.Animation>();

            activeAnimKeyframe = null;
            activeAnimHandle = null;
            animHost.Child = null;
            animKeyframeData.Visible = false;
            animTracks.Clear();

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
                CAGEAnimation.Connection connection = filteredConnections.FirstOrDefault(o => o.keyframeID == filteredAnims[i].shortGUID);
                for (int x = 0; x < filteredAnims[i].keyframes.Count; x++)
                {
                    CAGEAnimation.Animation.Keyframe keyframeData = filteredAnims[i].keyframes[x];
                    string keyframeText = connection.parameterSubID.val == null || connection.parameterSubID.ToString() == "" ? connection.parameterID.ToString() : connection.parameterID.ToString() + " [" + connection.parameterSubID.ToString() + "]";
                    Keyframe keyframeUI = animTimeline.AddKeyframe(keyframeData.secondsSinceStart, keyframeText);
                    keyframeUI.OnMoved += OnHandleMoved;
                    keyframeUI.HandleText = keyframeData.paramValue.ToString("0.00");
                    keyframeHandlesAnim.Add(keyframeUI, keyframeData);
                    animTracks.Add(keyframeText);
                    if (!tracksAnim.ContainsKey(keyframeUI.Track)) tracksAnim.Add(keyframeUI.Track, filteredAnims[i]);
                }
            }
            animHost.Child = animTimeline;
        }

        List<string> eventTracks = new List<string>();
        private void SetupEventTimeline()
        {
            keyframeHandlesEvent = new Dictionary<Keyframe, CAGEAnimation.Event.Keyframe>();
            tracksEvent = new Dictionary<Track, CAGEAnimation.Event>();

            activeEventKeyframe = null;
            activeEventHandle = null;
            eventHost.Child = null;
            eventKeyframeData.Visible = false;
            eventTracks.Clear();

            float anim_step = anim_length < 10.0f ? 1.0f : anim_length / 10.0f;

            Timeline eventTimeline = new Timeline(eventHost.Width, eventHost.Height);
            eventTimeline.OnNewKeyframe += OnKeyframeAddedToTrack_Event;
            eventTimeline.Setup(0, anim_length, anim_step, 150);
            for (int i = 0; i < animEntity.events.Count; i++)
            {
                //TODO: Frequently CHARACTER and MARKER objects both point to the same Event object - need to handle this better!
                CAGEAnimation.Connection connection = animEntity.connections.FirstOrDefault(o => o.keyframeID == animEntity.events[i].shortGUID);
                for (int x = 0; x < animEntity.events[i].keyframes.Count; x++)
                {
                    CAGEAnimation.Event.Keyframe keyframeData = animEntity.events[i].keyframes[x];
                    string keyframeText = (connection == null) ? EntityUtils.GetName(Editor.selected.composite, animEntity) : connection.connectedEntity.GetHierarchyAsString(Editor.commands, Editor.selected.composite, false);
                    Keyframe keyframeUI = eventTimeline.AddKeyframe(keyframeData.secondsSinceStart, keyframeText);
                    keyframeUI.OnMoved += OnHandleMoved;
                    keyframeHandlesEvent.Add(keyframeUI, keyframeData);
                    eventTracks.Add(keyframeText);
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
            EditHierarchy hierarchyEditor = new EditHierarchy(_editor, Editor.selected.composite, false);
            hierarchyEditor.Show();
            hierarchyEditor.OnHierarchyGenerated += HierarchyEditor_HierarchyGenerated;
        }
        private void HierarchyEditor_HierarchyGenerated(List<ShortGuid> generatedHierarchy)
        {
            EntityHierarchy hierarchy = new EntityHierarchy(generatedHierarchy);

            //Prevent the same entity being added again (doesn't make sense as the list is unique)
            for (int i = 0; i < entityListToHierarchies.Count; i++)
            {
                if (entityListToHierarchies[i] == hierarchy)
                {
                    entityList.SelectedIndex = i;
                    this.BringToFront();
                    this.Focus();
                    return;
                }
            }

            //Creating a placeholder here that points to nothing so that the dropdown will pick it up - not ideal, but shouldn't affect anything
            CAGEAnimation.Connection newConnection = new CAGEAnimation.Connection();
            newConnection.connectedEntity.hierarchy = generatedHierarchy;
            newConnection.objectType = ObjectType.ENTITY;
            animEntity.connections.Add(newConnection);

            UpdateEntityList();
            entityList.SelectedIndex = entityList.Items.Count - 1;
            this.BringToFront();
            this.Focus();
        }

        private void addAnimationTrack_Click(object sender, EventArgs e)
        {
            if (entityList.SelectedIndex == -1) return;
            try
            {
                CAGEAnimation_SelectParameter paramSelector = new CAGEAnimation_SelectParameter(_editor, entityListToHierarchies[entityList.SelectedIndex].GetPointedEntity(Editor.commands, Editor.selected.composite));
                paramSelector.OnParamSelected += OnParameterSelected;
                paramSelector.Show();
            }
            catch { }
        }
        private void OnParameterSelected(Parameter param)
        {
            //Make sure the same parameter isn't being added twice for the same entity
            if (animEntity.connections.FindAll(o => o.connectedEntity == entityListToHierarchies[entityList.SelectedIndex] && o.parameterID == param.name).Count != 0)
            {
                MessageBox.Show("This parameter is already controlled by the CAGEAnimation!", "Parameter already selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.BringToFront();
                this.Focus();
                return;
            }

            CAGEAnimation.Connection connection = new CAGEAnimation.Connection();
            connection.connectedEntity.hierarchy = entityListToHierarchies[entityList.SelectedIndex].hierarchy;
            connection.objectType = ObjectType.ENTITY;
            connection.parameterDataType = param.content.dataType;

            switch (param.content.dataType)
            {
                case DataType.FLOAT:
                    AddNewConnectionSet(connection.Copy(), ((cFloat)param.content).value, param.name);
                    break;
                case DataType.TRANSFORM:
                    string[] transformSubProperties = new string[6] { "x", "y", "z", "Yaw", "Pitch", "Roll" };
                    foreach (string subProp in transformSubProperties)
                    {
                        float val = 0;
                        switch (subProp)
                        {
                            case "x":
                                val = ((cTransform)param.content).position.X;
                                break;
                            case "y":
                                val = ((cTransform)param.content).position.Y;
                                break;
                            case "z":
                                val = ((cTransform)param.content).position.Z;
                                break;
                            case "Yaw":
                                val = ((cTransform)param.content).rotation.Y;
                                break;
                            case "Pitch":
                                val = ((cTransform)param.content).rotation.X;
                                break;
                            case "Roll":
                                val = ((cTransform)param.content).rotation.Z;
                                break;
                        }
                        AddNewConnectionSet(connection.Copy(), val, param.name, subProp);
                    }
                    break;
                //TODO: even though the base game doesn't use other datatypes in anims, we probably can!
                default:
                    MessageBox.Show("The datatype of the parameter you selected is not currently supported - please select either a FLOAT or TRANSFORM parameter to animate.", "Unsupported datatype", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            SetupAnimTimeline();
            this.BringToFront();
            this.Focus();
        }
        private void AddNewConnectionSet(CAGEAnimation.Connection conn, float defaultKeyValue, ShortGuid paramID, string subProp = "")
        {
            CAGEAnimation.Animation.Keyframe keyStart = new CAGEAnimation.Animation.Keyframe();
            keyStart.secondsSinceStart = 0.0f;
            keyStart.paramValue = defaultKeyValue;
            CAGEAnimation.Animation.Keyframe keyEnd = new CAGEAnimation.Animation.Keyframe();
            keyEnd.secondsSinceStart = anim_length;
            keyEnd.paramValue = defaultKeyValue;
            CAGEAnimation.Animation anim = new CAGEAnimation.Animation();
            anim.shortGUID = ShortGuidUtils.GenerateRandom();
            anim.keyframes.Add(keyStart);
            anim.keyframes.Add(keyEnd);
            animEntity.animations.Add(anim);
            conn.shortGUID = ShortGuidUtils.GenerateRandom();
            conn.parameterID = paramID;
            conn.parameterSubID = ShortGuidUtils.Generate(subProp);
            conn.keyframeID = anim.shortGUID;
            animEntity.connections.Add(conn);
        }

        private void deleteAnimationTrack_Click(object sender, EventArgs e)
        {
            try
            {
                CAGEAnimation_DeleteParam deleteParamWindow = new CAGEAnimation_DeleteParam(_editor, animTracks);
                deleteParamWindow.OnParamSelected += OnDeleteParamSelected;
                deleteParamWindow.Show();
            }
            catch { }
        }
        private void OnDeleteParamSelected(int index)
        {
            animEntity.animations.RemoveAt(index);
            SetupAnimTimeline();
        }

        private void addEventTrack_Click(object sender, EventArgs e)
        {
            EditHierarchy hierarchyEditor = new EditHierarchy(_editor, Editor.selected.composite, false);
            hierarchyEditor.Show();
            hierarchyEditor.OnHierarchyGenerated += HierarchyEditor2_HierarchyGenerated;
            SetupEventTimeline();
        }
        private void HierarchyEditor2_HierarchyGenerated(List<ShortGuid> generatedHierarchy)
        {
            EntityHierarchy hierarchy = new EntityHierarchy(generatedHierarchy);

            //TODO

            SetupEventTimeline();
            this.BringToFront();
            this.Focus();
        }

        private void deleteEventTrack_Click(object sender, EventArgs e)
        {
            try
            {
                CAGEAnimation_DeleteEvent deleteEventWindow = new CAGEAnimation_DeleteEvent(_editor, eventTracks);
                deleteEventWindow.OnTrackSelected += OnDeleteEventSelected;
                deleteEventWindow.Show();
            }
            catch { }
        }
        private void OnDeleteEventSelected(int index)
        {
            animEntity.events.RemoveAt(index);
            SetupEventTimeline();
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
