using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nonograms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BlackCrossMenu : ContentPage
    {
        public BlackCrossMenu()
        {
            InitializeComponent();
        }

        private async void FiveToFiveButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FiveToFiveMenu());
        }

        private void TenToTenButton_Clicked(object sender, EventArgs e)
        {

        }

        private void FifteenToFifteenButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}