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
/// <returns>Number of elements put into <paramref name="info"/>.</returns>
int iddump_amd(struct cpuidinfo *info, size_t bytes)
{
	int i = 2;
	int p = 1;
	int q = 0;
	int c = 0;

	while (p <= (int)(info[0].peax & 0x0FFFFFFF) && bytes >= (i + 1) * sizeof(struct cpuidinfo)) {
		switch (p) {
		case 7:
			// Structured Extended Feature Flags. 
			info[i].veax = p;
			info[i].vecx = q;
			cpuidget(info + i);

			// EAX contains the number of subleaves when ECX == 0.
			// EAX == 0 as return means no subleaves.
			if (q == 0) {
				c = info[i].peax;
			}
			if (q < c) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 13:
			info[i].veax = p;
			info[i].vecx = q;
			cpuidget(info + i);

			switch(q) {
			case 0:
			case 1:
			case 11:
				q++;
				break;
			case 2:
				q = 11;
				break;
			case 12:
				q = 62;
				break;
			case 62:
				q = 0; p++;
				break;
			}
			break;
		default:
			info[i].veax = p;
			info[i].vecx = 0;
			cpuidget(info + i);
			p++;
			break;
		}
		i++;
	}

	if (info[1].peax & 0x80000000) {
		p = 1;
		q = 0;
		while (p <= (int)(info[1].peax & 0x0FFFFFFF) && bytes >= (i + 1) * sizeof(struct cpuidinfo)) {
			switch (p) {
			case 29:
				info[i].veax = 0x80000000 + p;
				info[i].vecx = q;
				cpuidget(info + i);

				// Enumerate ECX=0, 1, 2, and only 3 or higher if SGX is set and the Type is not invalid
				if (info[i].peax & 0xF) {
					q++;
				} else {
					q = 0; p++;
				}
				break;
			default:
				info[i].veax = 0x80000000 + p;
				info[i].vecx = 0;
				cpuidget(info + i);
				p++;
				break;
			}
			i++;
		}
	}

	return i;
}
