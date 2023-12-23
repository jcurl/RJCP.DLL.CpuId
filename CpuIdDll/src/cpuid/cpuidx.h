#ifndef RJCP_DIAGNOSTICS_CPUID_CPUIDX_H
#define RJCP_DIAGNOSTICS_CPUID_CPUIDX_H

#include "stdafx.h"

// The assembly code is written with the stdcall calling convention with no name mangling.
extern "C" {
/// <summary>
/// Execute the CPUID instruction on IA32 architectures (32-bit or 64-bit).
/// </summary>
/// <param name="eax">The CPUID leaf to call.</param>
/// <param name="ecx">The CPUID subleaf to call.</param>
/// <param name="peax">Pointer to the result.</param>
/// <param name="pebx">Pointer to the result.</param>
/// <param name="pecx">Pointer to the result.</param>
/// <param name="pedx">Pointer to the result.</param>
/// <returns>Returns zero on success</returns>
int APIENTRY cpuidl(DWORD eax, DWORD ecx, LPDWORD peak, LPDWORD pebx, LPDWORD pecx, LPDWORD pedx);

/// <summary>
/// Tests if the CPUID instruction exists and works.
/// </summary>
/// <returns>Returns non-zero if the CPUID function is supported.</returns>
int APIENTRY cpuidt();
}
#endif
