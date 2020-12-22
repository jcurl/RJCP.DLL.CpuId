////////////////////////////////////////////////////////////////////////////////
//
// (C) 2012-2020, Jason Curl
//
////////////////////////////////////////////////////////////////////////////////
//
// Header: cpuidx.h
//
// Description:
//  Defines signatures for entry points to the assembly code to execute the
//  CPUID instruction. This is the same for 32-bit and 64-bit
//
////////////////////////////////////////////////////////////////////////////////

#ifndef _PRV_CPUIDX_H
#define _PRV_CPUIDX_H

#include "stdafx.h"

// The assembly code is written with the stdcall calling convention.
extern "C" {

/// <summary>
/// Execute the CPUID instruction on IA32 architectures (32-bit or 64-bit).
/// </summary>
/// <param name='eax'>The CPUID leaf to call.</param>
/// <param name='ecx'>The CPUID subleaf to call.</param>
/// <param name='peax'>Pointer to the result.</param>
/// <param name='pebx'>Pointer to the result.</param>
/// <param name='pecx'>Pointer to the result.</param>
/// <param name='pedx'>Pointer to the result.</param>
/// <returns>Returns zero on success</returns>
int __stdcall cpuidl(DWORD eax, DWORD ecx, LPDWORD peax, LPDWORD pebx, LPDWORD pecx, LPDWORD pedx);

/// <summary>
/// Tests if the CPUID instruction exists and works.
/// </summary>
/// <returns>Returns non-zero if the CPUID function is supported.</returns>
int __stdcall cpuidt();
}

#define cpuidget(info) cpuidl((info)->veax, (info)->vecx, &((info)->peax), &((info)->pebx), &((info)->pecx), &((info)->pedx))

#endif