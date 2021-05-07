#include "FSMMethod.h"

#include <set>

using namespace std;

Matrix FSMMethod::solve(const Matrix& rows, const Matrix& cols) {
	Matrix table(rows.getRows(), cols.getRows());
	for (int i = 0; i < table.getRows(); ++i) {
		for (int j = 0; j < table.getCols(); ++j) {
			table[i][j] = -1;
		}
	}

	vector<FSM> fsmRows(rows.getRows());
	for (int i = 0; i < rows.getRows(); ++i) {
		fsmRows[i] = createFSMRow(i, rows);
	}
	vector<FSM> fsmCols(cols.getRows());
	for (int j = 0; j < cols.getRows(); ++j) {
		fsmCols[j] = createFSMCol(j, cols);
	}

	int countNone = table.getRows() * table.getCols();
	int lastCountNone = countNone;

	while (countNone > 0) {
		for (int i = 0; i < table.getRows() && countNone > 0; ++i) {
			for (int j = 0; j < table.getCols() && countNone > 0; ++j) {
				if (table[i][j] == -1) {
					bool canBlack = canBlackRow(i, j, table, fsmRows[i]);
					bool canWhite = canWhiteRow(i, j, table, fsmRows[i]);

					if (canBlack && !canWhite) {
						table[i][j] = 1;
						countNone--;
					}
					else if (!canBlack && canWhite) {
						table[i][j] = 0;
						countNone--;
					}
					else if (!canBlack && !canWhite) {
						throw exception("No decision");
					}
					else {
						bool canBlackC = canBlackCol(i, j, table, fsmCols[j]);
						bool canWhiteC = canWhiteCol(i, j, table, fsmCols[j]);

						if (canBlackC && !canWhiteC) {
							table[i][j] = 1;
							countNone--;
						}
						else if (!canBlackC && canWhiteC) {
							table[i][j] = 0;
							countNone--;
						}
						else if (!canBlackC && !canWhiteC) {
							throw exception("No decision");
						}
					}
				}
			}
		}

		if (lastCountNone == countNone) {
			throw exception("No decision");
		}
		else {
			lastCountNone = countNone;
		}
	}

	return table;
}

bool FSMMethod::canBlackRow(int row, int col, const Matrix& table, const FSM& fsm) {
	set<int> tmpNodes = { 0 };
	set<int> newNodes;
	for (int j = 0; j < col; ++j) {
		newNodes.clear();
		for (int node : tmpNodes) {
			if (table[row][j] == -1) {
				if (fsm[node].next != Color::None) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[row][j] == 0) {
				if (fsm[node].next == Color::White) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[row][j] == 1) {
				if (fsm[node].next == Color::Black) {
					newNodes.insert(node + 1);
				}
			}
		}
		tmpNodes = newNodes;
	}

	newNodes.clear();
	for (int node : tmpNodes) {
		if (fsm[node].next == Color::Black) {
			newNodes.insert(node + 1);
		}
	}
	tmpNodes = newNodes;


	for (int j = col + 1; j < table.getCols(); ++j) {
		newNodes.clear();
		for (int node : tmpNodes) {
			if (table[row][j] == -1) {
				if (fsm[node].next != Color::None) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[row][j] == 0) {
				if (fsm[node].next == Color::White) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[row][j] == 1) {
				if (fsm[node].next == Color::Black) {
					newNodes.insert(node + 1);
				}
			}
		}
		tmpNodes = newNodes;
	}

	for (int numb : tmpNodes) {
		if (numb == fsm.size() - 1) {
			return true;
		}
	}
	return false;
}

bool FSMMethod::canWhiteRow(int row, int col, const Matrix& table, const FSM& fsm) {
	set<int> tmpNodes = { 0 };
	set<int> newNodes;
	for (int j = 0; j < col; ++j) {
		newNodes.clear();
		for (int node : tmpNodes) {
			if (table[row][j] == -1) {
				if (fsm[node].next != Color::None) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[row][j] == 0) {
				if (fsm[node].next == Color::White) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[row][j] == 1) {
				if (fsm[node].next == Color::Black) {
					newNodes.insert(node + 1);
				}
			}
		}
		tmpNodes = newNodes;
	}

	newNodes.clear();
	for (int node : tmpNodes) {
		if (fsm[node].next == Color::White) {
			newNodes.insert(node + 1);
		}
		if (fsm[node].hasLoop) {
			newNodes.insert(node);
		}
	}
	tmpNodes = newNodes;


	for (int j = col + 1; j < table.getCols(); ++j) {
		newNodes.clear();
		for (int node : tmpNodes) {
			if (table[row][j] == -1) {
				if (fsm[node].next != Color::None) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[row][j] == 0) {
				if (fsm[node].next == Color::White) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[row][j] == 1) {
				if (fsm[node].next == Color::Black) {
					newNodes.insert(node + 1);
				}
			}
		}
		tmpNodes = newNodes;
	}

	for (int numb : tmpNodes) {
		if (numb == fsm.size() - 1) {
			return true;
		}
	}
	return false;
}

