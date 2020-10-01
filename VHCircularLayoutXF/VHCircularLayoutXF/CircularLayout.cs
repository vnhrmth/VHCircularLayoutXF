using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace VHCircularLayoutXF
{
    [DesignTimeVisible(true)]
    public class CircularLayout : Layout<View>
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
        ((CircularLayout)bindable).ItemSelected?.Invoke(bindable,
        new SelectedItemChangedEventArgs(newValue, ((CircularLayout)bindable).Children.IndexOf((View)newValue)));

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

        public bool ShouldRotateChild
        {
            get => (bool)GetValue(ShouldRotateChildProperty);
            set => SetValue(ShouldRotateChildProperty, value);
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

        public static BindableProperty ShouldRotateChildProperty = BindableProperty.Create("ShouldRotateChild",
        typeof(bool),
        typeof(CircularLayout), false, propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((CircularLayout)bindable).InvalidateLayout();
        });

        public static BindableProperty RadiusProperty = BindableProperty.Create("Radius",
            typeof(string),
            typeof(CircularLayout), "100", propertyChanged: (bindable, oldValue, newValue) =>
             {
                 ((CircularLayout)bindable).InvalidateLayout();
             });

        public static readonly BindableProperty AngleProperty = BindableProperty.Create("Angle",
            typeof(string),
            typeof(CircularLayout), "30", propertyChanged: (bindable, oldValue, newValue) =>
            {
                ((CircularLayout)bindable).InvalidateLayout();
            });

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
                double.PositiveInfinity);

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
            double initialAngle = -180;
            double requestedAngle;
            double.TryParse(Angle, out requestedAngle);
            double radius;
            
            double.TryParse(Radius, out radius);

            //change overlapping direction and last is shown first
            //for(int i = Children.Count - 1; i > 0; i--)
            //{
            //    View child = Children[i];
            //    // Skip the invisible children.
            //    if (!child.IsVisible)
            //        continue;
            //    // Get the child's requested size.
            //    SizeRequest childSizeRequest = child.Measure(width, Double.PositiveInfinity);

            //    // Initialize child position and size.
            //    double xChild = x;
            //    double yChild = y;
            //    double childWidth = childSizeRequest.Request.Width;
            //    double childHeight = childSizeRequest.Request.Height;

            //    double newXPoint = (radius * Math.Cos(initialAngle * Math.PI / 180)) + x;
            //    double newYPoint = (radius * Math.Sin(initialAngle * Math.PI / 180)) + y;

            //    if (RotateChild)
            //    {
            //        child.AnchorX = 0.5;
            //        child.AnchorY = 0.5;
            //        child.RotateTo(initialAngle + 90);
            //    }
            //    else
            //    {
            //        child.AnchorX = 0.5;
            //        child.AnchorY = 0.5;
            //        child.RotateTo(0);
            //    }

            //    initialAngle += requestedAngle;
            //    child.Opacity = 1;

            //    LayoutChildIntoBoundingRegion(child, new Rectangle(newXPoint, newYPoint, childWidth, childHeight));
            //}

            //FILO
            int i = 0;
            foreach (View child in Children)
            {
                // interleaving
                //if (i % 2 == 0)
                //{
                //    child.IsVisible = false;
                //}
                //else
                //{
                //    child.IsVisible = true;
                //}
                //i++;

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

                // if set rotates the child as per the angle set
                RotateChild(initialAngle, child);

                initialAngle += requestedAngle;
                child.Opacity = 1;

                LayoutChildIntoBoundingRegion(child, new Rectangle(newXPoint, newYPoint, childWidth, childHeight));
            }
        }

        private void RotateChild(double initialAngle, View child)
        {
            if (ShouldRotateChild)
            {
                child.AnchorX = 0.5;
                child.AnchorY = 0.5;
                child.RotateTo(initialAngle + 90);
            }
            else
            {
                child.AnchorX = 0.5;
                child.AnchorY = 0.5;
                child.RotateTo(0);
            }
        }

        protected override void InvalidateLayout()
        {
            base.InvalidateLayout();
            Console.WriteLine("called invalidte");
        }
    }
}
