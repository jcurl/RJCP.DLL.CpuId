# Compiling CpuId.dll

This project requires Visual Studio 2017 or later (with v141_xp installed as a
project target). It is compiled with Windows XP compatibility.

Once a batch build is done, run `signdll.bat` that uses `signtool.exe`. Update
the batch file to use the hash for a code sign certificate installed in your
private certificate store. The signature is not explicitly checked by the
software in this repository.

The script `cpdll.bat` copies the compiled DLL files into the folders for
immediate testing with `CpuIdWin` and `CpuIdCon`.
