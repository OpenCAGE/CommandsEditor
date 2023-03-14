using System.Windows.Controls;

namespace TimelineFramework
{
    public partial class TimeMarker : UserControl
    {
        public TimeMarker(float seconds)
        {
            InitializeComponent();
            this.text.Text = seconds.ToString("0.00") + "s";
        }
    }
}
