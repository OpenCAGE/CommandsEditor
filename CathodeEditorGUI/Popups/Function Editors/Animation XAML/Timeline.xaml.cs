using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TimelineFramework
{
    /// <summary>
    /// Interaction logic for Timeline.xaml
    /// </summary>
    public partial class Timeline : UserControl
    {
        List<TimeMarker> _timeMarkers = new List<TimeMarker>();
        List<Keyframe> _keyframes = new List<Keyframe>();
        List<Track> _tracks = new List<Track>();
        Border _border;

        int width;
        int height;
        int innerHeight;
        int elementTop;
        int spacing;
        float startSeconds;
        float endSeconds;
        internal int pixelDistance;

        public Timeline(int w, int h)
        {
            InitializeComponent();

            width = w;
            height = h;
            border.Width = w;
            border.Height = h;
        }

        public void AddElement(float seconds, int trackIndex)
        {
            double trackOffset = elementTop + (trackIndex * 20.0f);

            Track track = _tracks.FirstOrDefault(o => o.Index == trackIndex);
            if (track == null)
            {
                track = new Track(_border.Width, trackIndex);
                _tracks.Add(track);
                mainCanvas.Children.Add(track);
                Canvas.SetTop(track, trackOffset + 5);
                Canvas.SetLeft(track, 0);
                Canvas.SetZIndex(track, 0);
            }

            Keyframe key = new Keyframe(this, seconds);
            _keyframes.Add(key);
            mainCanvas.Children.Add(key);
            Canvas.SetTop(key, trackOffset);
            Canvas.SetLeft(key, (pixelDistance * (seconds - startSeconds) / (endSeconds - startSeconds)) - 2);
            Canvas.SetZIndex(track, 1);
        }

        public void RefreshElement(Keyframe key)
        {
            key.SetSeconds((float)((Canvas.GetLeft(key) + 2) * (endSeconds - startSeconds) / pixelDistance) + startSeconds);
        }

        public void Setup(float startSeconds, float endSeconds, float intervalSeconds, int spacing)
        {
            this.startSeconds = startSeconds;
            this.endSeconds = endSeconds;
            this.spacing = spacing;

            // Create first mark
            TimeMarker tmStart = new TimeMarker(startSeconds);
            _timeMarkers.Add(tmStart);
            mainCanvas.Children.Add(tmStart);

            // Create middle marks
            int intervalCount = (int)(((endSeconds - startSeconds) / intervalSeconds) - 1);
            for (int i = 1; i <= intervalCount; i++)
            {
                TimeMarker tm = new TimeMarker(startSeconds + (intervalSeconds * i));
                _timeMarkers.Add(tm);
                mainCanvas.Children.Add(tm);
            }

            // Create last mark
            TimeMarker tmEnd = new TimeMarker(endSeconds);
            _timeMarkers.Add(tmEnd);
            mainCanvas.Children.Add(tmEnd);

            // Setup spacing
            for (int k = 0; k < _timeMarkers.Count; k++)
            {
                Canvas.SetLeft(_timeMarkers[k], spacing * k);
                Canvas.SetTop(_timeMarkers[k], 1);
            }

            // Size & place the controls
            Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Arrange(new Rect(0, 0, width, height));

            _border = new Border();
            _border.BorderThickness = new Thickness(1);
            _border.BorderBrush = new SolidColorBrush(Color.FromRgb(12, 12, 12));
            _border.Background = new SolidColorBrush(Color.FromRgb(248, 248, 248));
            mainCanvas.Children.Add(_border);
            Canvas.SetTop(_border, 1 + tmStart.ActualHeight);
            elementTop = 1 + (int)tmStart.ActualHeight + 1; // Canvas.Top value for TimelineElements
            _border.Width = 1 + Canvas.GetLeft(tmEnd);
            _border.Height = height - 46; // To account for TimelineMark height & scrollbar height. This value assumes the height of the Aero-style scrollbar.
            innerHeight = height - 46 - 2; // Height of region inside the border

            pixelDistance = (int)_border.Width - 1; // Region of the border aka the timeline's length in pixels
            mainCanvas.Width = (spacing * (_timeMarkers.Count - 1)) + (int)tmEnd.ActualWidth; // Set the canvas's width so the ScrollViewer knows how big it is
        }
    }
}
