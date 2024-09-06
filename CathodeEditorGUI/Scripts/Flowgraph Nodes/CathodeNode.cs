using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using ST.Library.UI.NodeEditor;
using System;
using System.Drawing;
using System.Linq;

namespace CommandsEditor.Nodes
{
	//[STNode("/")]
	public class CathodeNode : STNode
	{
		public ShortGuid ShortGUID;

		protected override void OnCreate()
		{
            LetGetOptions = true;
            Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, (byte)0); 
            base.OnCreate();
		}

        public void Recompute()
        {
            this.SetOptionsLocation();
            this.BuildSize(false, true, false);
            this.OnResize(EventArgs.Empty);
            this.Invalidate();
        }

		public void SetName(string name, string subtitle = "")
		{
            int height = 20;
            if (subtitle != "")
                height = 35;

            Title = name;
            SubTitle = subtitle;
            TitleHeight = height;
        }

        public void SetColour(Color colourBG, Color colourFG)
        {
            TitleColor = colourBG;
            ForeColor = colourFG;
        }

        public void SetPosition(Point location)
        {
            Location = location;
        }

		public void AddOptions(ShortGuid[] inputOptions, ShortGuid[] outputOptions)
        {
            if (inputOptions != null)
                for (int i = 0; i < inputOptions.Length; i++)
                    AddInputOption(inputOptions[i]);
            if (outputOptions != null)
                for (int i = 0; i < outputOptions.Length; i++)
                    AddOutputOption(outputOptions[i]);
        }

        public STNodeOption AddInputOption(ShortGuid option, bool unique = false)
        {
            if (!unique)
                for (int i = 0; i < this.InputOptions.Count; i++)
                    if (this.InputOptions[i].Text == option.ToString())
                        return this.InputOptions[i];

            return this.InputOptions.Add(option, typeof(void), false);
        }
        public STNodeOption AddOutputOption(ShortGuid option, bool unique = false)
        {
            if (!unique)
                for (int i = 0; i < this.OutputOptions.Count; i++)
                    if (this.OutputOptions[i].Text == option.ToString())
                        return this.OutputOptions[i];

            return this.OutputOptions.Add(option, typeof(void), false);
        }

        public static CathodeNode CreateNode(STNodeEditor editor, Entity entity, Composite composite, CATHODE.Commands commands, bool allowDuplicate = false, int spawnOffset = 10)
        {
            if (entity == null)
                return null;

            CathodeNode node = null;
            if (!allowDuplicate)
            {
                for (int i = 0; i < editor.Nodes.Count; i++)
                {
                    if (!(editor.Nodes[i] is CathodeNode))
                        continue;

                    CathodeNode thisNode = (CathodeNode)editor.Nodes[i];
                    if (thisNode.ShortGUID != entity.shortGUID)
                        continue;

                    node = thisNode;
                    break;
                }
            }

            if (node == null)
            {
                node = new CathodeNode();
                node.ShortGUID = entity.shortGUID;
                switch (entity.variant)
                {
                    case EntityVariant.PROXY:
                    case EntityVariant.ALIAS:
                        Entity ent = CommandsUtils.ResolveHierarchy(commands, composite, (entity.variant == EntityVariant.PROXY) ? ((ProxyEntity)entity).proxy.path : ((AliasEntity)entity).alias.path, out Composite c, out string s);
                        node.SetColour(entity.variant == EntityVariant.PROXY ? Color.LightGreen : Color.Orange, Color.Black);
                        switch (ent.variant)
                        {
                            case EntityVariant.FUNCTION:
                                FunctionEntity function = (FunctionEntity)ent;
                                if (CommandsUtils.FunctionTypeExists(function.function))
                                {
                                    node.SetName(EntityUtils.GetName(c, ent), entity.variant + " TO: " + function.function.ToString());
                                }
                                else
                                    node.SetName(EntityUtils.GetName(c, ent), entity.variant + " TO: " + commands.GetComposite(function.function).name);
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
                            node.SetName(EntityUtils.GetName(composite, entity), funcEnt.function.ToString());
                        }
                        else
                        {
                            node.SetColour(Color.Blue, Color.White);
                            node.SetName(EntityUtils.GetName(composite, entity), commands.GetComposite(funcEnt.function).name);
                        }
                        break;
                    case EntityVariant.VARIABLE:
                        node.SetColour(Color.Red, Color.White);
                        node.SetName(((VariableEntity)entity).name.ToString());
                        break;
                }
                node.Recompute();
                editor.Nodes.Add(node);

                ((CathodeNode)node).SetPosition(new Point(0, spawnOffset));
                spawnOffset += node.Height + 10;
            }

            return node;
        }
    }
}
