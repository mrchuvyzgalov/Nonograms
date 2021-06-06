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

            ToolbarItem rulesButton = new ToolbarItem()
            {
                IconImageSource = "Book.png", 
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };

            rulesButton.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new RulesPage());
            };

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

            ToolbarItems.Add(rulesButton);
            ToolbarItems.Add(aboutAuthor);
            ToolbarItems.Add(aboutApplication);

            if (!File.Exists(Path.Combine(folderPath, "settings.txt"))) {
                string settings = "Средний\n15";
                File.WriteAllText(Path.Combine(folderPath, "settings.txt"), settings);
            }

            string tmpPath = Path.Combine(folderPath, "Data");
            if (!Directory.Exists(tmpPath))
            {
                Directory.CreateDirectory(tmpPath);
            }

            if (!Directory.Exists(Path.Combine(tmpPath, "Создать кроссворд")))
            {
                Directory.CreateDirectory(Path.Combine(tmpPath, "Создать кроссворд"));
            }
            if (!Directory.Exists(Path.Combine(tmpPath, "Цветные кроссворды")))
            {
                Directory.CreateDirectory(Path.Combine(tmpPath, "Цветные кроссворды"));
            }

            string colorPath = Path.Combine(tmpPath, "Цветные кроссворды");

            if (!Directory.Exists(Path.Combine(colorPath, "5x5")))
            {
                Directory.CreateDirectory(Path.Combine(colorPath, "5x5"));
            }

            if (!File.Exists(Path.Combine(colorPath, "5x5", "Андроид.jap")))
            {
                string android = "Андроид\n2\n113 245 6 255 251 20\n5 5\n3 113 245 6\n1 113 245 6 1 255 251 20 1 113 245 6 1 255 251 20 1 113 245 6\n5 113 245 6\n\n5 113 245 6\n5 4\n2 113 245 6 1 113 245 6\n1 113 245 6 1 255 251 20 1 113 245 6 1 113 245 6\n3 113 245 6 1 113 245 6\n1 113 245 6 1 255 251 20 1 113 245 6 1 113 245 6\n2 113 245 6 1 113 245 6";
                File.WriteAllText(Path.Combine(colorPath, "5x5", "Андроид.jap"), android);
            }
            if (!File.Exists(Path.Combine(colorPath, "5x5", "Дерево.jap")))
            {
                string tree = "Дерево\n2\n0 255 0 194 106 21\n5 3\n3 0 255 0\n5 0 255 0\n5 0 255 0\n1 0 255 0 1 194 106 21 1 0 255 0\n1 194 106 21\n5 2\n2 0 255 0\n4 0 255 0\n3 0 255 0 2 194 106 21\n4 0 255 0\n2 0 255 0";
                File.WriteAllText(Path.Combine(colorPath, "5x5", "Дерево.jap"), tree);
            }
            if (!File.Exists(Path.Combine(colorPath, "5x5", "Магнит.jap")))
            {
                string magnit = "Магнит\n6\n149 149 149 223 223 223 187 37 22 255 0 0 20 28 95 26 10 247\n5 4\n1 149 149 149 1 223 223 223 1 149 149 149 1 223 223 223\n1 187 37 22 1 255 0 0 1 20 28 95 1 26 10 247\n1 187 37 22 1 255 0 0 1 20 28 95 1 26 10 247\n1 187 37 22 2 255 0 0 2 26 10 247\n1 187 37 22 1 255 0 0 1 26 10 247\n5 3\n1 149 149 149 3 187 37 22\n1 223 223 223 3 255 0 0 1 187 37 22\n2 255 0 0\n1 149 149 149 2 20 28 95 2 26 10 247\n1 223 223 223 3 26 10 247";
                File.WriteAllText(Path.Combine(colorPath, "5x5", "Магнит.jap"), magnit);
            }
            if (!File.Exists(Path.Combine(colorPath, "5x5", "Мишень.jap")))
            {
                string target = "Мишень\n3\n0 0 255 255 0 0 255 255 0\n5 5\n3 0 0 255\n1 0 0 255 3 255 0 0 1 0 0 255\n1 0 0 255 1 255 0 0 1 255 255 0 1 255 0 0 1 0 0 255\n1 0 0 255 3 255 0 0 1 0 0 255\n3 0 0 255\n5 5\n3 0 0 255\n1 0 0 255 3 255 0 0 1 0 0 255\n1 0 0 255 1 255 0 0 1 255 255 0 1 255 0 0 1 0 0 255\n1 0 0 255 3 255 0 0 1 0 0 255\n3 0 0 255";
                File.WriteAllText(Path.Combine(colorPath, "5x5", "Мишень.jap"), target);
            }
            if (!File.Exists(Path.Combine(colorPath, "5x5", "Цветочек.jap")))
            {
                string flower = "Цветочек\n3\n0 0 0 255 251 20 235 18 255\n5 5\n3 235 18 255\n1 235 18 255 1 0 0 0 1 235 18 255 1 0 0 0 1 235 18 255\n2 235 18 255 1 255 251 20 2 235 18 255\n1 235 18 255 1 0 0 0 1 235 18 255 1 0 0 0 1 235 18 255\n3 235 18 255\n5 5\n3 235 18 255\n1 235 18 255 1 0 0 0 1 235 18 255 1 0 0 0 1 235 18 255\n2 235 18 255 1 255 251 20 2 235 18 255\n1 235 18 255 1 0 0 0 1 235 18 255 1 0 0 0 1 235 18 255\n3 235 18 255";
                File.WriteAllText(Path.Combine(colorPath, "5x5", "Цветочек.jap"), flower);
            }
            if (!File.Exists(Path.Combine(colorPath, "5x5", "Узор.jap")))
            {
                string data = "Узор\n4\n0 0 0 0 241 235 0 241 0 243 217 0\n5 5\n1 0 0 0 3 0 241 235 1 0 0 0\n1 0 241 0 1 0 0 0 1 0 241 235 1 0 0 0 1 0 241 0\n2 0 241 0 1 0 0 0 2 0 241 0\n1 0 241 0 1 0 0 0 1 243 217 0 1 0 0 0 1 0 241 0\n1 0 0 0 3 243 217 0 1 0 0 0\n5 5\n1 0 0 0 3 0 241 0 1 0 0 0\n1 0 241 235 1 0 0 0 1 0 241 0 1 0 0 0 1 243 217 0\n2 0 241 235 1 0 0 0 2 243 217 0\n1 0 241 235 1 0 0 0 1 0 241 0 1 0 0 0 1 243 217 0\n1 0 0 0 3 0 241 0 1 0 0 0";
                File.WriteAllText(Path.Combine(colorPath, "5x5", "Узор.jap"), data);
            }

            if (!File.Exists(Path.Combine(colorPath, "5x5", "Черепашка.jap")))
            {
                string data = "Черепашка\n2\n0 210 0 153 99 53\n5 3\n1 0 210 0 1 0 210 0 1 0 210 0\n1 0 210 0 3 153 99 53 1 0 210 0\n1 0 210 0 3 153 99 53 1 0 210 0\n3 153 99 53\n2 0 210 0 2 0 210 0\n5 2\n3 0 210 0 1 0 210 0\n3 153 99 53 1 0 210 0\n1 0 210 0 3 153 99 53\n3 153 99 53 1 0 210 0\n3 0 210 0 1 0 210 0";
                File.WriteAllText(Path.Combine(colorPath, "5x5", "Черепашка.jap"), data);
            }

            if (!Directory.Exists(Path.Combine(colorPath, "6x6")))
            {
                Directory.CreateDirectory(Path.Combine(colorPath, "6x6"));
            }

            if (!File.Exists(Path.Combine(colorPath, "6x6", "Спортсмен.jap")))
            {
                string data = "Спортсмен\n5\n93 107 110 212 191 102 169 111 110 0 0 0 106 88 86\n6 4\n1 106 88 86 1 212 191 102 1 93 107 110\n1 93 107 110 1 212 191 102 1 93 107 110 1 212 191 102\n1 212 191 102 1 0 0 0 1 212 191 102\n1 212 191 102 1 169 111 110 1 212 191 102 1 169 111 110\n3 169 111 110\n3 169 111 110\n6 4\n1 93 107 110\n3 212 191 102\n1 93 107 110 3 169 111 110\n1 106 88 86 1 0 0 0 1 212 191 102 2 169 111 110\n3 212 191 102 3 169 111 110\n1 93 107 110";
                File.WriteAllText(Path.Combine(colorPath, "6x6", "Спортсмен.jap"), data);
            }

            if (!File.Exists(Path.Combine(colorPath, "6x6", "Пистолет.jap")))
            {
                string data = "Пистолет\n3\n40 35 39 0 0 0 255 0 0\n6 3\n\n\n2 40 35 39 3 0 0 0\n1 0 0 0 1 255 0 0 1 0 0 0\n1 0 0 0 1 40 35 39\n\n6 3\n1 40 35 39\n1 40 35 39 1 0 0 0\n2 0 0 0 \n1 0 0 0 1 255 0 0 1 40 35 39\n1 0 0 0\n1 0 0 0";
                File.WriteAllText(Path.Combine(colorPath, "6x6", "Пистолет.jap"), data);
            }

            if (!File.Exists(Path.Combine(colorPath, "6x6", "Цветок.jap")))
            {
                string data = "Цветок\n3\n128 1 240 245 223 2 2 212 1\n6 4\n1 128 1 240 2 245 223 2\n1 128 1 240 1 245 223 2 1 128 1 240 2 245 223 2\n1 128 1 240\n2 2 212 1\n1 2 212 1\n6 2 212 1\n6 4\n1 2 212 1\n1 128 1 240 1 2 212 1\n1 128 1 240 1 245 223 2 1 128 1 240 3 2 212 1\n1 128 1 240 1 2 212 1 1 2 212 1\n2 245 223 2 1 2 212 1\n2 245 223 2 1 2 212 1";
                File.WriteAllText(Path.Combine(colorPath, "6x6", "Цветок.jap"), data);
            }

            if (!File.Exists(Path.Combine(colorPath, "6x6", "Ссора.jap")))
            {
                string data = "Ссора\n4\n0 0 0 0 0 238 243 1 0 214 182 97\n6 4\n1 0 0 0 1 0 0 0\n1 0 0 238 1 243 1 0\n1 0 0 238 1 243 1 0\n2 0 0 238 2 243 1 0\n1 0 0 238 4 214 182 97 1 243 1 0\n1 0 0 238 1 214 182 97 1 214 182 97 1 243 1 0\n6 2\n1 0 0 0 3 0 0 238\n3 0 0 238 2 214 182 97\n1 214 182 97\n1 214 182 97\n3 243 1 0 2 214 182 97\n1 0 0 0 3 243 1 0";
                File.WriteAllText(Path.Combine(colorPath, "6x6", "Ссора.jap"), data);
            }

            if (!File.Exists(Path.Combine(colorPath, "6x6", "Кот.jap")))
            {
                string data = "Кот\n2\n0 225 241 0 212 0\n6 2\n1 0 225 241 2 0 225 241\n2 0 225 241\n1 0 225 241 3 0 225 241\n1 0 225 241\n1 0 212 0 3 0 212 0\n2 0 212 0\n6 2\n2 0 225 241 1 0 212 0\n1 0 225 241\n1 0 225 241 2 0 212 0\n3 0 225 241 2 0 212 0\n3 0 225 241 1 0 212 0\n\n";
                File.WriteAllText(Path.Combine(colorPath, "6x6", "Кот.jap"), data);
            }

            if (!Directory.Exists(Path.Combine(colorPath, "6x5")))
            {
                Directory.CreateDirectory(Path.Combine(colorPath, "6x5"));
            }

            if (!File.Exists(Path.Combine(colorPath, "6x5", "Русский.jap")))
            {
                string data = "Русский\n4\n0 0 0 200 152 152 245 1 0 0 0 236\n6 3\n1 0 0 0\n1 200 152 152\n3 245 1 0\n1 245 1 0 1 200 152 152 1 245 1 0\n1 0 0 0 3 0 0 236\n1 0 0 0\n5 5\n1 0 0 0\n2 245 1 0 1 0 0 236\n1 0 0 0 1 200 152 152 1 245 1 0 1 200 152 152 1 0 0 236\n2 245 1 0 1 0 0 236 1 0 0 0\n\n";
                File.WriteAllText(Path.Combine(colorPath, "6x5", "Русский.jap"), data);
            }

            if (!Directory.Exists(Path.Combine(colorPath, "5x6")))
            {
                Directory.CreateDirectory(Path.Combine(colorPath, "5x6"));
            }

            if (!File.Exists(Path.Combine(colorPath, "5x6", "Кактус.jap")))
            {
                string data = "Кактус\n2\n99 154 97 78 78 76\n5 4\n2 99 154 97\n1 99 154 97 1 78 78 76 1 99 154 97\n1 99 154 97 4 99 154 97\n1 99 154 97 1 78 78 76 2 99 154 97 1 78 78 76\n1 99 154 97 1 78 78 76\n6 4\n2 99 154 97\n1 78 78 76\n5 99 154 97\n1 99 154 97 1 78 78 76 2 99 154 97 1 78 78 76\n1 99 154 97\n2 99 154 97 1 78 78 76";
                File.WriteAllText(Path.Combine(colorPath, "5x6", "Кактус.jap"), data);
            }

            if (!File.Exists(Path.Combine(colorPath, "5x6", "Пейзаж.jap")))
            {
                string data = "Пейзаж\n3\n0 0 0 242 0 0 0 151 240\n5 2\n1 0 0 0\n1 0 0 0 1 242 0 0\n1 0 0 0\n1 0 0 0 5 0 151 240\n1 0 0 0 5 0 151 240\n6 2\n5 0 0 0\n2 0 151 240\n2 0 151 240\n1 242 0 0 2 0 151 240\n2 0 151 240\n2 0 151 240";
                File.WriteAllText(Path.Combine(colorPath, "5x6", "Пейзаж.jap"), data);
            }

            if (!Directory.Exists(Path.Combine(colorPath, "7x7")))
            {
                Directory.CreateDirectory(Path.Combine(colorPath, "7x7"));
            }

            if (!File.Exists(Path.Combine(colorPath, "7x7", "Лиса.jap")))
            {
                string data = "Лиса\n2\n0 0 0 244 129 0\n7 5\n1 0 0 0 1 0 0 0\n2 244 129 0 2 244 129 0\n7 244 129 0\n2 244 129 0 1 0 0 0 1 244 129 0 1 0 0 0 2 244 129 0\n5 244 129 0\n3 244 129 0\n1 0 0 0\n7 3\n1 0 0 0 3 244 129 0\n4 244 129 0\n1 244 129 0 1 0 0 0 2 244 129 0\n4 244 129 0 1 0 0 0\n1 244 129 0 1 0 0 0 2 244 129 0\n4 244 129 0\n1 0 0 0 3 244 129 0";
                File.WriteAllText(Path.Combine(colorPath, "7x7", "Лиса.jap"), data);
            }

            if (!File.Exists(Path.Combine(colorPath, "7x7", "Морковь.jap")))
            {
                string data = "Морковь\n2\n1 211 0 243 94 0\n7 2\n1 1 211 0 2 1 211 0\n2 1 211 0 1 1 211 0\n3 1 211 0\n5 243 94 0\n3 243 94 0\n3 243 94 0\n1 243 94 0\n7 2\n\n2 1 211 0 1 243 94 0\n2 1 211 0 3 243 94 0\n1 1 211 0 4 243 94 0\n3 1 211 0 3 243 94 0\n1 1 211 0 1 243 94 0\n\n";
                File.WriteAllText(Path.Combine(colorPath, "7x7", "Морковь.jap"), data);
            }

            string blackPath = Path.Combine(tmpPath, "Черно-белые кроссворды");

            if (!Directory.Exists(Path.Combine(tmpPath, "Черно-белые кроссворды")))
            {
                Directory.CreateDirectory(blackPath);
            }

            if (!Directory.Exists(Path.Combine(blackPath, "5x5")))
            {
                Directory.CreateDirectory(Path.Combine(blackPath, "5x5"));
            }

            if (!File.Exists(Path.Combine(blackPath, "5x5", "Башня.jap")))
            {
                string tower = "Башня\n1\n0 0 0\n5 3\n1 0 0 0 1 0 0 0 1 0 0 0\n5 0 0 0\n3 0 0 0\n1 0 0 0 1 0 0 0\n3 0 0 0\n5 2\n2 0 0 0\n4 0 0 0\n3 0 0 0 1 0 0 0\n4 0 0 0\n2 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "5x5", "Башня.jap"), tower);
            }
            if (!File.Exists(Path.Combine(blackPath, "5x5", "Диез.jap")))
            {
                string dies = "Диез\n1\n0 0 0\n5 2\n1 0 0 0 1 0 0 0\n5 0 0 0\n1 0 0 0 1 0 0 0\n5 0 0 0\n1 0 0 0 1 0 0 0\n5 2\n1 0 0 0 1 0 0 0\n5 0 0 0\n1 0 0 0 1 0 0 0\n5 0 0 0\n1 0 0 0 1 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "5x5", "Диез.jap"), dies);
            }
            if (!File.Exists(Path.Combine(blackPath, "5x5", "Змейка.jap")))
            {
                string snake = "Змейка\n1\n0 0 0\n5 1\n5 0 0 0\n1 0 0 0\n5 0 0 0\n1 0 0 0\n5 0 0 0\n5 3\n3 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 3 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "5x5", "Змейка.jap"), snake);
            }
            if (!File.Exists(Path.Combine(blackPath, "5x5", "Летучая мышь.jap")))
            {
                string bat = "Летучая мышь\n1\n0 0 0\n5 3\n1 0 0 0 1 0 0 0\n5 0 0 0\n5 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0\n5 1\n4 0 0 0\n3 0 0 0\n3 0 0 0\n3 0 0 0\n4 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "5x5", "Летучая мышь.jap"), bat);
            }
            if (!File.Exists(Path.Combine(blackPath, "5x5", "Пси.jap")))
            {
                string psi = "Пси\n1\n0 0 0\n5 3\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n3 0 0 0\n1 0 0 0\n5 1\n3 0 0 0\n1 0 0 0\n5 0 0 0\n1 0 0 0\n3 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "5x5", "Пси.jap"), psi);
            }
            if (!File.Exists(Path.Combine(blackPath, "5x5", "Собака.jap")))
            {
                string dog = "Собака\n1\n0 0 0\n5 2\n1 0 0 0 1 0 0 0\n2 0 0 0 1 0 0 0\n5 0 0 0\n4 0 0 0\n1 0 0 0 1 0 0 0\n5 1\n2 0 0 0\n5 0 0 0\n2 0 0 0\n2 0 0 0\n5 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "5x5", "Собака.jap"), dog);
            }

            if (!Directory.Exists(Path.Combine(blackPath, "5x7")))
            {
                Directory.CreateDirectory(Path.Combine(blackPath, "5x7"));
            }

            if (!File.Exists(Path.Combine(blackPath, "5x7", "Не равно.jap")))
            {
                string data = "Не равно\n1\n0 0 0\n5 1\n1 0 0 0\n5 0 0 0\n1 0 0 0\n5 0 0 0\n1 0 0 0\n7 2\n\n1 0 0 0 1 0 0 0\n1 0 0 0 2 0 0 0\n3 0 0 0\n2 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0\n\n";
                File.WriteAllText(Path.Combine(blackPath, "5x7", "Не равно.jap"), data);
            }
            if (!File.Exists(Path.Combine(blackPath, "5x7", "Шляпа.jap")))
            {
                string data = "Шляпа\n1\n0 0 0\n5 1\n3 0 0 0\n5 0 0 0\n5 0 0 0\n5 0 0 0\n7 0 0 0\n7 1\n1 0 0 0\n4 0 0 0\n5 0 0 0\n5 0 0 0\n5 0 0 0\n4 0 0 0\n1 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "5x7", "Шляпа.jap"), data);
            }

            if (!Directory.Exists(Path.Combine(blackPath, "7x7")))
            {
                Directory.CreateDirectory(Path.Combine(blackPath, "7x7"));
            }

            if (!File.Exists(Path.Combine(blackPath, "7x7", "Стрела.jap")))
            {
                string data = "Стрела\n1\n0 0 0\n7 3\n5 0 0 0\n3 0 0 0\n5 0 0 0\n2 0 0 0 1 0 0 0 1 0 0 0\n2 0 0 0 2 0 0 0 1 0 0 0\n1 0 0 0 2 0 0 0\n3 0 0 0\n7 3\n3 0 0 0\n2 0 0 0 1 0 0 0\n1 0 0 0 2 0 0 0 2 0 0 0\n1 0 0 0 1 0 0 0 2 0 0 0\n5 0 0 0\n3 0 0 0\n5 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "7x7", "Стрела.jap"), data);
            }

            if (!File.Exists(Path.Combine(blackPath, "7x7", "Верблюд.jap")))
            {
                string data = "Верблюд\n1\n0 0 0\n7 2\n2 0 0 0\n1 0 0 0 3 0 0 0\n1 0 0 0 4 0 0 0\n6 0 0 0\n1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0\n7 1\n1 0 0 0\n4 0 0 0\n4 0 0 0\n3 0 0 0\n3 0 0 0\n3 0 0 0\n5 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "7x7", "Верблюд.jap"), data);
            }

            if (!File.Exists(Path.Combine(blackPath, "7x7", "Ножницы.jap")))
            {
                string data = "Ножницы\n1\n0 0 0\n7 4\n2 0 0 0 2 0 0 0\n2 0 0 0 2 0 0 0\n3 0 0 0\n1 0 0 0\n3 0 0 0 3 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0 1 0 0 0\n3 0 0 0 3 0 0 0\n7 3\n1 0 0 0 3 0 0 0\n2 0 0 0 1 0 0 0 1 0 0 0\n2 0 0 0 3 0 0 0\n2 0 0 0\n2 0 0 0 3 0 0 0\n2 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 3 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "7x7", "Ножницы.jap"), data);
            }

            if (!File.Exists(Path.Combine(blackPath, "7x7", "Яблоко.jap")))
            {
                string data = "Яблоко\n1\n0 0 0\n7 3\n1 0 0 0\n5 0 0 0\n2 0 0 0 1 0 0 0 2 0 0 0\n1 0 0 0 1 0 0 0\n1 0 0 0 2 0 0 0\n1 0 0 0 2 0 0 0\n3 0 0 0\n7 2\n3 0 0 0\n2 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0\n3 0 0 0 1 0 0 0\n1 0 0 0 2 0 0 0\n2 0 0 0 2 0 0 0\n3 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "7x7", "Яблоко.jap"), data);
            }

            if (!File.Exists(Path.Combine(blackPath, "7x7", "Лист.jap")))
            {
                string data = "Лист\n1\n0 0 0\n7 3\n3 0 0 0 1 0 0 0\n2 0 0 0 2 0 0 0\n1 0 0 0 2 0 0 0\n2 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 2 0 0 0\n1 0 0 0 3 0 0 0\n4 0 0 0\n7 3\n4 0 0 0\n3 0 0 0 1 0 0 0\n2 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0 2 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n2 0 0 0 2 0 0 0\n2 0 0 0 2 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "7x7", "Лист.jap"), data);
            }

            if (!File.Exists(Path.Combine(blackPath, "7x7", "Замок.jap")))
            {
                string data = "Замок\n1\n0 0 0\n7 3\n1 0 0 0 1 0 0 0 1 0 0 0\n7 0 0 0\n5 0 0 0\n5 0 0 0\n2 0 0 0 2 0 0 0\n2 0 0 0 2 0 0 0\n7 0 0 0\n7 2\n1 0 0 0 1 0 0 0\n7 0 0 0\n6 0 0 0\n4 0 0 0 1 0 0 0\n6 0 0 0\n7 0 0 0\n1 0 0 0 1 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "7x7", "Замок.jap"), data);
            }

            if (!File.Exists(Path.Combine(blackPath, "7x7", "Нота.jap")))
            {
                string data = "Нота\n1\n0 0 0\n7 2\n1 0 0 0\n2 0 0 0\n1 0 0 0 1 0 0 0\n3 0 0 0 1 0 0 0\n4 0 0 0\n4 0 0 0\n2 0 0 0\n7 1\n2 0 0 0\n4 0 0 0\n4 0 0 0\n6 0 0 0\n1 0 0 0\n2 0 0 0\n\n";
                File.WriteAllText(Path.Combine(blackPath, "7x7", "Нота.jap"), data);
            }

            if (!File.Exists(Path.Combine(blackPath, "7x7", "Руна.jap")))
            {
                string data = "Руна\n1\n0 0 0\n7 3\n3 0 0 0\n1 0 0 0 1 0 0 0\n1 0 0 0\n7 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 4 0 0 0\n7 3\n3 0 0 0\n1 0 0 0 1 0 0 0\n1 0 0 0\n7 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0 4 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "7x7", "Руна.jap"), data);
            }

            if (!Directory.Exists(Path.Combine(blackPath, "6x6")))
            {
                Directory.CreateDirectory(Path.Combine(blackPath, "6x6"));
            }

            if (!File.Exists(Path.Combine(blackPath, "6x6", "Токсин.jap")))
            {
                string data = "Токсин\n1\n0 0 0\n6 3\n2 0 0 0\n3 0 0 0\n4 0 0 0\n4 0 0 0 1 0 0 0\n2 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n6 2\n1 0 0 0 1 0 0 0\n2 0 0 0\n4 0 0 0\n3 0 0 0\n3 0 0 0\n4 0 0 0 1 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "6x6", "Токсин.jap"), data);
            }

            if (!File.Exists(Path.Combine(blackPath, "6x6", "Лестница.jap")))
            {
                string data = "Лестница\n1\n0 0 0\n6 2\n2 0 0 0\n3 0 0 0\n2 0 0 0 1 0 0 0\n2 0 0 0 1 0 0 0\n2 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0\n6 1\n2 0 0 0\n2 0 0 0\n2 0 0 0\n2 0 0 0\n2 0 0 0\n6 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "6x6", "Лестница.jap"), data);
            }

            if (!File.Exists(Path.Combine(blackPath, "6x6", "Кунай.jap")))
            {
                string data = "Кунай\n1\n0 0 0\n6 2\n2 0 0 0 1 0 0 0\n3 0 0 0\n1 0 0 0 2 0 0 0\n1 0 0 0 2 0 0 0\n4 0 0 0\n1 0 0 0 3 0 0 0\n6 2\n2 0 0 0 1 0 0 0\n3 0 0 0\n1 0 0 0 2 0 0 0\n1 0 0 0 2 0 0 0\n4 0 0 0\n1 0 0 0 3 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "6x6", "Кунай.jap"), data);
            }

            if (!File.Exists(Path.Combine(blackPath, "6x6", "Черепашка.jap")))
            {
                string data = "Черепашка\n1\n0 0 0\n6 2\n2 0 0 0\n1 0 0 0 1 0 0 0\n4 0 0 0\n5 0 0 0\n1 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0\n6 1\n1 0 0 0\n4 0 0 0\n3 0 0 0\n2 0 0 0\n6 0 0 0\n1 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "6x6", "Черепашка.jap"), data);
            }

            if (!Directory.Exists(Path.Combine(blackPath, "5x6")))
            {
                Directory.CreateDirectory(Path.Combine(blackPath, "5x6"));
            }

            if (!File.Exists(Path.Combine(blackPath, "5x6", "Червяк.jap")))
            {
                string data = "Червяк\n1\n0 0 0\n5 2\n4 0 0 0\n2 0 0 0 1 0 0 0\n4 0 0 0\n1 0 0 0 2 0 0 0\n4 0 0 0\n6 2\n2 0 0 0\n1 0 0 0\n5 0 0 0\n5 0 0 0\n1 0 0 0 1 0 0 0\n3 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "5x6", "Червяк.jap"), data);
            }

            if (!File.Exists(Path.Combine(blackPath, "5x6", "Лампочка.jap")))
            {
                string data = "Лампочка\n1\n0 0 0\n5 3\n1 0 0 0 2 0 0 0 1 0 0 0\n2 0 0 0 1 0 0 0\n1 0 0 0 1 0 0 0\n1 0 0 0 2 0 0 0 1 0 0 0\n2 0 0 0\n6 2\n1 0 0 0 1 0 0 0\n2 0 0 0\n2 0 0 0 2 0 0 0\n1 0 0 0 2 0 0 0\n2 0 0 0\n1 0 0 0 1 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "5x6", "Лампочка.jap"), data);
            }

            if (!File.Exists(Path.Combine(blackPath, "5x6", "Гаст.jap")))
            {
                string data = "Гаст\n1\n0 0 0\n5 2\n\n2 0 0 0 2 0 0 0\n\n2 0 0 0\n\n6 1\n1 0 0 0\n1 0 0 0\n1 0 0 0\n1 0 0 0\n1 0 0 0\n1 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "5x6", "Гаст.jap"), data);
            }

            if (!File.Exists(Path.Combine(blackPath, "5x6", "США.jap")))
            {
                string data = "США\n1\n0 0 0\n5 2\n\n4 0 0 0 1 0 0 0\n5 0 0 0\n3 0 0 0\n1 0 0 0 1 0 0 0\n6 2\n2 0 0 0\n3 0 0 0\n4 0 0 0\n3 0 0 0\n1 0 0 0 1 0 0 0\n1 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "5x6", "США.jap"), data);
            }

            if (!Directory.Exists(Path.Combine(blackPath, "7x6")))
            {
                Directory.CreateDirectory(Path.Combine(blackPath, "7x6"));
            }

            if (!File.Exists(Path.Combine(blackPath, "7x6", "Делить.jap")))
            {
                string data = "Делить\n1\n0 0 0\n7 1\n\n1 0 0 0\n\n5 0 0 0\n\n1 0 0 0\n\n6 3\n\n1 0 0 0\n1 0 0 0\n1 0 0 0 1 0 0 0 1 0 0 0\n1 0 0 0\n1 0 0 0";
                File.WriteAllText(Path.Combine(blackPath, "7x6", "Делить.jap"), data);
            }
        }

        private async void AboutApplication_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("О приложении", "Приложение \"Nonograms\" позволяет решать цветные и черно-белые японские кроссворды, а также создавать свои кроссворды и получать решения нонограмм", "Ок");
        }

        private async void AboutAuthor_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Об авторе", "Программист: Чувызгалов Кирилл Андреевич (mr.chvuvyzgalov@gmail.com)\nДизайнеры: Яковлева Анастасия Алексеевна и Петропавловских Елизавета", "Ок");
        }

        private async void PlayButton_Clicked(object sender, EventArgs e)
        {
            Menu page = new Menu(Path.Combine(folderPath, "Data"));
            await Navigation.PushAsync(page);
        }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string[] lines = File.ReadAllText(Path.Combine(folderPath, "settings.txt")).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                string ans = await DisplayActionSheet("Текущий уровень сложности: " + lines[0], null, null, "Легкий", "Средний", "Сложный");
                if (ans != null)
                {
                    File.WriteAllText(Path.Combine(folderPath, "settings.txt"), ans + "\n" + lines[1]);
                }
            }
            catch
            {
                await DisplayAlert("Ошибка", "Ошибка чтения файла настроек", "Ок");
            }
        }
    }
}
