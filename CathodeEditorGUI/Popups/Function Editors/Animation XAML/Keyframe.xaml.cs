using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace TimelineFramework
{
    public partial class Keyframe : UserControl
    {
        public float Seconds { get { return seconds; } }
        float seconds;

        Timeline parent;
        double canvasLeft;
        double mouseXInitial;
        bool primed = false;

        public Keyframe(Timeline parent, float seconds)
        {
            InitializeComponent();

            this.seconds = seconds;
            this.parent = parent;

            // Setup for draggability
            this.MouseEnter += TimelineElement_MouseEnter;
            this.MouseLeave += TimelineElement_MouseLeave;
            this.MouseLeftButtonDown += TimelineElement_MouseLeftButtonDown;
        }

        // Called by the parent to give it updated seconds based on its position
        public void SetSeconds(float seconds)
        {
            this.seconds = seconds;
            Console.WriteLine(seconds);
            diamond.ToolTip = seconds.ToString("0.00") + "s";
        }

        // Listeners
        private void TimelineElement_MouseEnter(object sender, MouseEventArgs e)
        {
            diamond.Opacity = 0.7;
            primed = true;
        }
        private void TimelineElement_MouseLeave(object sender, MouseEventArgs e)
        {
            diamond.Opacity = 0.5;
            primed = false;
        }
        private void TimelineElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (primed)
            {
                diamond.Opacity = 1;

                // Enter dragging
                canvasLeft = Canvas.GetLeft(this);
                mouseXInitial = Mouse.GetPosition(parent).X;
                parent.MouseMove += Parent_MouseMove;
                parent.MouseLeftButtonUp += Parent_MouseLeftButtonUp;
                this.ToolTip = "";
            }
        }

        // Dragging handler
        private void Parent_MouseMove(object sender, MouseEventArgs e)
        {
            double diff = mouseXInitial - Mouse.GetPosition(parent).X;
            Canvas.SetLeft(this, canvasLeft - diff);
            if (Canvas.GetLeft(this) > parent.pixelDistance - 2)
                Canvas.SetLeft(this, parent.pixelDistance - 2);
            if (Canvas.GetLeft(this) < -2)
                Canvas.SetLeft(this, -2);
        }
        private void Parent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            diamond.Opacity = 0.5;

            // Reset to default 'stance'
            primed = false;
            parent.MouseMove -= Parent_MouseMove;
            parent.MouseLeftButtonUp -= Parent_MouseLeftButtonUp;
            parent.RefreshElement(this); // Notify parent to give this element new data about its position in the timeline in seconds
        }
    }
}
