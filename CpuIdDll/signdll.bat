@echo off
echo Signing...
"C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x86\signtool.exe" sign /fd sha1 /td sha1 /sha1 516ee045d8f8b691f16926af7fff0974397e6025 Debug\x86\cpuid.dll
"C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x86\signtool.exe" sign /fd sha1 /td sha1 /sha1 516ee045d8f8b691f16926af7fff0974397e6025 Debug\x64\cpuid.dll
"C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x86\signtool.exe" sign /fd sha1 /td sha1 /sha1 516ee045d8f8b691f16926af7fff0974397e6025 /tr http://timestamp.digicert.com Release\x86\cpuid.dll
"C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x86\signtool.exe" sign /fd sha1 /td sha1 /sha1 516ee045d8f8b691f16926af7fff0974397e6025 /tr http://timestamp.digicert.com Release\x64\cpuid.dll

:done
pause
