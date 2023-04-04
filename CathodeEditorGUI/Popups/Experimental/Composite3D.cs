using CATHODE.Scripting.Internal;
using CATHODE.Scripting;
using CommandsEditor.Popups.UserControls;
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
using System.Numerics;
using CommandsEditor.Popups.Base;

namespace CommandsEditor
{
    public partial class Composite3D : BaseWindow
    {
        GUI_ModelViewer modelViewer;

        public Composite3D(CommandsEditor editor, Composite comp) : base(WindowClosesOn.NONE, editor)
        {
            InitializeComponent();
            this.Text += ": " + comp.name;

            List<GUI_ModelViewer.Model> models = new List<GUI_ModelViewer.Model>();
            models.AddRange(LoadComposite(comp));

            modelViewer = new GUI_ModelViewer(_editor);
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
                        Vector3 positionOffset = (offset == null) ? new Vector3() : new Vector3(offset.position.X, offset.position.Y, offset.position.Z);
                        if (positionParameter != null) positionOffset += ((cTransform)positionParameter.content).position;
                        Vector3 rotationOffset = (offset == null) ? new Vector3() : new Vector3(offset.rotation.X, offset.rotation.Y, offset.rotation.Z);
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

                    Vector3 positionOffset = (offset == null) ? new Vector3() : new Vector3(offset.position.X, offset.position.Y, offset.position.Z);
                    if (positionParameter != null) positionOffset += ((cTransform)positionParameter.content).position;
                    Vector3 rotationOffset = (offset == null) ? new Vector3() : new Vector3(offset.rotation.X, offset.rotation.Y, offset.rotation.Z);
                    if (positionParameter != null) rotationOffset += ((cTransform)positionParameter.content).rotation;

                    models.Add(new GUI_ModelViewer.Model(Editor.resource.reds.Entries[resource.startIndex].ModelIndex, positionOffset, rotationOffset));
                }
            }
            return models;
        }
    }
}
