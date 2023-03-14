using System.Windows.Controls;
using System.Windows.Input;

namespace TimelineFramework
{
    public partial class Track : UserControl
    {
        public int Index { get { return _trackIndex; } }
        int _trackIndex;

        public Track(double width, int track)
        {
            InitializeComponent();
            _trackIndex = track;
            rectOuter.Width = width;
        }
    }
}
