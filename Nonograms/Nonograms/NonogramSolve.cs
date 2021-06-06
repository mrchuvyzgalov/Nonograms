using System;
using System.Collections.Generic;
using System.Text;

namespace Nonograms
{
    static class NonogramSolve
    {
        public static Matrix<ColorRGB> Solve(Matrix<KeyValuePair<int, ColorRGB>> m_row, Matrix<KeyValuePair<int, ColorRGB>> m_col, HashSet<ColorRGB> colors)
        {
            ColorRGB[,] ArrayMatrix = new ColorRGB[m_row.Rows, m_col.Rows];
            for (int i = 0; i < ArrayMatrix.GetLength(0); ++i)
            {
                for (int j = 0; j < ArrayMatrix.GetLength(1); ++j)
                {
                    ArrayMatrix[i, j] = new ColorRGB();
                }
            }

            Matrix<ColorRGB> matrix = new Matrix<ColorRGB>(ArrayMatrix);

            try
            {
                int countNone = matrix.Cols * matrix.Rows;

                FSM[] rows = new FSM[m_row.Rows];
                FSM[] cols = new FSM[m_col.Rows];

                for (int i = 0; i < rows.Length; ++i)
                {
                    rows[i] = CreateFSMRow(i, m_row);
                }

                for (int j = 0; j < cols.Length; ++j)
                {
                    cols[j] = CreateFSMCol(j, m_col);
                }

                Solve(countNone, matrix, colors, rows, cols);
                return matrix;
            }
            catch (Exception error)
            {
                if (error.Message == "Данным алгоритмом кроссворд однозначно не решить")
                {
                    List<KeyValuePair<int, int>> noneCells = new List<KeyValuePair<int, int>>();

                    for (int row = 0; row < matrix.Rows; ++row)
                    {
                        for (int col = 0; col < matrix.Cols; ++col)
                        {
                            if (matrix[row,col].IsNone())
                            {
                                noneCells.Add(new KeyValuePair<int, int>(row, col));
                            }
                        }
                    }

                    if (noneCells.Count > 20) return null;

                    int[] colorsMatrix = new int[noneCells.Count];
                    for (int i = 0; i < colorsMatrix.Length; ++i)
                    {
                        colorsMatrix[i] = 0;
                    }
                    bool hasSolution = false, hasManySolutions = false;
                    BruteForceSolve(matrix, ref hasSolution, ref hasManySolutions, noneCells, colors, m_row, m_col);

                    if (hasSolution && !hasManySolutions) return matrix;
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        static private FSM CreateFSMRow(int row, Matrix<KeyValuePair<int, ColorRGB>> m_row)
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

        static private FSM CreateFSMCol(int col, Matrix<KeyValuePair<int, ColorRGB>> m_col)
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

        static private bool CanColorRow(int row, int col, ColorRGB color, Matrix<ColorRGB> matrix, FSM[] rows)
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
                if (color == new ColorRGB(255, 255, 255) && rows[row][node].hasLoop)
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

        static private bool CanColorCol(int row, int col, ColorRGB color, Matrix<ColorRGB> matrix, FSM[] cols)
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

        static private void Solve(int countNone, Matrix<ColorRGB> matrix, HashSet<ColorRGB> colors, FSM[] rows, FSM[] cols)
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
                            if (CanColorRow(row, col, new ColorRGB(255, 255, 255), matrix, rows))
                            {
                                newColor = new ColorRGB(255, 255, 255);
                            }
                            foreach (ColorRGB color in colors)
                            {
                                if (CanColorRow(row, col, color, matrix, rows))
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
                                if (CanColorCol(row, col, new ColorRGB(255, 255, 255), matrix, cols))
                                {
                                    newColor = new ColorRGB(255, 255, 255);
                                }
                                foreach (ColorRGB color in colors)
                                {
                                    if (CanColorCol(row, col, color, matrix, cols))
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
                    throw new Exception("Данным алгоритмом кроссворд однозначно не решить");
                }
                else
                {
                    lastCountNone = countNone;
                }
            }
        }

        static private void BruteForceSolve(Matrix<ColorRGB> matrix, ref bool hasSolution, ref bool hasManySolutions, List<KeyValuePair<int, int>> noneCells, HashSet<ColorRGB> colors, Matrix<KeyValuePair<int, ColorRGB>> m_row, Matrix<KeyValuePair<int, ColorRGB>> m_col)
        {
            Matrix<ColorRGB> tmpMatrix = new Matrix<ColorRGB>(matrix);
            ChangeColor(0, matrix, ref hasSolution, ref hasManySolutions, noneCells, colors, m_row, m_col, tmpMatrix);
        }
        static private void ChangeColor(int index, Matrix<ColorRGB> matrix, ref bool hasSolution, ref bool hasManySolutions, List<KeyValuePair<int, int>> noneCells, HashSet<ColorRGB> colors, Matrix<KeyValuePair<int, ColorRGB>> m_row, Matrix<KeyValuePair<int, ColorRGB>> m_col, Matrix<ColorRGB> tmpMatrix)
        {
            if (hasManySolutions)
            {
                return;
            }
            tmpMatrix[noneCells[index].Key, noneCells[index].Value] = new ColorRGB(255,255,255);

            if (index == noneCells.Count - 1)
            {
                if (IsCorrectSolution(tmpMatrix, m_row, m_col))
                {
                    if (hasSolution)
                    {
                        hasManySolutions = true;
                    }
                    else
                    {
                        hasSolution = true;

                        for (int i = 0; i < noneCells.Count; ++i)
                        {
                            matrix[noneCells[i].Key, noneCells[i].Value] = tmpMatrix[noneCells[i].Key, noneCells[i].Value];
                        }
                    }
                }
            }
            else
            {
                ChangeColor(index + 1, matrix, ref hasSolution, ref hasManySolutions, noneCells, colors, m_row, m_col, tmpMatrix);
            }

            foreach (ColorRGB color in colors)
            {
                if (hasManySolutions)
                {
                    return;
                }
                tmpMatrix[noneCells[index].Key, noneCells[index].Value] = color;

                if (index == noneCells.Count - 1)
                {
                    if (IsCorrectSolution(tmpMatrix, m_row, m_col))
                    {
                        if (hasSolution)
                        {
                            hasManySolutions = true;
                        }
                        else
                        {
                            hasSolution = true;
                            
                            for (int i = 0; i < noneCells.Count; ++i)
                            {
                                matrix[noneCells[i].Key, noneCells[i].Value] = tmpMatrix[noneCells[i].Key, noneCells[i].Value];
                            }
                        }
                    }
                }
                else
                {
                    ChangeColor(index + 1, matrix, ref hasSolution, ref hasManySolutions, noneCells, colors, m_row, m_col, tmpMatrix);
                }
            }
        }
        static private bool IsCorrectSolution(Matrix<ColorRGB> tmpMatrix, Matrix<KeyValuePair<int, ColorRGB>> m_row, Matrix<KeyValuePair<int, ColorRGB>> m_col)
        {
            for (int row = 0; row < tmpMatrix.Rows; ++row)
            {
                List<KeyValuePair<int, ColorRGB>> group = new List<KeyValuePair<int, ColorRGB>>();
                for (int col = 0; col < tmpMatrix.Cols; ++col)
                {
                    if (tmpMatrix[row,col] != new ColorRGB(255,255,255))
                    {
                        if (group.Count == 0 || tmpMatrix[row, col - 1] != tmpMatrix[row,col])
                        {
                            group.Add(new KeyValuePair<int, ColorRGB>(0, tmpMatrix[row, col]));
                        }

                        group[group.Count - 1] = new KeyValuePair<int, ColorRGB>(group[group.Count - 1].Key + 1, group[group.Count - 1].Value);
                    }
                }

                int startIndex = 0;
                while (startIndex < m_row.Cols && m_row[row, startIndex].Key == 0)
                {
                    startIndex++;
                }

                if (m_row.Cols - startIndex != group.Count)
                {
                    return false;
                }
                for (int k = startIndex; k < m_row.Cols; ++k)
                {
                    if (m_row[row, k].Key != group[k - startIndex].Key || m_row[row, k].Value != group[k - startIndex].Value)
                    {
                        return false;
                    }
                }
            }

            for (int col = 0; col < tmpMatrix.Cols; ++col)
            {
                List<KeyValuePair<int, ColorRGB>> group = new List<KeyValuePair<int, ColorRGB>>();
                for (int row = 0; row < tmpMatrix.Rows; ++row)
                {
                    if (tmpMatrix[row, col] != new ColorRGB(255, 255, 255))
                    {
                        if (group.Count == 0 || tmpMatrix[row - 1, col] != tmpMatrix[row, col])
                        {
                            group.Add(new KeyValuePair<int, ColorRGB>(0, tmpMatrix[row, col]));
                        }
                        group[group.Count - 1] = new KeyValuePair<int, ColorRGB>(group[group.Count - 1].Key + 1, group[group.Count - 1].Value);
                    }
                }

                int startIndex = 0;
                while (startIndex < m_col.Cols && m_col[col, startIndex].Key == 0)
                {
                    startIndex++;
                }

                if (m_col.Cols - startIndex != group.Count)
                {
                    return false;
                }
                for (int k = startIndex; k < m_col.Cols; ++k)
                {
                    if (m_col[col,k].Key != group[k - startIndex].Key || m_col[col, k].Value != group[k - startIndex].Value)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
