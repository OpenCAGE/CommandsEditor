using CATHODE;
using CATHODE.Scripting;
using CathodeLib;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using CommandsEditor.Popups.Function_Editors.CharacterEditor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandsEditor
{
    public partial class CharacterEditor : BaseWindow
    {
        private List<EntityHierarchy> _hierarchies = new List<EntityHierarchy>();
        private CharacterAccessorySets.Entry _accessories;

        private EntityDisplay _entityDisplay;

        public CharacterEditor(EntityDisplay editor) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor.Content)
        {
            _entityDisplay = editor;
            InitializeComponent();

            foreach (KeyValuePair<string, List<string>> skeletons in Singleton.Skeletons)
                gender.Items.Add(skeletons.Key);

            shirtDecal.Items.Clear();
            foreach (CharacterAccessorySets.Entry.Decal decal in Enum.GetValues(typeof(CharacterAccessorySets.Entry.Decal)))
                shirtDecal.Items.Add(decal);

            RefreshUI(ShortGuid.Invalid);
        }

        private void RefreshUI(ShortGuid selected)
        {
            int toSelect = 0;

            _hierarchies.Clear();
            List<EntityHierarchy> hierarchies = _entityDisplay.Content.editor_utils.GetHierarchiesForEntity(_entityDisplay.Composite, _entityDisplay.Entity);
            for (int i = 0; i < hierarchies.Count; i++)
            {
                ShortGuid instance = hierarchies[i].GenerateInstance();
                if (Content.resource.character_accessories.Entries.FirstOrDefault(o => o.character.composite_instance_id == instance) == null) continue;
                if (toSelect == 0 && instance == selected) toSelect = _hierarchies.Count;
                _hierarchies.Add(hierarchies[i]);
            }

            characterInstances.Items.Clear();
            for (int i = 0; i < _hierarchies.Count; i++)
                characterInstances.Items.Add(_hierarchies[i].GetHierarchyAsString(Content.commands, _entityDisplay.Composite, false));

            selectNewHead.Enabled = characterInstances.Items.Count != 0;
            selectNewShirt.Enabled = characterInstances.Items.Count != 0;
            selectNewArms.Enabled = characterInstances.Items.Count != 0;
            selectNewTrousers.Enabled = characterInstances.Items.Count != 0;
            selectNewShoes.Enabled = characterInstances.Items.Count != 0;
            selectNewCollision.Enabled = characterInstances.Items.Count != 0;
            bodyTypes.Enabled = characterInstances.Items.Count != 0;
            gender.Enabled = characterInstances.Items.Count != 0;
            shirtDecal.Enabled = characterInstances.Items.Count != 0;

            if (characterInstances.Items.Count != 0)
                characterInstances.SelectedIndex = toSelect;
        }

        private void RefreshSkeletonsForGender()
        {
            bodyTypes.Items.Clear();
            for (int i = 0; i < Singleton.Skeletons[gender.Text].Count; i++)
                bodyTypes.Items.Add(Singleton.Skeletons[gender.Text][i]);
        }

        private void characterInstances_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShortGuid hierarchyID = _hierarchies[characterInstances.SelectedIndex].GenerateInstance();
            _accessories = Content.resource.character_accessories.Entries.FirstOrDefault(o => o.character.composite_instance_id == hierarchyID);

            shirtComposite.Text = Content.commands.GetComposite(_accessories.shirt_composite)?.name;
            trousersComposite.Text = Content.commands.GetComposite(_accessories.trousers_composite)?.name;
            shoesComposite.Text = Content.commands.GetComposite(_accessories.shoes_composite)?.name;
            headComposite.Text = Content.commands.GetComposite(_accessories.head_composite)?.name;
            armsComposite.Text = Content.commands.GetComposite(_accessories.arms_composite)?.name;
            collisionComposite.Text = Content.commands.GetComposite(_accessories.collision_composite)?.name;

            gender.Text = _accessories.body_skeleton;
            RefreshSkeletonsForGender();
            bodyTypes.Text = _accessories.face_skeleton;
            shirtDecal.SelectedIndex = (int)_accessories.decal;
        }

        private void addNewCharacter_Click(object sender, EventArgs e)
        {
            List<ShortGuid> existingCharacters = new List<ShortGuid>();
            for (int i = 0; i < _hierarchies.Count; i++)
            {
                existingCharacters.Add(_hierarchies[i].GenerateInstance());
            }

            Character_InstanceSelection instanceSelector = new Character_InstanceSelection(_entityDisplay, existingCharacters);
            instanceSelector.Show();
            instanceSelector.OnInstanceSelected += OnCharacterInstanceSelected;
        }
        private void OnCharacterInstanceSelected(ShortGuid instance)
        {
            Content.resource.character_accessories.Entries.Add(new CharacterAccessorySets.Entry()
            {
                character = new CommandsEntityReference() { entity_id = _entityDisplay.Entity.shortGUID, composite_instance_id = instance }
            });

            RefreshUI(instance);
        }

        private SelectComposite CompositeSelector(string composite)
        {
            SelectComposite selectComposite = new SelectComposite(_content, composite);
            selectComposite.Show();
            return selectComposite;
        }
        private void selectNewHead_Click(object sender, EventArgs e)
        {
            CompositeSelector(headComposite.Text).OnCompositeGenerated += OnNewHeadSelected;
        }
        private void OnNewHeadSelected(Composite comp)
        {
            headComposite.Text = comp.name;
            _accessories.head_composite = comp.shortGUID;
        }
        private void selectNewShirt_Click(object sender, EventArgs e)
        {
            CompositeSelector(shirtComposite.Text).OnCompositeGenerated += OnNewShirtSelected;
        }
        private void OnNewShirtSelected(Composite comp)
        {
            shirtComposite.Text = comp.name;
            _accessories.shirt_composite = comp.shortGUID;
        }
        private void selectNewArms_Click(object sender, EventArgs e)
        {
            CompositeSelector(armsComposite.Text).OnCompositeGenerated += OnNewArmsSelected;
        }
        private void OnNewArmsSelected(Composite comp)
        {
            armsComposite.Text = comp.name;
            _accessories.arms_composite = comp.shortGUID;
        }
        private void selectNewTrousers_Click(object sender, EventArgs e)
        {
            CompositeSelector(trousersComposite.Text).OnCompositeGenerated += OnNewTrousersSelected;
        }
        private void OnNewTrousersSelected(Composite comp)
        {
            trousersComposite.Text = comp.name;
            _accessories.trousers_composite = comp.shortGUID;
        }
        private void selectNewShoes_Click(object sender, EventArgs e)
        {
            CompositeSelector(shoesComposite.Text).OnCompositeGenerated += OnNewShoesSelected;
        }
        private void OnNewShoesSelected(Composite comp)
        {
            shoesComposite.Text = comp.name;
            _accessories.shoes_composite = comp.shortGUID;
        }
        private void selectNewCollision_Click(object sender, EventArgs e)
        {
            CompositeSelector(collisionComposite.Text).OnCompositeGenerated += OnNewCollisionSelected;
        }
        private void OnNewCollisionSelected(Composite comp)
        {
            collisionComposite.Text = comp.name;
            _accessories.collision_composite = comp.shortGUID;
        }

        private void gender_SelectedIndexChanged(object sender, EventArgs e)
        {
            _accessories.body_skeleton = gender.Text;
            RefreshSkeletonsForGender();
        }

        private void bodyTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            _accessories.face_skeleton = bodyTypes.Text;
        }

        private void shirtDecal_SelectedIndexChanged(object sender, EventArgs e)
        {
            _accessories.decal = (CharacterAccessorySets.Entry.Decal)shirtDecal.SelectedIndex;
        }
    }
}
