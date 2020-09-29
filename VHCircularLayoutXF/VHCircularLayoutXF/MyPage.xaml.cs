using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace VHCircularLayoutXF
{
    public partial class MyPage : ContentPage
    {
        double x = 0;
        double y = 0;

        public MyPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
           
            //await myLayout.RotateTo(0, 1000, Easing.Linear);
            await myLayout.RotateTo(360, 1000, Easing.Linear);
            myLayout.Rotation = 0;
        }

        void Slider_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            Slider slider = (Slider)sender;
            myLayout.AnchorX = slider.Value;
        }

        void Slider_ValueChanged2(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            Slider slider = (Slider)sender;
            myLayout.AnchorY = slider.Value;
        }

        void PanGestureRecognizer_PanUpdated(System.Object sender, Xamarin.Forms.PanUpdatedEventArgs e)
        {
            myLayout.AnchorX = 0.5;
            myLayout.AnchorY = 0.3;

            switch (e.StatusType)
            {
                case GestureStatus.Started:

                    break;
                case GestureStatus.Running:
                    // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                    x = e.TotalX + myLayout.TranslationX;
                    y = e.TotalY + myLayout.TranslationY; 
                    

                    
                    var rad = Math.Atan2(y, x); // In radians
                    var deg = rad * (180 / Math.PI);
                    myLayout.RotateTo(deg);
                    myLayout.Rotation = deg;
                    Console.WriteLine("running" + myLayout.Rotation);
                    //myLayout.RotateYTo(y, 1000, Easing.Linear);
                    break;

                case GestureStatus.Completed:
                    Console.WriteLine("Completed"+myLayout.Rotation);
                    // Store the translation applied during the pan
                    //x = 0;
                    //y = 0;
                    //myLayout.RotateTo(0);
                    //myLayout.Rotation = 0;
                    break;
            }

        }

        async void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
            Button button = (Button)sender;
            await button.ScaleTo(1.2);
            await button.ScaleTo(1);
        }

        void myLayout_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
        }
    }
}
