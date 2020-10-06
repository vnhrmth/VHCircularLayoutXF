using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VHCircularLayoutXF
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Device.SetFlags(new string[] { "Brush_Experimental" , "Shapes_Experimental" });
            //MainPage = new MyPage();
            //MainPage = new ViewCreatorPage();
            //MainPage = new RadiusChangerPage();
            //MainPage = new AngleChangerPage();
            //MainPage = new NonGenericPromoPage();
            //MainPage = new ChangeStackingPromoPage();
            //MainPage = new AnchorPointPromoPage();
            //MainPage = new InitialStartPositionAnglePromoPage();
            //MainPage = new MoonPhasesPromoPage();
            //MainPage = new PokerCardsPromoPage();
            MainPage = new FlowerPromoPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
