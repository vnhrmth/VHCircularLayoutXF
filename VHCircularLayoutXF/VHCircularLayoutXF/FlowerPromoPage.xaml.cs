using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace VHCircularLayoutXF
{
    public partial class FlowerPromoPage : ContentPage
    {
        public FlowerPromoPage()
        {
            InitializeComponent();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {

           Animation outerCircleAnimation = new Animation((x) =>
            {
                OuterCircularLayout.IsVisible = true;
                OuterCircularLayout.Angle = x;
            }, 0, 22.72, Easing.SinOut, () =>
            {

            });

           Animation midCircularAnimation = new Animation((x) =>
            {
                MidCircularLayout.IsVisible = true;
                MidCircularLayout.Angle = x;
            }, 0, 32.72, Easing.SinOut, () =>
            {
                //outerCircleAnimation.Commit(this, "MidCircularAnimation", 16, 2000, Easing.Linear, null, () => false);
            });

            Animation innerCircleAnimation = new Animation((x) =>
            {
                InnerCircularLayout.IsVisible = true;
                InnerCircularLayout.Angle = x;
            }, 0, 32.72, Easing.SinOut, () =>
            {
                //midCircularAnimation.Commit(this, "Animation3", 16, 2000, Easing.Linear, null, () => false);
            });

            new Animation
                   {
                       { 0,0.3, innerCircleAnimation},
                       { 0.3,0.6, midCircularAnimation},
                       { 0.6,1.0, outerCircleAnimation},
                   }.Commit(this, "FlowerAnimation", 16, 2000, Easing.Linear, null, () => false);
        }
    }
}
