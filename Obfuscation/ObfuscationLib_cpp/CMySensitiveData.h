#pragma once

#include <string>

class MySensitiveData {
public:
  static constexpr const char* mySecretKey = "MySuperSecret";

  std::string PrintSecret() const {
    return std::string("The secret key is: ") + mySecretKey;
  }
};


