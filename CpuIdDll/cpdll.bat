@echo off

REM Copy the object build from the C++ solution to a standard location, which
REM the .NET builds use.

echo Copying... CpuId folder...
if not exist ..\CpuId\x86 mkdir ..\CpuId\x86
copy Release\x86\cpuid.dll ..\CpuId\x86\cpuid.dll > NUL
copy Release\x86\cpuid.pdb ..\CpuId\x86\cpuid.pdb > NUL
if not exist ..\CpuId\x64 mkdir ..\CpuId\x64
copy Release\x64\cpuid.dll ..\CpuId\x64\cpuid.dll > NUL
copy Release\x64\cpuid.pdb ..\CpuId\x64\cpuid.pdb > NUL

REM So the .NET projects don't need continuous rebuilding, just copy the DLLs
REM to the correct location.
REM
REM Microsoft suggests that copying the file means breakpoints won't work [1],
REM but this doesn't appear to be the case.
REM
REM [1] https://learn.microsoft.com/en-us/visualstudio/debugger/how-to-debug-from-a-dll-project

call :SUB_Link CpuIdTest\legacy Debug net48 x86
call :SUB_Link CpuIdTest\legacy Debug net48 x64
call :SUB_Link CpuIdTest Debug netcoreapp3.1 x86
call :SUB_Link CpuIdTest Debug netcoreapp3.1 x64

call :SUB_Copy CpuIdWin Debug net48 x86
call :SUB_Copy CpuIdWin Debug net48 x64
call :SUB_Copy CpuIdWin Debug netcoreapp3.1 x86
call :SUB_Copy CpuIdWin Debug netcoreapp3.1 x64
call :SUB_Copy CpuIdWin Release net48 x86
call :SUB_Copy CpuIdWin Release net48 x64
call :SUB_Copy CpuIdWin Release netcoreapp3.1 x86
call :SUB_Copy CpuIdWin Release netcoreapp3.1 x64

call :SUB_Copy CpuIdCon Debug net40 x86
call :SUB_Copy CpuIdCon Debug net40 x64
call :SUB_Copy CpuIdCon Debug netcoreapp3.1 x86
call :SUB_Copy CpuIdCon Debug netcoreapp3.1 x64
call :SUB_Copy CpuIdCon Release net40 x86
call :SUB_Copy CpuIdCon Release net40 x64
call :SUB_Copy CpuIdCon Release netcoreapp3.1 x86
call :SUB_Copy CpuIdCon Release netcoreapp3.1 x64
pause
exit

REM ============================================================================
REM Subroutines
REM ============================================================================

:SUB_Copy
set "tgtfldr=%~1"
set "release=%~2"
set "netver=%~3"
set "arch=%~4"
if not exist "..\%tgtfldr%\bin\%release%\%netver%\%arch%" exit /b
echo Copying... %tgtfldr%\bin\%release%\%netver%\%arch%
call :SUB_CopyFile "%release%\%arch%\cpuid.dll" "..\%tgtfldr%\bin\%release%\%netver%\%arch%\cpuid.dll" > NUL
call :SUB_CopyFile "%release%\%arch%\cpuid.pdb" "..\%tgtfldr%\bin\%release%\%netver%\%arch%\cpuid.pdb" > NUL
exit /b

:SUB_CopyFile
set "source=%~1"
set "dest=%~2"
if exist "%dest%" del "%dest%"
copy "%source%" "%dest%"
exit /b

:SUB_Link
set "tgtfldr=%~1"
set "release=%~2"
set "netver=%~3"
set "arch=%~4"
if not exist "..\%tgtfldr%\bin\%release%\%netver%\%arch%" exit /b
echo Linking... %tgtfldr%\bin\%release%\%netver%\%arch%
call :SUB_LinkFile "%release%\%arch%\cpuid.dll" "..\%tgtfldr%\bin\%release%\%netver%\%arch%\cpuid.dll" > NUL
call :SUB_LinkFile "%release%\%arch%\cpuid.pdb" "..\%tgtfldr%\bin\%release%\%netver%\%arch%\cpuid.pdb" > NUL
exit /b

:SUB_LinkFile
set "source=%~1"
set "dest=%~2"
if exist "%dest%" del "%dest%"
mklink /h "%dest%" "%source%"
exit /b
