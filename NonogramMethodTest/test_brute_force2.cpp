#include "test_brute_force2.h"
#include "brute_force2.h"
#include "Matrix.h"
#include "UnitTest.h"

using namespace std;
using namespace brute_force2;

void test_brute_force2::testAll() {
	TestRunner tr;

	RUN_TEST(tr, test_brute_force2::testEmpty);
	RUN_TEST(tr, test_brute_force2::testFill);
	RUN_TEST(tr, test_brute_force2::testWrong);
}

void test_brute_force2::testEmpty() {
	{
		vector<Matrix> matrixes = readFile("test\\testEmpty.jap");
		Matrix correct(vector<vector<int>>(5, vector<int>(3)));
		Matrix ans = solve(matrixes[0], matrixes[1]);
		ASSERT_EQUAL(ans, correct);
	}
}

void test_brute_force2::testFill() {
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

void test_brute_force2::testWrong() {
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