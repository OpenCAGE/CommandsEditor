using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;
using System;
using System.Drawing;
using System.Linq;

namespace CommandsEditor.Nodes
{
	//[STNode("/")]
	public class CustomNode : STNode
	{
		public ShortGuid ID;

		protected override void OnCreate()
		{
			base.OnCreate();
		}

        public void Recompute()
        {
            this.SetOptionsLocation();
            this.BuildSize(false, true, false);
            this.OnResize(EventArgs.Empty);
            this.Invalidate();
        }

		public void SetName(string name)
		{
            Title = name;
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

        public STNodeOption AddInputOption(string option)
        {
            return this.InputOptions.Add(option, typeof(void), false);
        }
        public STNodeOption AddOutputOption(string option)
        {
            return this.OutputOptions.Add(option, typeof(void), false);
        }
    }
}
