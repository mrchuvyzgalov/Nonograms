#include "Matrix.h"

#include <fstream>

using namespace std;

Matrix::Matrix(const vector<vector<int>>& matrix_) : matrix(matrix_) { }

Matrix::Matrix(vector<vector<int>>&& matrix_) : matrix(move(matrix_)) { }

Matrix::Matrix(int n, int m) : matrix(n, vector<int>(m)) { }

const vector<vector<int>>& Matrix::getMatrix() const { return matrix; }

int Matrix::getRows() const { return matrix.size(); }

int Matrix::getCols() const { return matrix.size() == 0 ? 0 : matrix[0].size(); }

vector<int>& Matrix::operator [](int index) { return matrix[index]; }

const vector<int>& Matrix::operator [](int index) const { return matrix[index]; }

bool operator ==(const Matrix& a, const Matrix& b) {
	return a.matrix == b.matrix;
}

ostream& operator <<(ostream& out, const Matrix& a) {
	for (const auto& arr : a.matrix) {
		for (const auto& elem : arr) {
			out << elem << " ";
		}
		out << "\n";
	}
	return out;
}

vector<Matrix> readFile(const string& file) {
	ifstream in(file);

	if (in.is_open()) {
		int nRows, mRows;
		in >> nRows >> mRows;

		Matrix rows(nRows, mRows);

		for (int i = 0; i < nRows; ++i) {
			for (int j = 0; j < mRows; ++j) {
				in >> rows[i][j];
			}
		}

		int nCols, mCols;
		in >> nCols >> mCols;

		Matrix cols(nCols, mCols);

		for (int i = 0; i < nCols; ++i) {
			for (int j = 0; j < mCols; ++j) {
				in >> cols[i][j];
			}
		}

		return { rows, cols };
	}
	else {
		throw exception("Файл не найден");
	}
}