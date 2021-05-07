#pragma once

#include "Matrix.h"

#include <vector>

namespace FSMMethod {

	enum class Color { None, White, Black };

	struct FSMNode {
		Color next;
		bool hasLoop;
	};

	using FSM = std::vector<FSMNode>;

	Matrix solve(const Matrix& rows, const Matrix& cols);

	bool canBlackRow(int row, int col, const Matrix& table, const FSM& fsm);

	bool canWhiteRow(int row, int col, const Matrix& table, const FSM& fsm);

	bool canBlackCol(int row, int col, const Matrix& table, const FSM& fsm);

	bool canWhiteCol(int row, int col, const Matrix& table, const FSM& fsm);

	FSM createFSMRow(int row, const Matrix& rows);
	FSM createFSMCol(int col, const Matrix& cols);
}