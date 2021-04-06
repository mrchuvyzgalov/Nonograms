using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nonograms
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Nonogram : ContentPage
	{
        Matrix<int> m_row, m_col;
        Matrix<bool> matrix;
        Grid table = new Grid();
        public Nonogram (string path)
		{
			InitializeComponent ();

            try
            {
                using (StreamReader sr = new StreamReader(@path))
                {
                    string[] line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (line.Length != 2) throw new Exception();

                    m_row = new Matrix<int>(new int[int.Parse(line[0]), int.Parse(line[1])]);
                    for (int row = 0; row < m_row.Rows; ++row)
                    {
                        line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        if (line.Length != m_row.Cols) throw new Exception();

                        for (int col = 0; col < line.Length; ++col)
                        {
                            m_row[row, col] = int.Parse(line[col]);
                        }
                    }

                    line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (line.Length != 2) throw new Exception();

                    m_col = new Matrix<int>(new int[int.Parse(line[0]), int.Parse(line[1])]);
                    for (int row = 0; row < m_col.Rows; ++row)
                    {
                        line = sr.ReadLine().Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        if (line.Length != m_col.Cols) throw new Exception();

                        for (int col = 0; col < line.Length; ++col)
                        {
                            m_col[row, col] = int.Parse(line[col]);
                        }
                    }
                }

                bool[,] boolMatrix = new bool[m_row.Rows, m_col.Rows];
                for (int i = 0; i < boolMatrix.GetLength(0); ++i)
                {
                    for (int j = 0; j < boolMatrix.GetLength(1); ++j)
                    {
                        boolMatrix[i, j] = false;
                    }
                }

                matrix = new Matrix<bool>(boolMatrix);
                DrawTable();
            }
            catch
            {
                PrintError("Файла не существует или в нем некорректные данные");
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
            for (int i = 0; i < m_row.Rows + m_col.Cols; ++i)
            {
                table.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < m_row.Cols + m_col.Rows; ++i)
            {
                table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int row = 0; row < table.RowDefinitions.Count; ++row)
            {
                for (int col = 0; col < table.ColumnDefinitions.Count; ++col)
                {
                    table.Children.Add(new Button { BackgroundColor = Color.Gray, IsEnabled = false }, col, row);
                }
            }

            for (int row = m_col.Cols; row < table.RowDefinitions.Count; ++row)
            {
                for (int col = m_row.Cols; col < table.ColumnDefinitions.Count; ++col)
                {
                    Button tmp = (Button)table.Children[row * table.ColumnDefinitions.Count + col];
                    tmp.IsEnabled = true;
                    tmp.BackgroundColor = Color.White;
                    tmp.Clicked += Button_Click;
                }
            }

            for (int row = 0; row < m_row.Rows; ++row)
            {
                for (int col = 0; col < m_row.Cols; ++col)
                {
                    if (m_row[row, col] != 0)
                    {
                        table.Children.Add(new Label { Text = m_row[row, col].ToString(), FontSize = 23, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }, col, row + m_col.Cols);
                    }
                }
            }

            for (int row = 0; row < m_col.Rows; ++row)
            {
                for (int col = 0; col < m_col.Cols; ++col)
                {
                    if (m_col[row, col] != 0)
                    {
                        table.Children.Add(new Label { Text = m_col[row, col].ToString(), FontSize = 23, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }, row + m_row.Cols, col);
                    }
                }
            }

            //table.BackgroundColor = Color.Black;

            Content = table;
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int x, y;
            GetCoordinates(table.Children.IndexOf(button), out x, out y);
            matrix[x, y] = !matrix[x, y];
            if (button.BackgroundColor == Color.White)
            {
                button.BackgroundColor = Color.Black;
            }
            else
            {
                button.BackgroundColor = Color.White;
            }

            if (IsAnswer())
            {
                await DisplayAlert("Победа", "Вы выиграли", "Ок");
            }
        }

        private void GetCoordinates(int number, out int x, out int y)
        {
            number -= m_col.Cols * table.ColumnDefinitions.Count;
            x = number / table.ColumnDefinitions.Count;
            y = number % table.ColumnDefinitions.Count - m_row.Cols;
        }

        private bool IsAnswer()
        {
            for (int row = 0; row < matrix.Rows; ++row)
            {
                int tmp = 0;
                while (tmp < m_row.Cols && m_row[row, tmp] == 0) tmp++;

                List<int> count = new List<int>();
                for (int col = 0; col < matrix.Cols; ++col)
                {
                    if (matrix[row, col])
                    {
                        if (col == 0 || !matrix[row, col - 1]) count.Add(0);
                        count[count.Count - 1]++;
                    }
                }

                if (m_row.Cols - tmp == count.Count)
                {
                    for (int ind = 0; ind < count.Count; ++ind)
                    {
                        if (m_row[row, ind + tmp] != count[ind])
                        {
                            return false;
                        }
                    }
                }
                else return false;
            }

            for (int col = 0; col < matrix.Cols; ++col)
            {
                int tmp = 0;
                while (tmp < m_col.Cols && m_col[col, tmp] == 0) tmp++;

                List<int> count = new List<int>();
                for (int row = 0; row < matrix.Rows; ++row)
                {
                    if (matrix[row, col])
                    {
                        if (row == 0 || !matrix[row - 1, col]) count.Add(0);
                        count[count.Count - 1]++;
                    }
                }

                if (m_col.Cols - tmp == count.Count)
                {
                    for (int ind = 0; ind < count.Count; ++ind)
                    {
                        if (m_col[col, ind + tmp] != count[ind])
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