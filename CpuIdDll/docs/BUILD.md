# Build Instructions

## Compiling CpuId.dll

This project requires Visual Studio 2017 or later (with v141_xp installed as a
project target). It is compiled with Windows XP compatibility.

## Signing CpuId.dll

Once the batch build is done, run `signdll.bat` that uses `signtool.exe`. Update
the batch file to use the hash for a code sign certificate installed in your
private certificate store. The signature is not explicitly checked by the
software in this repository.

This step is optional.

## Deploying CpuId.dll for .NET projects

Once built, the `cpdll.bat` script copies or hardlinks the DLL to the respective
folders. This allows building of the library, then copying to the .NET project,
without having to recompile the .NET project. It also puts the copy into the
`CpuId` folder, which is the version that is committed to the GIT repository.
