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
#include <unordered_set>

using namespace std;

enum class Time { milli, nano };
enum class Algo { brute1, brute2, FSM, genetic };

void testMethod(const vector<Matrix>& matrixes, Time time, unordered_set<Algo>&& has) {
	int countOfTests = 1;

	chrono::steady_clock::time_point start;
	unsigned long long sum;

	if (has.find(Algo::FSM) != has.end()) {
		start = chrono::steady_clock::now();
		sum = 0;
		for (int i = 0; i < countOfTests; ++i) {
			Matrix ans = FSMMethod::solve(matrixes[0], matrixes[1]);
			if (time == Time::nano) {
				sum += chrono::duration_cast<std::chrono::nanoseconds>(std::chrono::steady_clock::now() - start).count();
			}
			else {
				sum += chrono::duration_cast<std::chrono::milliseconds>(std::chrono::steady_clock::now() - start).count();
			}
			start = chrono::steady_clock::now();
		}

		cerr << "Test of the FSM method: " << sum / countOfTests;
		if (time == Time::nano) {
			cerr << " ns";
		}
		else {
			cerr << " ms";
		}
		cerr << endl;
	}

	if (has.find(Algo::brute1) != has.end()) {
		sum = 0;
		start = chrono::steady_clock::now();
		for (int i = 0; i < countOfTests; ++i) {
			Matrix ans = brute_force1::solve(matrixes[0], matrixes[1]);
			if (time == Time::nano) {
				sum += chrono::duration_cast<std::chrono::nanoseconds>(std::chrono::steady_clock::now() - start).count();
			}
			else {
				sum += chrono::duration_cast<std::chrono::milliseconds>(std::chrono::steady_clock::now() - start).count();
			}
			start = chrono::steady_clock::now();
		}

		cerr << "Test of the 1st brute force method: " << sum / countOfTests;
		if (time == Time::nano) {
			cerr << " ns";
		}
		else {
			cerr << " ms";
		}
		cerr << endl;
	}

	if (has.find(Algo::brute2) != has.end()) {
		start = chrono::steady_clock::now();
		sum = 0;
		for (int i = 0; i < countOfTests; ++i) {
			Matrix ans = brute_force2::solve(matrixes[0], matrixes[1]);
			if (time == Time::nano) {
				sum += chrono::duration_cast<std::chrono::nanoseconds>(std::chrono::steady_clock::now() - start).count();
			}
			else {
				sum += chrono::duration_cast<std::chrono::milliseconds>(std::chrono::steady_clock::now() - start).count();
			}
			start = chrono::steady_clock::now();
		}

		cerr << "Test of the 2st brute force method: " << sum / countOfTests;
		if (time == Time::nano) {
			cerr << " ns";
		}
		else {
			cerr << " ms";
		}
		cerr << endl;
	}

	if (has.find(Algo::genetic) != has.end()) {
		start = chrono::steady_clock::now();
		sum = 0;
		for (int i = 0; i < countOfTests; ++i) {
			Matrix ans = genetic_algorithm::solve(matrixes[0], matrixes[1]);
			if (time == Time::nano) {
				sum += chrono::duration_cast<std::chrono::nanoseconds>(std::chrono::steady_clock::now() - start).count();
			}
			else {
				sum += chrono::duration_cast<std::chrono::milliseconds>(std::chrono::steady_clock::now() - start).count();
			}
			start = chrono::steady_clock::now();
		}

		cerr << "Test of the genetic method: " << sum / countOfTests;
		if (time == Time::nano) {
			cerr << " ns";
		}
		else {
			cerr << " ms";
		}
		cerr << endl;
	}
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
	test_genetic_algorithm::testAll();
	cerr << "\n";

	cout << "Time testing:\n\n";

	cout << "\nTest 5x5:\n";
	for (int i = 1; i <= 5; ++i) {
		cout << i << endl;
		testMethod(readFile("test\\timetest\\5x5_" + to_string(i) + ".jap"), Time::nano, { Algo::brute1, Algo::brute2, Algo::FSM, Algo::genetic });
	}

	cout << "\nTest 10x10:\n";
	for (int i = 1; i <= 5; ++i) {
		cout << i << endl;
		testMethod(readFile("test\\timetest\\10x10_" + to_string(i) + ".jap"), Time::milli, { Algo::FSM, Algo::brute2 });
	}

	system("pause");
	return 0;
}