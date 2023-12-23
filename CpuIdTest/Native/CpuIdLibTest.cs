namespace RJCP.Diagnostics.Native
{
    using System;
    using System.Runtime.InteropServices;
    using CpuId;
    using NUnit.Framework;

    [TestFixture]
    [Platform("Win")]
    public class CpuIdLibTest
    {
        [OneTimeSetUp]
        public void InitLibrary()
        {
            // The factory loads the library into memory. This must be done before we can use the native classes.
            var factory = new CpuIdFactory();
            ICpuId cpu;
            try {
                cpu = factory.Create();
            } catch (Exception) {
                // We might get exceptions as we're testing. But don't fail here. Just assume the library could be
                // loaded.
                return;
            }
            Assert.That(cpu, Is.Not.Null);
        }

        [Test]
        public void HasCpuId()
        {
            int result = CpuIdLib.hascpuid();
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void GetCpuId()
        {
            int result = CpuIdLib.cpuid(0, 0, out int eax, out int ebx, out int ecx, out int edx);
            Assert.That(result, Is.EqualTo(0));
            Console.WriteLine($"EAX={eax:x08}h EBX={ebx:x08}h ECX={ecx:x08}h EDX={edx:x08}h");
        }

        [Test]
        public void GetCpuIdDumpCoreZero()
        {
            CpuIdLib.CpuIdInfo[] data = new CpuIdLib.CpuIdInfo[1024];
            int r;
            unsafe {
                fixed (CpuIdLib.CpuIdInfo* cpuidptr = &data[0]) {
                    r = CpuIdLib.iddumponcore(cpuidptr, Marshal.SizeOf(data[0]) * data.Length, 0);
                }
            }

            Console.WriteLine($"Elements: {r}");
            Assert.That(r, Is.Not.Zero);
            uint previous = 0;
            for (int i = 0; i < r; i++) {
                uint current = unchecked((uint)data[i].veax);
                Console.WriteLine($" EAX={data[i].veax:x08} ECX={data[i].vecx:x08} -> {data[i].peax:x08} {data[i].pebx:x08} {data[i].pecx:x08} {data[i].pedx:x08}");
                Assert.That(current, Is.GreaterThanOrEqualTo(previous),
                    $"Element {i} has CPUID {current:x08}h, previous CPUID {previous:x08}h");
                previous = current;
            }
            for (int i = r; i < data.Length; i++) {
                Assert.That(data[i].veax, Is.Zero, $"Expected empty VEAX for element {i}");
                Assert.That(data[i].vecx, Is.Zero, $"Expected empty VEBX for element {i}");
                Assert.That(data[i].peax, Is.Zero, $"Expected empty PEAX for element {i}");
                Assert.That(data[i].pebx, Is.Zero, $"Expected empty PEBX for element {i}");
                Assert.That(data[i].pecx, Is.Zero, $"Expected empty PECX for element {i}");
                Assert.That(data[i].pedx, Is.Zero, $"Expected empty PEDX for element {i}");
            }
        }

        [Test]
        public void GetCpuIdDumpCurrentCore()
        {
            CpuIdLib.CpuIdInfo[] data = new CpuIdLib.CpuIdInfo[1024];
            int r;
            unsafe {
                fixed (CpuIdLib.CpuIdInfo* cpuidptr = &data[0]) {
                    r = CpuIdLib.iddump(cpuidptr, Marshal.SizeOf(data[0]) * data.Length);
                }
            }

            Console.WriteLine($"Elements: {r}");
            Assert.That(r, Is.Not.Zero);
            uint previous = 0;
            for (int i = 0; i < r; i++) {
                uint current = unchecked((uint)data[i].veax);
                Console.WriteLine($" EAX={data[i].veax:x08} ECX={data[i].vecx:x08} -> {data[i].peax:x08} {data[i].pebx:x08} {data[i].pecx:x08} {data[i].pedx:x08}");
                Assert.That(current, Is.GreaterThanOrEqualTo(previous),
                    $"Element {i} has CPUID {current:x08}h, previous CPUID {previous:x08}h");
                previous = current;
            }
            for (int i = r; i < data.Length; i++) {
                Assert.That(data[i].veax, Is.Zero, $"Expected empty VEAX for element {i}");
                Assert.That(data[i].vecx, Is.Zero, $"Expected empty VEBX for element {i}");
                Assert.That(data[i].peax, Is.Zero, $"Expected empty PEAX for element {i}");
                Assert.That(data[i].pebx, Is.Zero, $"Expected empty PEBX for element {i}");
                Assert.That(data[i].pecx, Is.Zero, $"Expected empty PECX for element {i}");
                Assert.That(data[i].pedx, Is.Zero, $"Expected empty PEDX for element {i}");
            }
        }

        [Test]
        public void GetCpuIdDumpAll()
        {
            CpuIdLib.CpuIdInfo[] data = new CpuIdLib.CpuIdInfo[64 * 256];
            int r;
            unsafe {
                fixed (CpuIdLib.CpuIdInfo* cpuidptr = &data[0]) {
                    r = CpuIdLib.iddumpall(cpuidptr, Marshal.SizeOf(data[0]) * data.Length);
                }
            }

            Console.WriteLine($"Elements: {r}");
            Assert.That(r, Is.Not.Zero);
            uint previous = 0;
            int cores = 0;
            for (int i = 0; i < r; i++) {
                uint current = unchecked((uint)data[i].veax);
                if (current == 0xFFFFFFFF) {
                    // Indicates a new core.
                    previous = 0;
                    Console.WriteLine($"Core {cores}");
                    cores++;
                } else {
                    Console.WriteLine($" EAX={data[i].veax:x08} ECX={data[i].vecx:x08} -> {data[i].peax:x08} {data[i].pebx:x08} {data[i].pecx:x08} {data[i].pedx:x08}");
                    Assert.That(current, Is.GreaterThanOrEqualTo(previous),
                        $"Element {i} has CPUID {current:x08}h, previous CPUID {previous:x08}h");
                    previous = current;
                }
            }
            for (int i = r; i < data.Length; i++) {
                Assert.That(data[i].veax, Is.Zero, $"Expected empty VEAX for element {i}");
                Assert.That(data[i].vecx, Is.Zero, $"Expected empty VEBX for element {i}");
                Assert.That(data[i].peax, Is.Zero, $"Expected empty PEAX for element {i}");
                Assert.That(data[i].pebx, Is.Zero, $"Expected empty PEBX for element {i}");
                Assert.That(data[i].pecx, Is.Zero, $"Expected empty PECX for element {i}");
                Assert.That(data[i].pedx, Is.Zero, $"Expected empty PEDX for element {i}");
            }
            Assert.That(cores, Is.Not.Zero);
        }
    }
}
