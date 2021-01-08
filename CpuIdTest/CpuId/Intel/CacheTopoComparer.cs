namespace RJCP.Diagnostics.CpuId.Intel
{
    using System.Collections.Generic;

    internal class CacheTopoComparer : IEqualityComparer<CacheTopo>
    {
        public bool Equals(CacheTopo x, CacheTopo y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            if (x.GetType() != y.GetType()) return false;

            if (x.CacheType != y.CacheType || x.Level != y.Level) return false;

            if (x is CacheTopoCpu xc) {
                CacheTopoCpu yc = (CacheTopoCpu)y;
                return
                    xc.Associativity == yc.Associativity &&
                    xc.LineSize == yc.LineSize &&
                    xc.Sets == yc.Sets &&
                    xc.Size == yc.Size;
            } else if (x is CacheTopoTlb xt) {
                CacheTopoTlb yt = (CacheTopoTlb)y;
                return
                    xt.Associativity == yt.Associativity &&
                    xt.Sets == yt.Sets &&
                    xt.Entries == yt.Entries;
            }
            return false;
        }

        public int GetHashCode(CacheTopo obj)
        {
            return obj.GetHashCode();
        }
    }
}
