using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiligentEngine.GltfPbr
{
    class PSOKey
    {
        public PbrAlphaMode AlphaMode = PbrAlphaMode.ALPHA_MODE_OPAQUE;
        public bool DoubleSided = false;
        public bool GetShadows = false;
    }

    class PSOCache : IDisposable
    {
        private List<AutoPtr<IPipelineState>> m_PSOCache = new List<AutoPtr<IPipelineState>>();

        static uint GetPSOIdx(PSOKey Key)
        {
            uint PSOIdx = 0;

            PSOIdx += (uint)(Key.GetShadows ? 1 : 0) << 2;
            PSOIdx += (uint)(Key.AlphaMode == PbrAlphaMode.ALPHA_MODE_BLEND ? 1 : 0) << 1;
            PSOIdx += (uint)(Key.DoubleSided ? 1 : 0);
            return PSOIdx;
        }

        /// <summary>
        /// Add a pso. Creates its own AutoPtr. Caller must dispose their AutoPtr.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="pPSO"></param>
        public void AddPSO(PSOKey Key, IPipelineState pPSO)
        {
            var Idx = GetPSOIdx(Key);
            if (m_PSOCache.Count <= Idx)
            {
                var start = m_PSOCache.Count;
                var end = (int)Idx + 1;
                m_PSOCache.Capacity = end;
                for (var i = start; i < end; ++i)
                {
                    m_PSOCache.Add(null);
                }
            }

            m_PSOCache[(int)Idx]?.Dispose();

            m_PSOCache[(int)Idx] = new AutoPtr<IPipelineState>(pPSO);
        }

        public IPipelineState GetPSO(PSOKey Key)
        {
            var Idx = GetPSOIdx(Key);
            return Idx < m_PSOCache.Count ? m_PSOCache[(int)Idx].Obj : null;
        }

        public void Dispose()
        {
            foreach (var pso in m_PSOCache.Where(i => i != null))
            {
                pso.Dispose();
            }
        }

        public IEnumerable<IPipelineState> Items =>
            m_PSOCache.Where(i => i != null).Select(i => i.Obj);
    }
}
