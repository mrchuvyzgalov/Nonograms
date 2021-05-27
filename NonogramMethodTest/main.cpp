#include "test_brute_force1.h"
#include "test_brute_force2.h"
#include "test_FSMMethod.h"
#include "test_genetic_algorithm.h"
#include "brute_force1.h"
#include "brute_force2.h"
#include "genetic_algorithm.h"
#include "FSMMethod.h"
#include "Matrix.h"

#include <iostream>
#include <vector>
#include <fstream>
#include <chrono>

using namespace std;

void testMethod(const vector<Matrix>& matrixes) {
	int countOfTests = 20;

	chrono::steady_clock::time_point start = chrono::steady_clock::now();
	unsigned long long sum = 0;
	for (int i = 0; i < countOfTests; ++i) {
		Matrix ans = brute_force1::solve(matrixes[0], matrixes[1]);
		sum += chrono::duration_cast<std::chrono::nanoseconds>(std::chrono::steady_clock::now() - start).count();
		start = chrono::steady_clock::now();
	}

	cerr << "Test of the 1st brute force method: " << sum / countOfTests << " ns\n";

	start = chrono::steady_clock::now();
	sum = 0;
	for (int i = 0; i < countOfTests; ++i) {
		Matrix ans = brute_force2::solve(matrixes[0], matrixes[1]);
		sum += chrono::duration_cast<std::chrono::nanoseconds>(std::chrono::steady_clock::now() - start).count();
		start = chrono::steady_clock::now();
	}

	cerr << "Test of the 2st brute force method: " << sum / countOfTests << " ns\n";

	start = chrono::steady_clock::now();
	sum = 0;
	for (int i = 0; i < countOfTests; ++i) {
		Matrix ans = genetic_algorithm::solve(matrixes[0], matrixes[1]);
		sum += chrono::duration_cast<std::chrono::nanoseconds>(std::chrono::steady_clock::now() - start).count();
		start = chrono::steady_clock::now();
	}

	cerr << "Test of the genetic method:        " << sum / countOfTests << " ns\n";

	start = chrono::steady_clock::now();
	sum = 0;
	for (int i = 0; i < countOfTests; ++i) {
		Matrix ans = genetic_algorithm::solve(matrixes[0], matrixes[1]);
		sum += chrono::duration_cast<std::chrono::nanoseconds>(std::chrono::steady_clock::now() - start).count();
		start = chrono::steady_clock::now();
	}

	cerr << "Test of the FSM method:             " << sum / countOfTests << " ns\n";
}

int main() {
	//testing
	cerr << "Testing:\n\n";
	test_brute_force1::testAll();
	cerr << "\n";
	test_brute_force2::testAll();
	cerr << "\n";
	test_FSMMethod::testAll();
	cerr << "\n";
	//test_genetic_algorithm::testAll();
	cerr << "\n";

	cout << "Time testing:\n\n";

	cout << "Test empty nonogram:\n";
	testMethod(readFile("test\\TimeEmpty.jap"));



	system("pause");
	return 0;
}