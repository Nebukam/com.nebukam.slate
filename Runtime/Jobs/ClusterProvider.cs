// Copyright (c) 2019 Timothé Lapetite - nebukam@gmail.com.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
