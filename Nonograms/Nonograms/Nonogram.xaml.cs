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
        enum Level { Easy, Middle, Hard };

        readonly string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        Matrix<KeyValuePair<int, ColorRGB>> m_row, m_col;
        Matrix<ColorRGB> matrix;
        Matrix<bool> hasXmatrix;
        HashSet<ColorRGB> colors = new HashSet<ColorRGB>();
        ColorRGB tmpColor = new ColorRGB(255, 255, 255);
        string savepath;
        string[] parts;
        Matrix<ColorRGB> solveMatrix;
        bool isFinish = false;
        int hitPoints;
        int countOfHints;
        Matrix<int> numbOfGroupRow, numbOfGroupCol;
        Level tmpLevel;
        int countAddCrosses = 0;
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

            try
            {
                string[] lines = File.ReadAllText(Path.Combine(folderPath, "settings.txt")).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                if (lines[0] == "Легкий")
                {
                    tmpLevel = Level.Easy;
                }
                else if (lines[0] == "Средний")
                {
                    tmpLevel = Level.Middle;
                }
                else 
                {
                    tmpLevel = Level.Hard;
                }

                countOfHints = int.Parse(lines[1]);
            }
            catch
            {
                PrintError("Ошибка чтения файла настроек");
            }

            ToolbarItem bulbButton = new ToolbarItem()
            {
                IconImageSource = "Bulb.png",
                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };

            bulbButton.Clicked += async (s, e) =>
            {
                if (isFinish || hitPoints == 0) return;
                if (countOfHints == 0)
                {
                    await DisplayAlert("Пусто", "Подсказки закончились", "Ок");
                    return;
                }
                if (await DisplayAlert("Подсказка", "Использовать подсказку?\nОсталось: " + countOfHints, "Да", "Нет"))
                {
                    for (int row = 0; row < matrix.Rows; ++row)
                    {
                        for (int col = 0; col < matrix.Cols; ++col)
                        {
                            if (solveMatrix[row,col] == new ColorRGB(255,255,255) && !hasXmatrix[row,col])
                            {
                                ((Button)table.Children[GetNumber(row, col)]).Text = "X";
                                hasXmatrix[row, col] = true;
                                UpdateSettings();
                                UpdateSaveFileAsync();
                                countOfHints--;
                                isFinish = IsAnswer();

                                if (isFinish)
                                {
                                    await DisplayAlert("Победа", "Вы выиграли", "Ок");
                                }
                                return;
                            }
                            else if (matrix[row, col] != solveMatrix[row, col])
                            {
                                matrix[row, col] = solveMatrix[row, col];
                                DrawCrossCondition(row, col);
                                ((Button)table.Children[GetNumber(row, col)]).BackgroundColor = Color.FromRgb(matrix[row, col].Red, matrix[row, col].Green, matrix[row, col].Blue);
                                UpdateSettings();
                                UpdateSaveFileAsync();
                                countOfHints--;
                                isFinish = IsAnswer();
                                if (isFinish)
                                {
                                    await DisplayAlert("Победа", "Вы выиграли", "Ок");
                                }
                                return;
                            }
                        } 
                    }
                }
            };

            ToolbarItems.Add(bulbButton);

            ToolbarItem clearButton = new ToolbarItem()
            {
                Text = "Начать заново",
                Order = ToolbarItemOrder.Secondary,
                Priority = 0
            };

            clearButton.Clicked += (s,e) =>
            {
                while (countAddCrosses > 0)
                {
                    table.Children.RemoveAt(table.Children.Count - 1);
                    countAddCrosses--;
                }
                isFinish = false;
                if (File.Exists(@savepath))
                {
                    File.Delete(savepath);
                }

                for (int row = 0; row < matrix.Rows; ++row)
                {
                    for (int col = 0; col < matrix.Cols; ++col)
                    {
                        matrix[row, col] = new ColorRGB(255, 255, 255);
                    }
                }

                for (int row = m_col.Cols; row < table.RowDefinitions.Count; ++row)
                {
                    for (int col = m_row.Cols; col < table.ColumnDefinitions.Count; ++col)
                    {
                        Button tmp = (Button)table.Children[row * table.ColumnDefinitions.Count + col];

                        tmp.Text = "";
                        if (matrix[row - m_col.Cols, col - m_row.Cols].IsNone())
                        {
                            tmp.BackgroundColor = Color.White;
                        }
                        else
                        {
                            tmp.BackgroundColor = Color.FromRgb(matrix[row - m_col.Cols, col - m_row.Cols].Red, matrix[row - m_col.Cols, col - m_row.Cols].Green, matrix[row - m_col.Cols, col - m_row.Cols].Blue);
                        }
                    }
                }

                if (tmpLevel == Level.Easy)
                {
                    hitPoints = 5;
                }
                else if (tmpLevel == Level.Middle)
                {
                    hitPoints = 3;
                }
                else
                {
                    hitPoints = 1;
                }

                Hearts.Children.Clear();
                for (int i = 0; i < hitPoints; ++i)
                {
                    Image image = new Image
                    {
                        Source = "Heart.png"
                    };

                    Hearts.Children.Add(image);
                }

                try
                {
                    UpdateSaveFileAsync();
                }
                catch
                {
                    PrintError("Ошибка обновления файла");
                }
            };

            ToolbarItems.Add(clearButton);

            ToolbarItem solveButton = new ToolbarItem()
            {
                Text = "Получить решение",
                Order = ToolbarItemOrder.Secondary,
                Priority = 1
            };

            solveButton.Clicked += (s, e) =>
            {
                while (countAddCrosses > 0)
                {
                    table.Children.RemoveAt(table.Children.Count - 1);
                    countAddCrosses--;
                }
                for (int row = 0; row < matrix.Rows; ++row)
                {
                    for (int col = 0; col < matrix.Cols; ++col)
                    {
                        matrix[row, col] = solveMatrix[row, col];
                        hasXmatrix[row, col] = false;
                    }
                }

                for (int row = m_col.Cols; row < table.RowDefinitions.Count; ++row)
                {
                    for (int col = m_row.Cols; col < table.ColumnDefinitions.Count; ++col)
                    {
                        Button tmp = (Button)table.Children[row * table.ColumnDefinitions.Count + col];

                        tmp.Text = "";
                        if (matrix[row - m_col.Cols, col - m_row.Cols].IsNone())
                        {
                            tmp.BackgroundColor = Color.White;
                        }
                        else
                        {
                            tmp.BackgroundColor = Color.FromRgb(matrix[row - m_col.Cols, col - m_row.Cols].Red, matrix[row - m_col.Cols, col - m_row.Cols].Green, matrix[row - m_col.Cols, col - m_row.Cols].Blue);
                        }
                    }
                }

                DrawCrosses();

                try
                {
                    UpdateSaveFileAsync();
                }
                catch
                {
                    PrintError("Ошибка обновления файла");
                }

                isFinish = true;
            };

            ToolbarItems.Add(solveButton);

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

            solveMatrix = NonogramSolve.Solve(m_row, m_col, colors);
            numbOfGroupRow = new Matrix<int>(matrix.Rows, matrix.Cols);
            numbOfGroupCol = new Matrix<int>(matrix.Rows, matrix.Cols);

            for (int row = 0; row < matrix.Rows; ++row)
            {
                int countOfGroups = m_row.Cols;
                while (countOfGroups > 0 && m_row[row, m_row.Cols - countOfGroups].Key == 0)
                {
                    countOfGroups--;
                }
                int numb = m_row.Cols - countOfGroups - 1;
                bool newGroup = true;
                for (int col = 0; col < matrix.Cols; ++col)
                {
                    if (solveMatrix[row, col] == new ColorRGB(255, 255, 255))
                    {
                        numbOfGroupRow[row, col] = -1;
                        newGroup = true;
                    }
                    else
                    {
                        if (newGroup)
                        {
                            numbOfGroupRow[row, col] = ++numb;
                            newGroup = false;
                        }
                        else
                        {
                            numbOfGroupRow[row, col] = numb;
                        }

                        if (col + 1 < matrix.Cols && solveMatrix[row, col + 1] != solveMatrix[row, col])
                        {
                            newGroup = true;
                        }
                    }
                }
            }

            for (int col = 0; col < matrix.Cols; ++col)
            {
                int countOfGroups = m_col.Cols;
                while (countOfGroups > 0 && m_col[col, m_col.Cols - countOfGroups].Key == 0)
                {
                    countOfGroups--;
                }
                int numb = m_col.Cols - countOfGroups - 1;
                bool newGroup = true;
                for (int row = 0; row < matrix.Rows; ++row)
                {
                    if (solveMatrix[row, col] == new ColorRGB(255, 255, 255))
                    {
                        numbOfGroupCol[row, col] = -1;
                        newGroup = true;
                    }
                    else
                    {
                        if (newGroup)
                        {
                            numbOfGroupCol[row, col] = ++numb;
                            newGroup = false;
                        }
                        else
                        {
                            numbOfGroupCol[row, col] = numb;
                        }

                        if (row + 1 < matrix.Rows && solveMatrix[row + 1, col] != solveMatrix[row, col])
                        {
                            newGroup = true;
                        }
                    }
                }
            }

            isFinish = IsAnswer();

            Device.StartTimer(TimeSpan.FromMilliseconds(100), DrawCrosses);
        }

        private bool DrawCrosses()
        {
            for (int row = 0; row < matrix.Rows; ++row)
            {
                for (int col = 0; col < matrix.Cols; ++col)
                {
                    DrawCrossCondition(row, col);

                    Button button = ((Button)table.Children[GetNumber(row, col)]);
                    button.FontSize = (int)(Math.Min(button.Height, button.Width) / 3);

                    if (hasXmatrix[row,col])
                    {
                        button.Text = "X";
                    }
                }
            }



            return false;
        }

        private void DrawCrossCondition(int x, int y)
        {
            if (matrix[x, y] != new ColorRGB(255, 255, 255))
            {
                int countLeft = 0;
                int col = y - 1;
                while (col >= 0 && matrix[x, col] == matrix[x, y])
                {
                    countLeft++;
                    col--;
                }

                int countRight = 0;
                col = y + 1;
                while (col < matrix.Cols && matrix[x, col] == matrix[x, y])
                {
                    countRight++;
                    col++;
                }

                if (countLeft + countRight + 1 == m_row[x, numbOfGroupRow[x, y]].Key)
                {
                    countAddCrosses++;
                    Image cross = new Image
                    {
                        Source = "Cross.png",
                        BackgroundColor = Color.Transparent,
                        HeightRequest = table.Children[0].Height,
                        WidthRequest = table.Children[0].Width
                    };

                    table.Children.Add(cross, numbOfGroupRow[x, y], m_col.Cols + x);
                }

                int countUp = 0;
                int row = x - 1;
                while (row >= 0 && matrix[row, y] == matrix[x, y])
                {
                    countUp++;
                    row--;
                }

                int countDown = 0;
                row = x + 1;
                while (row < matrix.Rows && matrix[row, y] == matrix[x, y])
                {
                    countDown++;
                    row++;
                }


                if (countUp + countDown + 1 == m_col[y, numbOfGroupCol[x, y]].Key)
                {
                    countAddCrosses++;
                    Image cross = new Image
                    {
                        Source = "Cross.png",
                        BackgroundColor = Color.Transparent,
                        HeightRequest = table.Children[0].Height,
                        WidthRequest = table.Children[0].Width
                    };

                    table.Children.Add(cross, m_row.Cols + y, numbOfGroupCol[x, y]);
                }
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

                if (tmpLevel == Level.Easy)
                {
                    hitPoints = 5;
                }
                else if (tmpLevel == Level.Middle)
                {
                    hitPoints = 3;
                }
                else
                {
                    hitPoints = 1;
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

                string level = sr.ReadLine();
                int hit = int.Parse(sr.ReadLine());
                if (hit < 0 && hit > 5)
                {
                    PrintError("Неверное число жизней в файле сохранения");
                }

                if (level == "Легкий")
                {
                    if (tmpLevel == Level.Easy)
                    {
                        hitPoints = hit;
                    }
                    else if (tmpLevel == Level.Middle)
                    {
                        if (hit == 5)
                        {
                            hitPoints = 3;
                        }
                        else if (hit >= 3)
                        {
                            hitPoints = 2;
                        }
                        else if (hit >= 1)
                        {
                            hitPoints = 1;
                        }
                        else
                        {
                            hitPoints = 0;
                        }
                    }
                    else
                    {
                        if (hit == 0)
                        {
                            hitPoints = 0;
                        }
                        else
                        {
                            hitPoints = 1;
                        }
                    }
                }
                else if (level == "Средний")
                {
                    if (tmpLevel == Level.Easy)
                    {
                        if (hit == 3)
                        {
                            hitPoints = 5;
                        }
                        else if (hit == 2)
                        {
                            hitPoints = 3;
                        }
                        else if (hit == 1)
                        {
                            hitPoints = 1;
                        }
                        else
                        {
                            hitPoints = 0;
                        }
                    }
                    else if (tmpLevel == Level.Middle)
                    {
                        hitPoints = hit;
                    }
                    else
                    {
                        if (hit == 0)
                        {
                            hitPoints = 0;
                        }
                        else
                        {
                            hitPoints = 1;
                        }
                    }
                }
                else if (level == "Сложный")
                {
                    if (tmpLevel == Level.Easy)
                    {
                        if (hitPoints == 1)
                        {
                            hitPoints = 5;
                        }
                        else
                        {
                            hitPoints = 0;
                        }
                    }
                    else if (tmpLevel == Level.Middle)
                    {
                        if (hitPoints == 1)
                        {
                            hitPoints = 3;
                        }
                        else
                        {
                            hitPoints = 0;
                        }
                    }
                    else
                    {
                        hitPoints = hit;
                    }
                }
                else
                {
                    PrintError("Неверный уровень сложности в файле сохранения");
                }
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

            // создание BoxView и Butttons
            for (int row = 0; row < table.RowDefinitions.Count; ++row)
            {
                for (int col = 0; col < table.ColumnDefinitions.Count; ++col)
                {
                    if (row >= m_col.Cols && col >= m_row.Cols)
                    {
                        ColorRGB color = matrix[row - m_col.Cols, col - m_row.Cols];
                        Button button1 = new Button
                        {
                            BackgroundColor = Color.FromRgb(color.Red, color.Green, color.Blue),
                            TextColor = Application.Current.UserAppTheme == OSAppTheme.Light ? Color.Black : Color.White
                        };

                        button1.Clicked += Button_Click;
                        button1.SizeChanged += Cell_SizeChanged;

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

                        Color color = Color.White;
                        if (m_row[row, col].Value.isBright())
                        {
                            color = Color.Black;
                        }

                        BoxView box = (BoxView)table.Children[(m_col.Cols + row) * (m_col.Rows + m_row.Cols) + col];
                        table.Children.Add(new Label { Text = m_row[row, col].Key.ToString(), FontSize = (int)Math.Min(box.Width, box.Height) / 2, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = color }, col, row + m_col.Cols);
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

                        Color color = Color.White;
                        if (m_col[row, col].Value.isBright())
                        {
                            color = Color.Black;
                        }
                        BoxView box = (BoxView)table.Children[col * (m_col.Rows + m_row.Cols) + row + m_row.Cols];
                        table.Children.Add(new Label { Text = m_col[row, col].Key.ToString(), FontSize = (int)Math.Min(box.Width, box.Height) / 2, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = color }, row + m_row.Cols, col);
                    }
                }
            }

            // Создание жизней
            int maxiHit = 5;
            if (tmpLevel == Level.Middle)
            {
                maxiHit = 3;
            }
            else if (tmpLevel == Level.Hard)
            {
                maxiHit = 1;
            }
            for (int i = 0; i < hitPoints; ++i)
            {
                Image image = new Image
                {
                    Source = "Heart.png"
                };

                Hearts.Children.Add(image);
            }
            for (int i = hitPoints; i < maxiHit; ++i)
            {
                Image image = new Image
                {
                    Source = "DeadHeart.png"
                };

                Hearts.Children.Add(image);
            }

            // Создание палитры цветов
            colorsGrid.ColumnDefinitions.Clear();
            colorsGrid.Children.Clear();
            for (int i = 0; i <= colors.Count; ++i)
            {
                colorsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            Button button = new Button
            {
                BackgroundColor = Color.FromRgb(255, 255, 255),
                TextColor = Color.Black,
                Text = "X",
                BorderColor = Application.Current.UserAppTheme == OSAppTheme.Light ? Color.Black : Color.White,
                BorderWidth = 5
            };

            button.SizeChanged += XButton_SizeChanged;
            button.Clicked += Button_Click_X;

            colorsGrid.Children.Add(button, 0, 0);

            int numbOfColor = 1;

            foreach (ColorRGB color in colors)
            {
                Button button1 = new Button
                {
                    BackgroundColor = Color.FromRgb(color.Red, color.Green, color.Blue),
                    BorderColor = Application.Current.UserAppTheme == OSAppTheme.Light ? Color.Black : Color.White
                };

                button1.Clicked += Button_Click_Color;

                colorsGrid.Children.Add(button1, numbOfColor, 0);
                numbOfColor++;
            }
        }

        private void XButton_SizeChanged(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.FontSize = Math.Min(button.Width, button.Height) / 3;
        }

        private void Cell_SizeChanged(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.FontSize = Math.Min(button.Width, button.Height) / 4;
        }

        private void Button_Click_X(object sender, EventArgs e)
        {
            tmpColor = new ColorRGB(255, 255, 255);
            Button cross = (Button)sender;
            cross.BorderWidth = 5;
            cross.BorderColor = Application.Current.UserAppTheme == OSAppTheme.Light ? Color.Black : Color.White;

            for (int i = 1; i < colorsGrid.Children.Count; ++i)
            {
                Button tmpColor = (Button)colorsGrid.Children[i];
                tmpColor.BorderWidth = 1;
                ColorRGB color = new ColorRGB((int)(tmpColor.BackgroundColor.R * 255), (int)(tmpColor.BackgroundColor.G * 255), (int)(tmpColor.BackgroundColor.B * 255));
                if (color.isBright())
                {
                    tmpColor.BorderColor = Application.Current.UserAppTheme == OSAppTheme.Light ? Color.Black : Color.White;
                }
                else
                {
                    tmpColor.BorderColor = Application.Current.UserAppTheme == OSAppTheme.Light ? Color.White : Color.Black;
                }
            }
        }

        private void Button_Click_Color(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int red = (int)(button.BackgroundColor.R * 255);
            int green = (int)(button.BackgroundColor.G * 255);
            int blue = (int)(button.BackgroundColor.B * 255);
            tmpColor = new ColorRGB(red, green, blue);

            Button cross = (Button)colorsGrid.Children[0];
            cross.BorderWidth = 1;
            cross.BorderColor = Application.Current.UserAppTheme == OSAppTheme.Light ? Color.Black : Color.White;

            for (int i = 1; i < colorsGrid.Children.Count; ++i)
            {
                Button tmpColor = (Button)colorsGrid.Children[i];
                tmpColor.BorderWidth = 1;
                ColorRGB color = new ColorRGB((int)(tmpColor.BackgroundColor.R * 255), (int)(tmpColor.BackgroundColor.G * 255), (int)(tmpColor.BackgroundColor.B * 255));
                if (color.isBright())
                {
                    tmpColor.BorderColor = Application.Current.UserAppTheme == OSAppTheme.Light ? Color.Black : Color.White;
                }
                else
                {
                    tmpColor.BorderColor = Application.Current.UserAppTheme == OSAppTheme.Light ? Color.White : Color.Black;
                }
            }

            ((Button)sender).BorderWidth = 5;
            if (tmpColor.isBright())
            {
                button.BorderColor = Application.Current.UserAppTheme == OSAppTheme.Light ? Color.Black : Color.White;
            }
            else
            {
                button.BorderColor = Application.Current.UserAppTheme == OSAppTheme.Light ? Color.White : Color.Black;
            }
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            if (isFinish || hitPoints == 0) return;

            Button button = (Button)sender;
            int x, y;
            GetCoordinates(table.Children.IndexOf(button), out x, out y);

            if (tmpColor != solveMatrix[x,y])
            {
                ((Image)Hearts.Children[--hitPoints]).Source = "DeadHeart.png";
                await DisplayAlert("Ошибка", "Клетка имеет другой цвет", "Ок");
                if (hitPoints == 0)
                {
                    await DisplayAlert("Проигрыш", "Жизней больше нет. Попробуйте заново", "Ок");
                }
                UpdateSaveFileAsync();
                return;
            } 

            ColorRGB lastColor = new ColorRGB((int)(255 * button.BackgroundColor.R), (int)(255 * button.BackgroundColor.G), (int)(255 * button.BackgroundColor.B));

            if (tmpColor == new ColorRGB(255, 255, 255))
            {
                button.Text = "X";
                hasXmatrix[x, y] = true;
            }
            else
            {
                button.BackgroundColor = Color.FromRgb(tmpColor.Red, tmpColor.Green, tmpColor.Blue);
                matrix[x, y] = tmpColor;
                DrawCrossCondition(x, y);
            }

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
                    isFinish = true;
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
            
            if (tmpLevel == Level.Easy)
            {
                data += "Легкий\n";
            }
            else if (tmpLevel == Level.Middle)
            {
                data += "Средний\n";
            }
            else
            {
                data += "Сложный\n";
            }

            data += hitPoints;

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

        private void UpdateSettings()
        {
            string[] lines = File.ReadAllText(Path.Combine(folderPath, "settings.txt")).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            File.WriteAllText(Path.Combine(folderPath, "settings.txt"), lines[0] + "\n" + countOfHints);
        }

        private void GetCoordinates(int number, out int x, out int y)
        {
            number -= m_col.Cols * table.ColumnDefinitions.Count;
            x = number / table.ColumnDefinitions.Count;
            y = number % table.ColumnDefinitions.Count - m_row.Cols;
        }

        private int GetNumber(int x, int y)
        {
            int number = x * table.ColumnDefinitions.Count + y + m_row.Cols + m_col.Cols * table.ColumnDefinitions.Count;
            int xout, yout;
            GetCoordinates(number, out xout, out yout);
            return x * table.ColumnDefinitions.Count + y + m_row.Cols + m_col.Cols * table.ColumnDefinitions.Count;
        }

        private bool IsAnswer()
        {
            for (int row = 0; row < matrix.Rows; ++row)
            {
                for (int col = 0; col < matrix.Cols; ++col)
                {
                    if (solveMatrix[row, col] != matrix[row,col])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}