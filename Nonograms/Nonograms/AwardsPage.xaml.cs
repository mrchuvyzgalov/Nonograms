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
    public partial class AwardsPage : ContentPage
    {
        public AwardsPage()
        {
            InitializeComponent();

            ToolbarItem back = new ToolbarItem()
            {
                Text = "Назад",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };

            back.Clicked += Back_Clicked;

            ToolbarItems.Add(back);
        }

        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}