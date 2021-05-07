#include "test_genetic_algorithm.h"
#include "genetic_algorithm.h"
#include "UnitTest.h"
#include "Matrix.h"

#include <vector>

using namespace std;
using namespace genetic_algorithm;

void test_genetic_algorithm::testAll() {
	TestRunner tr;

	RUN_TEST(tr, test_genetic_algorithm::testEmpty);
	RUN_TEST(tr, test_genetic_algorithm::testFill);
}

void test_genetic_algorithm::testEmpty() {
	{
		vector<Matrix> matrixes = readFile("test\\testEmpty.jap");
		Matrix correct(vector<vector<int>>(5, vector<int>(3)));
		Matrix ans = solve(matrixes[0], matrixes[1]);
		ASSERT_EQUAL(ans, correct);
	}
}

void test_genetic_algorithm::testFill() {
	{
		vector<Matrix> matrixes = readFile("test\\testFill1.jap");
		Matrix correct(vector<vector<int>>(3, vector<int>(3, 1)));
		Matrix ans = solve(matrixes[0], matrixes[1]);
		ASSERT_EQUAL(ans, correct);
	}
	{
		vector<Matrix> matrixes = readFile("test\\testFill2.jap");
		Matrix correct({ { 1,0,1 }, { 0,1,0 }, {1,0,1} });
		Matrix ans = solve(matrixes[0], matrixes[1]);
		ASSERT_EQUAL(ans, correct);
	}
	{
		vector<Matrix> matrixes = readFile("test\\testFill3.jap");
		Matrix correct({ {0,0,0}, {0,1,0}, {0,0,0} });
		Matrix ans = solve(matrixes[0], matrixes[1]);
		ASSERT_EQUAL(ans, correct);
	}
}
