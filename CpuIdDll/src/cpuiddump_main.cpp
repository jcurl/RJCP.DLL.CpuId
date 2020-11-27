////////////////////////////////////////////////////////////////////////////////
//
// (C) 2012-2020, Jason Curl
//
////////////////////////////////////////////////////////////////////////////////
//
// Header: cpuiddump_main.cpp
//
// Description:
//  Main entry point for logic to dump CPUID instructions
//
////////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "cpuiddll.h"
#include "cpuid.h"

/// <summary>
/// Entry point for dumping CPUID for all processors.
/// </summary>
/// <param name="info">A pointer to an array of preallocated cpuidinfo items.</param>
/// <param name="bytes">The amount of memory allocated by <paramref name="info"/>
/// <returns>
/// The number of entries filled and populated in the <paramref name="info"/> struct.
/// </returns>
/// <remarks>
/// On exit, the <paramref name="info"/> struct is populated with results of calling
/// the CPUID instruction. It will fill up to the number of <paramref name="bytes"/>
/// provided. To ensure that all elements are populated, the return value should be
/// at least one less than the maximum possible number of entries possible give by the
/// <paramref name="bytes"/> parameter.
/// </remarks>
int iddump_main(struct cpuidinfo *info, size_t bytes)
{
	if (info == NULL || bytes < 2 * sizeof(struct cpuidinfo)) return 0;

	info[0].veax = 0x00000000;
	info[0].vecx = 0x00000000;
	cpuidget(info);

	info[1].veax = 0x80000000;
	info[1].vecx = 0x00000000;
	cpuidget(info+1);

	vendor_t vendor;
	if (info[0].pebx == 0x756E6547 && info[0].pecx == 0x6C65746E && info[0].pedx == 0x49656E69) {
		vendor = VENDOR_INTEL;
	} else if (info[0].pebx == 0x68747541 && info[0].pecx == 0x444d4163 && info[0].pedx == 0x69746e65) {
		vendor = VENDOR_AMD;
	} else {
		vendor = VENDOR_UNKNOWN;
	}

	switch (vendor) {
	case VENDOR_INTEL:
		return iddump_intel(info, bytes);
	case VENDOR_AMD:
		return iddump_amd(info, bytes);
	default:
		return iddump_default(info, bytes);
	}
}
