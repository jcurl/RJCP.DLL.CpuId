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
int iddump_default(struct cpuidinfo *info, size_t bytes)
{
	int el = 2;
	int p = 1;
	while (p <= (int)(info[0].peax & 0x7FFFFFFF) && bytes > (el + 1) * sizeof(struct cpuidinfo)) {
		info[el].veax = p;
		info[el].vecx = 0;
		cpuidget(info+el);
		p++;
		el++;
	}

	// Dump the extended values second
	p = 1;
	while (p <= (int)(info[1].peax & 0x7FFFFFFF) && bytes > (el + 1) * sizeof(struct cpuidinfo)) {
		info[el].veax = 0x80000000 + p;
		info[el].vecx = 0;
		cpuidget(info+el);
		p++;
		el++;
	}
	return el;
}
