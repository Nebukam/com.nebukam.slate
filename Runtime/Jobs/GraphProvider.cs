using System.Collections.Generic;
using Nebukam.JobAssist;
using Unity.Collections;

namespace Nebukam.Slate
{
    /*
    public interface IGraphProvider : IProcessor
    {
        NativeArray<NodeInfos> outputNodeInfos { get; }
        NativeArray<int> outputNeighborsIndices { get; }
        List<INode> lockedNodes { get; }
    }

    public class GraphProvider : Processor<Unemployed>
    {

        protected ISlate<INode> m_slate = null;
        protected List<INode> m_lockedNodes = new List<INode>();
        protected Dictionary<INode, int> m_nodeIndices = new Dictionary<INode, int>();
        protected NativeArray<NodeInfos> m_outputNodeInfos = new NativeArray<NodeInfos>(0, Allocator.Persistent);
        protected NativeArray<int> m_outputNeighborsIndices = new NativeArray<int>(0, Allocator.Persistent);
        protected int
            m_lockedNeighborsIndicesCount = 0,
            m_neighborIndicesCount = 0,
            m_lockedNodeCount = 0,
            m_nodeCount = 0;

        public List<INode> lockedNodes { get { return m_lockedNodes; } }
        public NativeArray<NodeInfos> outputNodeInfos { get { return m_outputNodeInfos; } }
        public NativeArray<int> outputNeighborsIndices { get { return m_outputNeighborsIndices; } }

        protected override void InternalLock()
        {
            m_lockedNodeCount = m_slate == null ? 0 : m_slate.Count;

            m_lockedNodes.Clear();
            m_lockedNodes.Capacity = m_lockedNodeCount;

            m_nodeIndices.Clear();
            m_lockedNeighborsIndicesCount = 0;

            INode node;
            for(int i = 0; i < m_lockedNodeCount; i++)
            {
                node = m_slate[i];
                m_lockedNodes.Add(node);
                m_nodeIndices[node] = i;
                m_lockedNeighborsIndicesCount += node.neighbors.Count;
            }

        }

        protected override void Prepare(ref Unemployed job, float delta)
        {

            if (m_nodeCount != m_lockedNodeCount)
            {
                m_nodeCount = m_lockedNodeCount;
                m_outputNodeInfos.Dispose();
                m_outputNodeInfos = new NativeArray<NodeInfos>(m_nodeCount, Allocator.Persistent);
            }

            if(m_neighborIndicesCount != m_lockedNeighborsIndicesCount)
            {
                m_neighborIndicesCount = m_lockedNeighborsIndicesCount;
                m_outputNeighborsIndices.Dispose();
                m_outputNeighborsIndices = new NativeArray<int>(m_neighborIndicesCount, Allocator.Persistent);
            }

            INode node;
            List<INode> neighbors;
            int index = 0, nCount;
            for (int i = 0; i < m_nodeCount; i++)
            {
                node = m_lockedNodes[i];
                neighbors = node.neighbors;
                nCount = neighbors.Count;

                m_outputNodeInfos[i] = new NodeInfos()
                {
                    index = i,
                    start = index,
                    length = nCount
                };

                for (int n = 0; n < nCount; n++)
                    m_outputNeighborsIndices[index++] = m_nodeIndices[neighbors[n]];

            }


        }

        protected override void Apply(ref Unemployed job) { }

        protected override void InternalUnlock() { }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing) { return; }

            m_outputNodeInfos.Dispose();

        }

    }
    */
}
