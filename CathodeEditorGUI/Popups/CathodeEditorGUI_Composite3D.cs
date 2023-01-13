using CATHODE.Scripting.Internal;
using CATHODE.Scripting;
using CathodeEditorGUI.Popups.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using CathodeLib;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_Composite3D : Form
    {
        GUI_ModelViewer modelViewer;

        public CathodeEditorGUI_Composite3D(Composite comp)
        {
            InitializeComponent();
            this.Text += ": " + comp.name;

            List<GUI_ModelViewer.Model> models = new List<GUI_ModelViewer.Model>();
            models.AddRange(LoadComposite(comp));

            modelViewer = new GUI_ModelViewer();
            modelRendererHost.Child = modelViewer;
            modelViewer.ShowModel(models);
        }

        private List<GUI_ModelViewer.Model> LoadComposite(Composite comp, cTransform offset = null)
        {
            List<GUI_ModelViewer.Model> models = new List<GUI_ModelViewer.Model>();
            if (comp == null) return models;

            List<Entity> entities = comp.GetEntities();
            foreach (Entity entity in entities)
            {
                Parameter positionParameter = entity.GetParameter("position");
                if (entity.variant == EntityVariant.FUNCTION)
                {
                    FunctionEntity function = (FunctionEntity)entity;
                    if (!CommandsUtils.FunctionTypeExists(function.function))
                    {
                        Vector3 positionOffset = (offset == null) ? new Vector3() : new Vector3(offset.position.x, offset.position.y, offset.position.z);
                        if (positionParameter != null) positionOffset += ((cTransform)positionParameter.content).position;
                        Vector3 rotationOffset = (offset == null) ? new Vector3() : new Vector3(offset.rotation.x, offset.rotation.y, offset.rotation.z);
                        if (positionParameter != null) rotationOffset += ((cTransform)positionParameter.content).rotation;

                        cTransform newOffset = new cTransform(positionOffset, rotationOffset);
                        models.AddRange(LoadComposite(Editor.commands.GetComposite(function.function), newOffset));
                    }
                }

                Parameter resourceParameter = entity.GetParameter("resource");
                if (resourceParameter == null) continue;
                List<ResourceReference> resourceRefs = ((cResource)(resourceParameter.content)).value;
                foreach (ResourceReference resource in resourceRefs)
                {
                    if (resource.entryType != ResourceType.RENDERABLE_INSTANCE) continue;

                    //Convert model BIN index from REDs to PAK index
                    int pakModelIndex = -1;
                    for (int y = 0; y < Editor.resource.models.Models.Count; y++)
                    {
                        for (int z = 0; z < Editor.resource.models.Models[y].Submeshes.Count; z++)
                        {
                            if (Editor.resource.models.Models[y].Submeshes[z].binIndex == Editor.resource.reds.RenderableElements[resource.startIndex].ModelIndex)
                            {
                                pakModelIndex = y;
                                break;
                            }
                        }
                        if (pakModelIndex != -1) break;
                    }

                    Vector3 positionOffset = (offset == null) ? new Vector3() : new Vector3(offset.position.x, offset.position.y, offset.position.z);
                    if (positionParameter != null) positionOffset += ((cTransform)positionParameter.content).position;
                    Vector3 rotationOffset = (offset == null) ? new Vector3() : new Vector3(offset.rotation.x, offset.rotation.y, offset.rotation.z);
                    if (positionParameter != null) rotationOffset += ((cTransform)positionParameter.content).rotation;

                    models.Add(new GUI_ModelViewer.Model(pakModelIndex, positionOffset, rotationOffset));
                }
            }
            return models;
        }
    }
}
