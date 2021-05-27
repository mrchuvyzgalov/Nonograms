using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace Nonograms
{
    public partial class MainPage : ContentPage
    {
        readonly string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public MainPage()
        {
            InitializeComponent();

            ToolbarItem aboutAuthor = new ToolbarItem()
            {
                Text = "Об авторах",
                Order = ToolbarItemOrder.Secondary,
                Priority = 1
            };

            aboutAuthor.Clicked += AboutAuthor_Clicked;

            ToolbarItem aboutApplication = new ToolbarItem()
            {
                Text = "О приложении",
                Order = ToolbarItemOrder.Secondary,
                Priority = 2
            };

            aboutApplication.Clicked += AboutApplication_Clicked;

            ToolbarItems.Add(aboutAuthor);
            ToolbarItems.Add(aboutApplication);

            string tmpPath = Path.Combine(folderPath, "Data");
            if (!Directory.Exists(tmpPath))
            {
                Directory.CreateDirectory(tmpPath);
                if (!Directory.Exists(Path.Combine(tmpPath, "Создать кроссворд")))
                {
                    Directory.CreateDirectory(Path.Combine(tmpPath, "Создать кроссворд"));
                }
                if (!Directory.Exists(Path.Combine(tmpPath, "Цветные кроссворды")))
                {
                    Directory.CreateDirectory(Path.Combine(tmpPath, "Цветные кроссворды"));

                    string colorPath = Path.Combine(Path.Combine(tmpPath, "Цветные кроссворды"), "5x5");

                    if (!Directory.Exists(colorPath))
                    {
                        Directory.CreateDirectory(colorPath);

                        if (!File.Exists(Path.Combine(colorPath,"Андроид.jap")))
                        {
                            string android = "Андроид\n2\n113 245 6 255 251 20\n5 5\n3 113 245 6\n1 113 245 6 1 255 251 20 1 113 245 6 1 255 251 20 1 113 245 6\n5 113 245 6\n\n5 113 245 6\n5 4\n2 113 245 6 1 113 245 6\n1 113 245 6 1 255 251 20 1 113 245 6 1 113 245 6\n3 113 245 6 1 113 245 6\n1 113 245 6 1 255 251 20 1 113 245 6 1 113 245 6\n2 113 245 6 1 113 245 6";
                            File.WriteAllText(Path.Combine(colorPath, "Андроид.jap"), android);
                        }
                        if (!File.Exists(Path.Combine(colorPath, "Дерево.jap")))
                        {
                            string tree = "Дерево\n2\n0 255 0 194 106 21\n5 3\n3 0 255 0\n5 0 255 0\n5 0 255 0\n1 0 255 0 1 194 106 21 1 0 255 0\n1 194 106 21\n5 2\n2 0 255 0\n4 0 255 0\n3 0 255 0 2 194 106 21\n4 0 255 0\n2 0 255 0";
                            File.WriteAllText(Path.Combine(colorPath, "Дерево.jap"), tree);
                        }
                        if (!File.Exists(Path.Combine(colorPath, "Магнит.jap")))
                        {
                            string magnit = "Магнит\n6\n149 149 149 223 223 223 187 37 22 255 0 0 20 28 95 26 10 247\n5 4\n1 149 149 149 1 223 223 223 1 149 149 149 1 223 223 223\n1 187 37 22 1 255 0 0 1 20 28 95 1 26 10 247\n1 187 37 22 1 255 0 0 1 20 28 95 1 26 10 247\n1 187 37 22 2 255 0 0 2 26 10 247\n1 187 37 22 1 255 0 0 1 26 10 247\n5 3\n1 149 149 149 3 187 37 22\n1 223 223 223 3 255 0 0 1 187 37 22\n2 255 0 0\n1 149 149 149 2 20 28 95 2 26 10 247\n1 223 223 223 3 26 10 247";
                            File.WriteAllText(Path.Combine(colorPath, "Магнит.jap"), magnit);
                        }
                        if (!File.Exists(Path.Combine(colorPath, "Мишень.jap")))
                        {
                            string target = "Мишень\n3\n0 0 255 255 0 0 255 255 0\n5 5\n3 0 0 255\n1 0 0 255 3 255 0 0 1 0 0 255\n1 0 0 255 1 255 0 0 1 255 255 0 1 255 0 0 1 0 0 255\n1 0 0 255 3 255 0 0 1 0 0 255\n3 0 0 255\n5 5\n3 0 0 255\n1 0 0 255 3 255 0 0 1 0 0 255\n1 0 0 255 1 255 0 0 1 255 255 0 1 255 0 0 1 0 0 255\n1 0 0 255 3 255 0 0 1 0 0 255\n3 0 0 255";
                            File.WriteAllText(Path.Combine(colorPath, "Мишень.jap"), target);
                        }
                        if (!File.Exists(Path.Combine(colorPath, "Цветочек.jap")))
                        {
                            string flower = "Цветочек\n3\n0 0 0 255 251 20 235 18 255\n5 5\n3 235 18 255\n1 235 18 255 1 0 0 0 1 235 18 255 1 0 0 0 1 235 18 255\n2 235 18 255 1 255 251 20 2 235 18 255\n1 235 18 255 1 0 0 0 1 235 18 255 1 0 0 0 1 235 18 255\n3 235 18 255\n5 5\n3 235 18 255\n1 235 18 255 1 0 0 0 1 235 18 255 1 0 0 0 1 235 18 255\n2 235 18 255 1 255 251 20 2 235 18 255\n1 235 18 255 1 0 0 0 1 235 18 255 1 0 0 0 1 235 18 255\n3 235 18 255";
                            File.WriteAllText(Path.Combine(colorPath, "Цветочек.jap"), flower);
                        }
                    }
                }
                if (!Directory.Exists(Path.Combine(tmpPath, "Черно-белые кроссворды")))
                {
                    Directory.CreateDirectory(Path.Combine(tmpPath, "Черно-белые кроссворды"));

                    string blackPath = Path.Combine(Path.Combine(tmpPath, "Черно-белые кроссворды"),"5x5");

                    if (!Directory.Exists(blackPath))
                    {
                        Directory.CreateDirectory(blackPath);

                        if (!File.Exists(Path.Combine(blackPath, "Башня.jap")))
                        {
                            string tower = "Башня\n1\n0 0 0\n5 3\n1 0 0 0 1 0 0 0 1 0 0 0\n5 0 0 0\n3 0 0 0\n1 0 0 0 1 0 0 0\n3 0 0 0\n5 2\n2 0 0 0\n4 0 0 0\n3 0 0 0 1 0 0 0\n4 0 0 0\n2 0 0 0";
                            File.WriteAllText(Path.Combine(blackPath, "Башня.jap"), tower);
                        }
                        if (!File.Exists(Path.Combine(blackPath, "Диез.jap")))
                        {
                            string dies = "Диез\n1\n0 0 0\n5 2\n1 0 0 0 1 0 0 0\n5 0 0 0\n1 0 0 0 1 0 0 0\n5 0 0 0\n1 0 0 0 1 0 0 0\n5 2\n1 0 0 0 1 0 0 0\n5 0 0 0\n1 0 0 0 1 0 0 0\n5 0 0 0\n1 0 0 0 1 0 0 0";
                            File.WriteAllText(Path.Combine(blackPath, "Диез.jap"), dies);
                        }
                        if (!File.Exists(Path.Combine(blackPath, "Змейка.jap")))
                        {
                            string snake = "Змейка\n1\n0 0 0\n5 1\n5 0 0 0\n1 0 0 0\n5 0 0 0\n1 0 0 0\n5 0 0 0\n5 3\n3 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 3 0 0 0";
                            File.WriteAllText(Path.Combine(blackPath, "Змейка.jap"), snake);
                        }
                        if (!File.Exists(Path.Combine(blackPath, "Летучая мышь.jap")))
                        {
                            string bat = "Летучая мышь\n1\n0 0 0\n5 3\n1 0 0 0 1 0 0 0\n5 0 0 0\n5 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0\n5 1\n4 0 0 0\n3 0 0 0\n3 0 0 0\n3 0 0 0\n4 0 0 0";
                            File.WriteAllText(Path.Combine(blackPath, "Летучая мышь.jap"), bat);
                        }
                        if (!File.Exists(Path.Combine(blackPath, "Пси.jap")))
                        {
                            string psi = "Пси\n1\n0 0 0\n5 3\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n3 0 0 0\n1 0 0 0\n5 1\n3 0 0 0\n1 0 0 0\n5 0 0 0\n1 0 0 0\n3 0 0 0";
                            File.WriteAllText(Path.Combine(blackPath, "Пси.jap"), psi);
                        }
                    }
                }
            }
        }

        private async void AboutApplication_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("О приложении", "Приложение \"Nonograms\" позволяет решать цветные и черно-белые японские кроссворды, а также создавать свои кроссорды и получать решения других нонограмм", "Ок");
        }

        private async void AboutAuthor_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Об авторе", "Программист: Чувызгалов Кирилл Андреевич (mr.chvuvyzgalov@gmail.com)\nДизайнеры: Яковлева Анастасия Алексеевна и Петропавловских Елизавета", "Ок");
        }

        private async void PlayButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Menu(Path.Combine(folderPath, "Data")));
        }

        private async void SolveButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SolveMenu());
        }
    }
}
