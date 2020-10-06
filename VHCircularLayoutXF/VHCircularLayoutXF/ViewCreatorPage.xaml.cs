using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace VHCircularLayoutXF
{
    public partial class ViewCreatorPage : ContentPage
    {
        public ViewCreatorPage()
        {
            InitializeComponent();
        }

        void stepper_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            if (e.NewValue > e.OldValue)
            {
                Frame aFrame = new Frame();
                aFrame.BorderColor = Color.LightGray;
                aFrame.BackgroundColor = Brush.Beige.Color;
                aFrame.CornerRadius = 40;
                aFrame.WidthRequest = 40;
                aFrame.HeightRequest = 40;
                testLayout.Children.Add(aFrame);
            }
            else
            {
                testLayout.Children.RemoveAt(testLayout.Children.Count - 1);
            }

        }
    }
}
