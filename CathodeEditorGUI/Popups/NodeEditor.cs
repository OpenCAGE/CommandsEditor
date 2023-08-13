using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ST.Library.UI.NodeEditor;
using CATHODE.Scripting.Internal;
using CATHODE.Scripting;
using CommandsEditor.UserControls;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CommandsEditor.Popups.Base;
using WebSocketSharp;
using System.Security.Cryptography;
using CommandsEditor.Nodes;
using CommandsEditor.DockPanels;

namespace CommandsEditor
{
    public partial class NodeEditor : BaseWindow
    {
        EntityDisplay _entityDisplay;

        public NodeEditor(EntityDisplay entityDisplay) : base(WindowClosesOn.NONE, entityDisplay.Content)
        {
            _entityDisplay = entityDisplay;

            InitializeComponent();
            AddEntities(entityDisplay.Composite, entityDisplay.Entity);
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            stNodeEditor1.LoadAssembly(Application.ExecutablePath);

            //stNodeEditor1.OptionConnected += (s, ea) => stNodeEditor1.ShowAlert(ea.Status.ToString(), Color.White, ea.Status == ConnectionStatus.Connected ? Color.FromArgb(125, Color.Green) : Color.FromArgb(125, Color.Red));
            //stNodeEditor1.CanvasScaled += (s, ea) => stNodeEditor1.ShowAlert(stNodeEditor1.CanvasScale.ToString("F2"), Color.White, Color.FromArgb(125, Color.Yellow));
            //stNodeEditor1.NodeAdded += (s, ea) => ea.Node.ContextMenuStrip = contextMenuStrip1;
            stNodeEditor1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

            //contextMenuStrip1.ShowImageMargin = false;
            //contextMenuStrip1.Renderer = new ToolStripRendererEx();
        }

        public void AddEntities(Composite composite, Entity entity)
        {
            stNodeEditor1.Nodes.Clear();
            if (composite == null || entity == null) return;

            CustomNode mainNode = EntityToNode(entity, composite);
            stNodeEditor1.Nodes.Add(mainNode);
            
            //Generate input nodes
            List<Entity> ents = composite.GetEntities();
            List<CustomNode> inputNodes = new List<CustomNode>();
            foreach (Entity ent in ents)
            {
                foreach (EntityLink link in ent.childLinks)
                {
                    if (link.childID != entity.shortGUID) continue;
                    CustomNode node = null;
                    for (int i = 0; i < stNodeEditor1.Nodes.Count; i++)
                    {
                        if (((CustomNode)stNodeEditor1.Nodes[i]).ID == ent.shortGUID)
                        {
                            node = (CustomNode)stNodeEditor1.Nodes[i];
                            break;
                        }
                    }
                    if (node == null)
                    {
                        node = EntityToNode(ent, composite);
                        inputNodes.Add(node);
                        stNodeEditor1.Nodes.Add(node);
                    }
                    STNodeOption opt1 = node.AddOutputOption(link.parentParamID.ToString());
                    STNodeOption opt2 = mainNode.AddInputOption(link.childParamID.ToString());
                    opt1.ConnectOption(opt2);
                }
            }

            //Generate output nodes
            List<CustomNode> outputNodes = new List<CustomNode>();
            foreach (EntityLink link in entity.childLinks)
            {
                CustomNode node = null;
                for (int i = 0; i < stNodeEditor1.Nodes.Count; i++)
                {
                    if (((CustomNode)stNodeEditor1.Nodes[i]).ID == link.childID)
                    {
                        node = (CustomNode)stNodeEditor1.Nodes[i];
                        break;
                    }
                }
                if (node == null)
                {
                    node = EntityToNode(composite.GetEntityByID(link.childID), composite);
                    outputNodes.Add(node);
                    stNodeEditor1.Nodes.Add(node);
                }
                STNodeOption opt1 = node.AddInputOption(link.childParamID.ToString());
                STNodeOption opt2 = mainNode.AddOutputOption(link.parentParamID.ToString());
                opt1.ConnectOption(opt2);
            }

            //Compute node sizes
            foreach (STNode node in stNodeEditor1.Nodes)
                ((CustomNode)node).Recompute();
            int inputStackedHeight = 0;
            foreach (STNode node in inputNodes)
                inputStackedHeight += node.Height + 10;
            int outputStackedHeight = 0;
            foreach (STNode node in outputNodes)
                outputStackedHeight += node.Height + 10;

            //Set node positions
            int height = (this.Size.Height / 2) - (inputStackedHeight / 2) - 20;
            foreach (STNode node in inputNodes)
            {
                ((CustomNode)node).SetPosition(new Point(10, height));
                height += node.Height + 10;
            }
            height = (this.Size.Height / 2) - (outputStackedHeight / 2) - 20;
            foreach (STNode node in outputNodes)
            {
                ((CustomNode)node).SetPosition(new Point(this.Size.Width - node.Width - 50, height));
                height += node.Height + 10;
            }
            mainNode.SetPosition(new Point((this.Size.Width / 2) - (mainNode.Width / 2) - 10, (this.Size.Height / 2) - (((outputStackedHeight > inputStackedHeight) ? outputStackedHeight : inputStackedHeight) / 2) - 20));
       
            //Lock options for now
            foreach (STNode node in stNodeEditor1.Nodes)
                node.LockOption = true;

            stNodeEditor1.SelectedChanged += Owner_SelectedChanged;
        }

        private void Owner_SelectedChanged(object sender, EventArgs e)
        {
            if (!clickToSelect.Checked) return;

            //when a node is selected, load it in the commands editor
            STNode[] nodes = stNodeEditor1.GetSelectedNode();
            if (nodes.Length == 0) return;

            //TODO: IMPLEMENT THIS
            //_editor.LoadEntity(((CustomNode)nodes[0]).ID);
        }

        private CustomNode EntityToNode(Entity entity, Composite composite)
        {
            CustomNode node = new CustomNode();
            node.ID = entity.shortGUID;
            switch (entity.variant)
            {
                case EntityVariant.PROXY:
                case EntityVariant.OVERRIDE:
                    Entity ent = CommandsUtils.ResolveHierarchy(Editor.commands, composite, (entity.variant == EntityVariant.PROXY) ? ((ProxyEntity)entity).connectedEntity.hierarchy : ((OverrideEntity)entity).connectedEntity.hierarchy, out Composite c, out string s);
                    node.SetColour(entity.variant == EntityVariant.PROXY ? Color.LightGreen : Color.Orange, Color.Black);
                    switch (ent.variant)
                    {
                        case EntityVariant.FUNCTION:
                            FunctionEntity function = (FunctionEntity)ent;
                            if (CommandsUtils.FunctionTypeExists(function.function))
                            {
                                node.SetName(entity.variant + " TO: " + function.function.ToString() + "\n" + EntityUtils.GetName(c, ent), 35);
                            }
                            else
                                node.SetName(entity.variant + " TO: " + Editor.commands.GetComposite(function.function).name + "\n" + EntityUtils.GetName(c, ent), 35);
                            break;
                        case EntityVariant.VARIABLE:
                            node.SetName(entity.variant + " TO: " + ((VariableEntity)ent).name.ToString());
                            break;
                    }
                    break;
                case EntityVariant.FUNCTION:
                    FunctionEntity funcEnt = (FunctionEntity)entity;
                    if (CommandsUtils.FunctionTypeExists(funcEnt.function))
                    {
                        node.SetName(funcEnt.function.ToString() + "\n" + EntityUtils.GetName(composite, entity), 35);
                    }
                    else
                    {
                        node.SetColour(Color.Blue, Color.White);
                        node.SetName(Editor.commands.GetComposite(funcEnt.function).name + "\n" + EntityUtils.GetName(composite, entity), 35);
                    }
                    break;
                case EntityVariant.VARIABLE:
                    node.SetColour(Color.Red, Color.White);
                    node.SetName(((VariableEntity)entity).name.ToString());
                    break;
            }
            node.Recompute();
            return node;
        }

