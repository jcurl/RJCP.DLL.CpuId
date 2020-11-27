@echo off
echo Copying... CpuId folder...
if not exist ..\CpuId\x86 mkdir ..\CpuId\x86
copy Release\x86\cpuid.dll ..\CpuId\x86\cpuid.dll > NUL
copy Release\x86\cpuid.pdb ..\CpuId\x86\cpuid.pdb > NUL
if not exist ..\CpuId\x64 mkdir ..\CpuId\x64
copy Release\x64\cpuid.dll ..\CpuId\x64\cpuid.dll > NUL
copy Release\x64\cpuid.pdb ..\CpuId\x64\cpuid.pdb > NUL

:cpuidwin32debug
if not exist ..\CpuIdWin\bin\Debug\x86 goto cpuidwin64debug
echo Copying... CpuIdWin Debug\x86
copy Debug\x86\cpuid.dll ..\CpuIdWin\bin\Debug\x86\cpuid.dll > NUL
copy Debug\x86\cpuid.pdb ..\CpuIdWin\bin\Debug\x86\cpuid.pdb > NUL
:cpuidwin64debug
if not exist ..\CpuIdWin\bin\Debug\x64 goto cpuidwin32release
echo Copying... CpuIdWin Debug\x64
copy Debug\x64\cpuid.dll ..\CpuIdWin\bin\Debug\x64\cpuid.dll > NUL
copy Debug\x64\cpuid.pdb ..\CpuIdWin\bin\Debug\x64\cpuid.pdb > NUL
:cpuidwin32release
if not exist ..\CpuIdWin\bin\Release\x86 goto cpuidwin64release
echo Copying... CpuIdWin Release\x86
copy Release\x86\cpuid.dll ..\CpuIdWin\bin\Release\x86\cpuid.dll > NUL
copy Release\x86\cpuid.pdb ..\CpuIdWin\bin\Release\x86\cpuid.pdb > NUL
:cpuidwin64release
if not exist ..\CpuIdWin\bin\Release\x64 goto cpuidcon32debug
echo Copying... CpuIdWin Release\x64
copy Release\x64\cpuid.dll ..\CpuIdWin\bin\Release\x64\cpuid.dll > NUL
copy Release\x64\cpuid.pdb ..\CpuIdWin\bin\Release\x64\cpuid.pdb > NUL

:cpuidcon32debug
if not exist ..\CpuIdCon\bin\Debug\x86 goto cpuidcon64debug
echo Copying... CpuIdCon Debug\x86
copy Debug\x86\cpuid.dll ..\CpuIdCon\bin\Debug\x86\cpuid.dll > NUL
copy Debug\x86\cpuid.pdb ..\CpuIdCon\bin\Debug\x86\cpuid.pdb > NUL
:cpuidcon64debug
if not exist ..\CpuIdCon\bin\Debug\x64 goto cpuidcon32release
echo Copying... CpuIdCon Debug\x64
copy Debug\x64\cpuid.dll ..\CpuIdCon\bin\Debug\x64\cpuid.dll > NUL
copy Debug\x64\cpuid.pdb ..\CpuIdCon\bin\Debug\x64\cpuid.pdb > NUL
:cpuidcon32release
if not exist ..\CpuIdCon\bin\Release\x86 goto cpuidcon64release
echo Copying... CpuIdCon Release\x86
copy Release\x86\cpuid.dll ..\CpuIdCon\bin\Release\x86\cpuid.dll > NUL
copy Release\x86\cpuid.pdb ..\CpuIdCon\bin\Release\x86\cpuid.pdb > NUL
:cpuidcon64release
if not exist ..\CpuIdCon\bin\Release\x64 goto done
echo Copying... CpuIdCon Release\x64
copy Release\x64\cpuid.dll ..\CpuIdCOn\bin\Release\x64\cpuid.dll > NUL
copy Release\x64\cpuid.pdb ..\CpuIdCOn\bin\Release\x64\cpuid.pdb > NUL

:done
pause
