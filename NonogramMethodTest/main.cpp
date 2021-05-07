#include "profile.h"
#include "test_brute_force1.h"
#include "test_brute_force2.h"
#include "test_FSMMethod.h"
#include "test_genetic_algorithm.h"

#include <iostream>
#include <fstream>

using namespace std;

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
	system("pause");
	return 0;
}