# CPU ID <!-- omit in toc -->

This is a small proof of concept library that obtains CPU information using
native CPU instructions on x86 compatible processors, using the `cpuid`
instruction.

It can also read a specific XML format instead of querying the local processor
for offline parsing.

- [1. Features](#1-features)
- [2. Project Organization](#2-project-organization)
  - [2.1. The Windows Tool](#21-the-windows-tool)
- [3. Programming Notes](#3-programming-notes)
  - [3.1. Supported Processors and Features](#31-supported-processors-and-features)
    - [3.1.1. Intel](#311-intel)
    - [3.1.2. AMD](#312-amd)
- [4. References](#4-references)

## 1. Features

This program is limited to querying Intel and AMD processors, as of November
2020.

It only shows the raw register dumps of the CPU the program is running on and a
list of the processor features. See [Supported Processors and
Features](#31-supported-processors-and-features) for more information on what is
supported.

## 2. Project Organization

There are 4 projects:

- A C++ library compiled with Visual Studio 2012 IDE with Windows XP
  compatibility
- A .NET Assembly reusable in other projects
- CPUID for Windows
- CPUID Console to dump CPUID data to an XML file

### 2.1. The Windows Tool

Just run the Windows application and you'll be presented a very basic view. The
first tab shows the basic CPU identification features. The next tab shows a
register dump of the CPUID instruction for a random core in the system. THe
final tab shows a list of processor features supported (this list may be
incomplete).

## 3. Programming Notes

The library supports executing a dump and setting the affinity of the current
thread to the current logical processor. You should generally however set the
affinity of the current thread to the processor you wish to detect before
executing to obtain consistent results.

### 3.1. Supported Processors and Features

#### 3.1.1. Intel

The Intel processor is identified as `GenuineIntel`. Functionality supported,
based on [1] and [2].

- Register dumps as given in [2].
- Function 01h
  - Processor Signature
  - Family
  - Model
  - Stepping
  - Processor Type
- Function 80000002-4h
  - Brand String partial
- Function 07h
  - Extra features obtained from [3], which is also reflected in [4].

#### 3.1.2. AMD

The AMD processor is identified as `AuthenticAMD`

Functionality supported, based on [6].

- Register dumps as given in [6]
- Function 01h
  - Processor Signature
  - Family
  - Model
  - Stepping
- Function 80000002-4h
  - Brand strings for Family Fh, 10h, 11h, 12h, 14h
- Function 07h
  - Features as defined in [6]
- Function 80000001h as defined in [6] and [7]
  - Additional feature flags from [7].
- Function 80000008h as defined in [6] and [7]
  - Additional feature flags from [7].

## 4. References

This section contains references to documentation used to collate CPUID
information.

- [1] Intel, _Intel(R) Processor Identification and the CPUID Instruction,
  Application Note 485_, Order Number: 241618-039, May 2012
- [2] Intel, _Intel(R) 64 and IA-32 Architectures Software Developer's Manual,
  Combined Volumes: 1, 2A, 2B, 2C, 2D, 3A, 3B, 3C, 3D and 4_, Order Number:
  325462-072US, May 2020
- [3] Intel, _Intel(R) Architecture Instruction Set Extensions and Future
  Features Programming Reference_, Document Number: 319433-038, March 2020
- [4] Wikipedia contributors. _CPUID_. Wikipedia, The Free Encyclopedia.
  November 19, 2020, 17:05 UTC. Available at:
  https://en.wikipedia.org/w/index.php?title=CPUID&oldid=989549561. Accessed
  November 29, 2020.
- [5] AMD, _CPUID Specification_, Publication Number: 25481, Revision 2.34,
  September 2010
- [6] AMD, _AMD64 Architecture Programmers Manual, Volume 3: General-Purpose and
  System Instructions_, Publication Number: 24594, Revision 3.31, October 2020
- [7] Christian Ludloff, Sandpile.org. Available at:
  https://www.sandpile.org/x86/cpuid.htm. Accessed November 29, 2020, 16:54 UTC
