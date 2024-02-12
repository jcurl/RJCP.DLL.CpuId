namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using InternalCheck;

    /// <summary>
    /// The base abstract class for which all 80x86 CPU information implementations should derive.
    /// </summary>
    public abstract class GenericIntelCpuBase : ICpuIdX86
    {
        internal const int FeatureInformationFunction = 1;
        internal const int ExtendedFeatureFunction = 7;
        internal const int ExtendedProcessorState = 13;
        internal const int MaxExtendedFunction = unchecked((int)0x80000000);
        internal const int ExtendedInformationFunction = unchecked((int)0x80000001);
        internal const int ProcessorBrand1Function = unchecked((int)0x80000002);
        internal const int ProcessorBrand2Function = unchecked((int)0x80000003);
        internal const int ProcessorBrand3Function = unchecked((int)0x80000004);
        internal const int ExtendedFeatureIds = unchecked((int)0x80000008);

        private readonly BasicCpu m_Cpu;

        private protected GenericIntelCpuBase(BasicCpu cpu)
        {
            m_Cpu = cpu;
        }

        /// <inheritdoc/>
        public virtual CpuVendor CpuVendor
        {
            get { return CpuVendor.Unknown; }
        }

        /// <inheritdoc/>
        public string VendorId
        {
            get { return m_Cpu.VendorId; }
        }

        /// <inheritdoc/>
        public ICpuRegisters Registers
        {
            get { return m_Cpu.CpuRegisters; }
        }

        /// <inheritdoc/>
        public string Description { get; protected set; }

        /// <inheritdoc/>
        public string BrandString { get; protected set; }

        /// <inheritdoc/>
        public int ProcessorSignature { get; protected set; }

        /// <inheritdoc/>
        public int Family { get; protected set; }

        /// <inheritdoc/>
        public int Model { get; protected set; }

        /// <inheritdoc/>
        public int Stepping { get; protected set; }

        /// <inheritdoc/>
        public int ProcessorType { get; protected set; }

        /// <inheritdoc/>
        public CpuFeatures Features { get; } = new CpuFeatures();

        /// <inheritdoc/>
        public Topology Topology { get; } = new Topology();

        /// <inheritdoc/>
        public int FeatureLevel { get; protected set; }

        /// <summary>
        /// Gets the brand string from registers 80000002-80000004h.
        /// </summary>
        /// <returns>
        /// The decoded brand string, or <see langword="null"/> if the CPU doesn't support this feature.
        /// </returns>
        protected string GetProcessorBrandString()
        {
            if (m_Cpu.ExtendedFunctionCount >= 4) {
                StringBuilder brand = new StringBuilder(50);
                WriteDescription(brand, m_Cpu.CpuRegisters.GetCpuId(ProcessorBrand1Function, 0));
                WriteDescription(brand, m_Cpu.CpuRegisters.GetCpuId(ProcessorBrand2Function, 0));
                WriteDescription(brand, m_Cpu.CpuRegisters.GetCpuId(ProcessorBrand3Function, 0));
                return brand.ToString().Trim();
            }
            return null;
        }

        private static void WriteDescription(StringBuilder brand, CpuIdRegister register)
        {
            if (register == null) return;

            int eax = register.Result[0];
            int ebx = register.Result[1];
            int ecx = register.Result[2];
            int edx = register.Result[3];

            Append(brand, eax & 0xFF);
            Append(brand, (eax >> 8) & 0xFF);
            Append(brand, (eax >> 16) & 0xFF);
            Append(brand, (eax >> 24) & 0xFF);
            Append(brand, ebx & 0xFF);
            Append(brand, (ebx >> 8) & 0xFF);
            Append(brand, (ebx >> 16) & 0xFF);
            Append(brand, (ebx >> 24) & 0xFF);
            Append(brand, ecx & 0xFF);
            Append(brand, (ecx >> 8) & 0xFF);
            Append(brand, (ecx >> 16) & 0xFF);
            Append(brand, (ecx >> 24) & 0xFF);
            Append(brand, edx & 0xFF);
            Append(brand, (edx >> 8) & 0xFF);
            Append(brand, (edx >> 16) & 0xFF);
            Append(brand, (edx >> 24) & 0xFF);
        }

        private static void Append(StringBuilder brand, int value)
        {
            if (value == 0) return;
            brand.Append((char)value);
        }

#if DEBUG
        // This section adds debug checks, to ensure that a field is not defined more than once. If it is, DEBUG mode
        // will cause an exception, indicating a programmatic error.
        private readonly MainFunction m_MainFunction = new MainFunction();
#endif

        /// <summary>
        /// Tests a bit in the feature flag.
        /// </summary>
        /// <param name="feature">The name of the CPU feature, e.g. "FPU".</param>
        /// <param name="register">The feature register obtained from a query of CPUID.</param>
        /// <param name="outRegister">The register to query, 0 is EAX, to 3 for EDX.</param>
        /// <param name="bit">The bit to test for.</param>
        protected void TestFeature(string feature, CpuIdRegister register, int outRegister, int bit)
        {
            TestFeature(feature, FeatureGroup.Unknown, register, outRegister, bit);
        }

        /// <summary>
        /// Tests a bit in the feature flag.
        /// </summary>
        /// <param name="feature">The name of the CPU feature, e.g. "FPU".</param>
        /// <param name="group">The group the feature belongs to.</param>
        /// <param name="register">The feature register obtained from a query of CPUID.</param>
        /// <param name="outRegister">The register to query, 0 is EAX, to 3 for EDX.</param>
        /// <param name="bit">The bit to test for.</param>
        protected void TestFeature(string feature, FeatureGroup group, CpuIdRegister register, int outRegister, int bit)
        {
            bool value = (register.Result[outRegister] & (1 << bit)) != 0;
            Features.Add(feature, value, group, BitGroup(register, outRegister, bit));

#if DEBUG
            m_MainFunction.Set(register.Function, register.SubFunction, outRegister, 1 << bit);
#endif
        }

        internal static string BitGroup(CpuIdRegister register, int outRegister, int bit)
        {
            if (register.SubFunction == 0) {
                return $"CPUID.{register.Function:X02}h:{GetRegisterName(outRegister)}[{bit}]";
            }
            return $"CPUID.{register.Function:X02}h.{register.SubFunction:X02}:{GetRegisterName(outRegister)}[{bit}]";
        }

        /// <summary>
        /// Tests a range of features, based on an enumerable where the position represents the bit position.
        /// </summary>
        /// <param name="features">The enuemrable list of features. A <see langword="null"/> or empty feature is skipped.</param>
        /// <param name="group">The group the feature belongs to.</param>
        /// <param name="register">The feature register obtained from a query of CPUID.</param>
        /// <param name="outRegister">The register to query, 0 is EAX, to 3 for EDX.</param>
        protected void TestFeatures(IEnumerable<string> features, FeatureGroup group, CpuIdRegister register, int outRegister)
        {
            int bit = 0;
            foreach (string feature in features) {
                if (!string.IsNullOrWhiteSpace(feature)) {
                    TestFeature(feature, group, register, outRegister, bit);
                }

                if (bit == 31) return;
                bit++;
            }
        }

        /// <summary>
        /// Adds a bit field that a feature is reserved, only if it is set.
        /// </summary>
        /// <param name="register">The feature register obtained from a query of CPUID.</param>
        /// <param name="outRegister">The register to query, 0 is EAX, to 3 for EDX.</param>
        /// <param name="mask">The bit mask of reserved bits set to 1.</param>
        /// <remarks>
        /// This should be used after testing for all features, to ensure that any unknown features are tested for and
        /// set with the generic name <c>CPUID(XXh).REG[bit]</c> or <c>CPUID(EAX=XXh,ECX=XXh).REG[bit]</c>, where the
        /// REG is one of EAX, EBX, ECX, EDX and the bit is the bit given in <paramref name="mask"/> which is a bit
        /// field of reserved features. CPUs in the future may set these bits and so it's useful to see if there are new
        /// features which are not defined. Bits that are "don't care" in the specifications should not be part of the
        /// bit field, but as most bits are "reserved" are usually zero, so that future processors can set them as set
        /// when a new feature is defined.
        /// </remarks>
        protected void ReservedFeature(CpuIdRegister register, int outRegister, int mask)
        {
            ReservedFeature(FeatureGroup.Unknown, register, outRegister, mask);
        }

        /// <summary>
        /// Adds a bit field that a feature is reserved, only if it is set.
        /// </summary>
        /// <param name="group">The group the feature belongs to.</param>
        /// <param name="register">The feature register obtained from a query of CPUID.</param>
        /// <param name="outRegister">The register to query, 0 is EAX, to 3 for EDX.</param>
        /// <param name="mask">The bit mask of reserved bits set to 1.</param>
        /// <remarks>
        /// This should be used after testing for all features, to ensure that any unknown features are tested for and
        /// set with the generic name <c>CPUID(XXh).REG[bit]</c> or <c>CPUID(EAX=XXh,ECX=XXh).REG[bit]</c>, where the
        /// REG is one of EAX, EBX, ECX, EDX and the bit is the bit given in <paramref name="mask"/> which is a bit
        /// field of reserved features. CPUs in the future may set these bits and so it's useful to see if there are new
        /// features which are not defined. Bits that are "don't care" in the specifications should not be part of the
        /// bit field, but as most bits are "reserved" are usually zero, so that future processors can set them as set
        /// when a new feature is defined.
        /// </remarks>
        protected void ReservedFeature(FeatureGroup group, CpuIdRegister register, int outRegister, int mask)
        {
#if DEBUG
            m_MainFunction.CheckReservedBitMask(register, outRegister, mask);
#endif

            int bit = 0;
            uint checkMask = unchecked((uint)(mask & register.Result[outRegister]));
            while (checkMask != 0) {
                if ((checkMask & 0x01) != 0) {
                    string feature = GetReservedFeatureName(register, outRegister, bit);
                    string bitGroup = BitGroup(register, outRegister, bit);
                    CpuFeature cpuFeature = Features.Add(feature, true, group, bitGroup);
                    cpuFeature.IsReserved = true;
                }
                checkMask >>= 1;     // Unsigned means zero is rolled into MSB, so we don't need to clear the MSB.
                bit++;
            }
        }

        private static string GetReservedFeatureName(CpuIdRegister register, int result, int bit)
        {
            if (register.SubFunction == 0) {
                return string.Format("CPUID({0:X2}h).{1}[{2}]",
                    register.Function, GetRegisterName(result), bit);
            }
            return string.Format("CPUID({0:X2}h,{1:X2}h).{2}[{3}]",
                register.Function, register.SubFunction, GetRegisterName(result), bit);
        }

        private readonly static string[] BitRegisterName = new[] { "EAX", "EBX", "ECX", "EDX" };

        internal static string GetRegisterName(int outRegister)
        {
            if (outRegister >= 0 && outRegister <= 3) {
                return BitRegisterName[outRegister];
            }
            return $"R{outRegister}";
        }

        private bool HasFeatures(params string[] features)
        {
            foreach (string feature in features) {
                if (!Features[feature].Value) return false;
            }
            return true;
        }

        /// <summary>
        /// Uses the already identified features to identify the current AMD64 feature level.
        /// </summary>
        /// <returns>The feature level, as defined by https://gitlab.com/x86-psABIs/x86-64-ABI.</returns>
        /// <remarks>
        /// The default feature level is 0 (32-bit). The following feature levels are defined, and are cumulative.
        /// <list type="bullet">
        /// <item>1 - CMOV, CX8, FPU, FXSR, MMX, OSFXSR, SCE, SSE, SSE2</item>
        /// <item>2 - CPMXCHG16B, LAHF-SAHF, POPCNT, SSE3, SSE4_1, SSE4_2, SSSE3</item>
        /// <item>3 - AVX, AVX2, BMI1, BMI2, F16C, FMA, LZCNT, MOVBE, OSXSAVE</item>
        /// <item>4 - AVX512F, AVX512BW, AVX512CD, AVX512DQ, AVX512VL</item>
        /// </list>
        /// </remarks>
        protected virtual int IdentifyFeatureLevel()
        {
            // SCE is defined in IA32_EFER.SCE and CPUID bit is "SYSCALL". We can't test in user mode if this is
            // enabled.

            // OSFXSR is defined in CR4 and CPUID bit is "FXSR".
            if (!HasFeatures("CMOV", "CX8", "FPU", "FXSR", "MMX", "FXSR", "SYSCALL", "SSE", "SSE2"))
                return 0;

            if (!HasFeatures("CMPXCHG16B", "AHF64", "POPCNT", "SSE3", "SSE4_1", "SSE4_2", "SSSE3"))
                return 1;

            if (!HasFeatures("AVX", "AVX2", "BMI1", "BMI2", "F16C", "FMA", "LZCNT", "MOVBE", "OSXSAVE"))
                return 2;

            if (!HasFeatures("AVX512F", "AVX512BW", "AVX512CD", "AVX512DQ", "AVX512VL"))
                return 3;

            return 4;
        }

        /// <summary>
        /// A common function to get the Log_2 of the Power of 2 for a value.
        /// </summary>
        /// <param name="value">The value to get the Log2 for.</param>
        /// <returns>The Log_2 of the Power of 2 for a value.</returns>
        /// <remarks>
        /// While the title sounds complicated, and is the name used in Intel and AMD documentation, this function is
        /// nothing more than just calculating the most significant bit of the value rounded up to the next power of 2.
        /// For values that are exactly powers of 2, this is just the position of that single bit. For values that
        /// aren't, the result is the next bit position more than the upper most bit.
        /// <para>For example, 2^3 = 8 returns the value of 3.</para>
        /// <para>
        /// For example, for value = 2^x + y, where y is non-zero, this function returns x + 1. Thus, if value = 6 = 2^2
        /// + 2, the result is x + 1 = 2 + 1 = 3.
        /// </para>
        /// </remarks>
        protected static int Log2Pof2(int value)
        {
            if (value == 0) return -1;

            int lowBits = 0;
            for (int i = 0; i < 31; i++) {
                bool shifted = (value & 0x01) != 0;
                value = (value >> 1) & 0x7FFFFFFF;
                if (value == 0) return i + lowBits;
                if (shifted) lowBits = 1;
            }
            return 31 + lowBits;
        }

        /// <summary>
        /// Gets the cache topology leaf.
        /// </summary>
        /// <param name="leaf">The leaf number to use.</param>
        /// <exception cref="NotSupportedException">
        /// A cache entry is fully associative, with the number of sets not one.
        /// </exception>
        /// <remarks>
        /// The Intel leaf 4, and AMD leaf 8000001D have the same implementation, this method provides for common code
        /// between the two. If the exception <see cref="NotSupportedException"/> occurs, this should be reported with a
        /// CPUID dump for investigation.
        /// </remarks>
        protected void GetCacheTopologyLeaf(int leaf)
        {
            int subleaf = 0;
            CpuIdRegister cache = m_Cpu.CpuRegisters.GetCpuId(leaf, subleaf);
            while (cache != null && (cache.Result[0] & 0xF) != 0) {
                int ltype = cache.Result[0] & 0xF;

                CacheType ctype = CacheType.Invalid;
                switch (ltype) {
                case 1:
                    ctype = CacheType.Data;
                    break;
                case 2:
                    ctype = CacheType.Instruction;
                    break;
                case 3:
                    ctype = CacheType.Unified;
                    break;
                }

                if (ctype != CacheType.Invalid) {
                    bool fullyAssoc = (cache.Result[0] & 0x200) != 0;
                    int sets = cache.Result[2] + 1;

                    if (fullyAssoc && sets != 1)
                        throw new NotSupportedException("A cache entry is fully associative, with the number of sets not one");

                    int level = (cache.Result[0] >> 5) & 0x7;
                    int lineSize = (cache.Result[1] & 0xFFF) + 1;
                    int partitions = ((cache.Result[1] >> 12) & 0x3FF) + 1;
                    int ways = ((cache.Result[1] >> 22) & 0x3FF) + 1;

                    CacheTopoCpu cacheTopoCpu = new CacheTopoCpu(level, ctype, ways, lineSize, sets, partitions);

                    int numSharingCache = Log2Pof2(((cache.Result[0] >> 14) & 0xFFF) + 1);
                    cacheTopoCpu.Mask = ~(-1 << numSharingCache);
                    Topology.CacheTopology.Add(cacheTopoCpu);
                }

                subleaf++;
                cache = m_Cpu.CpuRegisters.GetCpuId(leaf, subleaf);
            }
        }

        /// <summary>
        /// Gets the translation lookaside buffer cache topology leaf.
        /// </summary>
        /// <param name="leaf">The leaf number to use.</param>
        protected void GetCacheTlbTopologyLeaf(int leaf)
        {
            int subleaf = 0;
            CpuIdRegister cache = m_Cpu.CpuRegisters.GetCpuId(leaf, subleaf);
            int subleaves = cache.Result[0];

            while (cache != null && subleaf <= subleaves) {
                int ttype = cache.Result[3] & 0x1F;
                if (ttype != 0) {
                    CacheType ctype;
                    switch (ttype) {
                    case 1:
                        ctype = CacheType.Data | CacheType.Tlb;
                        break;
                    case 2:
                        ctype = CacheType.Instruction | CacheType.Tlb;
                        break;
                    case 3:
                        ctype = CacheType.Unified | CacheType.Tlb;
                        break;
                    case 4:
                        ctype = CacheType.LoadOnlyTlb;
                        break;
                    case 5:
                        ctype = CacheType.StoreOnlyTlb;
                        break;
                    default:
                        ctype = CacheType.Tlb;
                        break;
                    }

                    if ((cache.Result[1] & 0x01) != 0) ctype |= CacheType.Page4k;
                    if ((cache.Result[1] & 0x02) != 0) ctype |= CacheType.Page2M;
                    if ((cache.Result[1] & 0x04) != 0) ctype |= CacheType.Page4M;
                    if ((cache.Result[1] & 0x08) != 0) ctype |= CacheType.Page1G;

                    int level = (cache.Result[3] & 0xE0) >> 5;
                    int ways = cache.Result[1] >> 16;
                    int sets = cache.Result[2];
                    bool fullAssociative = (cache.Result[3] & 0x100) != 0;

                    int numSharingCache = Log2Pof2(((cache.Result[3] >> 14) & 0xFFF) + 1);
                    long mask = ~(-1 << numSharingCache);
                    CacheTopoTlb cacheTopoTlb;
                    if (fullAssociative) {
                        cacheTopoTlb = new CacheTopoTlb(level, ctype, 0, ways, mask);
                    } else {
                        cacheTopoTlb = new CacheTopoTlb(level, ctype, ways, ways * sets, mask);
                    }

                    Topology.CacheTopology.Add(cacheTopoTlb);
                }

                if (subleaf < subleaves) {
                    subleaf++;
                    cache = m_Cpu.CpuRegisters.GetCpuId(leaf, subleaf);
                } else {
                    cache = null;
                }
            }
        }

        /// <summary>
        /// Writes the cached CPUID registers (those found in <see cref="Registers"/> to an XML writer.
        /// </summary>
        /// <param name="xmlWriter">The XML writer to write to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="xmlWriter"/> may not be <see langword="null"/>.</exception>
        public void Save(XmlWriter xmlWriter)
        {
            ThrowHelper.ThrowIfNull(xmlWriter);
            xmlWriter.WriteStartElement("processor");
            WriteRegisters(xmlWriter);
            xmlWriter.WriteEndElement();
        }

        private void WriteRegisters(XmlWriter xmlWriter)
        {
            foreach (CpuIdRegister register in Registers) {
                xmlWriter.WriteStartElement("register");
                xmlWriter.WriteAttributeString("eax", register.Function.ToString("X8"));
                xmlWriter.WriteAttributeString("ecx", register.SubFunction.ToString("X8"));
                string value = string.Format("{0:X8},{1:X8},{2:X8},{3:X8}",
                    register.Result[0], register.Result[1], register.Result[2], register.Result[3]);
                xmlWriter.WriteString(value);
                xmlWriter.WriteEndElement();
            }
        }
    }
}
