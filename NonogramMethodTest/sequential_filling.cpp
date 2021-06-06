#include "sequential_filling.h"

#include <algorithm>

using namespace std;

/*

Matrix sequential_filling::solve(const Matrix& rows, const Matrix& cols) {
	Matrix table(rows.getRows(), cols.getRows());
	for (int i = 0; i < table.getRows(); ++i) {
		for (int j = 0; j < table.getCols(); ++j) {
			table[i][j] = -1;
		}
	}

	int countNone = table.getRows() * table.getCols();
	int lastCountNone = countNone;

	while (countNone > 0) {
		for (int i = 0; i < table.getRows(); ++i) {
			fillRow(i, countNone, table, rows);
		}
		for (int j = 0; j < table.getCols(); ++j) {
			fiilCol(j, countNone, table, cols);
		}

		if (lastCountNone == countNone) {
			throw exception("No decision");
		}
		lastCountNone = countNone;
	}

	return table;
}

void sequential_filling::fillRow(int row, int& countNone, Matrix& table, const Matrix& rows) {
	int countGroups = rows.getCols();
	for (int j = 0; j < rows.getCols() && rows[row][j] == 0; ++j) {
		countGroups--;
	}

	vector<bool> canBlack(table.getCols(), false);
	vector<bool> canWhite(table.getCols(), false);
	vector<int> countBlack(table.getCols(), 0);
	vector<int> countWhite(table.getCols(), 0);
	vector<vector<int[2]>> calculation(table.getCols(), vector<int[2]>(countGroups, { -1, -1 }));

	for (int j = 0; j < table.getCols(); ++j) {
		if (table[row][j] == 0) {
			canWhite[j] = true;
			countWhite[j]++;
		}
		else if (table[row][j] == 1) {
			canBlack[j] = true;
			countBlack[j]++;
		}
		
		if (j > 0) {
			countWhite[j] += countWhite[j - 1];
		}
	}

	int res = calculateProcessRow(0, 0, 0, row, countGroups, rows, table, canBlack, canWhite, countWhite, countBlack, calculation);
	if (res != 0) {
		for (int i = 1; i < canBlack.size(); ++i) {
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

int calculateProcessRow(int col, int numbOfBlock, int lastColor, int row, int countGroups, const Matrix& rows, const Matrix& table, vector<bool>& canBlack, vector<bool>& canWhite, const vector<int>& countWhite, const vector<int>& countBlack, vector<vector<int[2]>>& calc) {
	if (col == table.getCols()) {
		return numbOfBlock == countGroups ? 1 : 0;
	}
	if (col > table.getCols()) {
		return 0;
	}
	if (calc[col][numbOfBlock][lastColor] != -1) {
		return calc[col][numbOfBlock][lastColor];
	}
	int ans = 0;
	if (numbOfBlock < countGroups && lastColor == 0 && !hasWhites(col, col + rows[row][rows.getCols() - countGroups + numbOfBlock] - 1, countWhite, table)) {
		int val = calculateProcessRow(col + rows[row][rows.getCols() - countGroups + numbOfBlock], numbOfBlock + 1, 1, row, countGroups, rows, table, canBlack, canWhite, countWhite, countBlack, calc);
		if (val == 1)
		{
			ans = 1;
			canBlack[col] = true;
			canBlack[min(table.getCols(), col + rows[row][rows.getCols() - countGroups + numbOfBlock])] = false;
		}
	}
	if (countBlack[col] == 0)
	{
		int val = calculateProcessRow(col + 1, numbOfBlock, 0, row, countGroups, rows, table, canBlack, canWhite, countWhite, countBlack, calc);
		if (val == 1)
		{
			ans = 1;
			canWhite[col] = true;
		}
	}
	calc[col][numbOfBlock][lastColor] = ans;
	return ans;
}

bool hasWhites(int left, int right, const vector<int>& countWhite, const Matrix& table) {
	int ans = countWhite[min(right, table.getCols() - 1)];
	if (left > 0) {
		ans -= countWhite[left - 1];
	}
	return ans > 0;
}

void sequential_filling::fiilCol(int numb, int& countNone, Matrix& table, const Matrix& cols) {

}
*/