using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace Nebukam.Slate
{

    public interface ISlotModel
    {
        float3 size { get; set; }
    }

    public class SlotModel : Nebukam.Pooling.PoolItem, ISlotModel
    {
        protected internal float3 m_slotSize = float3(0f);
        protected internal float3 m_anchor = float3(0f);
        protected internal float3 m_offset = float3(0f);

        public float3 size
        {
            get { return m_slotSize; }
            set { m_slotSize = value; }
        }

        
        public float3 anchor
        {
            get { return m_anchor; }
            set
            {
                m_anchor = value;
                m_offset = m_slotSize * m_anchor;
            }
        }


    }
}
