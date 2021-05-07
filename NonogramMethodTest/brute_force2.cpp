#include "brute_force2.h"

using namespace std;

Matrix brute_force2::solve(const Matrix& rows, const Matrix& cols) {
	Matrix table(rows.getRows(), cols.getRows());

	vector<int> countBlocks(rows.getRows());
	for (int i = 0; i < rows.getRows(); ++i) {
		countBlocks[i] = rows.getCols();
		while (countBlocks[i] > 0 && rows[i][rows.getCols() - countBlocks[i]] == 0) {
			countBlocks[i]--;
		}
		countBlocks[i]++;
	}

	vector<int> sum(rows.getRows());
	for (int i = 0; i < rows.getRows(); ++i) {
		sum[i] = table.getCols();
		for (int j = 0; j < rows.getCols(); ++j) {
			sum[i] -= rows[i][j];
		}
	}

	bool isFinish = false;
	forCycle(0, table, rows, cols, isFinish, countBlocks, sum);

	if (!isFinish) {
		throw exception("No decision");
	}
	return table;
}

void brute_force2::forCycle(int row, Matrix& table, const Matrix& rows, const Matrix& cols, bool& isFinish, const std::vector<int>& countBlocks, const std::vector<int>& sum) {
	if (!isFinish) {
		if (row == table.getRows()) {
			isFinish = isCorrect(table, cols);
		}
		else {
			changeRow(row, 0, table, rows, cols, isFinish, 0, 0, countBlocks, sum);
		}
	}
}

void brute_force2::changeRow(int row, int col, Matrix& table, const Matrix& rows, const Matrix& cols, bool& isFinish, int tmpSum, int numbOfBlock, const std::vector<int>& countBlocks, const std::vector<int>& sum) {
	if (numbOfBlock == countBlocks[row] - 1) {
		for (int j = col; j < table.getCols(); ++j) {
			table[row][j] = 0;
		}

		forCycle(row + 1, table, rows, cols, isFinish, countBlocks, sum);
	}
	else {
		for (int countWhiteBlocks = (numbOfBlock == 0 ? 0 : 1); countWhiteBlocks + tmpSum + countBlocks[row] - 2 - numbOfBlock <= sum[row] && !isFinish; ++countWhiteBlocks) {
			for (int j = col; j < col + countWhiteBlocks; ++j) {
				table[row][j] = 0;
			}
			for (int j = col + countWhiteBlocks; j < col + countWhiteBlocks + rows[row][rows.getCols() - countBlocks[row] + numbOfBlock + 1]; ++j) {
				table[row][j] = 1;
			}
			changeRow(row, col + countWhiteBlocks + rows[row][rows.getCols() - countBlocks[row] + numbOfBlock + 1], table, rows, cols, isFinish, tmpSum + countWhiteBlocks, numbOfBlock + 1, countBlocks, sum);
		}
	}

}

bool brute_force2::isCorrect(const Matrix& table, const Matrix& cols) {
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