using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nonograms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Application.Current.UserAppTheme = OSAppTheme.Light;

            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromRgb(192,103,135),
                BarTextColor = Color.Black
            };
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
