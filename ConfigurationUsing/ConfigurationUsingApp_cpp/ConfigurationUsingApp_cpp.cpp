// ConfigurationUsingApp_cpp.cpp : This file contains the 'main' function. Program execution begins and ends there.
//


#include <windows.h>
#include <iostream>
#include <string>


std::wstring GetEnvVarW(const std::wstring& name) {
  DWORD size = GetEnvironmentVariableW(name.c_str(), nullptr, 0);
  if (size == 0) {
    return L""; // Variable non trouvée
  }

  std::wstring value(size, L'\0');
  GetEnvironmentVariableW(name.c_str(), &value[0], size);
  value.pop_back(); // Supprimer le caractère nul final
  return value;
}

int main() {
  std::wstring message = GetEnvVarW(L"AppConfig__Message");
  if (!message.empty()) {
    std::wcout << L"AppConfig__Message = " << message << std::endl;
  }
  else {
    std::wcout << L"Environment variable not found." << std::endl;
  }

  return 0;
}


// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
