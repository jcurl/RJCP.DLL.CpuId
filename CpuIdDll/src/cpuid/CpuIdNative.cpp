#include "stdafx.h"

#include "cpuid/CpuIdNative.h"

#include "cpuiddll.h"
#include "cpuidx.h"

#include <memory>

namespace rjcp {
namespace diagnostics {
namespace cpuid {

CpuIdNative::CpuIdNative() noexcept { }

CpuIdNative::CpuIdNative(unsigned int cpunum) noexcept : thread_(native::win32::Thread(cpunum)) { }

auto CpuIdNative::GetCpuId(std::uint32_t eax, std::uint32_t ecx) const noexcept -> const std::optional<CpuIdRegister> {
  cpuidinfo info{
      eax,
      ecx,
  };

  // This should run on the core that was configured in the constructor.
  if (cpuidl(eax, ecx, &info.peax, &info.pebx, &info.pecx, &info.pedx)) {
    return std::nullopt;
  }

  return CpuIdRegister(eax, ecx, info.peax, info.pebx, info.pecx, info.pedx);
}

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
