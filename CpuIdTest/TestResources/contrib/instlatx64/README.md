# README InstLatx64

This folder contains CPUID samples obtained by
https://github.com/InstLatx64/InstLatx64. They are used for testing the
algorithms.

The original files are not stored here, only the converted XML files.

## Conversion of Files to XML

There's the script `instlatx64.py` which takes on the command line a single
parameter, which is the text file containing the CPUID information.

CPUID lines are expected to be of the form:

```text
CPUID 0000000F: 00000000-00000040-000000FF-00000007 [SL 01]
```

Each new CPU is interpreted as the CPUID value EAX=00000000. The input value ECX
is given by the text `[SL ECX]`

To do the conversion, download the TXT file and run the script:

```sh
python3 instlatx64py cpuid.txt
```

The output is written as `cpuid.xml`, which can be loaded with `CpuIdWin.exe`.