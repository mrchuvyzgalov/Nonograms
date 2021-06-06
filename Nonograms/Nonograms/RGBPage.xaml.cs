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
	public partial class RGBPage : ContentPage
	{
		ColorRGB color;
		public RGBPage (ColorRGB color)
		{
			InitializeComponent ();

			this.color = color;
		}

        private async void StartButton_Clicked(object sender, EventArgs e)
        {
			int red, green, blue;
			if (int.TryParse(Red.Text, out red) && red >= 0 && red <= 255)
            {
				if (int.TryParse(Green.Text, out green) && green >= 0 && green <= 255)
				{
					if (int.TryParse(Blue.Text, out blue) && blue >= 0 && blue <= 255)
					{
						color.red = red;
						color.green = green;
						color.blue = blue;
						await Navigation.PopAsync();
					}
					else
					{
						await DisplayAlert("Ошибка", "Синий цвет должен быть целым числом от 0 до 255", "Исправить");
					}
				}
				else
				{
					await DisplayAlert("Ошибка", "Зеленый цвет должен быть целым числом от 0 до 255", "Исправить");
				}
			}
			else
            {
				await DisplayAlert("Ошибка", "Красный цвет должен быть целым числом от 0 до 255", "Исправить");
            }
        }
    }
}