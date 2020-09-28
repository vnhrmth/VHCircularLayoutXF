using System;
using Xamarin.Forms;

namespace VHCircularLayoutXF
{
    public class MyLayout : Layout<View>
    {
        public string Radius
        {
            get => (string)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public string Angle
        {
            get => (string)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public bool HideEmptyCells
        {
            get => (bool)GetValue(HideEmptyCellsProperty);
            set => SetValue(HideEmptyCellsProperty, value);
        }

        public static BindableProperty RadiusProperty = BindableProperty.Create("Radius",
            typeof(string),
            typeof(MyLayout), "10", propertyChanged: (bindable, oldValue, newValue) =>
             {
                 ((MyLayout)bindable).InvalidateLayout();
             });

        public static readonly BindableProperty AngleProperty = BindableProperty.Create("Angle",
            typeof(string),
            typeof(MyLayout), "30", propertyChanged: (bindable, oldValue, newValue) =>
            {
                ((MyLayout)bindable).InvalidateLayout();
            });

        public static readonly BindableProperty HideEmptyCellsProperty = BindableProperty.Create("HideEmptyCells",
            typeof(bool),
            typeof(MyLayout), false, propertyChanged: (bindable, oldValue, newValue) =>
            {
                ((MyLayout)bindable).InvalidateLayout();
            });

        public event EventHandler<ScrollToRequestedEventArgs> ScrollToRequested;

        public Rectangle LayoutAreaOverride { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            Size reqSize = new Size();
            Size minSize = new Size();

            // Enumerate through all the children.
            foreach (View child in Children)
            {
                // Skip the invisible children.
                if (!child.IsVisible)
                    continue;

                // Get the child's requested size.
                SizeRequest childSizeRequest = child.Measure(widthConstraint,
                Double.PositiveInfinity);

                // Find the maximum width and accumulate the height.
                reqSize.Width = Math.Max(reqSize.Width, childSizeRequest.Request.Width);
                reqSize.Height += childSizeRequest.Request.Height;

                // Do the same for the minimum size request.
                minSize.Width = Math.Max(minSize.Width, childSizeRequest.Minimum.Width);
                minSize.Height += childSizeRequest.Minimum.Height;
            }
            return new SizeRequest(reqSize, minSize);
        }


        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            double initialAngle = 0;
            double radius = 10;
            double requestedAngle = 0;
            double.TryParse(Angle, out requestedAngle);

            double.TryParse(Radius, out radius);

            foreach (View child in Children)
            {
                // Skip the invisible children.
                if (!child.IsVisible)
                    continue;
                // Get the child's requested size.
                SizeRequest childSizeRequest = child.Measure(width, Double.PositiveInfinity);
                // Initialize child position and size.
                double xChild = x;
                double yChild = y;
                double childWidth = childSizeRequest.Request.Width;
                double childHeight = childSizeRequest.Request.Height;
                // Adjust position and size based on HorizontalOptions.
                //switch (child.HorizontalOptions.Alignment)
                //{
                //    case LayoutAlignment.Start:
                //        break;
                //    case LayoutAlignment.Center:
                //        xChild += (width - childWidth) / 2;
                //        break;
                //    case LayoutAlignment.End:
                //        xChild += (width - childWidth);
                //        break;
                //    case LayoutAlignment.Fill:
                //        childWidth = width;
                //        break;
                //}
                double newXPoint = (radius * Math.Cos(initialAngle * Math.PI / 180)) + x;
                double newYPoint = (radius * Math.Sin(initialAngle * Math.PI / 180)) + y;

                initialAngle += requestedAngle;
                // Layout the child.
                //child.Layout(new Rectangle(xChild, yChild, childWidth, childHeight));
                LayoutChildIntoBoundingRegion(child, new Rectangle(newXPoint, newYPoint, childWidth, childHeight));
                // Get the next child’s vertical position.
                //x += 
                //y += childHeight;
            }
        }

       
    }
}
