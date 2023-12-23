#include "cpuid/CpuIdDumpGeneric.h"

#include "cpuid/CpuIdDumpUtils.h"

#include <cstdint>

namespace rjcp {
namespace diagnostics {
namespace cpuid {

namespace {

auto CpuIdDumpNormal(ICpuId& cpuid, std::vector<CpuIdRegister>& registers) -> void {
  utils::CpuIdDump<0x00000000>(cpuid, registers);
}

auto CpuIdDumpExtended(ICpuId& cpuid, std::vector<CpuIdRegister>& registers) -> void {
  utils::CpuIdDump<0x80000000>(cpuid, registers);
}

}  // namespace

auto CpuIdDumpGeneric::CpuIdDump(ICpuId& cpuid) -> std::vector<CpuIdRegister> {
  std::vector<CpuIdRegister> registers{};
  CpuIdDumpNormal(cpuid, registers);
  CpuIdDumpExtended(cpuid, registers);
  return registers;
}

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
