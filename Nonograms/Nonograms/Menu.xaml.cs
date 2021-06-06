using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace Nonograms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        bool isFiles = false;
        FileInfo[] files;
        DirectoryInfo[] dirs;

        public Menu(string path)
        {
            InitializeComponent();

            // создание кнопок
            try
            {
                DirectoryInfo dir = new DirectoryInfo(@path);

                if (!dir.Exists)
                {
                    throw new Exception("Неправильный путь " + @path);
                }

                files = dir.GetFiles();
                dirs = dir.GetDirectories();

                if (files.Length * dirs.Length != 0)
                {
                    throw new Exception("Некорректное содержание директории " + @path);
                }
                if (files.Length + dirs.Length == 0)
                {
                    throw new Exception("Директория " + @path + " пуста");
                }
                if (files.Length != 0)
                {
                    isFiles = true;
                    CreateButtonsFromFiles(files);
                }
                else
                {
                    CreateButtonsFromDirs(dirs);
                }

            }
            catch (Exception error)
            {
                PrintError(error.Message);
            }
        }

        private async void PrintError(string message)
        {
            if (await DisplayAlert("Ошибка", message, "Выход", "Отмена"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void CreateButtonsFromFiles(FileInfo[] files)
        {
            foreach (FileInfo file in files)
            {
                StackLayout stack = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(3,3,3,3)
                };

                Grid grid = new Grid
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(3, 3, 3, 3)
                };

                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150, GridUnitType.Absolute) });

                ImageButton image = new ImageButton
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Source = "Button.png",
                    BackgroundColor = Color.Transparent
                };

                image.Clicked += Image_Clicked;

                Button button = new Button
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Text = file.Name.Split(new char[] { '.' })[0],
                    TextColor = Color.Black,
                    FontSize = 17.0,
                    BackgroundColor = Color.Transparent
                };

                button.Clicked += Image_Clicked;

                grid.Children.Add(image, 0, 0);
                grid.Children.Add(button, 0, 0);

                stack.Children.Add(grid);
                MainStack.Children.Add(stack);
            }
        }

        private async void Image_Clicked(object sender, EventArgs e)
        {
            int index = 0;

            if (sender is ImageButton)
            {
                ImageButton image = (ImageButton)sender;
                foreach (StackLayout stack in MainStack.Children)
                {
                    if (((Grid)stack.Children[0]).Children.Contains(image))
                    {
                        break;
                    }
                    index++;
                }
            }
            else
            {
                Button image = (Button)sender;
                foreach (StackLayout stack in MainStack.Children)
                {
                    if (((Grid)stack.Children[0]).Children.Contains(image))
                    {
                        break;
                    }
                    index++;
                }
            }

            if (isFiles)
            {
                await Navigation.PushAsync(new Nonogram(@files[index].FullName));
            }
            else
            {
                if (@dirs[index].FullName.Contains(Path.Combine("Data", "Создать кроссворд")))
                {
                    await Navigation.PushAsync(new CreateNonogram());
                }
                else
                {
                    await Navigation.PushAsync(new Menu(@dirs[index].FullName));
                }
            }
        }

        private void CreateButtonsFromDirs(DirectoryInfo[] dirs)
        {
            foreach (DirectoryInfo dir in dirs)
            {
                StackLayout stack = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(3, 3, 3, 3)
                };

                Grid grid = new Grid
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(3, 3, 3, 3)
                };

                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(150, GridUnitType.Absolute)});

                ImageButton image = new ImageButton
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Source = "Button.png",
                    BackgroundColor = Color.Transparent
                };

                image.Clicked += Image_Clicked;

                Button button = new Button
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Text = dir.Name,
                    TextColor = Color.Black,
                    FontSize = 17.0,
                    BackgroundColor = Color.Transparent
                };

                button.Clicked += Image_Clicked;

                grid.Children.Add(image, 0, 0);
                grid.Children.Add(button, 0, 0);

                stack.Children.Add(grid);
                MainStack.Children.Add(stack);
            }
        }
    }
}