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
	public partial class FiveToFiveMenu : ContentPage
	{
		public FiveToFiveMenu ()
		{
			InitializeComponent ();
		}

        private async void SnakeButton_Clicked(object sender, EventArgs e)
        {
			await Navigation.PushAsync(new Nonogram(@"Snake.jap"));
        }
    }
}