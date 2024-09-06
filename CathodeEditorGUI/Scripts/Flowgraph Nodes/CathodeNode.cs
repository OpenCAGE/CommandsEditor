using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;
using System;
using System.Drawing;
using System.Linq;

namespace CommandsEditor.Nodes
{
	//[STNode("/")]
	public class CathodeNode : STNode
	{
		public ShortGuid ID;

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

		public void AddOptions(string[] inputOptions, string[] outputOptions)
        {
            if (inputOptions != null)
                for (int i = 0; i < inputOptions.Length; i++)
                    AddInputOption(inputOptions[i]);
            if (outputOptions != null)
                for (int i = 0; i < outputOptions.Length; i++)
                    AddOutputOption(outputOptions[i]);
        }

        public STNodeOption AddInputOption(string option, bool unique = false)
        {
            if (!unique)
                for (int i = 0; i < this.InputOptions.Count; i++)
                    if (this.InputOptions[i].Text == option)
                        return this.InputOptions[i];

            return this.InputOptions.Add(option, typeof(void), false);
        }
        public STNodeOption AddOutputOption(string option, bool unique = false)
        {
            if (!unique)
                for (int i = 0; i < this.OutputOptions.Count; i++)
                    if (this.OutputOptions[i].Text == option)
                        return this.OutputOptions[i];

            return this.OutputOptions.Add(option, typeof(void), false);
        }
    }
}
