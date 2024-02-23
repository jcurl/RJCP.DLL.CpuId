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
  - [3.1. NuGet Package](#31-nuget-package)
  - [3.2. Supported Processors and Features](#32-supported-processors-and-features)
    - [3.2.1. Intel](#321-intel)
    - [3.2.2. AMD](#322-amd)
- [4. Further Help](#4-further-help)
- [5. References](#5-references)
- [6. Release History](#6-release-history)
  - [6.1. Version 0.8.1](#61-version-081)
  - [6.2. Version 0.8.0](#62-version-080)

## 1. Features

This program is limited to querying Intel and AMD processors, as of November
2020.

It performs a register dump, or can load a previously saved register dump, and
interpret:

- the features for AMD/Intel (see [Supported Processors and
  Features](#31-supported-processors-and-features) for more information)
- the brand string/description
- CPU and Cache and Topology

It is written for Windows XP SP3, Windows Server 2003 and later, using .NET 4.0,
to try and target as many CPUs as possible.

## 2. Project Organization

There are 4 projects:

- A C++ library compiled with Visual Studio 2022 IDE with Windows XP
  compatibility
- A .NET Assembly reusable in other projects
- CPUID for Windows
- CPUID Console to dump CPUID data to an XML file

### 2.1. The Windows Tool

Just run the Windows application and you'll be presented a very basic view. The
first tab shows the basic CPU identification features. It shows a tree for each
CPUID node found and a break down for some of the CPUID leaves.

## 3. Programming Notes

The library supports executing a dump and setting the affinity of the current
thread to the current logical processor. You should generally however set the
affinity of the current thread to the processor you wish to detect before
executing to obtain consistent results.

### 3.1. NuGet Package

Include the NuGet package `RJCP.Diagnostics.CpuId` in all projects and dependent
projects if not including the sources direct. The NuGet package must be included
in all dependent projects in addition so that the native libraries are made
available (this is a limitation of NuGet).

### 3.2. Supported Processors and Features

Features are not shared between processors, but are derived directly from the
manufacturers documentation.

#### 3.2.1. Intel

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
  - Brand String partial, defined from [1]
- Features from Function 01h, 07h, defined in [3], [4].
- Cache Topology in 04h, defined in [3].
- CPU Topology, defined in [3].

#### 3.2.2. AMD

The AMD processor is identified as `AuthenticAMD`

Functionality supported, based on [6].

- Register dumps as given in [6]
- Function 01h
  - Processor Signature
  - Family
  - Model
  - Stepping
- Function 80000002-4h
  - Brand strings for Family Fh [11] [12], 10h [13], 11h [14], 12h [15], 14h
    [16]
- Features from function 01h, 07h, 80000001h, 80000008h, 8000001Fh, defined in
  [6], [6a], [7], [8], [9], [10].
- Cache Topology in function 80000005h, 80000006h, 80000019h, 8000001Dh, defined
  in [6].
- CPU Topology in function 8000001Eh, defined in [6].

## 4. Further Help

It is appreciated if CPUID dumps are provided. Most online resources no longer
provide the raw CPUID data, which is essential to test this library. Many bugs
could be found by the large amount of CPUID dumps found by
[http://users.atw.hu/instlatx64/](InstLatX64) for example.

## 5. References

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
- [6a] AMD, _AMD64 Architecture Programmers Manual, Volume 3: General-Purpose and
  System Instructions_, Publication Number: 24594, Revision 3.20, May 2013
- [7] Christian Ludloff, Sandpile.org. Available at:
  https://www.sandpile.org/x86/cpuid.htm. Accessed November 29, 2020, 16:54 UTC
- [8] AMD, _Reference PPR for AMD Family 17h Model 18h, Rev B1 Processors_,
  Publication Number: 55570-B1, Revision 3.15, July 9th, 2020
- [9] AMD, _BIOS and Kernel Developer's Guide (BKDG) for AMD Family 15h Models
  10h-1Fh Processors_, Publication Number: 42300, Revision 3.12, July 14th, 2015
- [10] AMD, _Processor Programming Reference for AMD Family 17h Model 60h,
  Revision A1 Processors_, Publication Number: 55922, Revision 3.06, September
  28th, 2020
- [11] AMD, _Revision Guide for AMD Athlon(tm) 64 and AMD Opteron(tm)
  Processors_, Publication Number: 25759, Revision 3.79, July 2009
- [12] AMD, _Revision Guide for AMD NPT Family 0Fh Processors_, Publication
  Number: 33610, Revision 3.48, December 2011
- [13] AMD, _Revision Guide for AMD Family 10h Processors_, Publication Number:
  41322, Revision 3.84, August 2011
- [14] AMD, _Revision Guide for AMD Family 11h Processors_, Publication Number:
  41788, Revision 3.00, July 2008
- [15] AMD, _Revision Guide for AMD Family 12h Processors_, Publication Number:
  44739, Revision 3.10, March 2012
- [16] AMD, _Revision Guide for AMD Family 14h Model 00h-0Fh Processors_,
  Publication Number: 47534, Revision 3.18, February 2013
- [17] AMD, _AMD Processor Recognition Application Note_, Publication Number:
  20734, Revision P, December 1999
- [17a] AMD, _AMD Processor Recognition Application Note_, Publication Number:
  20734, Revision 3.00, April 2003

## 6. Release History

### 6.1. Version 0.8.1

Features:

- Interpret the TLB structure for more modern Intel CPUs (DOTNET-302)
- Add the "Die" group to `CpuTopoType` (DOTNET-474)
- Update Intel Feature Flags to SDM 2023-11, upto 1Fh (DOTNET-882, DOTNET-890)
- Update AMD Feature Flags (DOTNET-882)
- CpuFeatures now have descriptive strings, as we add more more features
  (DOTNET-883, DOTNET-885, DOTNET-887)
- Added AMD features 8000001B, 8000001C, 80000020, 80000021, 80000022, 80000023
  (DOTNET-884)
- Windows XP alternative to `GetCurrentProcessorNumber()` used (DOTNET-309)
- Add Intel Hybrid BIG.little (DOTNET-891)
- For Linux, calculate the AMD64 feature level (DOTNET-903)

Bufgixes:

- Properly set the number of partitions (DOTNET-878)
- AMD RDPID (8000001A) corrected (DOTNET-882)
- Bit feature flags checked for Intel and AMD and corrected (DOTNET-883)
- AMD CPUID check infinite loop in DLL fixed (DOTNET-884)

Quality:

- Update README.md for NuGet package (DOTNET-809)
- Minor performance warnings (DOTNET-886)
- CPUDLL upgraded to use VS2022 and compile for Windows XP (DOTNET-888,
  DOTNET-889)
- Documentation clean up (DOTNET-891, DOTNET-904)
- Update .NET 4.5 to 4.6.2 (DOTNET-936, DOTNET-937, DOTNET-938, DOTNET-942,
  DOTNET-945, DOTNET-959)

### 6.2. Version 0.8.0

- Initial release