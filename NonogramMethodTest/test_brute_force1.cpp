#include "test_brute_force1.h"
#include "brute_force1.h"
#include "Matrix.h"
#include "UnitTest.h"

using namespace std;
using namespace brute_force1;

void test_brute_force1::testAll() {
	TestRunner tr;

	RUN_TEST(tr, test_brute_force1::testEmpty);
	RUN_TEST(tr, test_brute_force1::testFill);
	RUN_TEST(tr, test_brute_force1::testWrong);
}

void test_brute_force1::testEmpty() {
	{
		vector<Matrix> matrixes = readFile("test\\testEmpty.jap");
		Matrix correct(vector<vector<int>>(5, vector<int>(3)));
		Matrix ans = solve(matrixes[0], matrixes[1]);
		ASSERT_EQUAL(ans, correct);
	}
}

void test_brute_force1::testFill() {
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

void test_brute_force1::testWrong() {
	{
		vector<Matrix> matrixes = readFile("test\\testWrong.jap");
		try {
			Matrix ans = solve(matrixes[0], matrixes[1]);
			ASSERT(false);
		}
		catch (...) {
			ASSERT(true);
		}
	}
}