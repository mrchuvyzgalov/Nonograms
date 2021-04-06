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
    public partial class PlayMenuPage : ContentPage
    {
        public PlayMenuPage()
        {
            InitializeComponent();
        }

        private void StudyButton_Clicked(object sender, EventArgs e)
        {

        }

        private async void BlackCrossButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BlackCrossMenu());
        }

        private void ColorCrossButton_Clicked(object sender, EventArgs e)
        {

        }

        private void MyCrossButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}