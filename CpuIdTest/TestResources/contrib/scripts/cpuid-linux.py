#!/usr/bin/env python3

import os
import re
import sys

# CPUID
#  0x00000000   0x00000001 0x756E6547 0x6C65746E 0x49656E69
_CPUID = re.compile(r'^\s*0x([0-9a-fA-F]+)\s*0x([0-9a-fA-F]+):\s+eax=0x([0-9a-fA-F]+)\s+ebx=0x([0-9a-fA-F]+)\s+ecx=0x([0-9a-fA-F]+)\s+edx=0x([0-9a-fA-F]+)\s*$')

if (len(sys.argv) < 2):
    print("Must provide the file name from Todd Allens (http://etallen.com/cpuid.html) CPUID tool to read")
    sys.exit(-1)

fileName = sys.argv[1]
with open(fileName, "r") as f:
    # A list of CPUs. Each CPU is a list of registers. A list of registers contains
    # EAX, ECX, then the results EAX, EBX, ECX, EDX
    cpus = []
    cpu = []

    # Read in the file into the arrays above. Then we'll write the XML
    while(True):
        line = f.readline()
        if not line:
            break

        m = _CPUID.match(line)
        if (m != None):
            EAX = m.group(1)
            ECX = m.group(2)
            PEAX = m.group(3)
            PEBX = m.group(4)
            PECX = m.group(5)
            PEDX = m.group(6)
            cpuentry = [ EAX, ECX, PEAX, PEBX, PECX, PEDX ]

            if (EAX == "00000000"):
                if (len(cpu) > 0):
                    cpus.append(cpu)
                    cpu = []
                    ECX = 0
            cpu.append(cpuentry)

    if (len(cpu) > 0):
        cpus.append(cpu)

# Write the output to a compatible XML file

outFileName = os.path.splitext(fileName)[0] + ".xml"
with open(outFileName, "w") as x:
    x.write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n")
    x.write("<cpuid>\n")
    for cpu in cpus:
        x.write("	<processor>\n")
        for entry in cpu:
            x.write("		<register eax=\"{}\" ecx=\"{}\">{},{},{},{}</register>\n"
                .format(entry[0], entry[1], entry[2], entry[3], entry[4], entry[5]))
        x.write("	</processor>\n")
    
    x.write("</cpuid>\n")
