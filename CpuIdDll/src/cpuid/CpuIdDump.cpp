#include "cpuid/CpuIdDump.h"

#include "cpuid/CpuIdDumpAuthenticAmd.h"
#include "cpuid/CpuIdDumpGeneric.h"
#include "cpuid/CpuIdDumpGenuineIntel.h"
#include "cpuid/ICpuIdDump.h"

#include <algorithm>
#include <array>
#include <memory>
#include <string>

namespace rjcp {
namespace diagnostics {
namespace cpuid {

namespace {

template<class Tv, class T, std::size_t N>
auto CopyArray(Tv value, std::array<T, N>& buffer, size_t offset) -> void {
  // Check we don't exceed bounds of the array with a static_assert or a runtime check otherwise.
  if ((offset + 1) * sizeof(Tv) >= N * sizeof(T)) {
    return;
  }

  // NOLINTBEGIN(cppcoreguidelines-pro-bounds-pointer-arithmetic)
  auto first = static_cast<const T*>(static_cast<const void*>(&value));
  auto last = static_cast<const T*>(static_cast<const void*>(&value)) + sizeof(value);
  auto dest = buffer.data() + offset * sizeof(value);
  std::copy(first, last, dest);
  // NOLINTEND(cppcoreguidelines-pro-bounds-pointer-arithmetic)
}

auto GetCpuBrand(CpuIdRegister& cpureg) noexcept -> std::string {
  if (cpureg.InEax() != 0 || cpureg.InEcx() != 0) {
    // Check that ensures we pass the correct value.
    return std::string();
  }

  std::array<char, 13> brand{};

  // We want to copy the content of the registers into 12 byte ASCII string. We don't want a std::copy or a memmove or
  // friends, as this means we need to put the register somewhere, and then copy it again.

  CopyArray(cpureg.Ebx(), brand, 0);
  CopyArray(cpureg.Edx(), brand, 1);
  CopyArray(cpureg.Ecx(), brand, 2);

  return std::string(brand.data());
}

}  // namespace

auto CpuIdDump(ICpuId& cpuid) -> std::vector<CpuIdRegister> {
  auto leaf0 = cpuid.GetCpuId(0, 0);
  if (!leaf0) return std::vector<CpuIdRegister>{};

  std::string cpu_brand = GetCpuBrand(leaf0.value());

  std::unique_ptr<ICpuIdDump> dumper;
  if (cpu_brand == "GenuineIntel") {
    dumper = std::make_unique<CpuIdDumpGenuineIntel>();
  } else if (cpu_brand == "AuthenticAMD" || cpu_brand == "AMDisbetter!") {
    dumper = std::make_unique<CpuIdDumpAuthenticAmd>();
  } else {
    dumper = std::make_unique<CpuIdDumpGeneric>();
  }

  return dumper->CpuIdDump(cpuid);
}

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
