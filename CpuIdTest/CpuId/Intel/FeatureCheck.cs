namespace RJCP.Diagnostics.CpuId.Intel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;

    public class FeatureCheck
    {
        private class FeatureSet
        {
            public FeatureSet(string name, string[] featureSet)
            {
                Name = name;
                Set = featureSet;
            }

            public string Name { get; private set; }

            public string[] Set { get; private set; }
        }

        // Key is the group (e.g. "standard", or "extended")
        // Value is list of [RegisterGroup, Features]
        private readonly Dictionary<string, List<FeatureSet>> m_FeatureSet = new();

        public void AddFeatureSet(string group, string name, string[] featureSet)
        {
            ThrowHelper.ThrowIfNullOrEmpty(group);
            ThrowHelper.ThrowIfNullOrEmpty(name);
            ThrowHelper.ThrowIfNull(featureSet);

            if (!m_FeatureSet.TryGetValue(group, out List<FeatureSet> features)) {
                features = new List<FeatureSet>();
                m_FeatureSet.Add(group, features);
            }
            features.Add(new FeatureSet(name, featureSet));
        }

        public HashSet<string> Expected { get; private set; } = new HashSet<string>();

        public HashSet<string> Missing { get; private set; } = new HashSet<string>();

        public HashSet<string> Additional { get; private set; } = new HashSet<string>();

        public void LoadCpu(string fileName)
        {
            CpuIdXmlFactory factory = new();
            Cpu = factory.Create(fileName) as ICpuIdX86;
            if (Cpu is null) throw new InvalidOperationException("Couldn't load CPU file");

            Cpus = factory.CreateAll(fileName).OfType<ICpuIdX86>();

            Initialize();
        }

        public void LoadCpu(ICpuIdX86 cpu)
        {
            ThrowHelper.ThrowIfNull(cpu);
            Cpu = cpu;
            Cpus = new ICpuIdX86[] { cpu };
            Initialize();
        }

        private void Initialize()
        {
            Expected.Clear();
            Missing.Clear();
            Additional.Clear();
        }

        public ICpuIdX86 Cpu { get; private set; }

        public IEnumerable<ICpuIdX86> Cpus { get; private set; }

        public FeatureCheck GetFeatureCpu(ICpuIdX86 cpu)
        {
            FeatureCheck newFeature = new();
            newFeature.LoadCpu(cpu);
            foreach (string group in m_FeatureSet.Keys) {
                foreach (FeatureSet set in m_FeatureSet[group]) {
                    newFeature.AddFeatureSet(group, set.Name, set.Set);
                }
            }
            return newFeature;
        }

        public void Check(string group, params uint[] registers)
        {
            ThrowHelper.ThrowIfNull(group);
            ThrowHelper.ThrowIfNull(registers);
            if (registers.Length == 0) throw new ArgumentException("No registers to check are provided", nameof(registers));
            if (Cpu is null) throw new InvalidOperationException("CPU not loaded");

            if (!m_FeatureSet.TryGetValue(group, out List<FeatureSet> features)) {
                string message = string.Format("Group '{0}' not found", group);
                throw new ArgumentException(message, nameof(group));
            }

            if (registers.Length > features.Count)
                throw new InvalidOperationException("More registers provided to check than in the feature set check list");

            for (int i = 0; i < registers.Length; i++) {
                CalculateFeatures(registers[i], features[i]);
            }
            CheckFeatures();
        }

        private void CalculateFeatures(uint reg, FeatureSet featureSet)
        {
            int bitMask = 1;
            for (int i = 0; i < 32; i++) {
                if ((reg & bitMask) != 0 && featureSet.Set[i] is not null) {
                    string regName = featureSet.Set[i];
                    if (regName.Equals(string.Empty)) regName = string.Format("{0}[{1}]", featureSet.Name, i);
                    Expected.Add(regName);
                }
                bitMask <<= 1;
            }
        }

        private void CheckFeatures()
        {
            Missing.Clear();
            Additional.Clear();

            foreach (string feature in Cpu.Features) {
                bool expected = Expected.Contains(feature);
                bool present = Cpu.Features[feature].Value;
                if (expected && !present) {
                    Missing.Add(feature);
                } else if (!expected && present) {
                    Additional.Add(feature);
                }
            }

            foreach (string feature in Expected) {
                if (!Cpu.Features[feature].Value) {
                    Missing.Add(feature);
                }
            }
        }

        public void AssertOnDifference()
        {
            if (Cpu is null) throw new InvalidOperationException("CPU not loaded");

            if (Missing.Count == 0 && Additional.Count == 0) return;

            StringBuilder featureMissing = new();
            foreach (string feature in Missing) {
                if (featureMissing.Length > 0) featureMissing.Append(", ");
                featureMissing.Append(feature);
            }
            if (featureMissing.Length == 0) featureMissing.Append('-');

            StringBuilder featurePresent = new();
            foreach (string feature in Additional) {
                if (featurePresent.Length > 0) featurePresent.Append(", ");
                featurePresent.Append(feature);
            }
            if (featurePresent.Length == 0) featurePresent.Append('-');

            string message = string.Format("Missing Features: CPU has {0}; missing {1}", featurePresent, featureMissing);
            Assert.Fail(message);
        }

        public void AssertOnMissingDescription()
        {
            if (Cpu is null) throw new InvalidOperationException("CPU not loaded");

            // All features in the test case should have a description. An error here indicates a problem in the test
            // case.
            HashSet<string> missing = new();
            HashSet<string> knownFeatures = new();
            foreach (List<FeatureSet> group in m_FeatureSet.Values) {
                foreach (FeatureSet featureSet in group) {
                    foreach (string feature in featureSet.Set) {
                        if (!string.IsNullOrEmpty(feature)) {
                            // The feature will be marked as Reserved if it isn't loaded already (because it wasn't
                            // tested), but the description is still valid.
                            knownFeatures.Add(feature);
                            if (string.IsNullOrWhiteSpace(Cpu.Features[feature].Description)) {
                                missing.Add(feature);
                            }
                        }
                    }
                }
            }

            // All features (which are reserved) should have a description.
            foreach (string feature in Cpu.Features) {
                if (!string.IsNullOrEmpty(feature)) {
                    CpuFeature cpuFeature = Cpu.Features[feature];
                    if (!feature.Equals(cpuFeature.Feature, StringComparison.InvariantCultureIgnoreCase) &&
                        knownFeatures.Contains(cpuFeature.Feature)) {
                        // Add the alias to the already known feature.
                        knownFeatures.Add(feature);
                    }

                    // Only check if the feature is not marked as reserved (as it's expected there will be no description)
                    if (!Cpu.Features[feature].IsReserved && string.IsNullOrWhiteSpace(Cpu.Features[feature].Description)) {
                        missing.Add(feature);
                    }
                }
            }

            if (missing.Count > 0) {
                StringBuilder missingText = new();
                foreach (string entry in missing) {
                    if (missingText.Length != 0) missingText.Append(", ");
                    missingText.Append(entry);
                }
                Assert.Fail("Missing descriptions for: {0}", missingText);
            }

            // Now check if we're missing a feature in our feature set in the test case. Indicates a feature was added
            // but the feature set for the test case wasn't added.
            foreach (string feature in Cpu.Features) {
                if (!string.IsNullOrEmpty(feature)) {
                    CpuFeature cpuFeature = Cpu.Features[feature];

                    // Only check if the feature is not marked as reserved (as it's expected there test case doesn't know
                    // about it)
                    if (!knownFeatures.Contains(feature) && !cpuFeature.IsReserved) {
                        missing.Add(feature);
                    }
                }
            }

            if (missing.Count > 0) {
                StringBuilder missingText = new();
                foreach (string entry in missing) {
                    if (missingText.Length != 0) missingText.Append(", ");
                    missingText.Append(entry);
                }
                Assert.Fail("Test case doesn't know about feature: {0}", missingText);
            }
        }

        public void AssertCoreTopo(CpuTopoType topoType, int id, int mask)
        {
            foreach (CpuTopo cpuTopo in Cpu.Topology.CoreTopology) {
                if (cpuTopo.TopoType == topoType && cpuTopo.Id == id) {
                    Assert.That(cpuTopo.Mask, Is.EqualTo(mask),
                        "CPU Topo '{0}' of id {1} mask mismatch", topoType.ToString(), id);
                    return;
                }
            }

            Assert.Fail("CPU Topo '{0}' of id {1} not found", topoType.ToString(), id);
        }
    }
}
