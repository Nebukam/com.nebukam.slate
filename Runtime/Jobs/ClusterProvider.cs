using System.Collections.Generic;
using Nebukam.JobAssist;
using Unity.Collections;

namespace Nebukam.Slate
{

    public interface IClusterProvider<T> : IProcessor
        where T : struct, ISlotInfos
    {
        NativeArray<T> outputSlotInfos { get; }
        List<ISlot> lockedSlots { get; }
    }

    public class ClusterProvider<T> : Processor<Unemployed>, IClusterProvider<T>
        where T : struct, ISlotInfos
    {

        protected ISlotCluster<ISlot> m_slotCluster = null;
        protected List<ISlot> m_lockedSlots = new List<ISlot>();
        protected NativeArray<T> m_outputSlotInfos;

        public List<ISlot> lockedSlots { get { return m_lockedSlots; } }
        public NativeArray<T> outputSlotInfos { get { return m_outputSlotInfos; } }

        protected override void InternalLock()
        {
            //Copy slots ?
        }

        protected override void Prepare(ref Unemployed job, float delta)
        {
            //Fill slot list
        }

        protected override void Apply(ref Unemployed job) { }

        protected override void InternalUnlock() { }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing) { return; }

        }

    }
}
