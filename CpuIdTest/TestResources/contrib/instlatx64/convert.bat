@ECHO OFF
set CONVDIR=GeniuneIntel

for %%d in ("%CONVDIR%\*.txt") do (
    python instlatx64.py %%d
)