bool FSMMethod::canBlackCol(int row, int col, const Matrix& table, const FSM& fsm) {
	set<int> tmpNodes = { 0 };
	set<int> newNodes;
	for (int i = 0; i < row; ++i) {
		newNodes.clear();
		for (int node : tmpNodes) {
			if (table[i][col] == -1) {
				if (fsm[node].next != Color::None) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[i][col] == 0) {
				if (fsm[node].next == Color::White) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[i][col] == 1) {
				if (fsm[node].next == Color::Black) {
					newNodes.insert(node + 1);
				}
			}
		}
		tmpNodes = newNodes;
	}

	newNodes.clear();
	for (int node : tmpNodes) {
		if (fsm[node].next == Color::Black) {
			newNodes.insert(node + 1);
		}
	}
	tmpNodes = newNodes;

	for (int i = row + 1; i < table.getRows(); ++i) {
		newNodes.clear();
		for (int node : tmpNodes) {
			if (table[i][col] == -1) {
				if (fsm[node].next != Color::None) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[i][col] == 0) {
				if (fsm[node].next == Color::White) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[i][col] == 1) {
				if (fsm[node].next == Color::Black) {
					newNodes.insert(node + 1);
				}
			}
		}
		tmpNodes = newNodes;
	}

	for (int numb : tmpNodes) {
		if (numb == fsm.size() - 1) {
			return true;
		}
	}
	return false;
}

bool FSMMethod::canWhiteCol(int row, int col, const Matrix& table, const FSM& fsm) {
	set<int> tmpNodes = { 0 };
	set<int> newNodes;
	for (int i = 0; i < row; ++i) {
		newNodes.clear();
		for (int node : tmpNodes) {
			if (table[i][col] == -1) {
				if (fsm[node].next != Color::None) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[i][col] == 0) {
				if (fsm[node].next == Color::White) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[i][col] == 1) {
				if (fsm[node].next == Color::Black) {
					newNodes.insert(node + 1);
				}
			}
		}
		tmpNodes = newNodes;
	}

	newNodes.clear();
	for (int node : tmpNodes) {
		if (fsm[node].next == Color::White) {
			newNodes.insert(node + 1);
		}
		if (fsm[node].hasLoop) {
			newNodes.insert(node);
		}
	}
	tmpNodes = newNodes;

	for (int i = row + 1; i < table.getRows(); ++i) {
		newNodes.clear();
		for (int node : tmpNodes) {
			if (table[i][col] == -1) {
				if (fsm[node].next != Color::None) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[i][col] == 0) {
				if (fsm[node].next == Color::White) {
					newNodes.insert(node + 1);
				}
				if (fsm[node].hasLoop) {
					newNodes.insert(node);
				}
			}
			else if (table[i][col] == 1) {
				if (fsm[node].next == Color::Black) {
					newNodes.insert(node + 1);
				}
			}
		}
		tmpNodes = newNodes;
	}

	for (int numb : tmpNodes) {
		if (numb == fsm.size() - 1) {
			return true;
		}
	}
	return false;
}

FSMMethod::FSM FSMMethod::createFSMRow(int row, const Matrix& rows) {
	FSM fsm(1);
	fsm[0] = { Color::None, true };

	for (int j = 0; j < rows.getCols(); ++j) {
		if (rows[row][j] != 0) {
			for (int i = 0; i < rows[row][j]; ++i) {
				fsm.back().next = Color::Black;
				fsm.push_back({ Color::None, false });
			}

			if (j < rows.getCols() - 1) {
				fsm.back().next = Color::White;
				fsm.push_back({ Color::None, true });
			}
			else {
				fsm.back().hasLoop = true;
			}
		}
	}

	return fsm;
}

FSMMethod::FSM FSMMethod::createFSMCol(int col, const Matrix& cols) {
	FSM fsm(1);
	fsm[0] = { Color::None, true };

	for (int j = 0; j < cols.getCols(); ++j) {
		if (cols[col][j] != 0) {
			for (int i = 0; i < cols[col][j]; ++i) {
				fsm.back().next = Color::Black;
				fsm.push_back({ Color::None, false });
			}

			if (j < cols.getCols() - 1) {
				fsm.back().next = Color::White;
				fsm.push_back({ Color::None, true });
			}
			else {
				fsm.back().hasLoop = true;
			}
		}
	}

	return fsm;
}