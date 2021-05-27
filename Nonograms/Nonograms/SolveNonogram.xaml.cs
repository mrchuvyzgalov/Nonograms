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
    public partial class SolveNonogram : ContentPage
    {
        Matrix<KeyValuePair<int, ColorRGB>> m_row, m_col;
        Matrix<ColorRGB> matrix;
        FSM[] rows, cols;
        int countNone = 0;
        HashSet<ColorRGB> colors = new HashSet<ColorRGB>();
        Grid table = new Grid();
        readonly string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public SolveNonogram(Matrix<KeyValuePair<int, ColorRGB>> m_row, Matrix<KeyValuePair<int, ColorRGB>> m_col, HashSet<ColorRGB> colors)
        {
            InitializeComponent();

            this.m_row = m_row;
            this.m_col = m_col;
            this.colors = colors;

            try
            {
                ColorRGB[,] ArrayMatrix = new ColorRGB[m_row.Rows, m_col.Rows];
                for (int i = 0; i < ArrayMatrix.GetLength(0); ++i)
                {
                    for (int j = 0; j < ArrayMatrix.GetLength(1); ++j)
                    {
                        ArrayMatrix[i, j] = new ColorRGB();
                    }
                }

                matrix = new Matrix<ColorRGB>(ArrayMatrix);

                countNone = matrix.Cols * matrix.Rows;
                DrawTable();

                rows = new FSM[m_row.Rows];
                cols = new FSM[m_col.Rows];

                for (int i = 0; i < rows.Length; ++i)
                {
                    rows[i] = CreateFSMRow(i);
                }

                for (int j = 0; j < cols.Length; ++j)
                {
                    cols[j] = CreateFSMCol(j);
                }

                Solve();
                UpdateTable();
            }
            catch (Exception error) {
                PrintError("Ошибка решения: " + error.Message);
            }

            UpdateTable();
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


            // Создание Lable и изменение цвета у строк
            for (int row = 0; row < m_row.Rows; ++row)
            {
                for (int col = 0; col < m_row.Cols; ++col)
                {
                    if (m_row[row,col].Key != 0)
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

            Content = table;
        }

        private void Solve()
        {
            int lastCountNone = countNone;
            while (countNone > 0)
            {
                for (int row = 0; row < matrix.Rows && countNone > 0; ++row)
                {
                    for (int col = 0; col < matrix.Cols && countNone > 0; ++col)
                    {
                        if (matrix[row, col].IsNone())
                        {
                            ColorRGB newColor = new ColorRGB();
                            bool manyColors = false;
                            if (CanColorRow(row, col, new ColorRGB(255, 255, 255)))
                            {
                                newColor = new ColorRGB(255, 255, 255);
                            }
                            foreach (ColorRGB color in colors)
                            {
                                if (CanColorRow(row, col, color))
                                {
                                    if (newColor.IsNone())
                                    {
                                        newColor = color;
                                    }
                                    else
                                    {
                                        manyColors = true;
                                        break;
                                    }
                                }
                            }

                            if (newColor.IsNone())
                            {
                                throw new Exception("Нет решения");
                            }
                            else if (!manyColors)
                            {
                                matrix[row, col] = newColor;
                                countNone--;
                            }
                            else
                            {
                                newColor = new ColorRGB();
                                manyColors = false;
                                if (CanColorCol(row, col, new ColorRGB(255, 255, 255)))
                                {
                                    newColor = new ColorRGB(255, 255, 255);
                                }
                                foreach (ColorRGB color in colors)
                                {
                                    if (CanColorCol(row, col, color))
                                    {
                                        if (newColor.IsNone())
                                        {
                                            newColor = color;
                                        }
                                        else
                                        {
                                            manyColors = true;
                                            break;
                                        }
                                    }
                                }
                                if (newColor.IsNone())
                                {
                                    throw new Exception("Нет решения");
                                }
                                else if (!manyColors)
                                {
                                    matrix[row, col] = newColor;
                                    countNone--;
                                }
                            }
                        }
                    }
                }

                if (lastCountNone == countNone)
                {
                    throw new Exception("Нет решения");
                }
                else
                {
                    lastCountNone = countNone;
                }
            }
        }

        private FSM CreateFSMRow(int row)
        {
            FSM fsm = new FSM();
            FSMNode fsmNode = new FSMNode();
            fsmNode.nextColor = new ColorRGB();
            fsmNode.hasLoop = true;
            fsm.Add(fsmNode);

            for (int col = 0; col < m_row.Cols; ++col)
            {
                if (m_row[row, col].Key != 0)
                {
                    for (int i = 0; i < m_row[row, col].Key; ++i)
                    {
                        fsmNode = new FSMNode();
                        fsmNode = fsm[fsm.Count - 1];
                        fsmNode.nextColor = m_row[row, col].Value;

                        fsmNode = new FSMNode();
                        fsmNode.nextColor = new ColorRGB();
                        fsmNode.hasLoop = false;

                        fsm.Add(fsmNode);
                    }

                    if (col + 1 < m_row.Cols && fsm[fsm.Count - 2].nextColor == m_row[row, col + 1].Value)
                    {
                        fsmNode = new FSMNode();
                        fsmNode = fsm[fsm.Count - 1];
                        fsmNode.nextColor = new ColorRGB(255, 255, 255); // белый цвет

                        fsmNode = new FSMNode();
                        fsmNode.nextColor = new ColorRGB();
                        fsmNode.hasLoop = true;

                        fsm.Add(fsmNode);
                    }
                    else
                    {
                        fsmNode = new FSMNode();
                        fsmNode = fsm[fsm.Count - 1];
                        fsmNode.hasLoop = true;
                    }
                }
            }

            return fsm;
        }

        private FSM CreateFSMCol(int col)
        {
            FSM fsm = new FSM();
            FSMNode fsmNode = new FSMNode();
            fsmNode.nextColor = new ColorRGB();
            fsmNode.hasLoop = true;
            fsm.Add(fsmNode);

            for (int j = 0; j < m_col.Cols; ++j)
            {
                if (m_col[col, j].Key != 0)
                {
                    for (int i = 0; i < m_col[col, j].Key; ++i)
                    {
                        fsmNode = new FSMNode();
                        fsmNode = fsm[fsm.Count - 1];
                        fsmNode.nextColor = m_col[col, j].Value;

                        fsmNode = new FSMNode();
                        fsmNode.nextColor = new ColorRGB();
                        fsmNode.hasLoop = false;

                        fsm.Add(fsmNode);
                    }

                    if (j + 1 < m_col.Cols && fsm[fsm.Count - 2].nextColor == m_col[col, j + 1].Value)
                    {
                        fsmNode = new FSMNode();
                        fsmNode = fsm[fsm.Count - 1];
                        fsmNode.nextColor = new ColorRGB(255, 255, 255); // белый цвет

                        fsmNode = new FSMNode();
                        fsmNode.nextColor = new ColorRGB();
                        fsmNode.hasLoop = true;

                        fsm.Add(fsmNode);
                    }
                    else
                    {
                        fsmNode = new FSMNode();
                        fsmNode = fsm[fsm.Count - 1];
                        fsmNode.hasLoop = true;
                    }
                }
            }

            return fsm;
        }

        private bool CanColorRow(int row, int col, ColorRGB color)
        {
            HashSet<int> tmpNodes = new HashSet<int>();
            tmpNodes.Add(0);
            HashSet<int> newNodes = new HashSet<int>();

            for (int j = 0; j < col; ++j)
            {
                newNodes = new HashSet<int>();
                foreach (int node in tmpNodes)
                {
                    if (matrix[row, j].IsNone())
                    {
                        if (!rows[row][node].nextColor.IsNone())
                        {
                            newNodes.Add(node + 1);
                        }
                        if (rows[row][node].hasLoop)
                        {
                            newNodes.Add(node);
                        }
                    }
                    else if (matrix[row, j] == new ColorRGB(255, 255, 255))
                    {
                        if (rows[row][node].nextColor == new ColorRGB(255, 255, 255))
                        {
                            newNodes.Add(node + 1);
                        }
                        if (rows[row][node].hasLoop)
                        {
                            newNodes.Add(node);
                        }
                    }
                    else
                    {
                        if (rows[row][node].nextColor == matrix[row, j])
                        {
                            newNodes.Add(node + 1);
                        }
                    }
                }
                tmpNodes = newNodes;
            }

            newNodes = new HashSet<int>();
            foreach (int node in tmpNodes)
            {
                if (rows[row][node].nextColor == color)
                {
                    newNodes.Add(node + 1);
                }
                if (color == new ColorRGB(255,255,255) && rows[row][node].hasLoop)
                {
                    newNodes.Add(node);
                }
            }
            tmpNodes = newNodes;

            for (int j = col + 1; j < matrix.Cols; ++j)
            {
                newNodes = new HashSet<int>();
                foreach (int node in tmpNodes)
                {
                    if (matrix[row, j].IsNone())
                    {
                        if (!rows[row][node].nextColor.IsNone())
                        {
                            newNodes.Add(node + 1);
                        }
                        if (rows[row][node].hasLoop)
                        {
                            newNodes.Add(node);
                        }
                    }
                    else if (matrix[row, j] == new ColorRGB(255, 255, 255))
                    {
                        if (rows[row][node].nextColor == new ColorRGB(255, 255, 255))
                        {
                            newNodes.Add(node + 1);
                        }
                        if (rows[row][node].hasLoop)
                        {
                            newNodes.Add(node);
                        }
                    }
                    else
                    {
                        if (rows[row][node].nextColor == matrix[row, j])
                        {
                            newNodes.Add(node + 1);
                        }
                    }
                }
                tmpNodes = newNodes;
            }

            return tmpNodes.Contains(rows[row].Count - 1);
        }

        private bool CanColorCol(int row, int col, ColorRGB color)
        {
            HashSet<int> tmpNodes = new HashSet<int>() { 0 };
            HashSet<int> newNodes = new HashSet<int>();

            for (int i = 0; i < row; ++i)
            {
                newNodes = new HashSet<int>();
                foreach (int node in tmpNodes)
                {
                    if (matrix[i, col].IsNone())
                    {
                        if (!cols[col][node].nextColor.IsNone())
                        {
                            newNodes.Add(node + 1);
                        }
                        if (cols[col][node].hasLoop)
                        {
                            newNodes.Add(node);
                        }
                    }
                    else if (matrix[i, col] == new ColorRGB(255, 255, 255))
                    {
                        if (cols[col][node].nextColor == new ColorRGB(255, 255, 255))
                        {
                            newNodes.Add(node + 1);
                        }
                        if (cols[col][node].hasLoop)
                        {
                            newNodes.Add(node);
                        }
                    }
                    else
                    {
                        if (cols[col][node].nextColor == matrix[i, col])
                        {
                            newNodes.Add(node + 1);
                        }
                    }
                }
                tmpNodes = newNodes;
            }

            newNodes = new HashSet<int>();
            foreach (int node in tmpNodes)
            {
                if (cols[col][node].nextColor == color)
                {
                    newNodes.Add(node + 1);
                }
                if (color == new ColorRGB(255, 255, 255) && cols[col][node].hasLoop)
                {
                    newNodes.Add(node);
                }
            }
            tmpNodes = newNodes;

            for (int i = row + 1; i < matrix.Rows; ++i)
            {
                newNodes = new HashSet<int>();
                foreach (int node in tmpNodes)
                {
                    if (matrix[i, col].IsNone())
                    {
                        if (!cols[col][node].nextColor.IsNone())
                        {
                            newNodes.Add(node + 1);
                        }
                        if (cols[col][node].hasLoop)
                        {
                            newNodes.Add(node);
                        }
                    }
                    else if (matrix[i, col] == new ColorRGB(255, 255, 255))
                    {
                        if (cols[col][node].nextColor == new ColorRGB(255, 255, 255))
                        {
                            newNodes.Add(node + 1);
                        }
                        if (cols[col][node].hasLoop)
                        {
                            newNodes.Add(node);
                        }
                    }
                    else
                    {
                        if (cols[col][node].nextColor == matrix[i, col])
                        {
                            newNodes.Add(node + 1);
                        }
                    }
                }
                tmpNodes = newNodes;
            }

            return tmpNodes.Contains(cols[col].Count - 1);
        }

        private void UpdateTable()
        {
            for (int row = m_col.Cols; row < table.RowDefinitions.Count; ++row)
            {
                for (int col = m_row.Cols; col < table.ColumnDefinitions.Count; ++col)
                {
                    BoxView tmp = (BoxView)table.Children[row * table.ColumnDefinitions.Count + col];

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
        }
    }
}