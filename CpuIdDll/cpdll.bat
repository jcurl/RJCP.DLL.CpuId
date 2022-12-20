@echo off
echo Copying... CpuId folder...
if not exist ..\CpuId\x86 mkdir ..\CpuId\x86
copy Release\x86\cpuid.dll ..\CpuId\x86\cpuid.dll > NUL
copy Release\x86\cpuid.pdb ..\CpuId\x86\cpuid.pdb > NUL
if not exist ..\CpuId\x64 mkdir ..\CpuId\x64
copy Release\x64\cpuid.dll ..\CpuId\x64\cpuid.dll > NUL
copy Release\x64\cpuid.pdb ..\CpuId\x64\cpuid.pdb > NUL

:cpuidwin32debug_net48
if not exist ..\CpuIdWin\bin\Debug\net48\x86 goto cpuidwin32debug_core31
echo Copying... CpuIdWin Debug\net48\x86
copy Debug\x86\cpuid.dll ..\CpuIdWin\bin\Debug\net48\x86\cpuid.dll > NUL
copy Debug\x86\cpuid.pdb ..\CpuIdWin\bin\Debug\net48\x86\cpuid.pdb > NUL
:cpuidwin32debug_core31
if not exist ..\CpuIdWin\bin\Debug\netcoreapp3.1\x86 goto cpuidwin64debug_net48
echo Copying... CpuIdWin Debug\netcoreapp3.1\x86
copy Debug\x86\cpuid.dll ..\CpuIdWin\bin\Debug\netcoreapp3.1\x86\cpuid.dll > NUL
copy Debug\x86\cpuid.pdb ..\CpuIdWin\bin\Debug\netcoreapp3.1\x86\cpuid.pdb > NUL
:cpuidwin64debug_net48
if not exist ..\CpuIdWin\bin\Debug\net48\x64 goto cpuidwin64debug_core31
echo Copying... CpuIdWin Debug\net48\x64
copy Debug\x64\cpuid.dll ..\CpuIdWin\bin\Debug\net48\x64\cpuid.dll > NUL
copy Debug\x64\cpuid.pdb ..\CpuIdWin\bin\Debug\net48\x64\cpuid.pdb > NUL
:cpuidwin64debug_core31
if not exist ..\CpuIdWin\bin\Debug\netcoreapp3.1\x64 goto cpuidwin32release_net48
echo Copying... CpuIdWin Debug\netcoreapp3.1\x64
copy Debug\x64\cpuid.dll ..\CpuIdWin\bin\Debug\netcoreapp3.1\x64\cpuid.dll > NUL
copy Debug\x64\cpuid.pdb ..\CpuIdWin\bin\Debug\netcoreapp3.1\x64\cpuid.pdb > NUL
:cpuidwin32release_net48
if not exist ..\CpuIdWin\bin\Release\net48\x86 goto cpuidwin32release_core31
echo Copying... CpuIdWin Release\net48\x86
copy Release\x86\cpuid.dll ..\CpuIdWin\bin\Release\net48\x86\cpuid.dll > NUL
copy Release\x86\cpuid.pdb ..\CpuIdWin\bin\Release\net48\x86\cpuid.pdb > NUL
:cpuidwin32release_core31
if not exist ..\CpuIdWin\bin\Release\netcoreapp3.1\x86 goto cpuidwin64release_net48
echo Copying... CpuIdWin Release\netcoreapp3.1\x86
copy Release\x86\cpuid.dll ..\CpuIdWin\bin\Release\netcoreapp3.1\x86\cpuid.dll > NUL
copy Release\x86\cpuid.pdb ..\CpuIdWin\bin\Release\netcoreapp3.1\x86\cpuid.pdb > NUL
:cpuidwin64release_net48
if not exist ..\CpuIdWin\bin\Release\net48\x64 goto cpuidwin64release_core31
echo Copying... CpuIdWin Release\net48\x64
copy Release\x64\cpuid.dll ..\CpuIdWin\bin\Release\net48\x64\cpuid.dll > NUL
copy Release\x64\cpuid.pdb ..\CpuIdWin\bin\Release\net48\x64\cpuid.pdb > NUL
:cpuidwin64release_core31
if not exist ..\CpuIdWin\bin\Release\netcoreapp3.1\x64 goto cpuidcon32debug_net40
echo Copying... CpuIdWin Release\netcoreapp3.1\x64
copy Release\x64\cpuid.dll ..\CpuIdWin\bin\Release\netcoreapp3.1\x64\cpuid.dll > NUL
copy Release\x64\cpuid.pdb ..\CpuIdWin\bin\Release\netcoreapp3.1\x64\cpuid.pdb > NUL

