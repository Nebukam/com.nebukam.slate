using System.Collections.Generic;

namespace Nebukam.Slate
{

    public interface IGraph<out V> : IEditableVertexGroup<V>
        where V : INode
    {

    }

    public interface IGraph : IGraph<INode> { }
    
    public class Graph<V> : VertexGroup<V>, IGraph<V>
        where V : Node, INode, new()
    {

        protected override void OnVertexAdded(V v)
        {
            base.OnVertexAdded(v);
            IGraph was = v.m_graph;
            v.m_graph = this as IGraph;
            if (was != null)
                was.Remove(v);
        }

        protected override void OnVertexRemoved(V v)
        {
            base.OnVertexRemoved(v);
            if (v.m_graph == this)
                v.m_graph = null;
        }

    }
}
