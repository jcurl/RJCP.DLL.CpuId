#!/usr/bin/env python3

import os
import re
import sys

# CPUID 0000000F: 00000000-00000040-000000FF-00000007 [SL 01]
_CPUID = re.compile(r'^CPUID ([0-9a-fA-F]+): ([0-9a-fA-F]+)-([0-9a-fA-F]+)-([0-9a-fA-F]+)-([0-9a-fA-F]+)( \[SL ([0-9a-fA-F]+))?')

if (len(sys.argv) < 2):
    print("Must provide the file name from AIDA tool to read")
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
            if (m.group(6) != None):
                ECX = m.group(7).rjust(8, "0")
            else:
                ECX = "00000000"

            PEAX = m.group(2)
            PEBX = m.group(3)
            PECX = m.group(4)
            PEDX = m.group(5)
            cpuentry = [ EAX, ECX, PEAX, PEBX, PECX, PEDX ]

            if (EAX == "00000000"):
                if (len(cpu) > 0):
                    cpus.append(cpu)
                    cpu = []
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