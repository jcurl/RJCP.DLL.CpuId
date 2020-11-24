# CPU ID

This is a small, concept library, to obtain CPU information using native CPU
instructions. On Intel processors, this is done using the `cpuid` instruction.

This library abstracts execution of the CPUID instruction. When called, it will
execute the CPU ID instruction multiple times to query the information about the
processor running on the current thread.

NOTE: You must manually set the thread affinity first before executing this
instruction, else multicore processor systems may return mixed / undefined
results.

## Supported Processors and Features

### Intel

The only processor supported at this time is the Intel processor, identified as
`GenuineIntel`.

Functionality supported, based on Application Note 485 (May 2012):

* Function 01h
  * Processor Signature
  * Family
  * Model
  * Stepping
  * Processor Type
  * Maximum Threads per package
  * Current APIC Id
* Function 80000002-4h
  * Brand String
* Function 07h
