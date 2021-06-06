#pragma once

#include <vector>
#include <array>
#include <string>
#include <sstream>

class Matrix {
public:
	explicit Matrix() = default;
	explicit Matrix(const std::vector<std::vector<int>>& matrix_);
	explicit Matrix(int n, int m);
	explicit Matrix(std::vector<std::vector<int>>&& matrix_);

	const std::vector<std::vector<int>>& getMatrix() const;

	int getRows() const;
	int getCols() const;

	std::vector<int>& operator [](int index);
	const std::vector<int>& operator [](int index) const;

	friend bool operator ==(const Matrix& a, const Matrix& b);
	friend std::ostream& operator <<(std::ostream& out, const Matrix& a);

private:
	std::vector<std::vector<int>> matrix;
};

std::vector<Matrix> readFile(const std::string& file);