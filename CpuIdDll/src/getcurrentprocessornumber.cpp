////////////////////////////////////////////////////////////////////////////////
//
// (C) 2020, Jason Curl
//
////////////////////////////////////////////////////////////////////////////////
//
// Header: getcurrentprocessornumber.cpp
//
// Description:
//  Entry points to the Windows DLL.
//
////////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "kernelfunc.h"
#include "cpuidx.h"

typedef DWORD (WINAPI *fnGetCurrentProcessorNumber)();
static int cGetCurrentProcessorNumber = 0;
static fnGetCurrentProcessorNumber pGetCurrentProcessorNumber;

int id_GetCurrentProcessorNumber()
{
	if (!cGetCurrentProcessorNumber) {
		HMODULE kernel = GetModuleHandle(TEXT("kernel32.dll"));
		if (kernel == NULL) {
			SetLastError(ERROR_INVALID_FUNCTION);
			return -1;
		}

		pGetCurrentProcessorNumber =
			(fnGetCurrentProcessorNumber)GetProcAddress(kernel, "GetCurrentProcessorNumber");
		cGetCurrentProcessorNumber = 1;
	}

	if (pGetCurrentProcessorNumber) {
		return pGetCurrentProcessorNumber();
	} else {
		// Windows XP doesn't offer this function. Some examples use the APICID of the CPUID
		// instruction, but this is wrong, the APICID isn't necessarily a one-to-one mapping.
		// The APIC IDs could be 0, 2, 4 for CPU's 0, 1, 2. We can't second guess how the OS
		// scheduler is configured.

		// Until we find a better solution, just say we're always on the primary processor.
		return 0;
	}
}
