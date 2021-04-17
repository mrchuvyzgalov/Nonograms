using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nonograms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            ToolbarItem awards = new ToolbarItem()
            {
                Text = "Достижения",
                Order = ToolbarItemOrder.Primary,
                Priority = 0,
                IconImageSource = "awards.png"
            };

            awards.Clicked += async (s, e) =>
            {
                AwardsPage ap = new AwardsPage();
                NavigationPage.SetHasBackButton(ap, false);
                await Navigation.PushAsync(ap);
            };

            ToolbarItem aboutAuthor = new ToolbarItem()
            {
                Text = "Об авторе",
                Order = ToolbarItemOrder.Secondary,
                Priority = 1
            };

            ToolbarItem aboutApplication = new ToolbarItem()
            {
                Text = "О приложении",
                Order = ToolbarItemOrder.Secondary,
                Priority = 2
            };

            ToolbarItems.Add(awards);
            ToolbarItems.Add(aboutAuthor);
            ToolbarItems.Add(aboutApplication);
        }

        private async void PlayButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlayMenuPage());
        }

        private async void SolveButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SolveMenuPage());
        }
    }
}
