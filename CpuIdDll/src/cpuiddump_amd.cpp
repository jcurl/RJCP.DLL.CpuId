////////////////////////////////////////////////////////////////////////////////
//
// (C) 2012-2020, Jason Curl
//
////////////////////////////////////////////////////////////////////////////////
//
// Header: cpuiddump_amd.cpp
//
// Description:
//  Dump registers according to AMD documentation. Reserved registers are
//  still dumped.
//
////////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "cpuiddll.h"
#include "cpuid.h"

/// <summary>
/// Dump registers according to the AMD specifications.
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
int iddump_amd(struct cpuidinfo *info, size_t bytes)
{
	int el = 2;
	int p = 1;
	int q = 0;
	while (p <= (int)(info[0].peax & 0x7FFFFFFF) && bytes >= (el + 1) * sizeof(struct cpuidinfo)) {
		switch (p) {
		case 13:
			switch(q) {
			case 0:
				info[el].veax = p;
				info[el].vecx = 0;
				cpuidget(info+el);
				q++;
				break;
			case 1:
				info[el].veax = p;
				info[el].vecx = 2;
				cpuidget(info+el);
				q++;
				break;
			case 2:
				info[el].veax = p;
				info[el].vecx = 62;
				cpuidget(info+el);
				q = 0; p++;
				break;
			}
			break;
		default:
			info[el].veax = p;
			info[el].vecx = 0;
			cpuidget(info+el);
			p++;
			break;
		}
		el++;
	}

	// Dump the extended values second
	p = 1;
	if (info[1].peax & 0x80000000) {
		while (p <= (int)(info[1].peax & 0x7FFFFFFF) && bytes >= (el + 1) * sizeof(struct cpuidinfo)) {
			info[el].veax = 0x80000000 + p;
			info[el].vecx = 0;
			cpuidget(info+el);
			p++;
			el++;
		}
	}

	return el;
}
