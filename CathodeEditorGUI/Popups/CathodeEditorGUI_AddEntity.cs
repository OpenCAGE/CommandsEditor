using CATHODE;
using CATHODE.Commands;
using CathodeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_AddEntity : Form
    {
        public CathodeEntity NewEntity = null;

        CathodeComposite composite = null;
        List<CathodeComposite> composites = null;
        List<CathodeEntityDatabase.EntityDefinition> availableEntities = null;
        public CathodeEditorGUI_AddEntity(CathodeComposite _comp, List<CathodeComposite> _comps)
        {
            composite = _comp;
            composites = _comps.OrderBy(o => o.name).ToList();
            availableEntities = CathodeEntityDatabase.GetEntities();
            InitializeComponent();

            //quick hack to reload dropdown
            createDatatypeEntity.Checked = true;
            createFunctionEntity.Checked = true;
        }

        //Repopulate UI
        private void selectedDatatypeEntity(object sender, EventArgs e)
        {
            //Datatype
            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            entityVariant.Items.AddRange(new object[] {
                                    "POSITION",
                                    "FLOAT",
                                    "STRING",
                                    "SPLINE_DATA",
                                    "ENUM",
                                    "SHORT_GUID",
                                    "FILEPATH",
                                    "BOOL",
                                    "DIRECTION",
                                    "INTEGER"
            });
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
        }
        private void selectedFunctionEntity(object sender, EventArgs e)
        {
            //Function
            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            for (int i = 0; i < availableEntities.Count; i++) entityVariant.Items.Add(availableEntities[i].className);
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
        }
        private void selectedCompositeEntity(object sender, EventArgs e)
        {
            //Composite
            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            for (int i = 0; i < composites.Count; i++) entityVariant.Items.Add(composites[i].name);
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
        }

        private void createEntity(object sender, EventArgs e)
        {
            cGUID thisID = Utilities.GenerateGUID(DateTime.Now.ToString("G"));

            if (createDatatypeEntity.Checked)
            {
                //Make the DatatypeEntity
                DatatypeEntity newEntity = new DatatypeEntity(thisID);
                newEntity.type = (CathodeDataType)entityVariant.SelectedIndex;
                newEntity.parameter = Utilities.GenerateGUID(textBox1.Text);

                //Make the parameter to give this DatatypeEntity a value (the only time you WOULDN'T want this is if the val is coming from a linked entity)
                CathodeParameter thisParam = null;
                switch (newEntity.type)
                {
                    case CathodeDataType.POSITION:
                        thisParam = new CathodeTransform();
                        break;
                    case CathodeDataType.FLOAT:
                        thisParam = new CathodeFloat();
                        break;
                    case CathodeDataType.FILEPATH:
                    case CathodeDataType.STRING:
                        thisParam = new CathodeString();
                        break;
                    case CathodeDataType.SPLINE_DATA:
                        thisParam = new CathodeSpline();
                        break;
                    case CathodeDataType.ENUM:
                        thisParam = new CathodeEnum();
                        ((CathodeEnum)thisParam).enumID = new cGUID("4C-B9-82-48"); //ALERTNESS_STATE is the first alphabetically
                        break;
                    case CathodeDataType.SHORT_GUID:
                        thisParam = new CathodeResource();
                        ((CathodeResource)thisParam).resourceID = new cGUID("00-00-00-00");
                        break;
                    case CathodeDataType.BOOL:
                        thisParam = new CathodeBool();
                        break;
                    case CathodeDataType.DIRECTION:
                        thisParam = new CathodeVector3();
                        break;
                    case CathodeDataType.INTEGER:
                        thisParam = new CathodeInteger();
                        break;
                }
                newEntity.parameters.Add(new CathodeLoadedParameter(newEntity.parameter, thisParam));

                //Add to composite & save name
                composite.datatypes.Add(newEntity);
                if (EntityDB.GetCathodeName(newEntity.parameter) == newEntity.parameter.ToString())
                    EntityDBEx.AddNewParameterName(newEntity.parameter, textBox1.Text);
                NewEntity = newEntity;
            }
            else if (createFunctionEntity.Checked)
            {
                //Create FunctionEntity
                FunctionEntity newEntity = new FunctionEntity(thisID);
                switch (entityVariant.Text)
                {
                    //TODO: find a nicer way of auto selecting this (E.G. can we reflect to class names?)
                    case "CAGEAnimation":
                        newEntity = new CAGEAnimation(thisID);
                        break;
                    case "TriggerSequence":
                        newEntity = new TriggerSequence(thisID);
                        break;
                }
                newEntity.function = CathodeEntityDatabase.GetEntityAtIndex(entityVariant.SelectedIndex).guid;
                //TODO: auto populate params here

                //Add to composite & save name
                composite.functions.Add(newEntity);
                EntityDBEx.AddNewEntityName(thisID, textBox1.Text);
                NewEntity = newEntity;
            }
            else if (createCompositeEntity.Checked)
            {
                //Create FunctionEntity
                FunctionEntity newEntity = new FunctionEntity(thisID);
                CathodeComposite composite = composites.FirstOrDefault(o => o.name == entityVariant.Text);
                if (composite == null)
                { 
                    MessageBox.Show("Failed to look up composite!\nPlease report this issue on GitHub.\n\n" + entityVariant.Text, "Could not find composite!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                newEntity.function = composite.shortGUID;

                //Add to composite & save name
                this.composite.functions.Add(newEntity);
                EntityDBEx.AddNewEntityName(thisID, textBox1.Text);
                NewEntity = newEntity;
            }

            this.Close();
        }
    }
}
