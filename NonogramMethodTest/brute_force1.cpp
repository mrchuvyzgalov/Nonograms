#include "brute_force1.h"

using namespace std;

Matrix brute_force1::solve(const Matrix& rows, const Matrix& cols) {
	Matrix table(rows.getRows(), cols.getRows());
	
	bool isFinish = false;
	changeMatrix(0, 0, table, rows, cols, isFinish);

	if (!isFinish) {
		throw exception("No decision");
	}
	return table;
}

void brute_force1::changeMatrix(int row, int col, Matrix& table, const Matrix& rows, const Matrix& cols, bool& isFinish) {
	for (int color = 0; color <= 1 && !isFinish; ++color) {
		table[row][col] = color;

		if (row == table.getRows() - 1 && col == table.getCols() - 1) {
			isFinish = isCorrect(table, rows, cols);
		}
		else {
			changeMatrix(row + (col + 1) / table.getCols(), (col + 1) % table.getCols(), table, rows, cols, isFinish);
		}
	}
}

bool brute_force1::isCorrect(const Matrix& table, const Matrix& rows, const Matrix& cols) {
	for (int i = 0; i < rows.getRows(); ++i) {
		vector<int> groups;
		for (int j = 0; j < table.getCols(); ++j) {
			if (table[i][j] == 1 && (j == 0 || table[i][j - 1] == 0)) {
				groups.push_back(1);
			}
			else if (table[i][j] == 1) {
				groups.back()++;
			}
		}

		int startIndex = 0;
		while (startIndex < rows.getCols() && rows[i][startIndex] == 0) {
			startIndex++;
		}

		if (rows.getCols() - startIndex != groups.size()) {
			return false;
		}
		for (int k = startIndex; k < rows.getCols(); ++k) {
			if (rows[i][k] != groups[k - startIndex]) {
				return false;
			}
		}
	}
	for (int j = 0; j < cols.getRows(); ++j) {
		vector<int> groups;
		for (int i = 0; i < table.getRows(); ++i) {
			if (table[i][j] == 1 && (i == 0 || table[i - 1][j] == 0)) {
				groups.push_back(1);
			}
			else if (table[i][j] == 1) {
				groups.back()++;
			}
		}

		int startIndex = 0;
		while (startIndex < cols.getCols() && cols[j][startIndex] == 0) {
			startIndex++;
		}

		if (cols.getCols() - startIndex != groups.size()) {
			return false;
		}
		for (int k = startIndex; k < cols.getCols(); ++k) {
			if (cols[j][k] != groups[k - startIndex]) {
				return false;
			}
		}
	}
	return true;
}