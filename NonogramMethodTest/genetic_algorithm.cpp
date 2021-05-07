#include "genetic_algorithm.h"

#include <random>
#include <algorithm>
#include <cmath>

using namespace std;

Matrix genetic_algorithm::solve(const Matrix& rows, const Matrix& cols) {
	int MutationPercentage = 60;
	int countOfPopulation = 5;

    // инициализация
    vector<Matrix> population(countOfPopulation, Matrix(rows.getRows(), cols.getRows()));
    for (int i = 0; i < countOfPopulation; ++i) {
        std::random_device rd;
        std::mt19937 mersenne(rd());

        for (int row = 0; row < population[i].getRows(); ++row) {
            int countOfOnes = 0;
            int countOFNotZeroCells = 0;
            for (int col = 0; col < rows.getCols(); ++col)
            {
                if (rows[row][col] > 0) countOFNotZeroCells++;
                countOfOnes += rows[row][col];
            }

            int freeZeroCells = 0;
            if (countOFNotZeroCells > 0) {
                freeZeroCells = population[i].getCols() - (countOfOnes + countOFNotZeroCells - 1);
            }

            int spaces = mersenne() % (freeZeroCells + 1);
            int step = spaces;
            freeZeroCells -= spaces;

            for (int col = 0; col < rows.getCols(); ++col) {
                if (rows[row][col] > 0) {
                    for (int col1 = step; col1 < step + rows[row][col]; ++col1) {
                        population[i][row][col1] = 1;
                    }
                    step += rows[row][col];

                    if (step < population[i].getCols()) {
                        step++;
                    }

                    spaces = mersenne() % (freeZeroCells + 1);
                    step += spaces;
                    freeZeroCells -= spaces;
                }
            }
        }
    }

    // алгоритм
    while (fitness(population[0], cols) > 0)//for (int i = 0; i < 100; ++i)
    {
        NextPopulation(population, MutationPercentage, rows, cols);
    }

    return population[0];
}

void genetic_algorithm::NextPopulation(vector<Matrix>& population, int MutationPercentage, const Matrix& rows, const Matrix& cols)
{
    std::random_device rd;
    std::mt19937 mersenne(rd());

    int lastSize = population.size();
    for (int i = 0; i < lastSize; ++i)
    {
        for (int j = i + 1; j < lastSize; ++j)
        {
            Matrix m(population[i].getRows(), population[i].getCols());

            int border = mersenne() % (population[i].getRows() - 1) + 1;

            for (int row = 0; row < border; ++row)
            {
                for (int col = 0; col < population[i].getCols(); ++col)
                {
                    m[row][col] = population[i][row][col];
                }
            }

            for (int row = border; row < population[j].getRows(); ++row)
            {
                for (int col = 0; col < population[j].getCols(); ++col)
                {
                    m[row, col] = population[j][row, col];
                }
            }

            if (mersenne() % 100 < MutationPercentage)
            {
                Mutation(m, rows);
            }
            population.push_back(m);
        }
    }


    sort(population.begin(), population.end(), [&](const Matrix& a, const Matrix& b) { return fitness(a, cols) - fitness(b, cols); });
    population = vector<Matrix>(population.begin(), population.begin() + lastSize);
}

void genetic_algorithm::Mutation(Matrix& m, const Matrix& rows)
{
    std::random_device rd;
    std::mt19937 mersenne(rd());

    for (int i = 0; i < 2; ++i)
    {
        int row = mersenne() % m.getRows();
        for (int col = 0; col < m.getCols(); ++col)
        {
            m[row][col] = 0;
        }
        int countOfOnes = 0;
        int countOFNotZeroCells = 0;
        for (int col = 0; col < rows.getCols(); ++col)
        {
            if (rows[row][col] > 0) countOFNotZeroCells++;
            countOfOnes += rows[row][col];
        }

        int freeZeroCells = 0;
        if (countOFNotZeroCells > 0)
        {
            freeZeroCells = m.getCols() - (countOfOnes + countOFNotZeroCells - 1);
        }

        int spaces = mersenne() % (freeZeroCells + 1);
        int step = 0;
        freeZeroCells -= spaces;
        step += spaces;

        for (int col = 0; col < rows.getCols(); ++col)
        {
            if (rows[row][col] > 0)
            {
                for (int col1 = step; col1 < step + rows[row][col]; ++col1)
                {
                    m[row][col1] = 1;
                }
                step += rows[row][col];

                if (step < m.getCols())
                {
                    step++;
                }

                spaces = mersenne() % (freeZeroCells + 1);
                step += spaces;
                freeZeroCells -= spaces;
            }
        }
    }
}

int genetic_algorithm::fitness(const Matrix& m, const Matrix& cols) {
    int res = 0;

    for (int i = 0; i < cols.getRows(); ++i) {
        int sum1 = 0;
        for (int j = 0; j < cols.getCols(); ++j) {
            sum1 += cols[i][j];
        }

        int sum2 = 0;
        for (int j = 0; j < m.getRows(); ++j) {
            if (m[j][i]) sum2++;
        }

        res += abs(sum1 - sum2);
    }

    for (int col = 0; col < m.getCols(); ++col) {
        int tmp = 0;
        while (tmp < cols.getCols() && cols[col][tmp] == 0) tmp++;

        vector<int> count;
        for (int row = 0; row < m.getRows(); ++row) {
            if (m[row][col]) {
                if (row == 0 || m[row - 1][col] == 0) count.push_back(0);
                count.back()++;
            }
        }

        if (cols.getCols() - tmp == count.size()) {
            for (int ind = 0; ind < count.size(); ++ind) {
                if (cols[col][ind + tmp] != count[ind]) {
                    res++;
                    break;
                }
            }
        }
        else {
            res++;
        }
    }

    return res;
}