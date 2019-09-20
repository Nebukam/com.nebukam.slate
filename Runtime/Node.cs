using System.Collections.Generic;

namespace Nebukam.Slate
{

    public interface INode : IVertex
    {
        IGraph graph { get; }
        int Count { get; }
        INode this[int i] { get; }
    }

    public abstract class Node : Vertex, INode
    {

        protected internal IGraph m_graph = null;
        protected internal List<INode> m_neighbors = new List<INode>();
        
        public IGraph graph { get { return m_graph; } }
        public List<INode> neighbors { get { return m_neighbors; } }
        public int Count { get { return m_neighbors.Count; } }
        public INode this[int i] { get { return m_neighbors[i]; } }

    }
}
