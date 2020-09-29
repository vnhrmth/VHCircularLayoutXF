using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace VHCircularLayoutXF
{
    [DesignTimeVisible(true)]
    public class MyLayout : Layout<View>
    {
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem", typeof(object), typeof(ListView), null, BindingMode.OneWayToSource,
    propertyChanged: OnSelectedItemChanged);

        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

        static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue) =>
        ((MyLayout)bindable).ItemSelected?.Invoke(bindable,
        new SelectedItemChangedEventArgs(newValue, ((MyLayout)bindable).Children.IndexOf((View)newValue)));

        Rectangle _layoutAreaOverride;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public Rectangle LayoutAreaOverride
        {
            get => _layoutAreaOverride;
            set
            {
                if (_layoutAreaOverride == value)
                    return;
                _layoutAreaOverride = value;
                // Dont invalidate here, we can relayout immediately since this only impacts our innards
                UpdateChildrenLayout();
            }
        }


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

        //public Rectangle LayoutAreaOverride { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


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
            double initialAngle = -90;
            double requestedAngle;
            double.TryParse(Angle, out requestedAngle);
            double radius;
            
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
                
                double newXPoint = (radius * Math.Cos(initialAngle * Math.PI / 180)) + x;
                double newYPoint = (radius * Math.Sin(initialAngle * Math.PI / 180)) + y;

                child.AnchorX = 0.5;
                child.AnchorY = 0.5;
                child.RotateTo(initialAngle+90);
                initialAngle += requestedAngle;
                


                LayoutChildIntoBoundingRegion(child, new Rectangle(newXPoint, newYPoint, childWidth, childHeight));
            }
        }
    }
}
