#include "cpuid/CpuIdRegister.h"

namespace rjcp {
namespace diagnostics {
namespace cpuid {

constexpr std::uint32_t Invalid = 0xFFFFFFFF;

CpuIdRegister::CpuIdRegister() noexcept
    : ineax_{Invalid}, inecx_{Invalid}, eax_{0}, ebx_{0}, ecx_{0}, edx_{0}, isvalid_{false} { }

CpuIdRegister::CpuIdRegister(std::uint32_t ieax, std::uint32_t iecx, std::uint32_t eax, std::uint32_t ebx,
                             std::uint32_t ecx, std::uint32_t edx) noexcept
    : ineax_{ieax}, inecx_{iecx}, eax_{eax}, ebx_{ebx}, ecx_{ecx}, edx_{edx}, isvalid_{true} { }

auto CpuIdRegister::InEax() const noexcept -> std::uint32_t {
  return ineax_;
}

auto CpuIdRegister::InEcx() const noexcept -> std::uint32_t {
  return inecx_;
}

auto CpuIdRegister::Eax() const noexcept -> std::uint32_t {
  return eax_;
}

auto CpuIdRegister::Ebx() const noexcept -> std::uint32_t {
  return ebx_;
}

auto CpuIdRegister::Ecx() const noexcept -> std::uint32_t {
  return ecx_;
}

auto CpuIdRegister::Edx() const noexcept -> std::uint32_t {
  return edx_;
}

auto CpuIdRegister::IsValid() const noexcept -> bool {
  return isvalid_;
}

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
