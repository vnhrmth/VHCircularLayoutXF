using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace VHCircularLayoutXF
{
    public partial class MyPage : ContentPage
    {
        public MyPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await myLayout.RotateTo(360, 1000, Easing.Linear);
        }
    }
}
