////////////////////////////////////////////////////////////////////////////////
//
// (C) 2012-2020, Jason Curl
//
////////////////////////////////////////////////////////////////////////////////
//
// Header: cpuiddump_intel.cpp
//
// Description:
//  Dump registers according to Intel documentation. Reserved registers are
//  still dumped.
//
////////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "cpuiddll.h"
#include "cpuid.h"

/// <summary>
/// Dump registers according to the Intel specifications.
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
int iddump_intel(struct cpuidinfo *info, size_t bytes)
{
	int el = 2;
	int p = 1;
	int q = 0;
	int c = 0;

	while (p <= (int)(info[0].peax & 0x7FFFFFFF) && bytes >= (el + 1) * sizeof(struct cpuidinfo)) {
		switch (p) {
		case 2:
			// Cache Descriptors
			info[el].veax = p;
			info[el].vecx = 0;
			cpuidget(info+el);

			// Intel documentation AP485, May 2012, says that we should check the lower
			// 8 bits of EAX to know how often to call this function.
			//
			// Newer documentation in Volume 2, May 2020 says that the first byte is
			// always 0x01 and should be ignored. Both documents are correct.
			if (q == 0) c = info[el].peax & 0xFF;
			if (c != 0 && q < c - 1) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 4:
			// Deterministic Cache Parameters
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (info[el].peax & 0x1F) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 7:
			// Structured Extended Feature Flags. 
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			// EAX contains the number of subleaves when ECX == 0.
			// EAX == 0 as return means no subleaves
			if (q == 0) c = info[el].peax;
			if (q < c) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 11:
			// x2APIC features
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (info[el].peax || info[el].pebx) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 13:
			// Processor Extended State
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (q < 2 || (info[el].peax || info[el].pebx || info[el].pecx || info[el].pedx)) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 15:
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (q < 1) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 16:
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (q < 3) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 18:
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (q < 2 || (info[el].peax & 0x03)) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 20:
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (q < 1) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 23:
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (q < 3) {
				q++;
			} else {
				q = 0; p++;
			}
			break;
		case 24:
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (q == 0) c = info[el].peax;
			if (q < c) {
				q++;
			} else {
				p++; q = 0;
			}
			break;
		case 31:
			info[el].veax = p;
			info[el].vecx = q;
			cpuidget(info+el);

			if (info[el].pebx & 0xFFFF) {
				q++;
			} else {
				p++; q = 0;
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
	if (info[1].peax & 0x80000000) {
		p = 1; c = (int)(info[1].peax & 0x7FFFFFFF);
		while (p <= c && bytes >= (el + 1) * sizeof(struct cpuidinfo)) {
			info[el].veax = 0x80000000 + p;
			info[el].vecx = 0;
			cpuidget(info+el);
			p++;
			el++;
		}
	}

	return el;
}
