﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace VHCircularLayoutXF
{
    [DesignTimeVisible(true)]
    public class CircularLayout : Layout<View>
    {
        ////public object SelectedItem
        ////{
        ////    get { return GetValue(SelectedItemProperty); }
        ////    set { SetValue(SelectedItemProperty, value); }
        ////}

        ////public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem", typeof(object), typeof(ListView), null, BindingMode.OneWayToSource,
        ////  propertyChanged: OnSelectedItemChanged);

        ////public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

        //static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue) =>
        //((CircularLayout)bindable).ItemSelected?.Invoke(bindable,
        //new SelectedItemChangedEventArgs(newValue, ((CircularLayout)bindable).Children.IndexOf((View)newValue)));

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

        public bool ShouldStackFirstToLast
        {
            get => (bool)GetValue(ShouldStackFirstToLastProperty);
            set => SetValue(ShouldStackFirstToLastProperty, value);
        }

        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public double InitialStartPositionAngle
        {
            get => (double)GetValue(InitialStartPositionAngleProperty);
            set => SetValue(InitialStartPositionAngleProperty, value);
        }

        // initial position is -180 which starts at the left side i.e 2nd qudrant.
        public static BindableProperty InitialStartPositionAngleProperty = BindableProperty.Create("InitialStartPositionAngle",
            typeof(double),
            typeof(CircularLayout), -180.0, propertyChanged: (bindable, oldValue, newValue) =>
            {
                ((CircularLayout)bindable).InvalidateLayout();
            });

        public static BindableProperty ShouldStackFirstToLastProperty = BindableProperty.Create("ShouldStackFirstToLast",
        typeof(bool),
        typeof(CircularLayout), true, propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((CircularLayout)bindable).InvalidateLayout();
        });

        public static BindableProperty ShouldRotateChildProperty = BindableProperty.Create("ShouldRotateChild",
        typeof(bool),
        typeof(CircularLayout), false, propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((CircularLayout)bindable).InvalidateLayout();
        });

        public static BindableProperty RadiusProperty = BindableProperty.Create("Radius",
            typeof(double),
            typeof(CircularLayout), 100.0, propertyChanged: (bindable, oldValue, newValue) =>
             {
                 ((CircularLayout)bindable).InvalidateLayout();
             });

        public static readonly BindableProperty AngleProperty = BindableProperty.Create("Angle",
            typeof(double),
            typeof(CircularLayout), 30.0, propertyChanged: (bindable, oldValue, newValue) =>
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
            double startPositionAngle = InitialStartPositionAngle;

            if(ShouldStackFirstToLast)
                StackFirstToLast(x, y, width, startPositionAngle, Angle, Radius);
            else
                StackLastToFirst(x, y, width, startPositionAngle, Angle, Radius);
        }

        private void StackFirstToLast(double x, double y,
                                      double width, double initialAngle,
                                      double sweepAngle, double radius)
        {
            //FILO
            //int i = 0; // use for interleaved
            foreach (View child in Children)
            {
                // interleaving feature
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
                double childWidth = childSizeRequest.Request.Width;
                double childHeight = childSizeRequest.Request.Height;

                double newXPoint = (radius * Math.Cos(initialAngle * Math.PI / 180)) + x;
                double newYPoint = (radius * Math.Sin(initialAngle * Math.PI / 180)) + y;
                
                // if Rotation for child is set the child is rotated as per the angle its parent is located
                RotateChild(initialAngle, child);

                initialAngle += sweepAngle;
                child.Opacity = 1;

                LayoutChildIntoBoundingRegion(child, new Rectangle(newXPoint, newYPoint, childWidth, childHeight));
            }

        }

        private void StackLastToFirst(double x, double y,
                                      double width, double initialAngle,
                                      double sweepAngle, double radius)
        {
            //change overlapping direction and last is shown first
            for (int index = Children.Count - 1; index >= 0; index--)
            {
                View child = Children[index];
                // Skip the invisible children.
                if (!child.IsVisible)
                    continue;
                // Get the child's requested size.
                SizeRequest childSizeRequest = child.Measure(width, Double.PositiveInfinity);

                // Initialize child position and size.
                double childWidth = childSizeRequest.Request.Width;
                double childHeight = childSizeRequest.Request.Height;

                double newXPoint = (radius * Math.Cos(initialAngle * Math.PI / 180)) + x;
                double newYPoint = (radius * Math.Sin(initialAngle * Math.PI / 180)) + y;

                RotateChild(initialAngle, child);

                initialAngle += sweepAngle;
                child.Opacity = 1;

                LayoutChildIntoBoundingRegion(child, new Rectangle(newXPoint, newYPoint, childWidth, childHeight));
            }
        }

        private void RotateChild(double initialAngle, View child)
        {
            child.AnchorX = 0.5;
            child.AnchorY = 0.5;
            if (ShouldRotateChild)
            {
                child.RotateTo(initialAngle + 90);
            }
            else
            {
                child.RotateTo(0);
            }
        }
    }
}
