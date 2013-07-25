using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System;

namespace FocalLengthCropper.Controls
{
    class FocalLengthCrop : Border
    {
        private UIElement child = null;
        private Point start;
        private Point origin;

        private double zoom = 1;
        public double Zoom
        {
            get
            {
                return zoom;
            }
            set
            {
                zoom = value;
                if (child != null)
                {
                    // reset zoom
                    var st = GetScaleTransform(child);
                    st.ScaleX = zoom;
                    st.ScaleY = zoom;
                }
            }
        }

        private TranslateTransform GetTranslateTransform(UIElement element)
        {
            return (TranslateTransform)((TransformGroup)element.RenderTransform).Children.First(tr => tr is TranslateTransform);
        }

        private ScaleTransform GetScaleTransform(UIElement element)
        {
            return (ScaleTransform)((TransformGroup)element.RenderTransform).Children.First(tr => tr is ScaleTransform);
        }

        public override UIElement Child
        {
            get { return base.Child; }
            set
            {
                if (!(value is Image))
                {
                    throw new ArgumentException("Only Image controls allowed");
                }

                if (value != null && value != this.Child)
                {
                    this.Initialize(value);
                }
                base.Child = value;
            }
        }

        public void Initialize(UIElement element)
        {
            this.child = element;
            if (child != null)
            {
                TransformGroup group = new TransformGroup();
                ScaleTransform st = new ScaleTransform();
                group.Children.Add(st);
                TranslateTransform tt = new TranslateTransform();
                group.Children.Add(tt);
                child.RenderTransform = group;
                child.RenderTransformOrigin = new Point(0.0, 0.0);
                this.MouseLeftButtonDown += child_MouseLeftButtonDown;
                this.MouseLeftButtonUp += child_MouseLeftButtonUp;
                this.MouseMove += child_MouseMove;
                //this.PreviewMouseRightButtonDown += new MouseButtonEventHandler(child_PreviewMouseRightButtonDown);
            }
        }

        public void Reset()
        {
            if (child != null)
            {
                // reset zoom
                var st = GetScaleTransform(child);
                st.ScaleX = 1.0;
                st.ScaleY = 1.0;

                // reset pan
                var tt = GetTranslateTransform(child);
                tt.X = 0.0;
                tt.Y = 0.0;
            }
        }

        public void Crop(double originFocalLength, double cropToFocalLength)
        {
            if (child != null)
            {
                var st = GetScaleTransform(child);
                var tt = GetTranslateTransform(child);

                this.Zoom = cropToFocalLength / originFocalLength;
            }
        }

        #region Child Events

        private void child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (child != null && this.Zoom > 1)
            {
                var tt = GetTranslateTransform(child);
                this.start = e.GetPosition(this);
                this.origin = new Point(tt.X, tt.Y);
                this.Cursor = Cursors.SizeAll;
                child.CaptureMouse();
            }
        }

        private void child_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (child != null)
            {
                child.ReleaseMouseCapture();
                this.Cursor = Cursors.Arrow;
            }
        }

        void child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Reset();
        }

        private void child_MouseMove(object sender, MouseEventArgs e)
        {
            if (child != null)
            {
                if (child.IsMouseCaptured)
                {
                    var tt = GetTranslateTransform(child);
                    Vector v = start - e.GetPosition(this);
                    tt.X = origin.X - v.X;
                    tt.Y = origin.Y - v.Y;
                }
            }
        }

        #endregion
    }
}
