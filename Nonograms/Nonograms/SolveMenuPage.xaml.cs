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
    enum ColorCell { None, Black, White };
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SolveMenuPage : ContentPage
    {
        Matrix<int> m_row, m_col;
        Matrix<ColorCell> matrix;
        int countNone = 0;
        Grid table = new Grid();
        public SolveMenuPage()
        {
            InitializeComponent();

            try
            {
                using (StreamReader sr = new StreamReader(@"Phone.jap"))
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

                ColorCell[,] boolMatrix = new ColorCell[m_row.Rows, m_col.Rows];
                for (int i = 0; i < boolMatrix.GetLength(0); ++i)
                {
                    for (int j = 0; j < boolMatrix.GetLength(1); ++j)
                    {
                        boolMatrix[i, j] = ColorCell.None;
                    }
                }

                matrix = new Matrix<ColorCell>(boolMatrix);

                countNone = matrix.Cols * matrix.Rows;
                DrawTable();
                Solve();
                UpdateTable();
            }
            catch {
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
                    table.Children.Add(new BoxView { BackgroundColor = Color.Gray }, col, row);
                }
            }

            for (int row = m_col.Cols; row < table.RowDefinitions.Count; ++row)
            {
                for (int col = m_row.Cols; col < table.ColumnDefinitions.Count; ++col)
                {
                    BoxView tmp = (BoxView)table.Children[row * table.ColumnDefinitions.Count + col];
                    tmp.BackgroundColor = Color.Red;
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

            Content = table;
        }

        private void Solve()
        {
            while (countNone > 0)
            {
                for (int i = 0; i < matrix.Rows; ++i)
                {
                    DrawLine(i);
                }
                for (int j = 0; j < matrix.Cols; ++j)
                {
                    DrawColumn(j);
                }
            }
        }

        private void UpdateTable()
        {
            for (int row = m_col.Cols; row < table.RowDefinitions.Count; ++row)
            {
                for (int col = m_row.Cols; col < table.ColumnDefinitions.Count; ++col)
                {
                    BoxView tmp = (BoxView)table.Children[row * table.ColumnDefinitions.Count + col];

                    if (matrix[row - m_col.Cols, col - m_row.Cols] == ColorCell.Black)
                    {
                        tmp.BackgroundColor = Color.Black;
                    }
                    else if (matrix[row - m_col.Cols, col - m_row.Cols] == ColorCell.White)
                    {
                        tmp.BackgroundColor = Color.White;
                    }
                    else
                    {
                        tmp.BackgroundColor = Color.Red;
                    }
                }
            }
        }

        private void DrawColumn(int col)
        {
            int countGroups = m_col.Cols;
            for (int j = 0; j < m_col.Cols; ++j)
            {
                if (m_col[col, j] == 0) countGroups--;
                else break;
            }

            int[][][] calc = new int[matrix.Rows][][];
            for (int i = 0; i < calc.Length; ++i)
            {
                calc[i] = new int[countGroups + 1][];

                for (int j = 0; j < calc[i].Length; ++j)
                {
                    calc[i][j] = new int[] { -1, -1 };
                }
            }

            int[] black = new int[matrix.Rows];
            int[] white = new int[matrix.Rows];
            int[] canBlack = new int[matrix.Rows + 1];
            int[] canWhite = new int[matrix.Rows + 1];

            for (int i = 0; i < matrix.Rows; ++i)
            {
                if (matrix[i, col] == ColorCell.Black) black[i]++;
                else if (matrix[i, col] == ColorCell.White) white[i]++;
            }

            for (int i = 1; i < matrix.Rows; ++i)
            {
                white[i] += white[i - 1];
            }

            int a = DpCol(0, 0, 0, countGroups, calc, black, white, canBlack, canWhite, col);
            if (a == 0)
            {
                PrintError("Кроссворд невозможно решить");
            }
            else
            {
                for (int i = 1; i < canBlack.Length; ++i)
                {
                    canBlack[i] += canBlack[i - 1];
                }
                for (int i = 0; i < matrix.Rows; ++i)
                {
                    if (canBlack[i] > 0 && canWhite[i] > 0)
                    {
                        matrix[i, col] = ColorCell.None;
                    }
                    else if (canBlack[i] > 0)
                    {
                        if (matrix[i, col] == ColorCell.None) countNone--;
                        matrix[i, col] = ColorCell.Black;
                    }
                    else
                    {
                        if (matrix[i, col] == ColorCell.None) countNone--;
                        matrix[i, col] = ColorCell.White;
                    }
                }
            }
        }

        private void DrawLine(int numb)
        {
            int countGroups = m_row.Cols;
            for (int i = 0; i < m_row.Cols; ++i)
            {
                if (m_row[numb, i] == 0) countGroups--;
                else break;
            }

            int[][][] calc = new int[matrix.Cols][][];
            for (int i = 0; i < calc.Length; ++i)
            {
                calc[i] = new int[countGroups + 1][];

                for (int j = 0; j < calc[i].Length; ++j)
                {
                    calc[i][j] = new int[] { -1, -1 };
                }
            }

            int[] black = new int[matrix.Cols];
            int[] white = new int[matrix.Cols];
            int[] canBlack = new int[matrix.Cols + 1];
            int[] canWhite = new int[matrix.Cols + 1];

            for (int i = 0; i < matrix.Cols; ++i)
            {
                if (matrix[numb, i] == ColorCell.Black) black[i]++;
                else if (matrix[numb, i] == ColorCell.White) white[i]++;
            }

            for (int i = 1; i < matrix.Cols; i++)
            {
                white[i] += white[i - 1];
            }

            int a = DpLine(0, 0, 0, countGroups, calc, black, white, canBlack, canWhite, numb);
            if (a == 0)
            {
                PrintError("Кроссворд невозможно решить");
            }
            else
            {
                for (int i = 1; i < canBlack.Length; ++i)
                {
                    canBlack[i] += canBlack[i - 1];
                }
                for (int i = 0; i < matrix.Cols; ++i)
                {
                    if (canBlack[i] > 0 && canWhite[i] > 0)
                    {
                        matrix[numb, i] = ColorCell.None;
                    }
                    else if (canBlack[i] > 0)
                    {
                        if (matrix[numb, i] == ColorCell.None) countNone--;
                        matrix[numb, i] = ColorCell.Black;
                    }
                    else
                    {
                        if (matrix[numb, i] == ColorCell.None) countNone--;
                        matrix[numb, i] = ColorCell.White;
                    }
                }
            }
        }

        private int DpCol(int v, int k, int e, int countGroups, int[][][] calc, int[] black, int[] white, int[] canBlack, int[] canWhite, int col)
        {
            if (v == matrix.Rows)
            {
                return k == countGroups ? 1 : 0;
            }
            if (v > matrix.Rows)
            {
                return 0;
            }
            if (calc[v][k][e] != -1)
            {
                return calc[v][k][e];
            }
            int ans = 0;
            int val;
            if (k < countGroups && e != 1 && getWhites(v, v + m_col[col, m_col.Cols - countGroups + k] - 1, white) == 0)
            {
                val = DpCol(v + m_col[col, m_col.Cols - countGroups + k], k + 1, 1, countGroups, calc, black, white, canBlack, canWhite, col);
                if (val == 1)
                {
                    ans = 1;
                    canBlack[v]++;
                    canBlack[Math.Min(matrix.Cols, v + m_col[col, m_col.Cols - countGroups + k])]--;
                }
            }
            if (black[v] == 0)
            {
                val = DpCol(v + 1, k, 0, countGroups, calc, black, white, canBlack, canWhite, col);
                if (val == 1)
                {
                    ans = 1;
                    canWhite[v] = 1;
                }
            }
            calc[v][k][e] = ans;
            return ans;
        }

        private int DpLine(int v, int k, int e, int countGroups, int[][][] calc, int[] black, int[] white, int[] canBlack, int[] canWhite, int line)
        {
            if (v == matrix.Cols)
            {
                return k == countGroups ? 1 : 0;
            }
            if (v > matrix.Cols)
            {
                return 0;
            }
            if (calc[v][k][e] != -1)
            {
                return calc[v][k][e];
            }
            int ans = 0;
            int val;
            if (k < countGroups && e != 1 && getWhites(v, v + m_row[line, m_row.Cols - countGroups + k] - 1, white) == 0)
            {
                val = DpLine(v + m_row[line, m_row.Cols - countGroups + k], k + 1, 1, countGroups, calc, black, white, canBlack, canWhite, line);
                if (val == 1)
                {
                    ans = 1;
                    canBlack[v]++;
                    canBlack[Math.Min(matrix.Cols, v + m_row[line, m_row.Cols - countGroups + k])]--;
                }
            }
            if (black[v] == 0)
            {
                val = DpLine(v + 1, k, 0, countGroups, calc, black, white, canBlack, canWhite, line);
                if (val == 1)
                {
                    ans = 1;
                    canWhite[v] = 1;
                }
            }
            calc[v][k][e] = ans;
            return ans;
        }

        private int getWhites(int a, int b, int[] white)
        {
            int ans = white[Math.Min(b, matrix.Cols - 1)];
            if (a > 0)
                ans -= white[a - 1];
            return ans;
        }
    }
}