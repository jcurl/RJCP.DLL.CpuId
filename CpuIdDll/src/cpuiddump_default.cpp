////////////////////////////////////////////////////////////////////////////////
//
// (C) 2012-2020, Jason Curl
//
////////////////////////////////////////////////////////////////////////////////
//
// Header: cpuiddump_default.cpp
//
// Description:
//  Do a generic dump of registers
//
////////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "cpuiddll.h"
#include "cpuid.h"

/// <summary>
/// Dump registers in a default manner for unknown CPUs.
/// </summary>
/// <param name="info">The memory buffer to write the results to.</param>
/// <param name="bytes">The total number of bytes allocated for <paramref name="info"/></param>
/// <remarks>
/// When dumping, the first two entries are expected to have:
/// <list>
/// <item>0: The CPUID[00h] results.</item>
/// <item>1: The CPUID[80000000h] results.</item>
/// </list>
/// </remarks>
/// <returns>Number of elements put into <paramref name="info"/>.</returns>
int iddump_default(struct cpuidinfo *info, size_t bytes)
{
	int i = 2;

	i += iddump_region(0x00000000, info    , info + i, bytes - i * sizeof(struct cpuidinfo));
	i += iddump_region(0x80000000, info + 1, info + i, bytes - i * sizeof(struct cpuidinfo));
	return i;
}
