using CATHODE;
using CATHODE.Scripting;
using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandsEditor
{
    public partial class CharacterEditor : BaseWindow
    {
        private List<EntityHierarchy> _hierarchies;
        private CharacterAccessorySets.Entry _accessories;

        public CharacterEditor(CommandsEditor editor) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor)
        {
            InitializeComponent();

            _hierarchies = CommandsUtils.GenerateHierarchies(Editor.commands, Editor.selected.composite, Editor.selected.entity);
            characterInstances.Items.Clear();
            for (int i = 0; i < _hierarchies.Count; i++)
            {
                characterInstances.Items.Add(_hierarchies[i].GetHierarchyAsString(Editor.commands, Editor.selected.composite));
            }
            characterInstances.SelectedIndex = 0;

            editCharacter_Click(null, null);
        }

        private void editCharacter_Click(object sender, EventArgs e)
        {
            ShortGuid hierarchyID = _hierarchies[characterInstances.SelectedIndex].GenerateInstance();

            _accessories = Editor.resource.character_accessories.Entries.FirstOrDefault(o => o.character.composite_instance_id == hierarchyID);
            if (_accessories == null)
            {
                _accessories = new CharacterAccessorySets.Entry();
                Editor.resource.character_accessories.Entries.Add(_accessories);
            }

            shirtComposite.Text = Editor.commands.GetComposite(_accessories.shirt_composite)?.name;
            trousersComposite.Text = Editor.commands.GetComposite(_accessories.trousers_composite)?.name;
            shoesComposite.Text = Editor.commands.GetComposite(_accessories.shoes_composite)?.name;
            headComposite.Text = Editor.commands.GetComposite(_accessories.head_composite)?.name;
            armsComposite.Text = Editor.commands.GetComposite(_accessories.arms_composite)?.name;
            collisionComposite.Text = Editor.commands.GetComposite(_accessories.collision_composite)?.name;

            //TODO: need to populate this dropdown from CUSTOMCHARACTERINFO.BIN
            bodyTypes.Text = _accessories.body_type;
            skeletons.Text = _accessories.skeleton;
        }

        private SelectComposite CompositeSelector(string composite)
        {
            SelectComposite selectComposite = new SelectComposite(_editor, composite);
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

        private void bodyTypes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void skeletons_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
