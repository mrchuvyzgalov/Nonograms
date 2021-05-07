#pragma once

#include "Matrix.h"

#include <vector>

namespace brute_force2 {
	Matrix solve(const Matrix& rows, const Matrix& cols);

	void forCycle(int row, Matrix& table, const Matrix& rows, const Matrix& cols, bool& isFinish, const std::vector<int>& countBlocks, const std::vector<int>& sum);

	void changeRow(int row, int col, Matrix& table, const Matrix& rows, const Matrix& cols, bool& isFinish, int tmpSum, int numbOfBlock, const std::vector<int>& countBlocks, const std::vector<int>& sum);

	bool isCorrect(const Matrix& table, const Matrix& cols);
}