#include "stdafx.h"

#include "cpuiddll.h"

#include "cpuid/CpuIdDump.h"
#include "cpuid/cpuidx.h"
#include "globalstate.h"

using namespace rjcp::diagnostics;

CPUIDDLL_API int APIENTRY hascpuid() {
  return cpuidt();
}

CPUIDDLL_API int APIENTRY cpuid(DWORD eax, DWORD ecx, LPDWORD peax, LPDWORD pebx, LPDWORD pecx, LPDWORD pedx) {
  if (!global->GetCpuId()) {
    SetLastError(ERROR_INVALID_OPERATION);
    return -1;
  }

  auto cpuidr = global->GetCpuId()->GetCpuId(eax, ecx);
  if (!cpuidr) {
    SetLastError(ERROR_INVALID_OPERATION);
    return -1;
  }

  cpuid::CpuIdRegister reg = *cpuidr;
  *peax = reg.Eax();
  *pebx = reg.Ebx();
  *pecx = reg.Ecx();
  *pedx = reg.Edx();
  return 0;
}

CPUIDDLL_API int APIENTRY iddump(struct cpuidinfo* info, size_t bytes) {
  if (info == nullptr) {
    SetLastError(ERROR_INVALID_PARAMETER);
    return -1;
  }

  auto cpuidq = global->GetCpuId();
  if (!cpuidq) {
    SetLastError(ERROR_INVALID_OPERATION);
    return -1;
  }
  auto results = cpuid::CpuIdDump(*cpuidq);

  unsigned int count{};
  unsigned int elements{static_cast<unsigned int>(bytes / sizeof(struct cpuidinfo))};
  for (auto& element : results) {
    if (count >= elements) return static_cast<int>(count);

    // NOLINTBEGIN(cppcoreguidelines-pro-bounds-pointer-arithmetic)
    info[count].veax = element.InEax();
    info[count].vecx = element.InEcx();
    info[count].peax = element.Eax();
    info[count].pebx = element.Ebx();
    info[count].pecx = element.Ecx();
    info[count].pedx = element.Edx();
    // NOLINTEND(cppcoreguidelines-pro-bounds-pointer-arithmetic)
    count++;
  }

  return static_cast<int>(count);
}

CPUIDDLL_API int APIENTRY iddumponcore(struct cpuidinfo* info, size_t bytes, int core) {
  if (core < 0 || core >= 64) {
    SetLastError(ERROR_INVALID_PARAMETER);
    return -1;
  }

  if (info == nullptr) {
    SetLastError(ERROR_INVALID_PARAMETER);
    return -1;
  }

  auto cpuidq = global->GetCpuId(core);
  if (!cpuidq) {
    SetLastError(ERROR_INVALID_OPERATION);
    return -1;
  }
  auto results = cpuid::CpuIdDump(*cpuidq);

  unsigned int count{};
  unsigned int elements{static_cast<unsigned int>(bytes / sizeof(struct cpuidinfo))};
  for (auto& element : results) {
    if (count >= elements) return static_cast<int>(count);

    // NOLINTBEGIN(cppcoreguidelines-pro-bounds-pointer-arithmetic)
    info[count].veax = element.InEax();
    info[count].vecx = element.InEcx();
    info[count].peax = element.Eax();
    info[count].pebx = element.Ebx();
    info[count].pecx = element.Ecx();
    info[count].pedx = element.Edx();
    // NOLINTEND(cppcoreguidelines-pro-bounds-pointer-arithmetic)
    count++;
  }

  return static_cast<int>(count);
}

CPUIDDLL_API int APIENTRY iddumpall(struct cpuidinfo* info, size_t bytes) {
  if (info == nullptr) {
    SetLastError(ERROR_INVALID_PARAMETER);
    return -1;
  }

  unsigned int count{};
  unsigned int elements{static_cast<unsigned int>(bytes / sizeof(struct cpuidinfo))};
  for (unsigned int i = 0; i < global->GetCpuCount(); i++) {
    auto cpuidq = global->GetCpuId(i);
    if (!cpuidq) {
      SetLastError(ERROR_INVALID_OPERATION);
      return -1;
    }
    auto results = cpuid::CpuIdDump(*cpuidq);

    // Add a "header" that contains the core number.
    if (elements == 0 || count >= elements - 1) return static_cast<int>(count);
    // NOLINTBEGIN(cppcoreguidelines-pro-bounds-pointer-arithmetic)
    info[count].veax = 0xFFFFFFFF;
    info[count].vecx = i;
    info[count].peax = 0;
    info[count].pebx = 0;
    info[count].pecx = 0;
    info[count].pedx = 0;
    // NOLINTEND(cppcoreguidelines-pro-bounds-pointer-arithmetic)
    count++;

    for (auto& element : results) {
      if (count >= elements) return static_cast<int>(count);

      // NOLINTBEGIN(cppcoreguidelines-pro-bounds-pointer-arithmetic)
      info[count].veax = element.InEax();
      info[count].vecx = element.InEcx();
      info[count].peax = element.Eax();
      info[count].pebx = element.Ebx();
      info[count].pecx = element.Ecx();
      info[count].pedx = element.Edx();
      // NOLINTEND(cppcoreguidelines-pro-bounds-pointer-arithmetic)
      count++;
    }
  }

  return static_cast<int>(count);
}
