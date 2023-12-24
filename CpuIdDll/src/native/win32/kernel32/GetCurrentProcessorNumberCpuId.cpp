#include "stdafx.h"

#include "native/win32/kernel32/GetCurrentProcessorNumberCpuId.h"

#include "cpuid/cpuidx.h"

#include <intrin.h>

#include <thread>

namespace rjcp {
namespace native {
namespace win32 {
namespace kernel32 {

GetCurrentProcessorNumberCpuId::GetCurrentProcessorNumberCpuId() {
  HANDLE current_thread = GetCurrentThread();
  DWORD_PTR old_affinity{};

  // We expect this to only be called on Windows XP, so there shouldn't be more than 32 threads. Further, we expect that
  // the ACPI IDs will therefore be also not more than 256, and thus we can use CPUID.01h.EBX[31:24] and not have to use
  // the 32-bit APICID.
  for (unsigned int cpunum = 0; cpunum < std::thread::hardware_concurrency(); cpunum++) {
    DWORD_PTR new_mask = (DWORD_PTR)((DWORD_PTR)1 << cpunum);  // C4334 should explicitly cast
    DWORD_PTR affinity = SetThreadAffinityMask(current_thread, new_mask);
    if (old_affinity == 0) old_affinity = affinity;

    DWORD eax{}, ebx{}, ecx{}, edx{};
    cpuidl(1, 0, &eax, &ebx, &ecx, &edx);
    uint8_t apic = (ebx >> 24) & 0xFF;
    acpi_[apic] = cpunum;  // NOLINT(cppcoreguidelines-pro-bounds-constant-array-index)
  }

  if (old_affinity != 0) SetThreadAffinityMask(current_thread, old_affinity);
}

auto GetCurrentProcessorNumberCpuId::GetCurrentProcessorNumber() -> DWORD {
  DWORD eax{}, ebx{}, ecx{}, edx{};
  cpuidl(1, 0, &eax, &ebx, &ecx, &edx);
  uint8_t apic = (ebx >> 24) & 0xFF;
  return acpi_[apic];  // NOLINT(cppcoreguidelines-pro-bounds-constant-array-index)
}

}  // namespace kernel32
}  // namespace win32
}  // namespace native
}  // namespace rjcp
