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
    public partial class SolveMenu : ContentPage
    {
        public SolveMenu()
        {
            InitializeComponent();

            RowsMatrix.Text = "Каждая клетка должна состоять из 2х частей: число клеток и цвет клеток в формате RGB\nПример описания клетки: 5 0 0 0\nПример описания матрицы:\n1 0 0 0 2 255 255 255\n2 255 0 0 3 0 0 255";
            ColsMatrix.Text = "Каждая клетка должна состоять из 2х частей: число клеток и цвет клеток в формате RGB\nПример описания клетки: 5 0 0 0\nПример описания матрицы:\n1 0 0 0 2 255 255 255\n2 255 0 0 3 0 0 255";
        }

        private async void SolveButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                Matrix<KeyValuePair<int, ColorRGB>> m_rows, m_cols;
                HashSet<ColorRGB> colors = new HashSet<ColorRGB>();

                string[] line = SizeRowsMatrix.Text.Split(new char[] { 'x' });
                if (line.Length != 2)
                {
                    throw new Exception("Неверный формат размера матрицы строк");
                }
                int rowsRows;
                if (int.TryParse(line[0], out rowsRows) && rowsRows > 0 && rowsRows <= 11)
                {
                    int colsRows;

                    if (int.TryParse(line[1], out colsRows) && colsRows > 0 && colsRows <= 11)
                    {
                        string[] lines = RowsMatrix.Text.Split(new char[] { '\n', '\r' });
                        if (lines.Length != rowsRows)
                        {
                            throw new Exception("Число строк в матрице строк не совпадает с число строк, указанным выше");
                        }
                        KeyValuePair<int, ColorRGB>[,] arrayRows = new KeyValuePair<int, ColorRGB>[rowsRows, colsRows];
                        for (int i = 0; i < lines.Length; ++i)
                        {
                            string[] data = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (data.Length % 4 != 0 || data.Length > 4 * colsRows)
                            {
                                throw new Exception("Неверный формат строки в массиве строк");
                            }
                            for (int j = 0; j < data.Length; j += 4)
                            {
                                int count, red, green, blue;
                                if (!int.TryParse(data[j], out count) || count < 1)
                                {
                                    throw new Exception("Некорректный ввод числа клеточек");
                                }
                                if (!int.TryParse(data[j + 1], out red) || !int.TryParse(data[j + 2], out green) || !int.TryParse(data[j + 3], out blue) || red < 0 || red > 255 || green < 0 || green > 255 || blue < 0 || blue > 255)
                                {
                                    throw new Exception("Некорректный цвет");
                                }
                                if (new ColorRGB(red, green, blue) == new ColorRGB(255,255,255))
                                {
                                    throw new Exception("Некорректный цвет. Нельзя писать белый цвет");
                                }
                                colors.Add(new ColorRGB(red, green, blue));
                                arrayRows[i, colsRows - data.Length / 4 + j / 4] = new KeyValuePair<int, ColorRGB>(count, new ColorRGB(red, green, blue));
                            }
                        }
                        m_rows = new Matrix<KeyValuePair<int, ColorRGB>>(arrayRows);
                    }
                    else
                    {
                        throw new Exception("Некорректное число столбцов. Оно должно быть от 1 до 10");
                    }
                }
                else
                {
                    throw new Exception("Некорректное число строк. Оно должно быть от 1 до 10");
                }

                line = SizeColsMatrix.Text.Split(new char[] { 'x' });
                if (line.Length != 2)
                {
                    throw new Exception("Неверный формат размера матрицы столбцов");
                }
                int rowsCols;
                if (int.TryParse(line[0], out rowsCols) && rowsCols > 0 && rowsCols <= 11)
                {
                    int colsCols;

                    if (int.TryParse(line[1], out colsCols) && colsCols > 0 && colsCols <= 11)
                    {
                        string[] lines = ColsMatrix.Text.Split(new char[] { '\n', '\r' });
                        if (lines.Length != rowsCols)
                        {
                            throw new Exception("Число строк в матрице столбцов не совпадает с число строк, указанным выше");
                        }
                        KeyValuePair<int, ColorRGB>[,] arrayCols = new KeyValuePair<int, ColorRGB>[rowsCols, colsCols];
                        for (int i = 0; i < lines.Length; ++i)
                        {
                            string[] data = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (data.Length % 4 != 0 || data.Length > 4 * colsCols)
                            {
                                throw new Exception("Неверный формат строки в массиве столбцов");
                            }
                            for (int j = 0; j < data.Length; j += 4)
                            {
                                int count, red, green, blue;
                                if (!int.TryParse(data[j], out count) || count < 1)
                                {
                                    throw new Exception("Некорректный цвет. Нельзя писать белый цвет");
                                }
                                if (!int.TryParse(data[j + 1], out red) || !int.TryParse(data[j + 2], out green) || !int.TryParse(data[j + 3], out blue) || red < 0 || red > 255 || green < 0 || green > 255 || blue < 0 || blue > 255)
                                {
                                    throw new Exception("Некорректный цвет");
                                }
                                if (new ColorRGB(red, green, blue) == new ColorRGB(255, 255, 255))
                                {
                                    throw new Exception("Некорректный цвет. Нельзя писать белый цвет");
                                }
                                colors.Add(new ColorRGB(red, green, blue));
                                arrayCols[i, colsCols - data.Length / 4 + j / 4] = new KeyValuePair<int, ColorRGB>(count, new ColorRGB(red, green, blue));
                            }
                        }
                        m_cols = new Matrix<KeyValuePair<int, ColorRGB>>(arrayCols);
                    }
                    else
                    {
                        throw new Exception("Некорректное число столбцов. Оно должно быть от 1 до 10");
                    }
                }
                else
                {
                    throw new Exception("Некорректное число строк. Оно должно быть от 1 до 10");
                }

                await Navigation.PushAsync(new SolveNonogram(m_rows, m_cols, colors));
            }
            catch (Exception error)
            {
                await DisplayAlert("Ошибка", error.Message, "Ok");
            }
        }
    }
}