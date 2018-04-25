/*******************************************************************************
* Description:
*   Oh snaps, what up!? So, this program cleans up the QtSpim log file to the
*   proper format that your simulation should be reading. It outputs the result
*   as a new file.
*
* Compilation:
*   g++ clean.cpp -o clean
*
* Usage:
*   The program expects 2 inputs from the command line, which is the name of the
*   input file and the name of the output file. For example,
*   ./clean logfile.txt cleanoutput.txt
*******************************************************************************/
#include <fstream>
#include <iostream>
#include <string>

using namespace std;

int main(int argc, char** argv) {
  // Ensure there is at least 2 command line arguments
  if (argc < 3) {
    cerr << "Error: need 2 arguments containing the input and output filenames."
         << endl;
    return 1;
  }

  // Open the log file for reading and a hex file for writing
  ifstream ifs;
  ofstream ofs;
  ifs.open(argv[1]);
  if (ifs.fail()) {
    cerr << "Error: unable to open specified file." << endl;
    return 1;
  }
  ofs.open(argv[2]);
  if (ofs.fail()) {
    cerr << "Error: unable to open output file." << endl;
    return 1;
  }

  // Skip the first 10 lines (inserted from MIPS)
  string str;
  for (int i = 0; i < 10; i++) {
    getline(ifs, str);
  }

  // Read the rest of the log file line-by-line
  bool flag = false;
  long mem = 0x10010000;
  while (!ifs.eof()) {
    if (!flag) {
      getline(ifs, str);
    }
    if (str == "") {
      while (!ifs.eof()) {
        getline(ifs, str);
        if (str == "User data segment [10000000]..[10040000]") {
          break;
        }
      }
      getline(ifs, str);
      ifs >> str;
      flag = true;
      ofs << "DATA SEGMENT\n";
    }

    if (!flag) {
      ofs << "0x" << str.substr(11, 8) << '\n';
    }
    else {
      if (str.find("..") != string::npos) {
        break;
      }
      for (int i = 0; i < 4; i++, mem += 4) {
        ifs >> str;
        if (str == "00000000") {
          break;
        }
        ofs << "0x" << hex << mem << " 0x" << str << '\n';
      }
      if (str == "00000000") {
        break;
      }
      getline(ifs, str);
      ifs >> str;
    }
  }

  // Close both files
  ifs.close();
  ofs.close();

  // Done
  return 0;
}
