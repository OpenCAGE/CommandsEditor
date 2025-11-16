using CATHODE.Scripting;
using System.Collections.Generic;
using CommandsEditor.Popups.Base;
using CommandsEditor.DockPanels;
using CathodeLib;
using CATHODE;
using System.Linq;
using System;
using System.Windows.Forms.Design;
using System.IO;
using System.Windows.Forms;
using CathodeLib.ObjectExtensions;
using System.Diagnostics;

namespace CommandsEditor
{
    public partial class ExportComposite : BaseWindow
    {
        Composite _composite;

        public ExportComposite(Composite composite, bool canExportChildren) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            _composite = composite;

            InitializeComponent();

            levelList.BeginUpdate();
            levelList.Items.AddRange(Level.GetLevels(SharedData.pathToAI, true).ToArray());
            levelList.Items.Remove(Content.Level.Name);
            levelList.EndUpdate();

            levelList.SelectedIndex = 0;

            this.Text = "Port '" + _composite.name + "'";
            
            if (!canExportChildren)
            {
                recurse.Checked = false;
                recurse.Enabled = false;
            }

            MessageBox.Show("Warning! This is a highly experimental feature which is not yet complete. Please use with caution! Take backups of any levels you plan to copy content to.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void export_Click(object sender, System.EventArgs e)
        {
            Log("Loading data for " + levelList.SelectedItem.ToString() + "...");
            Level lvl = new Level(SharedData.pathToAI + "/DATA/ENV/" + levelList.SelectedItem.ToString(), Singleton.Global);

            Log("Starting export...");
            AddCompositesRecursively(_composite, lvl);
            
            //Close alien down if it's open, it conflicts with our write locks!
            List<Process> allProcesses = new List<Process>(Process.GetProcessesByName("AI"));
            for (int x = 0; x < allProcesses.Count; x++)
            {
                try
                {
                    allProcesses[x].Kill();
                    allProcesses[x].WaitForExit();
                }
                catch { }
            }

            Log("Performing final save for " + levelList.SelectedItem.ToString() + "...");
            lvl.Save();

            MessageBox.Show("Finished porting '" + _composite.name + "' to '" + levelList.SelectedItem.ToString() + "'!", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void AddCompositesRecursively(Composite composite, Level lvl)
        {
            //Check to see if the composite already exists at our destination
            Composite dest = lvl.Commands.Entries.FirstOrDefault(o => o.shortGUID == composite.shortGUID);

            //If the user opted to overwrite & we found an existing matching comp in the destination, delete it
            if (overwrite.Checked)
            {
                if (dest != null)
                    lvl.Commands.Entries.Remove(dest);
                dest = null;
            }

            //TODO: reduce duplication here 

            if (dest == null)
            {
                //We need to add the composite to the new location
                Log("Copying " + composite.name);
                Composite copiedComp = composite.Copy();
                lvl.Commands.Entries.Add(copiedComp);

                //Check to see if we should copy resources to the destination, and point to them if we do
                foreach (FunctionEntity ent in copiedComp.functions)
                {
                    for (int i = 0; i < ent.resources.Count; i++)
                    {
                        switch (ent.resources[i].resource_type)
                        {
                            case ResourceType.ANIMATED_MODEL:
                                CopyAnimatedModel(lvl, ent.resources[i]);
                                break;
                            case ResourceType.RENDERABLE_INSTANCE:
                                CopyRenderableInstance(lvl, ent.resources[i]);
                                break;
                            case ResourceType.COLLISION_MAPPING:
                                CopyCollisionResource(lvl, ent.resources[i]);
                                break;
                        }
                    }

                    Parameter resources = ent.GetParameter("resource");
                    if (resources != null)
                    {
                        List<ResourceReference> resourceRefs = ((cResource)resources.content).value;
                        for (int i = 0; i < resourceRefs.Count; i++)
                        {
                            switch (resourceRefs[i].resource_type)
                            {
                                case ResourceType.ANIMATED_MODEL:
                                    CopyAnimatedModel(lvl, resourceRefs[i]);
                                    break;
                                case ResourceType.RENDERABLE_INSTANCE:
                                    CopyRenderableInstance(lvl, resourceRefs[i]);
                                    break;
                                case ResourceType.COLLISION_MAPPING:
                                    CopyCollisionResource(lvl, resourceRefs[i]);
                                    break;
                            }
                        }
                    }
                }
            }

            //TODO: Also take across associated OpenCAGE metadata!

            //If the user opted to recurse, follow any composite instances through, and copy those too
            if (!recurse.Checked) return;
            foreach (FunctionEntity ent in composite.functions)
            {
                if (ent.function.IsFunctionType) continue;

                Composite nestedComp = Content.Level.Commands.GetComposite(ent.function);
                if (nestedComp != null)
                    AddCompositesRecursively(nestedComp, lvl);
            }
        }

        private List<string> log = new List<string>();
        private void Log(string msg)
        {
            log.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + msg);

            richTextBox1.Text = "";
            for (int i = log.Count - 1;  i >= 0; i--)
            {
                richTextBox1.Text += log[i] + "\n";
            }
            richTextBox1.Refresh();
        }

        private void CopyRenderableInstance(Level lvl, ResourceReference resourceRef)
        {
            Log("Exporting " + resourceRef.RenderableInstance.Count + " RENDERABLE_INSTANCE resource(s)...");
            resourceRef.RenderableInstance = lvl.RenderableElements.AddEntry(resourceRef.RenderableInstance, Content.Level.Models);
        }

        private void CopyAnimatedModel(Level lvl, ResourceReference resourceRef)
        {
            Log("Exporting ANIMATED_MODEL resource...");
            int resourceIndex = lvl.EnvironmentAnimations.Entries.Count;
            resourceRef.AnimatedModel = lvl.EnvironmentAnimations.AddEntry(resourceRef.AnimatedModel);
            resourceRef.AnimatedModel.ResourceIndex = resourceIndex; //TODO: would be good to just handle this at build time
        }

        private void CopyCollisionResource(Level lvl, ResourceReference resourceRef)
        {
            Log("Exporting COLLISION_MAPPING resource...");
            resourceRef.CollisionMapping = lvl.CollisionMaps.AddEntry(resourceRef.CollisionMapping);
        }
    }
}