:cpuidcon32debug_net40
if not exist ..\CpuIdCon\bin\Debug\net40\x86 goto cpuidcon32debug_core31
echo Copying... CpuIdCon Debug\net40\x86
copy Debug\x86\cpuid.dll ..\CpuIdCon\bin\Debug\net40\x86\cpuid.dll > NUL
copy Debug\x86\cpuid.pdb ..\CpuIdCon\bin\Debug\net40\x86\cpuid.pdb > NUL
:cpuidcon32debug_core31
if not exist ..\CpuIdCon\bin\Debug\net40\x86 goto cpuidcon64debug_net40
echo Copying... CpuIdCon Debug\netcoreapp3.1\x86
copy Debug\x86\cpuid.dll ..\CpuIdCon\bin\Debug\net40\x86\cpuid.dll > NUL
copy Debug\x86\cpuid.pdb ..\CpuIdCon\bin\Debug\net40\x86\cpuid.pdb > NUL
:cpuidcon64debug_net40
if not exist ..\CpuIdCon\bin\Debug\net40\x64 goto cpuidcon64debug_core31
echo Copying... CpuIdCon Debug\net40\x64
copy Debug\x64\cpuid.dll ..\CpuIdCon\bin\Debug\net40\x64\cpuid.dll > NUL
copy Debug\x64\cpuid.pdb ..\CpuIdCon\bin\Debug\net40\x64\cpuid.pdb > NUL
:cpuidcon64debug_core31
if not exist ..\CpuIdCon\bin\Debug\net40\x64 goto cpuidcon32release_net40
echo Copying... CpuIdCon Debug\netcoreapp3.1\x64
copy Debug\x64\cpuid.dll ..\CpuIdCon\bin\Debug\net40\x64\cpuid.dll > NUL
copy Debug\x64\cpuid.pdb ..\CpuIdCon\bin\Debug\net40\x64\cpuid.pdb > NUL
:cpuidcon32release_net40
if not exist ..\CpuIdCon\bin\Release\net40\x86 goto cpuidcon32release_core31
echo Copying... CpuIdCon Release\net40\x86
copy Release\x86\cpuid.dll ..\CpuIdCon\bin\Release\net40\x86\cpuid.dll > NUL
copy Release\x86\cpuid.pdb ..\CpuIdCon\bin\Release\net40\x86\cpuid.pdb > NUL
:cpuidcon32release_core31
if not exist ..\CpuIdCon\bin\Release\net40\x86 goto cpuidcon64release_net40
echo Copying... CpuIdCon Release\netcoreapp3.1\x86
copy Release\x86\cpuid.dll ..\CpuIdCon\bin\Release\net40\x86\cpuid.dll > NUL
copy Release\x86\cpuid.pdb ..\CpuIdCon\bin\Release\net40\x86\cpuid.pdb > NUL
:cpuidcon64release_net40
if not exist ..\CpuIdCon\bin\Release\net40\x64 goto cpuidcon64release_core31
echo Copying... CpuIdCon Release\net40\x64
copy Release\x64\cpuid.dll ..\CpuIdCOn\bin\Release\net40\x64\cpuid.dll > NUL
copy Release\x64\cpuid.pdb ..\CpuIdCOn\bin\Release\net40\x64\cpuid.pdb > NUL
:cpuidcon64release_core31
if not exist ..\CpuIdCon\bin\Release\net40\x64 goto done
echo Copying... CpuIdCon Release\netcoreapp3.1\x64
copy Release\x64\cpuid.dll ..\CpuIdCOn\bin\Release\net40\x64\cpuid.dll > NUL
copy Release\x64\cpuid.pdb ..\CpuIdCOn\bin\Release\net40\x64\cpuid.pdb > NUL

:done
pause
