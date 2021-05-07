#include "test_FSMMethod.h"
#include "FSMMethod.h"
#include "UnitTest.h"

using namespace std;
using namespace FSMMethod;

void test_FSMMethod::testAll() {
	TestRunner tr;

	RUN_TEST(tr, test_FSMMethod::testEmpty);
	RUN_TEST(tr, test_FSMMethod::testFill);
	RUN_TEST(tr, test_FSMMethod::testWrong);
}

void test_FSMMethod::testEmpty() {
	{
		vector<Matrix> matrixes = readFile("test\\testEmpty.jap");
		Matrix correct(vector<vector<int>>(5, vector<int>(3)));
		Matrix ans = solve(matrixes[0], matrixes[1]);
		ASSERT_EQUAL(ans, correct);
	}
}

void test_FSMMethod::testFill() {
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

void test_FSMMethod::testWrong() {
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