namespace Nebukam.Slate
{

    public interface ISlot : IVertex
    {
        ISlotCluster<ISlot> cluster { get; set; }
    }

    public class Slot : Vertex, ISlot, Pooling.IRequireCleanUp
    {

        internal ByteTrio m_localCoordinates = ByteTrio.zero;
        internal ISlotCluster<ISlot> m_cluster = null;

        public ISlotCluster<ISlot> cluster
        {
            get { return m_cluster; }
            set { m_cluster = value; }
        }


        public virtual void CleanUp()
        {
            m_localCoordinates = ByteTrio.zero;
        }

    }
}
