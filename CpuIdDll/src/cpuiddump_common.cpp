////////////////////////////////////////////////////////////////////////////////
//
// (C) 2012-2020, Jason Curl
//
////////////////////////////////////////////////////////////////////////////////
//
// Header: cpuiddump_common.cpp
//
// Description:
//  Some common routines for dumping CPUID data.
//
////////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "cpuiddll.h"
#include "cpuid.h"

/// <summary>
/// Dump a regional block, where the base contains the first register.
/// </summary>
/// <param name="basefunction">The function base for CPUID.</param>
/// <param name="base">The base register. Set to NULL to retrieve.</param>
/// <param name="info">Start to where to dump the results.</param>
/// <param name="bytes">The maximum amount of bytes that can be written.</param>
/// <returns>The number of entries read</returns>
int iddump_region(int basefunction, struct cpuidinfo *base, struct cpuidinfo *info, size_t bytes)
{
	if (bytes < sizeof(struct cpuidinfo)) return 0;

	int i = 0;
	if (base == NULL) {
		info[0].veax = basefunction;
		info[0].vecx = 0;
		cpuidget(info);
		base = info;
		i++;
	} else {
		// Ensure that if the user did provide the base, that it has the correct function.
		if (base->veax != basefunction) return 0;
		if (base->vecx != 0) return 0;
	}

	int p = 1;
	if (basefunction == 0 || base->peax & basefunction) {
		int c = (int)(base->peax & 0x0FFFFFFF);
		while (p <= c && bytes >= (i + 1) * sizeof(struct cpuidinfo)) {
			info[i].veax = basefunction + p;
			info[i].vecx = 0;
			cpuidget(info+i);
			p++;
			i++;
		}
	}

	return i;
}

/// <summary>
/// Dump the hypervisor block.
/// </summary>
/// <param name="base">The base containing [0]=0h, [2]=01h results.</param>
/// <param name="info">Start to where to dump the results.</param>
/// <param name="bytes">The maximum amount of bytes that can be written.</param>
/// <returns>The number of entries rea.d</returns>
int iddump_hypervisor(struct cpuidinfo *base, struct cpuidinfo *info, size_t bytes)
{
	if (base == NULL) return 0;
	if (base[0].veax != 0 && base[2].veax != 1) return 0;
	if (base[0].peax < 1) return 0;

	// Check that the Hypervisor bit is set. If not, the block doesn't need to be queried.
	if (!(base[2].pecx & 0x80000000)) return 0;

	return iddump_region(0x40000000, NULL, info, bytes);
}
