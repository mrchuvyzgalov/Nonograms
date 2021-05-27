using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Castle.Core;

namespace Nonograms
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Nonogram : ContentPage
	{
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        Matrix<KeyValuePair<int, ColorRGB>> m_row, m_col;
        Matrix<ColorRGB> matrix;
        Matrix<bool> hasXmatrix;
        HashSet<ColorRGB> colors = new HashSet<ColorRGB>();
        ColorRGB tmpColor = new ColorRGB(255, 255, 255);
        bool hasX = false;
        string savepath;
        string[] parts;
        public Nonogram (string path)
		{
			InitializeComponent ();

            string tmppath = @path.Substring(path.LastIndexOf("Data") + 4) + "s";

            if (tmppath.Contains("\\"))
            {
                parts = tmppath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                parts = tmppath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            }

            savepath = Path.Combine(folderPath, tmppath.Substring(1));



            ToolbarItem clearButton = new ToolbarItem()
            {
                Text = "Начать заново",
                Order = ToolbarItemOrder.Secondary,
                Priority = 0
            };

            clearButton.Clicked += (s,e) =>
            {
                if (File.Exists(@savepath))
                {
                    File.Delete(savepath);
                }

                try
                {
                    ReadFile(@path);
                    DrawTable();
                }
                catch
                {
                    PrintError("Файла не существует или в нем некорректные данные");
                }
            };

            ToolbarItems.Add(clearButton);

            try
            {
                if (File.Exists(savepath))
                {
                    try
                    {
                        ReadFullFile(savepath);
                    }
                    catch
                    { 
                        ReadFile(@path);
                    }
                }
                else
                {
                    ReadFile(@path);
                }
                DrawTable();
            }
            catch
            {
                PrintError("Файла не существует или в нем некорректные данные");
            }

        }

        private void ReadFile(string file)
        {
            using (StreamReader sr = new StreamReader(@file))
            {
                // считывание имени файла
                Title = sr.ReadLine();

                // считывание количества цветов
                int countOfColors = int.Parse(sr.ReadLine());

                // считывание цветов
                string[] line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (line.Length != countOfColors * 3) throw new Exception("Количество цветов не равно количеству цветов в списке");
                for (int i = 0; i < countOfColors; ++i)
                {
                    int red = int.Parse(line[i * 3]);
                    int green = int.Parse(line[i * 3 + 1]);
                    int blue = int.Parse(line[i * 3 + 2]);
                    colors.Add(new ColorRGB(red, green, blue));
                }
                if (colors.Count != countOfColors) throw new Exception("Количество цветов не равно количеству цветов в списке");

                // считывание матрицы строк
                line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (line.Length != 2 || int.Parse(line[0]) < 1 || int.Parse(line[1]) < 1) throw new Exception("Неверный ввод размера матрицы строк");

                m_row = new Matrix<KeyValuePair<int, ColorRGB>>(new KeyValuePair<int, ColorRGB>[int.Parse(line[0]), int.Parse(line[1])]);
                for (int row = 0; row < m_row.Rows; ++row)
                {
                    line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (line.Length > m_row.Cols * 4 || line.Length % 4 != 0) throw new Exception("Неверные формат строчки");

                    for (int col = 0; col < m_row.Cols - line.Length / 4; ++col)
                    {
                        m_row[row, col] = new KeyValuePair<int, ColorRGB>(0, new ColorRGB());
                    }
                    for (int col = m_row.Cols - line.Length / 4, i = 0; col < m_row.Cols; ++col, i += 4)
                    {
                        int count = int.Parse(line[i]);
                        if (count < 1) throw new Exception("Неверная длина блока");
                        int red = int.Parse(line[i + 1]);
                        int green = int.Parse(line[i + 2]);
                        int blue = int.Parse(line[i + 3]);
                        m_row[row, col] = new KeyValuePair<int, ColorRGB>(count, new ColorRGB(red, green, blue));
                    }
                }

                // считывание матрицы столбцов
                line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (line.Length != 2 || int.Parse(line[0]) < 1 || int.Parse(line[1]) < 1) throw new Exception("Неверный ввод размера матрицы столбцов");

                m_col = new Matrix<KeyValuePair<int, ColorRGB>>(new KeyValuePair<int, ColorRGB>[int.Parse(line[0]), int.Parse(line[1])]);
                for (int row = 0; row < m_col.Rows; ++row)
                {
                    line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (line.Length > m_col.Cols * 4 || line.Length % 4 != 0) throw new Exception("Неверные формат столбца");

                    for (int col = 0; col < m_col.Cols - line.Length / 4; ++col)
                    {
                        m_col[row, col] = new KeyValuePair<int, ColorRGB>(0, new ColorRGB());
                    }
                    for (int col = m_col.Cols - line.Length / 4, i = 0; col < m_col.Cols; ++col, i += 4)
                    {
                        int count = int.Parse(line[i]);
                        if (count < 1) throw new Exception("Неверная длина блока");
                        int red = int.Parse(line[i + 1]);
                        int green = int.Parse(line[i + 2]);
                        int blue = int.Parse(line[i + 3]);
                        m_col[row, col] = new KeyValuePair<int, ColorRGB>(count, new ColorRGB(red, green, blue));
                    }
                }
            }

            ColorRGB[,] ArrayMatrix = new ColorRGB[m_row.Rows, m_col.Rows];
            for (int i = 0; i < ArrayMatrix.GetLength(0); ++i)
            {
                for (int j = 0; j < ArrayMatrix.GetLength(1); ++j)
                {
                    ArrayMatrix[i, j] = new ColorRGB(255, 255, 255);
                }
            }

            matrix = new Matrix<ColorRGB>(ArrayMatrix);

            bool[,] ArrayHasX = new bool[m_row.Rows, m_col.Rows];
            for (int i = 0; i < ArrayHasX.GetLength(0); ++i)
            {
                for (int j = 0; j < ArrayHasX.GetLength(1); ++j)
                {
                    ArrayHasX[i, j] = false;
                }
            }

            hasXmatrix = new Matrix<bool>(ArrayHasX);
        }

        private void ReadFullFile(string file)
        {
            using (StreamReader sr = new StreamReader(@file))
            {
                // считывание имени файла
                Title = sr.ReadLine();

                // считывание количества цветов
                int countOfColors = int.Parse(sr.ReadLine());

                // считывание цветов
                string[] line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (line.Length != countOfColors * 3) throw new Exception("Количество цветов не равно количеству цветов в списке");
                for (int i = 0; i < countOfColors; ++i)
                {
                    int red = int.Parse(line[i * 3]);
                    int green = int.Parse(line[i * 3 + 1]);
                    int blue = int.Parse(line[i * 3 + 2]);
                    colors.Add(new ColorRGB(red, green, blue));
                }
                if (colors.Count != countOfColors) throw new Exception("Количество цветов не равно количеству цветов в списке");

                // считывание матрицы строк
                line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (line.Length != 2 || int.Parse(line[0]) < 1 || int.Parse(line[1]) < 1) throw new Exception("Неверный ввод размера матрицы строк");

                m_row = new Matrix<KeyValuePair<int, ColorRGB>>(new KeyValuePair<int, ColorRGB>[int.Parse(line[0]), int.Parse(line[1])]);
                for (int row = 0; row < m_row.Rows; ++row)
                {
                    line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (line.Length > m_row.Cols * 4 || line.Length % 4 != 0) throw new Exception("Неверные формат строчки");

                    for (int col = 0; col < m_row.Cols - line.Length / 4; ++col)
                    {
                        m_row[row, col] = new KeyValuePair<int, ColorRGB>(0, new ColorRGB());
                    }
                    for (int col = m_row.Cols - line.Length / 4, i = 0; col < m_row.Cols; ++col, i += 4)
                    {
                        int count = int.Parse(line[i]);
                        if (count < 1) throw new Exception("Неверная длина блока");
                        int red = int.Parse(line[i + 1]);
                        int green = int.Parse(line[i + 2]);
                        int blue = int.Parse(line[i + 3]);
                        m_row[row, col] = new KeyValuePair<int, ColorRGB>(count, new ColorRGB(red, green, blue));
                    }
                }

                // считывание матрицы столбцов
                line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (line.Length != 2 || int.Parse(line[0]) < 1 || int.Parse(line[1]) < 1) throw new Exception("Неверный ввод размера матрицы столбцов");

                m_col = new Matrix<KeyValuePair<int, ColorRGB>>(new KeyValuePair<int, ColorRGB>[int.Parse(line[0]), int.Parse(line[1])]);
                for (int row = 0; row < m_col.Rows; ++row)
                {
                    line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (line.Length > m_col.Cols * 4 || line.Length % 4 != 0) throw new Exception("Неверные формат столбца");

                    for (int col = 0; col < m_col.Cols - line.Length / 4; ++col)
                    {
                        m_col[row, col] = new KeyValuePair<int, ColorRGB>(0, new ColorRGB());
                    }
                    for (int col = m_col.Cols - line.Length / 4, i = 0; col < m_col.Cols; ++col, i += 4)
                    {
                        int count = int.Parse(line[i]);
                        if (count < 1) throw new Exception("Неверная длина блока");
                        int red = int.Parse(line[i + 1]);
                        int green = int.Parse(line[i + 2]);
                        int blue = int.Parse(line[i + 3]);
                        m_col[row, col] = new KeyValuePair<int, ColorRGB>(count, new ColorRGB(red, green, blue));
                    }
                }

                ColorRGB[,] ArrayMatrix = new ColorRGB[m_row.Rows, m_col.Rows];
                for (int i = 0; i < ArrayMatrix.GetLength(0); ++i)
                {
                    line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (line.Length != ArrayMatrix.GetLength(1) * 3) throw new Exception("Неверный формат строки матрицы");
                    for (int j = 0; j < ArrayMatrix.GetLength(1); ++j)
                    {
                        int red = int.Parse(line[j * 3]);
                        int green = int.Parse(line[j * 3 + 1]);
                        int blue = int.Parse(line[j * 3 + 2]);
                        if (red < 0 || red > 255 || green < 0 || green > 255 || blue < 0 || blue > 255) throw new Exception("Неверный формат цвета");
                        ArrayMatrix[i, j] = new ColorRGB(red, green, blue);
                    }
                }

                matrix = new Matrix<ColorRGB>(ArrayMatrix);

                bool[,] ArrayHasX = new bool[m_row.Rows, m_col.Rows];
                for (int i = 0; i < ArrayHasX.GetLength(0); ++i)
                {
                    line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (line.Length != ArrayHasX.GetLength(1)) throw new Exception("Неверный формат строки матрицы");
                    for (int j = 0; j < ArrayHasX.GetLength(1); ++j)
                    {
                        int res = int.Parse(line[j]);
                        if (res != 0 && res != 1) throw new Exception("Неверный формат зачеркнутой клетки");
                        ArrayHasX[i, j] = res == 1;
                    }
                }

                hasXmatrix = new Matrix<bool>(ArrayHasX);
            }
        }

        private async void PrintError(string message)
        {
            if (await DisplayAlert("Ошибка", message, "Выход", "Попробовать снова"))
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void DrawTable()
        {
            table.RowDefinitions.Clear();
            table.ColumnDefinitions.Clear();
            table.Children.Clear();
            // информация о строчках
            for (int i = 0; i < m_row.Rows + m_col.Cols; ++i)
            {
                table.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // информация о столбцах
            for (int i = 0; i < m_row.Cols + m_col.Rows; ++i)
            {
                table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // создание BoxView
            for (int row = 0; row < table.RowDefinitions.Count; ++row)
            {
                for (int col = 0; col < table.ColumnDefinitions.Count; ++col)
                {
                    if (row >= m_col.Cols && col >= m_row.Cols)
                    {
                        ColorRGB color = matrix[row - m_col.Cols, col - m_row.Cols];
                        string text = hasXmatrix[row - m_col.Cols, col - m_row.Cols] ? "X" : "";
                        Button button1 = new Button
                        {
                            BackgroundColor = Color.FromRgb(color.Red, color.Green, color.Blue),
                            Text = text
                        };

                        button1.Clicked += Button_Click;

                        table.Children.Add(button1, col, row);
                    }
                    else
                    {
                        table.Children.Add(new BoxView { BackgroundColor = Color.Gray }, col, row);
                    }
                }
            }

            // Создание Lable и изменение цвета у строк
            for (int row = 0; row < m_row.Rows; ++row)
            {
                for (int col = 0; col < m_row.Cols; ++col)
                {
                    if (m_row[row, col].Key != 0)
                    {
                        table.Children[(m_col.Cols + row) * (m_col.Rows + m_row.Cols) + col].BackgroundColor = Color.FromRgb(m_row[row, col].Value.Red, m_row[row, col].Value.Green, m_row[row, col].Value.Blue);
                        table.Children.Add(new Label { Text = m_row[row, col].Key.ToString(), FontSize = 23, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = Color.White }, col, row + m_col.Cols);
                    }
                }
            }

            // Создание Lable и изменение цвета у столбцов
            for (int row = 0; row < m_col.Rows; ++row)
            {
                for (int col = 0; col < m_col.Cols; ++col)
                {
                    if (m_col[row, col].Key != 0)
                    {
                        table.Children[col * (m_col.Rows + m_row.Cols) + row + m_row.Cols].BackgroundColor = Color.FromRgb(m_col[row, col].Value.Red, m_col[row, col].Value.Green, m_col[row, col].Value.Blue);
                        table.Children.Add(new Label { Text = m_col[row, col].Key.ToString(), FontSize = 23, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = Color.White }, row + m_row.Cols, col);
                    }
                }
            }

            colorsGrid.ColumnDefinitions.Clear();
            colorsGrid.Children.Clear();
            for (int i = 0; i <= colors.Count + 1; ++i)
            {
                colorsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            Button button = new Button
            {
                BackgroundColor = Color.FromRgb(255, 255, 255),
                BorderColor = Color.GreenYellow
            };

            button.Clicked += Button_Click_Color;

            colorsGrid.Children.Add(button, 0, 0);

            button = new Button
            {
                BackgroundColor = Color.FromRgb(255, 255, 255),
                Text = "X",
                BorderColor = Color.Black
            };

            button.Clicked += Button_Click_X;

            colorsGrid.Children.Add(button, 1, 0);

            int numbOfColor = 2;

            foreach (ColorRGB color in colors)
            {
                button = new Button
                {
                    BackgroundColor = Color.FromRgb(color.Red, color.Green, color.Blue),
                    BorderColor = Color.Black
                };

                button.Clicked += Button_Click_Color;

                colorsGrid.Children.Add(button, numbOfColor, 0);
                numbOfColor++;
            }
        }

        private void Button_Click_X(object sender, EventArgs e)
        {
            hasX = true;
            tmpColor = new ColorRGB(255, 255, 255);
            for (int i = 0; i < colorsGrid.Children.Count; ++i)
            {
                ((Button)colorsGrid.Children[i]).BorderColor = Color.Black;
            }

            ((Button)sender).BorderColor = Color.GreenYellow;
        }

        private void Button_Click_Color(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int red = (int)(button.BackgroundColor.R * 255);
            int green = (int)(button.BackgroundColor.G * 255);
            int blue = (int)(button.BackgroundColor.B * 255);
            tmpColor = new ColorRGB(red, green, blue);
            hasX = false;

            for (int i = 0; i < colorsGrid.Children.Count; ++i)
            {
                ((Button)colorsGrid.Children[i]).BorderColor = Color.Black;
            }

            button.BorderColor = Color.GreenYellow;
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int x, y;
            GetCoordinates(table.Children.IndexOf(button), out x, out y);
            ColorRGB lastColor = new ColorRGB((int)(255 * button.BackgroundColor.R), (int)(255 * button.BackgroundColor.G), (int)(255 * button.BackgroundColor.B));

            if (!hasX)
            {
                button.Text = "";
                hasXmatrix[x, y] = false;
            }
            else
            {
                button.Text = "X";
                hasXmatrix[x, y] = true;
            }

            button.BackgroundColor = Color.FromRgb(tmpColor.Red, tmpColor.Green, tmpColor.Blue);
            matrix[x, y] = tmpColor;

            try
            {
                UpdateSaveFileAsync();
            }
            catch
            {
                PrintError("Ошибка обновления файла");
            }

            if (lastColor != tmpColor)
            {
                if (IsAnswer())
                {
                    await DisplayAlert("Победа", "Вы выиграли", "Ок");
                }
            }
        }

        private void UpdateSaveFileAsync()
        {
            string data = "";
            data += Title + "\n";
            data += "" + colors.Count + "\n";
            foreach (ColorRGB color in colors)
            {
                data += "" + color + " ";
            }
            data += "\n";
            data += "" + m_row.Rows + " " + m_row.Cols + "\n";
            for (int i = 0; i < m_row.Rows; ++i)
            {
                for (int j = 0; j < m_row.Cols; ++j)
                {
                    if (m_row[i,j].Key != 0)
                    {
                        data += "" + m_row[i, j].Key + " " + m_row[i, j].Value + " ";
                    }
                }
                data += "\n";
            }
            data += "" + m_col.Rows + " " + m_col.Cols + "\n";
            for (int i = 0; i < m_col.Rows; ++i)
            {
                for (int j = 0; j < m_col.Cols; ++j)
                {
                    if (m_col[i, j].Key != 0)
                    {
                        data += "" + m_col[i, j].Key + " " + m_col[i, j].Value + " ";
                    }
                }
                data += "\n";
            }
            for (int i = 0; i < matrix.Rows; ++i)
            {
                for (int j = 0; j < matrix.Cols; ++j)
                {
                    data += "" + matrix[i, j] + " ";
                }
                data += "\n";
            }
            for (int i = 0; i < hasXmatrix.Rows; ++i)
            {
                for (int j = 0; j < hasXmatrix.Cols; ++j)
                {
                    int res = hasXmatrix[i, j] ? 1 : 0;
                    data += "" + res + " ";
                }
                data += "\n";
            }

            string tmpPath = folderPath;
            for (int i = 0; i < parts.Length - 1; ++i)
            {
                tmpPath = Path.Combine(tmpPath, parts[i]);
                if (!Directory.Exists(tmpPath))
                {
                    Directory.CreateDirectory(tmpPath);
                }
            }
            tmpPath = Path.Combine(tmpPath, parts[parts.Length - 1]);
            File.WriteAllText(@tmpPath, data);
        }

        private void GetCoordinates(int number, out int x, out int y)
        {
            number -= m_col.Cols * table.ColumnDefinitions.Count;
            x = number / table.ColumnDefinitions.Count;
            y = number % table.ColumnDefinitions.Count - m_row.Cols;
        }

        private bool IsAnswer()
        {
            for (int row = 0; row < m_row.Rows; ++row)
            {
                List<Pair<int, ColorRGB>> tmpRow = new List<Pair<int, ColorRGB>>();
                bool newGroup = true;
                for (int col = 0; col < m_col.Rows; ++col)
                {
                    if (!(matrix[row, col].Red == 255 && matrix[row, col].Green == 255 && matrix[row, col].Blue == 255))
                    {
                        if (newGroup)
                        {
                            Pair<int, ColorRGB> pair = new Pair<int, ColorRGB>(1, matrix[row, col]);
                            tmpRow.Add(pair);
                        }
                        else
                        {
                            if (matrix[row, col] == tmpRow[tmpRow.Count - 1].Second)
                            {
                                tmpRow[tmpRow.Count - 1] = new Pair<int, ColorRGB>(tmpRow[tmpRow.Count - 1].First + 1, matrix[row, col]);
                            }
                            else
                            {
                                Pair<int, ColorRGB> pair = new Pair<int, ColorRGB>(1, matrix[row, col]);
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

                int index = 0;
                while (index < m_row.Cols && m_row[row, index].Value.IsNone())
                {
                    index++;
                }

                if (m_row.Cols - index == tmpRow.Count)
                {
                    for (int i = index; i < m_row.Cols; ++i)
                    {
                        if (m_row[row, i].Key != tmpRow[i - index].First || m_row[row, i].Value != tmpRow[i - index].Second)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            for (int col = 0; col < m_col.Rows; ++col)
            {
                List<Pair<int, ColorRGB>> tmpCol = new List<Pair<int, ColorRGB>>();
                bool newGroup = true;
                for (int row = 0; row < m_row.Rows; ++row)
                {
                    if (!(matrix[row, col].Red == 255 && matrix[row, col].Green == 255 && matrix[row, col].Blue == 255))
                    {
                        if (newGroup)
                        {
                            Pair<int, ColorRGB> pair = new Pair<int, ColorRGB>(1, matrix[row, col]);
                            tmpCol.Add(pair);
                        }
                        else
                        {
                            if (matrix[row, col] == tmpCol[tmpCol.Count - 1].Second)
                            {
                                tmpCol[tmpCol.Count - 1] = new Pair<int, ColorRGB>(tmpCol[tmpCol.Count - 1].First + 1, matrix[row, col]);
                            }
                            else
                            {
                                Pair<int, ColorRGB> pair = new Pair<int, ColorRGB>(1, matrix[row, col]);
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

                int index = 0;
                while (index < m_col.Cols && m_col[col, index].Value.IsNone())
                {
                    index++;
                }

                if (m_col.Cols - index == tmpCol.Count)
                {
                    for (int i = index; i < m_col.Cols; ++i)
                    {
                        if (m_col[col, i].Key != tmpCol[i - index].First || m_col[col, i].Value != tmpCol[i - index].Second)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}