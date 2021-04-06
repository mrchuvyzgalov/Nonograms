using System;
using System.Collections.Generic;
using System.Text;

namespace Nonograms
{
    class Matrix<T>
    {
        T[,] matrix;

        public Matrix(int rows, int cols)
        {
            if (rows < 1 || cols < 1) throw new ArgumentException();

            matrix = new T[rows, cols];
        }

        public Matrix(T[,] matrix)
        {
            if (matrix == null) throw new ArgumentException();

            this.matrix = new T[matrix.GetLength(0), matrix.GetLength(1)];

            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    this.matrix[i, j] = matrix[i, j];
                }
            }
        }

        public int Rows => matrix.GetLength(0);
        public int Cols => matrix.GetLength(1);

        public T this[int index1, int index2]
        {
            get
            {
                if (!IsCorrectIndexes(index1, index2)) throw new ArgumentException();
                return matrix[index1, index2];
            }
            set
            {
                if (!IsCorrectIndexes(index1, index2)) throw new ArgumentException();
                matrix[index1, index2] = value;
            }
        }

        private bool IsCorrectIndexes(int index1, int index2)
        {
            return index1 >= 0 && index2 >= 0 && index1 < matrix.GetLength(0) && index2 < matrix.GetLength(1);
        }
    }
}
