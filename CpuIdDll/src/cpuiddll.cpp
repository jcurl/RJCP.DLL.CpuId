////////////////////////////////////////////////////////////////////////////////
//
// (C) 2012-2020, Jason Curl
//
////////////////////////////////////////////////////////////////////////////////
//
// Header: cpuiddll.cpp
//
// Description:
//  Entry points to the Windows DLL.
//
////////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "kernelfunc.h"
#include "cpuiddll.h"
#include "cpuid.h"

#define CORES (sizeof(DWORD_PTR) << 3)

CPUIDDLL_API int WINAPI cpuid(DWORD eax, DWORD ecx, LPDWORD peax, LPDWORD pebx, LPDWORD pecx, LPDWORD pedx)
{
    return cpuidl(eax, ecx, peax, pebx, pecx, pedx);
}

CPUIDDLL_API int WINAPI hascpuid()
{
    return cpuidt() != 0;
}

CPUIDDLL_API int WINAPI iddump(struct cpuidinfo *info, size_t bytes)
{
    DWORD currentProcessor = id_GetCurrentProcessorNumber();
    return iddumponcore(info, bytes, currentProcessor);
}

CPUIDDLL_API int WINAPI iddumponcore(struct cpuidinfo *info, size_t bytes, int core)
{
    // DWORD_PTR is either 32-bit or 64-bit.
    if (core < 0 || core >= CORES) {
        SetLastError(ERROR_INVALID_PARAMETER);
        return -1;
    }

    HANDLE currentThread = GetCurrentThread();
    DWORD_PTR newMask = (DWORD_PTR)((DWORD_PTR)1 << core);    // C4334 should explicitly cast
    DWORD_PTR affinity = SetThreadAffinityMask(currentThread, newMask);
    if (!affinity) return -1;

    int result = iddump_main(info, bytes);
    SetThreadAffinityMask(currentThread, affinity);
    return result;
}

CPUIDDLL_API int WINAPI iddumpall(struct cpuidinfo *info, size_t bytes)
{
    if (info == NULL || bytes < 3 * sizeof(struct cpuidinfo)) return 0;

    SYSTEM_INFO sysInfo = {0, };
    GetNativeSystemInfo(&sysInfo);

    // On 32-bit machines, can only represent 32 processors. On 64-bit machines, can represent
    // 64 processors.
    DWORD_PTR processorMask = sysInfo.dwActiveProcessorMask;

    int i = 0;
    for (int core = 0; core < CORES && processorMask; core++) {
        if (processorMask & 0x01) {
            info[i].veax = 0xFFFFFFFF;
            info[i].vecx = core;

            int c = iddumponcore(info + i + 1, bytes - (i + 1) * sizeof(struct cpuidinfo), core);
            i += c + 1;
        }

        // Get the next processor
        processorMask = processorMask >> 1;
    }

    return i;
}
