#pragma once

#include "Matrix.h"

#include <vector>

namespace genetic_algorithm {

	Matrix solve(const Matrix& rows, const Matrix& cols);

	void NextPopulation(std::vector<Matrix>& population, int MutationPercentage, const Matrix& rows, const Matrix& cols);

	void Mutation(Matrix& m, const Matrix& rows);

	int fitness(const Matrix& m, const Matrix& cols);

}