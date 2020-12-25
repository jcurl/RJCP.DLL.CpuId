#!/usr/bin/env python3

import os
import re
import sys

# CPUID
#  0x00000000   0x00000001 0x756E6547 0x6C65746E 0x49656E69
_CPUID = re.compile(r'^\s*0x([0-9a-fA-F]+)\s+0x([0-9a-fA-F]+)\s+0x([0-9a-fA-F]+)\s+0x([0-9a-fA-F]+)\s+0x([0-9a-fA-F]+)\s*$')

if (len(sys.argv) < 2):
    print("Must provide the file name from CPU-Z to read")
    sys.exit(-1)

fileName = sys.argv[1]
with open(fileName, "r") as f:
    # A list of CPUs. Each CPU is a list of registers. A list of registers contains
    # EAX, ECX, then the results EAX, EBX, ECX, EDX
    cpus = []
    cpu = []

    # Read in the file into the arrays above. Then we'll write the XML
    prevEAX = None
    ECXnum=0
    while(True):
        line = f.readline()
        if not line:
            break

        m = _CPUID.match(line)
        if (m != None):
            EAX = m.group(1)
            # Because the dumps I had doesn't show the ECX value used, assume that they're
            # incremented every time the same EAX input is used.
            if (EAX == prevEAX):
                ECXnum = ECXnum + 1
            else:
                ECXnum = 0
            prevEAX = EAX

            PEAX = m.group(2)
            PEBX = m.group(3)
            PECX = m.group(4)
            PEDX = m.group(5)
            ECX = hex(ECXnum)[2:].rjust(8, '0')  # Without the 0x at the beginning
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
