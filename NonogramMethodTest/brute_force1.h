#pragma once

#include "Matrix.h"

namespace brute_force1 {

	Matrix solve(const Matrix& rows, const Matrix& cols);

	void changeMatrix(int row, int col, Matrix& table, const Matrix& rows, const Matrix& cols, bool& isFinish);

	bool isCorrect(const Matrix& table, const Matrix& rows, const Matrix& cols);
}