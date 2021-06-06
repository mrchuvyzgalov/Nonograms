using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nonograms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrawNonogram : ContentPage
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        int rows, cols;
        string name;
        ColorRGB tmpColor = new ColorRGB(255, 255, 255);
        public DrawNonogram(string name, int rows, int cols)
        {
            InitializeComponent();

            this.name = name;
            this.rows = rows;
            this.cols = cols;

            DrawMap();
        }

        private void DrawMap()
        {
            // информация о строчках
            for (int i = 0; i < rows; ++i)
            {
                table.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // информация о столбцах
            for (int i = 0; i < cols; ++i)
            {
                table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // создание кнопок
            for (int row = 0; row < rows; ++row)
            {
                for (int col = 0; col < cols; ++col)
                {
                    Button button = new Button
                    {
                        BackgroundColor = Color.White
                    };

                    button.Clicked += Button_Clicked;

                    table.Children.Add(button, col, row);
                }
            }
        }

        private void GetCoordinates(int number, out int x, out int y)
        {
            y = number % cols;
            x = number / cols;
        }

        private async void Button_Click_Color(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            for (int i = 0; i < colorsGrid.Children.Count; ++i)
            {
                Button tmp = ((Button)colorsGrid.Children[i]);
                tmp.BorderWidth = 1;
                ColorRGB color = new ColorRGB((int)(tmp.BackgroundColor.R * 255), (int)(tmp.BackgroundColor.G * 255), (int)(tmp.BackgroundColor.B * 255));
                if (color.isBright())
                {
                    tmp.BorderColor = Color.Black;
                }
                else
                {
                    tmp.BorderColor = Color.White;
                }
            }

            button.BorderWidth = 5;

            if (button.Text == "⋮")
            {
                await Navigation.PushAsync(new RGBPage(tmpColor));
            }
            else
            {
                int red = (int)(button.BackgroundColor.R * 255);
                int green = (int)(button.BackgroundColor.G * 255);
                int blue = (int)(button.BackgroundColor.B * 255);
                tmpColor = new ColorRGB(red, green, blue);
            }
        }

        private async void Create_Clicked(object sender, EventArgs e)
        {
            bool hasNoWhite = false;
            for (int row = 0; row < rows && !hasNoWhite; ++row)
            {
                for (int col = 0; col < cols && !hasNoWhite; ++col)
                {
                    Button button = (Button)table.Children[GetIndex(row, col)];
                    int red = (int)(button.BackgroundColor.R * 255);
                    int green = (int)(button.BackgroundColor.G * 255);
                    int blue = (int)(button.BackgroundColor.B * 255);

                    hasNoWhite = new ColorRGB(red, green, blue) != new ColorRGB(255, 255, 255); 
                }
            }

            if (!hasNoWhite)
            {
                await DisplayAlert("Ошибка", "Вы забыли нарисовать изображение", "Ок");
                return;
            }

            HashSet<ColorRGB> colors = new HashSet<ColorRGB>();
            for (int row = 0; row < rows; ++row)
            {
                for (int col = 0; col < cols; ++col)
                {
                    Button button = (Button)table.Children[GetIndex(row, col)];
                    int red = (int)(button.BackgroundColor.R * 255);
                    int green = (int)(button.BackgroundColor.G * 255);
                    int blue = (int)(button.BackgroundColor.B * 255);
                    if (new ColorRGB(red, green, blue) != new ColorRGB(255, 255, 255))
                    {
                        colors.Add(new ColorRGB(red, green, blue));
                    }
                }
            }

            if (colors.Count > 5)
            {
                await DisplayAlert("Ошибка", "Нельзя использовать более пяти цветов в изображении", "Ок");
                return;
            }

            string data = "\n";
            data += colors.Count + "\n";
            foreach (ColorRGB color in colors)
            {
                data += color + " ";
            }
            data += "\n";
            List<List<KeyValuePair<int, ColorRGB>>> m_rows = new List<List<KeyValuePair<int, ColorRGB>>>();
            List<List<KeyValuePair<int, ColorRGB>>> m_cols = new List<List<KeyValuePair<int, ColorRGB>>>();

            for (int row = 0; row < rows; ++row)
            {
                List<KeyValuePair<int, ColorRGB>> tmpRow = new List<KeyValuePair<int, ColorRGB>>();
                bool newGroup = true;
                for (int col = 0; col < cols; ++col)
                {
                    Button button = (Button)table.Children[GetIndex(row, col)];
                    int red = (int)(button.BackgroundColor.R * 255);
                    int green = (int)(button.BackgroundColor.G * 255);
                    int blue = (int)(button.BackgroundColor.B * 255);

                    if (red != 255 || green != 255 || blue != 255)
                    {
                        if (newGroup)
                        {
                            KeyValuePair<int, ColorRGB> pair = new KeyValuePair<int, ColorRGB>(1, new ColorRGB(red, green, blue));
                            tmpRow.Add(pair);
                        }
                        else
                        {
                            if (new ColorRGB(red, green, blue) == tmpRow[tmpRow.Count - 1].Value)
                            {
                                tmpRow[tmpRow.Count - 1] = new KeyValuePair<int, ColorRGB>(tmpRow[tmpRow.Count - 1].Key + 1, tmpRow[tmpRow.Count - 1].Value);
                            }
                            else
                            {
                                KeyValuePair<int, ColorRGB> pair = new KeyValuePair<int, ColorRGB>(1, new ColorRGB(red, green, blue));
                                tmpRow.Add(pair);
                            }
                        }

                        newGroup = false;
                    }
                    else
                    {
                        newGroup = true;
                    }
                }

                m_rows.Add(tmpRow);
            }

            for (int col = 0; col < cols; ++col)
            {
                List<KeyValuePair<int, ColorRGB>> tmpCol = new List<KeyValuePair<int, ColorRGB>>();
                bool newGroup = true;
                for (int row = 0; row < rows; ++row)
                {
                    Button button = (Button)table.Children[GetIndex(row, col)];
                    int red = (int)(button.BackgroundColor.R * 255);
                    int green = (int)(button.BackgroundColor.G * 255);
                    int blue = (int)(button.BackgroundColor.B * 255);

                    if (red != 255 || green != 255 || blue != 255)
                    {
                        if (newGroup)
                        {
                            KeyValuePair<int, ColorRGB> pair = new KeyValuePair<int, ColorRGB>(1, new ColorRGB(red, green, blue));
                            tmpCol.Add(pair);
                        }
                        else
                        {
                            if (new ColorRGB(red, green, blue) == tmpCol[tmpCol.Count - 1].Value)
                            {
                                tmpCol[tmpCol.Count - 1] = new KeyValuePair<int, ColorRGB>(tmpCol[tmpCol.Count - 1].Key + 1, tmpCol[tmpCol.Count - 1].Value);
                            }
                            else
                            {
                                KeyValuePair<int, ColorRGB> pair = new KeyValuePair<int, ColorRGB>(1, new ColorRGB(red, green, blue));
                                tmpCol.Add(pair);
                            }
                        }

                        newGroup = false;
                    }
                    else
                    {
                        newGroup = true;
                    }
                }

                m_cols.Add(tmpCol);
            }

            if (NonogramSolve.Solve(new Matrix<KeyValuePair<int, ColorRGB>>(m_rows), new Matrix<KeyValuePair<int, ColorRGB>>(m_cols), colors) == null)
            {
                await DisplayAlert("Неподходящий кроссворд", "Нарисуйте другое изображение. Кроссворд имеет несколько решений или его очень долго решать", "Ок");
                return;
            }

            int rowsMaxi = 0;
            for (int row = 0; row < rows; ++row)
            {
                if (rowsMaxi < m_rows[row].Count)
                {
                    rowsMaxi = m_rows[row].Count;
                }
            }

            int colsMaxi = 0;
            for (int col = 0; col < cols; ++col)
            {
                if (colsMaxi < m_cols[col].Count)
                {
                    colsMaxi = m_cols[col].Count;
                }
            }

            data += "" + rows + " " + rowsMaxi + "\n";
            for (int row = 0; row < rows; ++row)
            {
                for (int col = 0; col < m_rows[row].Count; ++col)
                {
                    data += "" + m_rows[row][col].Key + " " + m_rows[row][col].Value + " ";
                }
                data += "\n";
            }
            data += "" + cols + " " + colsMaxi + "\n";
            for (int i = 0; i < cols; ++i)
            {
                for (int j = 0; j < m_cols[i].Count; ++j)
                {
                    data += "" + m_cols[i][j].Key + " " + m_cols[i][j].Value + " ";
                }
                data += "\n";
            }

            string path = Path.Combine(folderPath, "Data");

            if (colors.Count == 1 && colors.Contains(new ColorRGB(0,0,0)))
            {
                path = Path.Combine(path, "Черно-белые кроссворды");
            }
            else
            {
                path = Path.Combine(path, "Цветные кроссворды");
            }

            path = Path.Combine(path, "" + rows + "x" + cols);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(Path.Combine(path, name) + ".jap"))
            {
                int numb = 1;
                string filename = name + "" + numb;
                while (File.Exists(Path.Combine(path, filename) + ".jap"))
                {
                    numb++;
                    filename = filename.Substring(0, name.Length) + "" + numb;
                }

                File.WriteAllText(Path.Combine(path, filename) + ".jap", filename + data);
            }
            else
            {
                File.WriteAllText(Path.Combine(path, name) + ".jap", name + data);
            }

            await Navigation.PopToRootAsync();
        }

        private int GetIndex(int x, int y)
        {
            return x * cols + y;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackgroundColor = Color.FromRgb(tmpColor.Red, tmpColor.Green, tmpColor.Blue);
        }
    }
}