        private STNode NodeFromString(string node)
        {
            switch (node)
            {
                case "AccessTerminal":
                    return new AccessTerminal();
                case "AchievementMonitor":
                    return new AchievementMonitor();
                case "AchievementStat":
                    return new AchievementStat();
                case "AchievementUniqueCounter":
                    return new AchievementUniqueCounter();
                case "AddExitObjective":
                    return new AddExitObjective();
                case "AddItemsToGCPool":
                    return new AddItemsToGCPool();
                case "AddToInventory":
                    return new AddToInventory();
                case "AILightCurveSettings":
                    return new AILightCurveSettings();
                case "AIMED_ITEM":
                    return new AIMED_ITEM();
                case "AIMED_WEAPON":
                    return new AIMED_WEAPON();
                case "ALLIANCE_ResetAll":
                    return new ALLIANCE_ResetAll();
                case "ALLIANCE_SetDisposition":
                    return new ALLIANCE_SetDisposition();
                case "AllocateGCItemFromPoolBySubset":
                    return new AllocateGCItemFromPoolBySubset();
                case "AllocateGCItemsFromPool":
                    return new AllocateGCItemsFromPool();
                case "AllPlayersReady":
                    return new AllPlayersReady();
                case "AnimatedModelAttachmentNode":
                    return new AnimatedModelAttachmentNode();
                case "AnimationMask":
                    return new AnimationMask();
                case "ApplyRelativeTransform":
                    return new ApplyRelativeTransform();
                case "AreaHitMonitor":
                    return new AreaHitMonitor();
                case "AssetSpawner":
                    return new AssetSpawner();
                case "Benchmark":
                    return new Benchmark();
                case "BindObjectsMultiplexer":
                    return new BindObjectsMultiplexer();
                case "BlendLowResFrame":
                    return new BlendLowResFrame();
                case "BloomSettings":
                    return new BloomSettings();
                case "BoneAttachedCamera":
                    return new BoneAttachedCamera();
                case "BooleanLogicOperation":
                    return new BooleanLogicOperation();
                case "Box":
                    return new Box();
                case "BroadcastTrigger":
                    return new BroadcastTrigger();
                case "BulletChamber":
                    return new BulletChamber();
                case "ButtonMashPrompt":
                    return new ButtonMashPrompt();
                case "CAGEAnimation":
                    return new Nodes.CAGEAnimation();
                case "CameraAimAssistant":
                    return new CameraAimAssistant();
                case "CameraCollisionBox":
                    return new CameraCollisionBox();
                case "CameraDofController":
                    return new CameraDofController();
                case "CameraFinder":
                    return new CameraFinder();
                case "CameraPath":
                    return new CameraPath();
                case "CameraPathDriven":
                    return new CameraPathDriven();
                case "CameraPlayAnimation":
                    return new CameraPlayAnimation();
                case "CameraResource":
                    return new CameraResource();
                case "CameraShake":
                    return new CameraShake();
                case "CamPeek":
                    return new CamPeek();
                case "Character":
                    return new Character();
                case "CharacterAttachmentNode":
                    return new CharacterAttachmentNode();
                case "CharacterCommand":
                    return new CharacterCommand();
                case "CharacterMonitor":
                    return new CharacterMonitor();
                case "CharacterShivaArms":
                    return new CharacterShivaArms();
                case "CharacterTypeMonitor":
                    return new CharacterTypeMonitor();
                case "Checkpoint":
                    return new Checkpoint();
                case "CheckpointRestoredNotify":
                    return new CheckpointRestoredNotify();
                case "ChokePoint":
                    return new ChokePoint();
                case "CHR_DamageMonitor":
                    return new CHR_DamageMonitor();
                case "CHR_DeathMonitor":
                    return new CHR_DeathMonitor();
                case "CHR_DeepCrouch":
                    return new CHR_DeepCrouch();
                case "CHR_GetAlliance":
                    return new CHR_GetAlliance();
                case "CHR_GetHealth":
                    return new CHR_GetHealth();
                case "CHR_GetTorch":
                    return new CHR_GetTorch();
                case "CHR_HasWeaponOfType":
                    return new CHR_HasWeaponOfType();
                case "CHR_HoldBreath":
                    return new CHR_HoldBreath();
                case "CHR_IsWithinRange":
                    return new CHR_IsWithinRange();
                case "CHR_KnockedOutMonitor":
                    return new CHR_KnockedOutMonitor();
                case "CHR_LocomotionDuck":
                    return new CHR_LocomotionDuck();
                case "CHR_LocomotionEffect":
                    return new CHR_LocomotionEffect();
                case "CHR_LocomotionModifier":
                    return new CHR_LocomotionModifier();
                case "CHR_ModifyBreathing":
                    return new CHR_ModifyBreathing();
                case "Chr_PlayerCrouch":
                    return new Chr_PlayerCrouch();
                case "CHR_PlayNPCBark":
                    return new CHR_PlayNPCBark();
                case "CHR_PlaySecondaryAnimation":
                    return new CHR_PlaySecondaryAnimation();
                case "CHR_RetreatMonitor":
                    return new CHR_RetreatMonitor();
                case "CHR_SetAlliance":
                    return new CHR_SetAlliance();
                case "CHR_SetAndroidThrowTarget":
                    return new CHR_SetAndroidThrowTarget();
                case "CHR_SetDebugDisplayName":
                    return new CHR_SetDebugDisplayName();
                case "CHR_SetFacehuggerAggroRadius":
                    return new CHR_SetFacehuggerAggroRadius();
                case "CHR_SetFocalPoint":
                    return new CHR_SetFocalPoint();
                case "CHR_SetHeadVisibility":
                    return new CHR_SetHeadVisibility();
                case "CHR_SetHealth":
                    return new CHR_SetHealth();
                case "CHR_SetInvincibility":
                    return new CHR_SetInvincibility();
                case "CHR_SetMood":
                    return new CHR_SetMood();
                case "CHR_SetShowInMotionTracker":
                    return new CHR_SetShowInMotionTracker();
                case "CHR_SetSubModelVisibility":
                    return new CHR_SetSubModelVisibility();
                case "CHR_SetTacticalPosition":
                    return new CHR_SetTacticalPosition();
                case "CHR_SetTacticalPositionToTarget":
                    return new CHR_SetTacticalPositionToTarget();
                case "CHR_SetTorch":
                    return new CHR_SetTorch();
                case "CHR_TakeDamage":
                    return new CHR_TakeDamage();
                case "CHR_TorchMonitor":
                    return new CHR_TorchMonitor();
                case "CHR_VentMonitor":
                    return new CHR_VentMonitor();
                case "CHR_WeaponFireMonitor":
                    return new CHR_WeaponFireMonitor();
                case "ChromaticAberrations":
                    return new ChromaticAberrations();
                case "ClearPrimaryObjective":
                    return new ClearPrimaryObjective();
                case "ClearSubObjective":
                    return new ClearSubObjective();
                case "ClipPlanesController":
                    return new ClipPlanesController();
                case "CMD_AimAt":
                    return new CMD_AimAt();
                case "CMD_AimAtCurrentTarget":
                    return new CMD_AimAtCurrentTarget();
                case "CMD_Die":
                    return new CMD_Die();
                case "CMD_Follow":
                    return new CMD_Follow();
                case "CMD_FollowUsingJobs":
                    return new CMD_FollowUsingJobs();
                case "CMD_ForceMeleeAttack":
                    return new CMD_ForceMeleeAttack();
                case "CMD_ForceReloadWeapon":
                    return new CMD_ForceReloadWeapon();
                case "CMD_GoTo":
                    return new CMD_GoTo();
                case "CMD_GoToCover":
                    return new CMD_GoToCover();
                case "CMD_HolsterWeapon":
                    return new CMD_HolsterWeapon();
                case "CMD_Idle":
                    return new CMD_Idle();
                case "CMD_LaunchMeleeAttack":
                    return new CMD_LaunchMeleeAttack();
                case "CMD_ModifyCombatBehaviour":
                    return new CMD_ModifyCombatBehaviour();
                case "CMD_MoveTowards":
                    return new CMD_MoveTowards();
                case "CMD_PlayAnimation":
                    return new CMD_PlayAnimation();
                case "CMD_Ragdoll":
                    return new CMD_Ragdoll();
                case "CMD_ShootAt":
                    return new CMD_ShootAt();
                case "CMD_StopScript":
                    return new CMD_StopScript();
                case "CollectIDTag":
                    return new CollectIDTag();
                case "CollectNostromoLog":
                    return new CollectNostromoLog();
                case "CollectSevastopolLog":
                    return new CollectSevastopolLog();
                case "CollisionBarrier":
                    return new CollisionBarrier();
                case "ColourCorrectionTransition":
                    return new ColourCorrectionTransition();
                case "ColourSettings":
                    return new ColourSettings();
                case "CompositeInterface":
                    return new CompositeInterface();
                case "CompoundVolume":
                    return new CompoundVolume();
                case "ControllableRange":
                    return new ControllableRange();
                case "Convo":
                    return new Convo();
                case "Counter":
                    return new Counter();
                case "CoverExclusionArea":
                    return new CoverExclusionArea();
                case "CoverLine":
                    return new CoverLine();
                case "Custom_Hiding_Controller":
                    return new Custom_Hiding_Controller();
                case "Custom_Hiding_Vignette_controller":
                    return new Custom_Hiding_Vignette_controller();
                case "DayToneMappingSettings":
                    return new DayToneMappingSettings();
                case "DEBUG_SenseLevels":
                    return new DEBUG_SenseLevels();
                case "DebugCamera":
                    return new DebugCamera();
                case "DebugCaptureCorpse":
                    return new DebugCaptureCorpse();
                case "DebugCaptureScreenShot":
                    return new DebugCaptureScreenShot();
                case "DebugCheckpoint":
                    return new DebugCheckpoint();
                case "DebugEnvironmentMarker":
                    return new DebugEnvironmentMarker();
                case "DebugGraph":
                    return new DebugGraph();
                case "DebugLoadCheckpoint":
                    return new DebugLoadCheckpoint();
                case "DebugMenuToggle":
                    return new DebugMenuToggle();
                case "DebugObjectMarker":
                    return new DebugObjectMarker();
                case "DebugPositionMarker":
                    return new DebugPositionMarker();
                case "DebugText":
                    return new DebugText();
                case "DebugTextStacking":
                    return new DebugTextStacking();
                case "DeleteBlankPanel":
                    return new DeleteBlankPanel();
                case "DeleteButtonDisk":
                    return new DeleteButtonDisk();
                case "DeleteButtonKeys":
                    return new DeleteButtonKeys();
                case "DeleteCuttingPanel":
                    return new DeleteCuttingPanel();
                case "DeleteHacking":
                    return new DeleteHacking();
                case "DeleteHousing":
                    return new DeleteHousing();
                case "DeleteKeypad":
                    return new DeleteKeypad();
                case "DeletePullLever":
                    return new DeletePullLever();
                case "DeleteRotateLever":
                    return new DeleteRotateLever();
                case "DepthOfFieldSettings":
                    return new DepthOfFieldSettings();
                case "DespawnCharacter":
                    return new DespawnCharacter();
                case "DespawnPlayer":
                    return new DespawnPlayer();
                case "Display_Element_On_Map":
                    return new Display_Element_On_Map();
                case "DisplayMessage":
                    return new DisplayMessage();
                case "DisplayMessageWithCallbacks":
                    return new DisplayMessageWithCallbacks();
                case "DistortionOverlay":
                    return new DistortionOverlay();
                case "DistortionSettings":
                    return new DistortionSettings();
                case "Door":
                    return new Door();
                case "DoorStatus":
                    return new DoorStatus();
                case "DurangoVideoCapture":
                    return new DurangoVideoCapture();
                case "EFFECT_DirectionalPhysics":
                    return new EFFECT_DirectionalPhysics();
                case "EFFECT_EntityGenerator":
                    return new EFFECT_EntityGenerator();
                case "EFFECT_ImpactGenerator":
                    return new EFFECT_ImpactGenerator();
                case "EggSpawner":
                    return new EggSpawner();
                case "ElapsedTimer":
                    return new ElapsedTimer();
                case "EnableMotionTrackerPassiveAudio":
                    return new EnableMotionTrackerPassiveAudio();
                case "EndGame":
                    return new EndGame();
                case "ENT_Debug_Exit_Game":
                    return new ENT_Debug_Exit_Game();
                case "EnvironmentMap":
                    return new EnvironmentMap();
                case "EnvironmentModelReference":
                    return new EnvironmentModelReference();
                case "EQUIPPABLE_ITEM":
                    return new EQUIPPABLE_ITEM();
                case "ExclusiveMaster":
                    return new ExclusiveMaster();
                case "Explosion_AINotifier":
                    return new Explosion_AINotifier();
                case "ExternalVariableBool":
                    return new ExternalVariableBool();
                case "FakeAILightSourceInPlayersHand":
                    return new FakeAILightSourceInPlayersHand();
                case "FilmGrainSettings":
                    return new FilmGrainSettings();
                case "Filter":
                    return new Filter();
                case "FilterAbsorber":
                    return new FilterAbsorber();
                case "FilterAnd":
                    return new FilterAnd();
                case "FilterBelongsToAlliance":
                    return new FilterBelongsToAlliance();
                case "FilterCanSeeTarget":
                    return new FilterCanSeeTarget();
                case "FilterHasBehaviourTreeFlagSet":
                    return new FilterHasBehaviourTreeFlagSet();
                case "FilterHasPlayerCollectedIdTag":
                    return new FilterHasPlayerCollectedIdTag();
                case "FilterHasWeaponEquipped":
                    return new FilterHasWeaponEquipped();
                case "FilterHasWeaponOfType":
                    return new FilterHasWeaponOfType();
                case "FilterIsACharacter":
                    return new FilterIsACharacter();
                case "FilterIsAgressing":
                    return new FilterIsAgressing();
                case "FilterIsAnySaveInProgress":
                    return new FilterIsAnySaveInProgress();
                case "FilterIsAPlayer":
                    return new FilterIsAPlayer();
                case "FilterIsCharacter":
                    return new FilterIsCharacter();
                case "FilterIsCharacterClass":
                    return new FilterIsCharacterClass();
                case "FilterIsCharacterClassCombo":
                    return new FilterIsCharacterClassCombo();
                case "FilterIsDead":
                    return new FilterIsDead();
                case "FilterIsEnemyOfAllianceGroup":
                    return new FilterIsEnemyOfAllianceGroup();
                case "FilterIsEnemyOfCharacter":
                    return new FilterIsEnemyOfCharacter();
                case "FilterIsEnemyOfPlayer":
                    return new FilterIsEnemyOfPlayer();
                case "FilterIsFacingTarget":
                    return new FilterIsFacingTarget();
                case "FilterIsHumanNPC":
                    return new FilterIsHumanNPC();
                case "FilterIsInAGroup":
                    return new FilterIsInAGroup();
                case "FilterIsInAlertnessState":
                    return new FilterIsInAlertnessState();
                case "FilterIsinInventory":
                    return new FilterIsinInventory();
                case "FilterIsInLocomotionState":
                    return new FilterIsInLocomotionState();
                case "FilterIsInWeaponRange":
                    return new FilterIsInWeaponRange();
                case "FilterIsLocalPlayer":
                    return new FilterIsLocalPlayer();
                case "FilterIsNotDeadManWalking":
                    return new FilterIsNotDeadManWalking();
                case "FilterIsObject":
                    return new FilterIsObject();
                case "FilterIsPhysics":
                    return new FilterIsPhysics();
                case "FilterIsPhysicsObject":
                    return new FilterIsPhysicsObject();
                case "FilterIsPlatform":
                    return new FilterIsPlatform();
                case "FilterIsUsingDevice":
                    return new FilterIsUsingDevice();
                case "FilterIsValidInventoryItem":
                    return new FilterIsValidInventoryItem();
                case "FilterIsWithdrawnAlien":
                    return new FilterIsWithdrawnAlien();
                case "FilterNot":
                    return new FilterNot();
                case "FilterOr":
                    return new FilterOr();
                case "FilterSmallestUsedDifficulty":
                    return new FilterSmallestUsedDifficulty();
                case "FixedCamera":
                    return new FixedCamera();
                case "FlareSettings":
                    return new FlareSettings();
                case "FlareTask":
                    return new FlareTask();
                case "FlashCallback":
                    return new FlashCallback();
                case "FlashInvoke":
                    return new FlashInvoke();
                case "FlashScript":
                    return new FlashScript();
                case "FloatAbsolute":
                    return new FloatAbsolute();
                case "FloatAdd":
                    return new FloatAdd();
                case "FloatAdd_All":
                    return new FloatAdd_All();
                case "FloatClamp":
                    return new FloatClamp();
                case "FloatClampMultiply":
                    return new FloatClampMultiply();
                case "FloatCompare":
                    return new FloatCompare();
                case "FloatDivide":
                    return new FloatDivide();
                case "FloatEquals":
                    return new FloatEquals();
                case "FloatGetLinearProportion":
                    return new FloatGetLinearProportion();
                case "FloatGreaterThan":
                    return new FloatGreaterThan();
                case "FloatGreaterThanOrEqual":
                    return new FloatGreaterThanOrEqual();
                case "FloatLessThan":
                    return new FloatLessThan();
                case "FloatLessThanOrEqual":
                    return new FloatLessThanOrEqual();
                case "FloatLinearInterpolateSpeed":
                    return new FloatLinearInterpolateSpeed();
                case "FloatLinearInterpolateSpeedAdvanced":
                    return new FloatLinearInterpolateSpeedAdvanced();
                case "FloatLinearInterpolateTimed":
                    return new FloatLinearInterpolateTimed();
                case "FloatLinearProportion":
                    return new FloatLinearProportion();
                case "FloatMath":
                    return new FloatMath();
                case "FloatMath_All":
                    return new FloatMath_All();
                case "FloatMax":
                    return new FloatMax();
                case "FloatMax_All":
                    return new FloatMax_All();
                case "FloatMin":
                    return new FloatMin();
                case "FloatMin_All":
                    return new FloatMin_All();
                case "FloatModulate":
                    return new FloatModulate();
                case "FloatModulateRandom":
                    return new FloatModulateRandom();
                case "FloatMultiply":
                    return new FloatMultiply();
                case "FloatMultiply_All":
                    return new FloatMultiply_All();
                case "FloatMultiplyClamp":
                    return new FloatMultiplyClamp();
                case "FloatNotEqual":
                    return new FloatNotEqual();
                case "FloatOperation":
                    return new FloatOperation();
                case "FloatReciprocal":
                    return new FloatReciprocal();
                case "FloatRemainder":
                    return new FloatRemainder();
                case "FloatSmoothStep":
                    return new FloatSmoothStep();
                case "FloatSqrt":
                    return new FloatSqrt();
                case "FloatSubtract":
                    return new FloatSubtract();
                case "FlushZoneCache":
                    return new FlushZoneCache();
                case "FogBox":
                    return new FogBox();
                case "FogPlane":
                    return new FogPlane();
                case "FogSetting":
                    return new FogSetting();
                case "FogSphere":
                    return new FogSphere();
                case "FollowCameraModifier":
                    return new FollowCameraModifier();
                case "FollowTask":
                    return new FollowTask();
                case "Force_UI_Visibility":
                    return new Force_UI_Visibility();
                case "FullScreenBlurSettings":
                    return new FullScreenBlurSettings();
                case "FullScreenOverlay":
                    return new FullScreenOverlay();
                case "GameDVR":
                    return new GameDVR();
                case "GameOver":
                    return new GameOver();
                case "GameOverCredits":
                    return new GameOverCredits();
                case "GameplayTip":
                    return new GameplayTip();
                case "GameStateChanged":
                    return new GameStateChanged();
                case "GenericHighlightEntity":
                    return new GenericHighlightEntity();
                case "GetBlueprintAvailable":
                    return new GetBlueprintAvailable();
                case "GetBlueprintLevel":
                    return new GetBlueprintLevel();
                case "GetCentrePoint":
                    return new GetCentrePoint();
                case "GetCharacterRotationSpeed":
                    return new GetCharacterRotationSpeed();
                case "GetClosestPercentOnSpline":
                    return new GetClosestPercentOnSpline();
                case "GetClosestPoint":
                    return new GetClosestPoint();
                case "GetClosestPointFromSet":
                    return new GetClosestPointFromSet();
                case "GetClosestPointOnSpline":
                    return new GetClosestPointOnSpline();
                case "GetCurrentCameraFov":
                    return new GetCurrentCameraFov();
                case "GetCurrentCameraPos":
                    return new GetCurrentCameraPos();
                case "GetCurrentCameraTarget":
                    return new GetCurrentCameraTarget();
                case "GetCurrentPlaylistLevelIndex":
                    return new GetCurrentPlaylistLevelIndex();
                case "GetFlashFloatValue":
                    return new GetFlashFloatValue();
                case "GetFlashIntValue":
                    return new GetFlashIntValue();
                case "GetGatingToolLevel":
                    return new GetGatingToolLevel();
                case "GetInventoryItemName":
                    return new GetInventoryItemName();
                case "GetNextPlaylistLevelName":
                    return new GetNextPlaylistLevelName();
                case "GetPlayerHasGatingTool":
                    return new GetPlayerHasGatingTool();
                case "GetPlayerHasKeycard":
                    return new GetPlayerHasKeycard();
                case "GetPointOnSpline":
                    return new GetPointOnSpline();
                case "GetRotation":
                    return new GetRotation();
                case "GetSelectedCharacterId":
                    return new GetSelectedCharacterId();
                case "GetSplineLength":
                    return new GetSplineLength();
                case "GetTranslation":
                    return new GetTranslation();
                case "GetX":
                    return new GetX();
                case "GetY":
                    return new GetY();
                case "GetZ":
                    return new GetZ();
                case "GlobalEvent":
                    return new GlobalEvent();
                case "GlobalEventMonitor":
                    return new GlobalEventMonitor();
                case "GlobalPosition":
                    return new GlobalPosition();
                case "GoToFrontend":
                    return new GoToFrontend();
                case "GPU_PFXEmitterReference":
                    return new GPU_PFXEmitterReference();
                case "HableToneMappingSettings":
                    return new HableToneMappingSettings();
                case "HackingGame":
                    return new HackingGame();
                case "HandCamera":
                    return new HandCamera();
                case "HasAccessAtDifficulty":
                    return new HasAccessAtDifficulty();
                case "HeldItem_AINotifier":
                    return new HeldItem_AINotifier();
                case "HighSpecMotionBlurSettings":
                    return new HighSpecMotionBlurSettings();
                case "HostOnlyTrigger":
                    return new HostOnlyTrigger();
                case "IdleTask":
                    return new IdleTask();
                case "ImpactSphere":
                    return new ImpactSphere();
                case "InhibitActionsUntilRelease":
                    return new InhibitActionsUntilRelease();
                case "IntegerAbsolute":
                    return new IntegerAbsolute();
                case "IntegerAdd":
                    return new IntegerAdd();
                case "IntegerAdd_All":
                    return new IntegerAdd_All();
                case "IntegerAnalyse":
                    return new IntegerAnalyse();
                case "IntegerAnd":
                    return new IntegerAnd();
                case "IntegerCompare":
                    return new IntegerCompare();
                case "IntegerCompliment":
                    return new IntegerCompliment();
                case "IntegerDivide":
                    return new IntegerDivide();
                case "IntegerEquals":
                    return new IntegerEquals();
                case "IntegerGreaterThan":
                    return new IntegerGreaterThan();
                case "IntegerGreaterThanOrEqual":
                    return new IntegerGreaterThanOrEqual();
                case "IntegerLessThan":
                    return new IntegerLessThan();
                case "IntegerLessThanOrEqual":
                    return new IntegerLessThanOrEqual();
                case "IntegerMath":
                    return new IntegerMath();
                case "IntegerMath_All":
                    return new IntegerMath_All();
                case "IntegerMax":
                    return new IntegerMax();
                case "IntegerMax_All":
                    return new IntegerMax_All();
                case "IntegerMin":
                    return new IntegerMin();
                case "IntegerMin_All":
                    return new IntegerMin_All();
                case "IntegerMultiply":
                    return new IntegerMultiply();
                case "IntegerMultiply_All":
                    return new IntegerMultiply_All();
                case "IntegerNotEqual":
                    return new IntegerNotEqual();
                case "IntegerOperation":
                    return new IntegerOperation();
                case "IntegerOr":
                    return new IntegerOr();
                case "IntegerRemainder":
                    return new IntegerRemainder();
                case "IntegerSubtract":
                    return new IntegerSubtract();
                case "Interaction":
                    return new Interaction();
                case "InteractiveMovementControl":
                    return new InteractiveMovementControl();
                case "Internal_JOB_SearchTarget":
                    return new Internal_JOB_SearchTarget();
                case "InventoryItem":
                    return new InventoryItem();
                case "IrawanToneMappingSettings":
                    return new IrawanToneMappingSettings();
                case "IsActive":
                    return new IsActive();
                case "IsAttached":
                    return new IsAttached();
                case "IsCurrentLevelAChallengeMap":
                    return new IsCurrentLevelAChallengeMap();
                case "IsCurrentLevelAPreorderMap":
                    return new IsCurrentLevelAPreorderMap();
                case "IsEnabled":
                    return new IsEnabled();
                case "IsInstallComplete":
                    return new IsInstallComplete();
                case "IsLoaded":
                    return new IsLoaded();
                case "IsLoading":
                    return new IsLoading();
                case "IsLocked":
                    return new IsLocked();
                case "IsMultiplayerMode":
                    return new IsMultiplayerMode();
                case "IsOpen":
                    return new IsOpen();
                case "IsOpening":
                    return new IsOpening();
                case "IsPaused":
                    return new IsPaused();
                case "IsPlaylistTypeAll":
                    return new IsPlaylistTypeAll();
                case "IsPlaylistTypeMarathon":
                    return new IsPlaylistTypeMarathon();
                case "IsPlaylistTypeSingle":
                    return new IsPlaylistTypeSingle();
                case "IsSpawned":
                    return new IsSpawned();
                case "IsStarted":
                    return new IsStarted();
                case "IsSuspended":
                    return new IsSuspended();
                case "IsVisible":
                    return new IsVisible();
                case "Job":
                    return new Job();
                case "JOB_AreaSweep":
                    return new JOB_AreaSweep();
                case "JOB_AreaSweepFlare":
                    return new JOB_AreaSweepFlare();
                case "JOB_Assault":
                    return new JOB_Assault();
                case "JOB_Follow":
                    return new JOB_Follow();
                case "JOB_Follow_Centre":
                    return new JOB_Follow_Centre();
                case "JOB_Idle":
                    return new JOB_Idle();
                case "JOB_Panic":
                    return new JOB_Panic();
                case "JOB_SpottingPosition":
                    return new JOB_SpottingPosition();
                case "JOB_SystematicSearch":
                    return new JOB_SystematicSearch();
                case "JOB_SystematicSearchFlare":
                    return new JOB_SystematicSearchFlare();
                case "JobWithPosition":
                    return new JobWithPosition();
                case "LeaderboardWriter":
                    return new LeaderboardWriter();
                case "LeaveGame":
                    return new LeaveGame();
                case "LensDustSettings":
                    return new LensDustSettings();
                case "LevelCompletionTargets":
                    return new LevelCompletionTargets();
                case "LevelInfo":
                    return new LevelInfo();
                case "LevelLoaded":
                    return new LevelLoaded();
                case "LightAdaptationSettings":
                    return new LightAdaptationSettings();
                case "LightingMaster":
                    return new LightingMaster();
                case "LightReference":
                    return new LightReference();
                case "LimitItemUse":
                    return new LimitItemUse();
                case "LODControls":
                    return new LODControls();
                case "Logic_MultiGate":
                    return new Logic_MultiGate();
                case "Logic_Vent_Entrance":
                    return new Logic_Vent_Entrance();
                case "Logic_Vent_System":
                    return new Logic_Vent_System();
                case "LogicAll":
                    return new LogicAll();
                case "LogicCounter":
                    return new LogicCounter();
                case "LogicDelay":
                    return new LogicDelay();
                case "LogicGate":
                    return new LogicGate();
                case "LogicGateAnd":
                    return new LogicGateAnd();
                case "LogicGateEquals":
                    return new LogicGateEquals();
                case "LogicGateNotEqual":
                    return new LogicGateNotEqual();
                case "LogicGateOr":
                    return new LogicGateOr();
                case "LogicNot":
                    return new LogicNot();
                case "LogicOnce":
                    return new LogicOnce();
                case "LogicPressurePad":
                    return new LogicPressurePad();
                case "LogicSwitch":
                    return new LogicSwitch();
                case "LowResFrameCapture":
                    return new LowResFrameCapture();
                case "Map_Floor_Change":
                    return new Map_Floor_Change();
                case "MapAnchor":
                    return new MapAnchor();
                case "MapItem":
                    return new MapItem();
                case "Master":
                    return new Master();
                case "MELEE_WEAPON":
                    return new MELEE_WEAPON();
                case "Minigames":
                    return new Minigames();
                case "MissionNumber":
                    return new MissionNumber();
                case "ModelReference":
                    return new ModelReference();
                case "MonitorActionMap":
                    return new MonitorActionMap();
                case "MonitorBase":
                    return new MonitorBase();
                case "MonitorPadInput":
                    return new MonitorPadInput();
                case "MotionTrackerMonitor":
                    return new MotionTrackerMonitor();
                case "MotionTrackerPing":
                    return new MotionTrackerPing();
                case "MoveAlongSpline":
                    return new MoveAlongSpline();
                case "MoveInTime":
                    return new MoveInTime();
                case "MoviePlayer":
                    return new MoviePlayer();
                case "MultipleCharacterAttachmentNode":
                    return new MultipleCharacterAttachmentNode();
                case "MultiplePickupSpawner":
                    return new MultiplePickupSpawner();
                case "MultitrackLoop":
                    return new MultitrackLoop();
                case "MusicController":
                    return new MusicController();
                case "MusicTrigger":
                    return new MusicTrigger();
                case "GCIP_WorldPickup":
                    return new GCIP_WorldPickup();
                case "Torch_Control":
                    return new Torch_Control();
                case "PlayForMinDuration":
                    return new PlayForMinDuration();
                case "NavMeshArea":
                    return new NavMeshArea();
                case "NavMeshBarrier":
                    return new NavMeshBarrier();
                case "NavMeshExclusionArea":
                    return new NavMeshExclusionArea();
                case "NavMeshReachabilitySeedPoint":
                    return new NavMeshReachabilitySeedPoint();
                case "NavMeshWalkablePlatform":
                    return new NavMeshWalkablePlatform();
                case "NetPlayerCounter":
                    return new NetPlayerCounter();
                case "NetworkedTimer":
                    return new NetworkedTimer();
                case "NetworkProxy":
                    return new NetworkProxy();
                case "NonInteractiveWater":
                    return new NonInteractiveWater();
                case "NonPersistentBool":
                    return new NonPersistentBool();
                case "NonPersistentInt":
                    return new NonPersistentInt();
                case "NPC_Aggression_Monitor":
                    return new NPC_Aggression_Monitor();
                case "NPC_AlienConfig":
                    return new NPC_AlienConfig();
                case "NPC_AllSensesLimiter":
                    return new NPC_AllSensesLimiter();
                case "NPC_ambush_monitor":
                    return new NPC_ambush_monitor();
                case "NPC_AreaBox":
                    return new NPC_AreaBox();
                case "NPC_behaviour_monitor":
                    return new NPC_behaviour_monitor();
                case "NPC_ClearDefendArea":
                    return new NPC_ClearDefendArea();
                case "NPC_ClearPursuitArea":
                    return new NPC_ClearPursuitArea();
                case "NPC_Coordinator":
                    return new NPC_Coordinator();
                case "NPC_Debug_Menu_Item":
                    return new NPC_Debug_Menu_Item();
                case "NPC_DefineBackstageAvoidanceArea":
                    return new NPC_DefineBackstageAvoidanceArea();
                case "NPC_DynamicDialogue":
                    return new NPC_DynamicDialogue();
                case "NPC_DynamicDialogueGlobalRange":
                    return new NPC_DynamicDialogueGlobalRange();
                case "NPC_FakeSense":
                    return new NPC_FakeSense();
                case "NPC_FollowOffset":
                    return new NPC_FollowOffset();
                case "NPC_ForceCombatTarget":
                    return new NPC_ForceCombatTarget();
                case "NPC_ForceNextJob":
                    return new NPC_ForceNextJob();
                case "NPC_ForceRetreat":
                    return new NPC_ForceRetreat();
                case "NPC_Gain_Aggression_In_Radius":
                    return new NPC_Gain_Aggression_In_Radius();
                case "NPC_GetCombatTarget":
                    return new NPC_GetCombatTarget();
                case "NPC_GetLastSensedPositionOfTarget":
                    return new NPC_GetLastSensedPositionOfTarget();
                case "NPC_Group_Death_Monitor":
                    return new NPC_Group_Death_Monitor();
                case "NPC_Group_DeathCounter":
                    return new NPC_Group_DeathCounter();
                case "NPC_Highest_Awareness_Monitor":
                    return new NPC_Highest_Awareness_Monitor();
                case "NPC_MeleeContext":
                    return new NPC_MeleeContext();
                case "NPC_multi_behaviour_monitor":
                    return new NPC_multi_behaviour_monitor();
                case "NPC_navmesh_type_monitor":
                    return new NPC_navmesh_type_monitor();
                case "NPC_NotifyDynamicDialogueEvent":
                    return new NPC_NotifyDynamicDialogueEvent();
                case "NPC_Once":
                    return new NPC_Once();
                case "NPC_ResetFiringStats":
                    return new NPC_ResetFiringStats();
                case "NPC_ResetSensesAndMemory":
                    return new NPC_ResetSensesAndMemory();
                case "NPC_SenseLimiter":
                    return new NPC_SenseLimiter();
                case "NPC_set_behaviour_tree_flags":
                    return new NPC_set_behaviour_tree_flags();
                case "NPC_SetAgressionProgression":
                    return new NPC_SetAgressionProgression();
                case "NPC_SetAimTarget":
                    return new NPC_SetAimTarget();
                case "NPC_SetAlertness":
                    return new NPC_SetAlertness();
                case "NPC_SetAlienDevelopmentStage":
                    return new NPC_SetAlienDevelopmentStage();
                case "NPC_SetAutoTorchMode":
                    return new NPC_SetAutoTorchMode();
                case "NPC_SetChokePoint":
                    return new NPC_SetChokePoint();
                case "NPC_SetDefendArea":
                    return new NPC_SetDefendArea();
                case "NPC_SetFiringAccuracy":
                    return new NPC_SetFiringAccuracy();
                case "NPC_SetFiringRhythm":
                    return new NPC_SetFiringRhythm();
                case "NPC_SetGunAimMode":
                    return new NPC_SetGunAimMode();
                case "NPC_SetHidingNearestLocation":
                    return new NPC_SetHidingNearestLocation();
                case "NPC_SetHidingSearchRadius":
                    return new NPC_SetHidingSearchRadius();
                case "NPC_SetInvisible":
                    return new NPC_SetInvisible();
                case "NPC_SetLocomotionStyleForJobs":
                    return new NPC_SetLocomotionStyleForJobs();
                case "NPC_SetLocomotionTargetSpeed":
                    return new NPC_SetLocomotionTargetSpeed();
                case "NPC_SetPursuitArea":
                    return new NPC_SetPursuitArea();
                case "NPC_SetRateOfFire":
                    return new NPC_SetRateOfFire();
                case "NPC_SetSafePoint":
                    return new NPC_SetSafePoint();
                case "NPC_SetSenseSet":
                    return new NPC_SetSenseSet();
                case "NPC_SetStartPos":
                    return new NPC_SetStartPos();
                case "NPC_SetTotallyBlindInDark":
                    return new NPC_SetTotallyBlindInDark();
                case "NPC_SetupMenaceManager":
                    return new NPC_SetupMenaceManager();
                case "NPC_Sleeping_Android_Monitor":
                    return new NPC_Sleeping_Android_Monitor();
                case "NPC_Squad_DialogueMonitor":
                    return new NPC_Squad_DialogueMonitor();
                case "NPC_Squad_GetAwarenessState":
                    return new NPC_Squad_GetAwarenessState();
                case "NPC_Squad_GetAwarenessWatermark":
                    return new NPC_Squad_GetAwarenessWatermark();
                case "NPC_StopAiming":
                    return new NPC_StopAiming();
                case "NPC_StopShooting":
                    return new NPC_StopShooting();
                case "NPC_SuspiciousItem":
                    return new NPC_SuspiciousItem();
                case "NPC_TargetAcquire":
                    return new NPC_TargetAcquire();
                case "NPC_TriggerAimRequest":
                    return new NPC_TriggerAimRequest();
                case "NPC_TriggerShootRequest":
                    return new NPC_TriggerShootRequest();
                case "NPC_WithdrawAlien":
                    return new NPC_WithdrawAlien();
                case "NumConnectedPlayers":
                    return new NumConnectedPlayers();
                case "NumDeadPlayers":
                    return new NumDeadPlayers();
                case "NumPlayersOnStart":
                    return new NumPlayersOnStart();
                case "PadLightBar":
                    return new PadLightBar();
                case "PadRumbleImpulse":
                    return new PadRumbleImpulse();
                case "ParticipatingPlayersList":
                    return new ParticipatingPlayersList();
                case "ParticleEmitterReference":
                    return new ParticleEmitterReference();
                case "PathfindingAlienBackstageNode":
                    return new PathfindingAlienBackstageNode();
                case "PathfindingManualNode":
                    return new PathfindingManualNode();
                case "PathfindingTeleportNode":
                    return new PathfindingTeleportNode();
                case "PathfindingWaitNode":
                    return new PathfindingWaitNode();
                case "Persistent_TriggerRandomSequence":
                    return new Persistent_TriggerRandomSequence();
                case "PhysicsApplyBuoyancy":
                    return new PhysicsApplyBuoyancy();
                case "PhysicsApplyImpulse":
                    return new PhysicsApplyImpulse();
                case "PhysicsApplyVelocity":
                    return new PhysicsApplyVelocity();
                case "PhysicsModifyGravity":
                    return new PhysicsModifyGravity();
                case "PhysicsSystem":
                    return new PhysicsSystem();
                case "PickupSpawner":
                    return new PickupSpawner();
                case "Planet":
                    return new Planet();
                case "PlatformConstantBool":
                    return new PlatformConstantBool();
                case "PlatformConstantFloat":
                    return new PlatformConstantFloat();
                case "PlatformConstantInt":
                    return new PlatformConstantInt();
                case "PlayEnvironmentAnimation":
                    return new PlayEnvironmentAnimation();
                case "Player_ExploitableArea":
                    return new Player_ExploitableArea();
                case "Player_Sensor":
                    return new Player_Sensor();
                case "PlayerCamera":
                    return new PlayerCamera();
                case "PlayerCameraMonitor":
                    return new PlayerCameraMonitor();
                case "PlayerCampaignDeaths":
                    return new PlayerCampaignDeaths();
                case "PlayerCampaignDeathsInARow":
                    return new PlayerCampaignDeathsInARow();
                case "PlayerDeathCounter":
                    return new PlayerDeathCounter();
                case "PlayerDiscardsItems":
                    return new PlayerDiscardsItems();
                case "PlayerDiscardsTools":
                    return new PlayerDiscardsTools();
                case "PlayerDiscardsWeapons":
                    return new PlayerDiscardsWeapons();
                case "PlayerHasEnoughItems":
                    return new PlayerHasEnoughItems();
                case "PlayerHasItem":
                    return new PlayerHasItem();
                case "PlayerHasItemEntity":
                    return new PlayerHasItemEntity();
                case "PlayerHasItemWithName":
                    return new PlayerHasItemWithName();
                case "PlayerHasSpaceForItem":
                    return new PlayerHasSpaceForItem();
                case "PlayerKilledAllyMonitor":
                    return new PlayerKilledAllyMonitor();
                case "PlayerLightProbe":
                    return new PlayerLightProbe();
                case "PlayerTorch":
                    return new PlayerTorch();
                case "PlayerTriggerBox":
                    return new PlayerTriggerBox();
                case "PlayerUseTriggerBox":
                    return new PlayerUseTriggerBox();
                case "PlayerWeaponMonitor":
                    return new PlayerWeaponMonitor();
                case "PointAt":
                    return new PointAt();
                case "PointTracker":
                    return new PointTracker();
                case "PopupMessage":
                    return new PopupMessage();
                case "PositionDistance":
                    return new PositionDistance();
                case "PositionMarker":
                    return new PositionMarker();
                case "PostprocessingSettings":
                    return new PostprocessingSettings();
                case "ProjectileMotion":
                    return new ProjectileMotion();
                case "ProjectileMotionComplex":
                    return new ProjectileMotionComplex();
                case "ProjectiveDecal":
                    return new ProjectiveDecal();
                case "ProximityDetector":
                    return new ProximityDetector();
                case "ProximityTrigger":
                    return new ProximityTrigger();
                case "QueryGCItemPool":
                    return new QueryGCItemPool();
                case "RadiosityIsland":
                    return new RadiosityIsland();
                case "RadiosityProxy":
                    return new RadiosityProxy();
                case "RandomBool":
                    return new RandomBool();
                case "RandomFloat":
                    return new RandomFloat();
                case "RandomInt":
                    return new RandomInt();
                case "RandomObjectSelector":
                    return new RandomObjectSelector();
                case "RandomSelect":
                    return new RandomSelect();
                case "RandomVector":
                    return new RandomVector();
                case "Raycast":
                    return new Raycast();
                case "Refraction":
                    return new Refraction();
                case "RegisterCharacterModel":
                    return new RegisterCharacterModel();
                case "RemoveFromGCItemPool":
                    return new RemoveFromGCItemPool();
                case "RemoveFromInventory":
                    return new RemoveFromInventory();
                case "RemoveWeaponsFromPlayer":
                    return new RemoveWeaponsFromPlayer();
                case "RespawnConfig":
                    return new RespawnConfig();
                case "RespawnExcluder":
                    return new RespawnExcluder();
                case "ReTransformer":
                    return new ReTransformer();
                case "Rewire":
                    return new Rewire();
                case "RewireAccess_Point":
                    return new RewireAccess_Point();
                case "RewireLocation":
                    return new RewireLocation();
                case "RewireSystem":
                    return new RewireSystem();
                case "RewireTotalPowerResource":
                    return new RewireTotalPowerResource();
                case "RibbonEmitterReference":
                    return new RibbonEmitterReference();
                case "RotateAtSpeed":
                    return new RotateAtSpeed();
                case "RotateInTime":
                    return new RotateInTime();
                case "RTT_MoviePlayer":
                    return new RTT_MoviePlayer();
                case "SaveGlobalProgression":
                    return new SaveGlobalProgression();
                case "SaveManagers":
                    return new SaveManagers();
                case "ScalarProduct":
                    return new ScalarProduct();
                case "ScreenEffectEventMonitor":
                    return new ScreenEffectEventMonitor();
                case "ScreenFadeIn":
                    return new ScreenFadeIn();
                case "ScreenFadeInTimed":
                    return new ScreenFadeInTimed();
                case "ScreenFadeOutToBlack":
                    return new ScreenFadeOutToBlack();
                case "ScreenFadeOutToBlackTimed":
                    return new ScreenFadeOutToBlackTimed();
                case "ScreenFadeOutToWhite":
                    return new ScreenFadeOutToWhite();
                case "ScreenFadeOutToWhiteTimed":
                    return new ScreenFadeOutToWhiteTimed();
                case "ScriptVariable":
                    return new ScriptVariable();
                case "SetAsActiveMissionLevel":
                    return new SetAsActiveMissionLevel();
                case "SetBlueprintInfo":
                    return new SetBlueprintInfo();
                case "SetBool":
                    return new SetBool();
                case "SetColour":
                    return new SetColour();
                case "SetEnum":
                    return new SetEnum();
                case "SetEnumString":
                    return new SetEnumString();
                case "SetFloat":
                    return new SetFloat();
                case "SetGamepadAxes":
                    return new SetGamepadAxes();
                case "SetGameplayTips":
                    return new SetGameplayTips();
                case "SetGatingToolLevel":
                    return new SetGatingToolLevel();
                case "SetHackingToolLevel":
                    return new SetHackingToolLevel();
                case "SetInteger":
                    return new SetInteger();
                case "SetLocationAndOrientation":
                    return new SetLocationAndOrientation();
                case "SetMotionTrackerRange":
                    return new SetMotionTrackerRange();
                case "SetNextLoadingMovie":
                    return new SetNextLoadingMovie();
                case "SetObject":
                    return new SetObject();
                case "SetObjectiveCompleted":
                    return new SetObjectiveCompleted();
                case "SetPlayerHasGatingTool":
                    return new SetPlayerHasGatingTool();
                case "SetPlayerHasKeycard":
                    return new SetPlayerHasKeycard();
                case "SetPosition":
                    return new SetPosition();
                case "SetPrimaryObjective":
                    return new SetPrimaryObjective();
                case "SetRichPresence":
                    return new SetRichPresence();
                case "SetString":
                    return new SetString();
                case "SetSubObjective":
                    return new SetSubObjective();
                case "SetupGCDistribution":
                    return new SetupGCDistribution();
                case "SetVector":
                    return new SetVector();
                case "SetVector2":
                    return new SetVector2();
                case "SharpnessSettings":
                    return new SharpnessSettings();
                case "Showlevel_Completed":
                    return new Showlevel_Completed();
                case "SimpleRefraction":
                    return new SimpleRefraction();
                case "SimpleWater":
                    return new SimpleWater();
                case "SmokeCylinder":
                    return new SmokeCylinder();
                case "SmokeCylinderAttachmentInterface":
                    return new SmokeCylinderAttachmentInterface();
                case "SmoothMove":
                    return new SmoothMove();
                case "Sound":
                    return new Sound();
                case "SoundBarrier":
                    return new SoundBarrier();
                case "SoundEnvironmentMarker":
                    return new SoundEnvironmentMarker();
                case "SoundEnvironmentZone":
                    return new SoundEnvironmentZone();
                case "SoundImpact":
                    return new SoundImpact();
                case "SoundLevelInitialiser":
                    return new SoundLevelInitialiser();
                case "SoundLoadBank":
                    return new SoundLoadBank();
                case "SoundLoadSlot":
                    return new SoundLoadSlot();
                case "SoundMissionInitialiser":
                    return new SoundMissionInitialiser();
                case "SoundNetworkNode":
                    return new SoundNetworkNode();
                case "SoundObject":
                    return new SoundObject();
                case "SoundPhysicsInitialiser":
                    return new SoundPhysicsInitialiser();
                case "SoundPlaybackBaseClass":
                    return new SoundPlaybackBaseClass();
                case "SoundPlayerFootwearOverride":
                    return new SoundPlayerFootwearOverride();
                case "SoundRTPCController":
                    return new SoundRTPCController();
                case "SoundSetRTPC":
                    return new SoundSetRTPC();
                case "SoundSetState":
                    return new SoundSetState();
                case "SoundSetSwitch":
                    return new SoundSetSwitch();
                case "SoundSpline":
                    return new SoundSpline();
                case "SoundTimelineTrigger":
                    return new SoundTimelineTrigger();
                case "SpaceSuitVisor":
                    return new SpaceSuitVisor();
                case "SpaceTransform":
                    return new SpaceTransform();
                case "SpawnGroup":
                    return new SpawnGroup();
                case "Speech":
                    return new Speech();
                case "SpeechScript":
                    return new SpeechScript();
                case "Sphere":
                    return new Sphere();
                case "SplineDistanceLerp":
                    return new SplineDistanceLerp();
                case "SplinePath":
                    return new SplinePath();
                case "SpottingExclusionArea":
                    return new SpottingExclusionArea();
                case "Squad_SetMaxEscalationLevel":
                    return new Squad_SetMaxEscalationLevel();
                case "StartNewChapter":
                    return new StartNewChapter();
                case "StateQuery":
                    return new StateQuery();
                case "StealCamera":
                    return new StealCamera();
                case "StreamingMonitor":
                    return new StreamingMonitor();
                case "SurfaceEffectBox":
                    return new SurfaceEffectBox();
                case "SurfaceEffectSphere":
                    return new SurfaceEffectSphere();
                case "SwitchLevel":
                    return new SwitchLevel();
                case "SyncOnAllPlayers":
                    return new SyncOnAllPlayers();
                case "SyncOnFirstPlayer":
                    return new SyncOnFirstPlayer();
                case "Task":
                    return new Task();
                case "TerminalContent":
                    return new TerminalContent();
                case "TerminalFolder":
                    return new TerminalFolder();
                case "Thinker":
                    return new Thinker();
                case "ThinkOnce":
                    return new ThinkOnce();
                case "ThrowingPointOfImpact":
                    return new ThrowingPointOfImpact();
                case "ToggleFunctionality":
                    return new ToggleFunctionality();
                case "TogglePlayerTorch":
                    return new TogglePlayerTorch();
                case "TorchDynamicMovement":
                    return new TorchDynamicMovement();
                case "TRAV_1ShotClimbUnder":
                    return new TRAV_1ShotClimbUnder();
                case "TRAV_1ShotFloorVentEntrance":
                    return new TRAV_1ShotFloorVentEntrance();
                case "TRAV_1ShotFloorVentExit":
                    return new TRAV_1ShotFloorVentExit();
                case "TRAV_1ShotLeap":
                    return new TRAV_1ShotLeap();
                case "TRAV_1ShotSpline":
                    return new TRAV_1ShotSpline();
                case "TRAV_1ShotVentEntrance":
                    return new TRAV_1ShotVentEntrance();
                case "TRAV_1ShotVentExit":
                    return new TRAV_1ShotVentExit();
                case "TRAV_ContinuousBalanceBeam":
                    return new TRAV_ContinuousBalanceBeam();
                case "TRAV_ContinuousCinematicSidle":
                    return new TRAV_ContinuousCinematicSidle();
                case "TRAV_ContinuousClimbingWall":
                    return new TRAV_ContinuousClimbingWall();
                case "TRAV_ContinuousLadder":
                    return new TRAV_ContinuousLadder();
                case "TRAV_ContinuousLedge":
                    return new TRAV_ContinuousLedge();
                case "TRAV_ContinuousPipe":
                    return new TRAV_ContinuousPipe();
                case "TRAV_ContinuousTightGap":
                    return new TRAV_ContinuousTightGap();
                case "Trigger_AudioOccluded":
                    return new Trigger_AudioOccluded();
                case "TriggerBindAllCharactersOfType":
                    return new TriggerBindAllCharactersOfType();
                case "TriggerBindAllNPCs":
                    return new TriggerBindAllNPCs();
                case "TriggerBindCharacter":
                    return new TriggerBindCharacter();
                case "TriggerBindCharactersInSquad":
                    return new TriggerBindCharactersInSquad();
                case "TriggerCameraViewCone":
                    return new TriggerCameraViewCone();
                case "TriggerCameraViewConeMulti":
                    return new TriggerCameraViewConeMulti();
                case "TriggerCameraVolume":
                    return new TriggerCameraVolume();
                case "TriggerCheckDifficulty":
                    return new TriggerCheckDifficulty();
                case "TriggerContainerObjectsFilterCounter":
                    return new TriggerContainerObjectsFilterCounter();
                case "TriggerDamaged":
                    return new TriggerDamaged();
                case "TriggerDelay":
                    return new TriggerDelay();
                case "TriggerExtractBoundCharacter":
                    return new TriggerExtractBoundCharacter();
                case "TriggerExtractBoundObject":
                    return new TriggerExtractBoundObject();
                case "TriggerFilter":
                    return new TriggerFilter();
                case "TriggerLooper":
                    return new TriggerLooper();
                case "TriggerObjectsFilter":
                    return new TriggerObjectsFilter();
                case "TriggerObjectsFilterCounter":
                    return new TriggerObjectsFilterCounter();
                case "TriggerRandom":
                    return new TriggerRandom();
                case "TriggerRandomSequence":
                    return new TriggerRandomSequence();
                case "TriggerSelect":
                    return new TriggerSelect();
                case "TriggerSelect_Direct":
                    return new TriggerSelect_Direct();
                case "TriggerSequence":
                    return new Nodes.TriggerSequence();
                case "TriggerSimple":
                    return new TriggerSimple();
                case "TriggerSwitch":
                    return new TriggerSwitch();
                case "TriggerSync":
                    return new TriggerSync();
                case "TriggerTouch":
                    return new TriggerTouch();
                case "TriggerUnbindCharacter":
                    return new TriggerUnbindCharacter();
                case "TriggerViewCone":
                    return new TriggerViewCone();
                case "TriggerVolumeFilter":
                    return new TriggerVolumeFilter();
                case "TriggerVolumeFilter_Monitored":
                    return new TriggerVolumeFilter_Monitored();
                case "TriggerWeightedRandom":
                    return new TriggerWeightedRandom();
                case "TriggerWhenSeeTarget":
                    return new TriggerWhenSeeTarget();
                case "TutorialMessage":
                    return new TutorialMessage();
                case "UI_Attached":
                    return new UI_Attached();
                case "UI_Container":
                    return new UI_Container();
                case "UI_Icon":
                    return new UI_Icon();
                case "UI_KeyGate":
                    return new UI_KeyGate();
                case "UI_Keypad":
                    return new UI_Keypad();
                case "UI_ReactionGame":
                    return new UI_ReactionGame();
                case "UIBreathingGameIcon":
                    return new UIBreathingGameIcon();
                case "UiSelectionBox":
                    return new UiSelectionBox();
                case "UiSelectionSphere":
                    return new UiSelectionSphere();
                case "UnlockAchievement":
                    return new UnlockAchievement();
                case "UnlockLogEntry":
                    return new UnlockLogEntry();
                case "UnlockMapDetail":
                    return new UnlockMapDetail();
                case "UpdateGlobalPosition":
                    return new UpdateGlobalPosition();
                case "UpdateLeaderBoardDisplay":
                    return new UpdateLeaderBoardDisplay();
                case "UpdatePrimaryObjective":
                    return new UpdatePrimaryObjective();
                case "UpdateSubObjective":
                    return new UpdateSubObjective();
                case "VariableAnimationInfo":
                    return new VariableAnimationInfo();
                case "VariableBool":
                    return new VariableBool();
                case "VariableColour":
                    return new VariableColour();
                case "VariableEnum":
                    return new VariableEnum();
                case "VariableEnumString":
                    return new VariableEnumString();
                case "VariableFilterObject":
                    return new VariableFilterObject();
                case "VariableFlashScreenColour":
                    return new VariableFlashScreenColour();
                case "VariableFloat":
                    return new VariableFloat();
                case "VariableHackingConfig":
                    return new VariableHackingConfig();
                case "VariableInt":
                    return new VariableInt();
                case "VariableObject":
                    return new VariableObject();
                case "VariablePosition":
                    return new VariablePosition();
                case "VariableString":
                    return new VariableString();
                case "VariableThePlayer":
                    return new VariableThePlayer();
                case "VariableTriggerObject":
                    return new VariableTriggerObject();
                case "VariableVector":
                    return new VariableVector();
                case "VariableVector2":
                    return new VariableVector2();
                case "VectorAdd":
                    return new VectorAdd();
                case "VectorDirection":
                    return new VectorDirection();
                case "VectorDistance":
                    return new VectorDistance();
                case "VectorLinearInterpolateSpeed":
                    return new VectorLinearInterpolateSpeed();
                case "VectorLinearInterpolateTimed":
                    return new VectorLinearInterpolateTimed();
                case "VectorLinearProportion":
                    return new VectorLinearProportion();
                case "VectorMath":
                    return new VectorMath();
                case "VectorModulus":
                    return new VectorModulus();
                case "VectorMultiply":
                    return new VectorMultiply();
                case "VectorMultiplyByPos":
                    return new VectorMultiplyByPos();
                case "VectorNormalise":
                    return new VectorNormalise();
                case "VectorProduct":
                    return new VectorProduct();
                case "VectorReflect":
                    return new VectorReflect();
                case "VectorRotateByPos":
                    return new VectorRotateByPos();
                case "VectorRotatePitch":
                    return new VectorRotatePitch();
                case "VectorRotateRoll":
                    return new VectorRotateRoll();
                case "VectorRotateYaw":
                    return new VectorRotateYaw();
                case "VectorScale":
                    return new VectorScale();
                case "VectorSubtract":
                    return new VectorSubtract();
                case "VectorYaw":
                    return new VectorYaw();
                case "VideoCapture":
                    return new VideoCapture();
                case "VignetteSettings":
                    return new VignetteSettings();
                case "VisibilityMaster":
                    return new VisibilityMaster();
                case "Weapon_AINotifier":
                    return new Weapon_AINotifier();
                case "WEAPON_AmmoTypeFilter":
                    return new WEAPON_AmmoTypeFilter();
                case "WEAPON_AttackerFilter":
                    return new WEAPON_AttackerFilter();
                case "WEAPON_DamageFilter":
                    return new WEAPON_DamageFilter();
                case "WEAPON_DidHitSomethingFilter":
                    return new WEAPON_DidHitSomethingFilter();
                case "WEAPON_Effect":
                    return new WEAPON_Effect();
                case "WEAPON_GiveToCharacter":
                    return new WEAPON_GiveToCharacter();
                case "WEAPON_GiveToPlayer":
                    return new WEAPON_GiveToPlayer();
                case "WEAPON_ImpactAngleFilter":
                    return new WEAPON_ImpactAngleFilter();
                case "WEAPON_ImpactCharacterFilter":
                    return new WEAPON_ImpactCharacterFilter();
                case "WEAPON_ImpactEffect":
                    return new WEAPON_ImpactEffect();
                case "WEAPON_ImpactFilter":
                    return new WEAPON_ImpactFilter();
                case "WEAPON_ImpactInspector":
                    return new WEAPON_ImpactInspector();
                case "WEAPON_ImpactOrientationFilter":
                    return new WEAPON_ImpactOrientationFilter();
                case "WEAPON_MultiFilter":
                    return new WEAPON_MultiFilter();
                case "WEAPON_TargetObjectFilter":
                    return new WEAPON_TargetObjectFilter();
                case "Zone":
                    return new Zone();
                case "ZoneExclusionLink":
                    return new ZoneExclusionLink();
                case "ZoneLink":
                    return new ZoneLink();
                case "ZoneLoaded":
                    return new ZoneLoaded();
            }
            return null;
        }

        private void btn_open_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.stn|*.stn";
            if (ofd.ShowDialog() != DialogResult.OK) return;
            stNodeEditor1.Nodes.Clear();
            stNodeEditor1.LoadCanvas(ofd.FileName);
        }

        private void btn_save_Click(object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*.stn|*.stn";
            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            stNodeEditor1.SaveCanvas(sfd.FileName);
        }

        private void lockConnectionToolStripMenuItem_Click(object sender, EventArgs e) {
            stNodeEditor1.ActiveNode.LockOption = !stNodeEditor1.ActiveNode.LockOption;
        }

        private void lockLocationToolStripMenuItem_Click(object sender, EventArgs e) {
            if (stNodeEditor1.ActiveNode == null) return;
            stNodeEditor1.ActiveNode.LockLocation = !stNodeEditor1.ActiveNode.LockLocation;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e) {
            if (stNodeEditor1.ActiveNode == null) return;
            stNodeEditor1.Nodes.Remove(stNodeEditor1.ActiveNode);
        }

        private void clickToSelect_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
