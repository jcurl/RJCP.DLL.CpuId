#include "cpuid/CpuIdDumpAuthenticAmd.h"

#include "cpuid/CpuIdDumpUtils.h"

namespace rjcp {
namespace diagnostics {
namespace cpuid {

namespace {
auto CpuIdDumpNormal(ICpuId& cpuid, std::vector<CpuIdRegister>& registers) -> void {
  std::uint32_t leaf{};
  std::uint32_t maxleaf{};

  while (true) {
    auto cpuidreg = cpuid.GetCpuId(leaf, 0);
    if (cpuidreg) {
      registers.push_back(*cpuidreg);
      switch (leaf) {
        case 0x00: {
          // Maximum number of leaves and branding.
          maxleaf = cpuidreg->Eax();
          break;
        }
        case 0x07: {
          // Structured Extended Feature Flags.
          std::uint32_t subleaves = cpuidreg->Eax();
          for (std::uint32_t subleaf = 1; subleaf <= subleaves; subleaf++) {
            cpuidreg = cpuid.GetCpuId(leaf, subleaf);
            if (!cpuidreg) break;
            registers.push_back(*cpuidreg);
          }
          break;
        }
        case 0x0D: {
          // Processor Extended State
          for (std::uint32_t subleaf = 1; subleaf < 64; subleaf++) {
            cpuidreg = cpuid.GetCpuId(leaf, subleaf);
            if (cpuidreg &&
                (subleaf < 2 || (cpuidreg->Eax() || cpuidreg->Ebx() || cpuidreg->Ecx() || cpuidreg->Edx()))) {
              // Store this entry, as it's interesting.
              registers.push_back(*cpuidreg);
            }
          }
          break;
        }
        default:
          // We've already added the element to the vector.
          break;
      }
    }

    // If we couldn't get CPUID.0h, then `maxleaf` is zero, and we return with no data.
    if (leaf == maxleaf) return;
    leaf++;
  }
}

auto CpuIdDumpExtended(ICpuId& cpuid, std::vector<CpuIdRegister>& registers) -> void {
  std::uint32_t leaf{0x80000000};
  std::uint32_t maxleaf{};

  while (true) {
    auto cpuidreg = cpuid.GetCpuId(leaf, 0);
    if (cpuidreg) {
      registers.push_back(*cpuidreg);
      switch (leaf) {
        case 0x80000000: {
          // Maximum number of leaves and branding.
          maxleaf = cpuidreg->Eax();
          break;
        }
        case 0x8000001D: {
          std::uint32_t subleaf = 1;
          while (subleaf <= 0xFF && (cpuidreg->Eax() & 0x1F)) {
            cpuidreg = cpuid.GetCpuId(leaf, subleaf);
            if (!cpuidreg) break;
            registers.push_back(*cpuidreg);
            subleaf++;
          }
          break;
        }
        case 0x80000020: {
          for (std::uint32_t subleaf : {1, 2, 3, 5}) {
            cpuidreg = cpuid.GetCpuId(leaf, subleaf);
            if (!cpuidreg) break;
            registers.push_back(*cpuidreg);
          }
          break;
        }
        case 0x80000026: {
          std::uint32_t subleaf = 1;
          while (subleaf <= 0xFF && (cpuidreg->Ecx() & 0xFF00)) {
            cpuidreg = cpuid.GetCpuId(leaf, subleaf);
            if (!cpuidreg) break;
            registers.push_back(*cpuidreg);
            subleaf++;
          }
          break;
        }
        default:
          // We've already added the element to the vector.
          break;
      }
    }

    // If we couldn't get CPUID.0h, then `maxleaf` is zero, and we return with no data.
    if (leaf == maxleaf) return;
    leaf++;
  }
}

auto CpuIdDumpHyperVisor(ICpuId& cpuid, std::vector<CpuIdRegister>& registers) -> void {
  utils::CpuIdDump<0x40000000>(cpuid, registers);
}

}  // namespace

auto CpuIdDumpAuthenticAmd::CpuIdDump(ICpuId& cpuid) -> std::vector<CpuIdRegister> {
  std::vector<CpuIdRegister> registers{};
  CpuIdDumpNormal(cpuid, registers);

  if (registers.size() > 1) {
    // We know where to find the HYPERVISOR bit if it is set.
    if (registers[1].InEax() == 0x00000001 && (registers[1].Ecx() & 0x80000000)) {
      CpuIdDumpHyperVisor(cpuid, registers);
    }
  }
  CpuIdDumpExtended(cpuid, registers);
  return registers;
}

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
