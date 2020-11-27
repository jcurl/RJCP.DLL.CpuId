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

#include "cpuiddll.h"
#include "cpuid.h"

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
	DWORD currentProcessor = GetCurrentProcessorNumber();
	return iddumponcore(info, bytes, currentProcessor);
}

CPUIDDLL_API int WINAPI iddumponcore(struct cpuidinfo *info, size_t bytes, int core)
{
	if (core < 0 || core > 64) {
		SetLastError(ERROR_INVALID_PARAMETER);
		return -1;
	}

	HANDLE currentThread = GetCurrentThread();
	DWORD_PTR newMask = 1 << core;
	DWORD_PTR affinity = SetThreadAffinityMask(currentThread, newMask);
	if (!affinity) return -1;

	int result = iddump_main(info, bytes);
	SetThreadAffinityMask(currentThread, affinity);
	return result;
}
