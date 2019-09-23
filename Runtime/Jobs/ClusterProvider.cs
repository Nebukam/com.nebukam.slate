﻿// Copyright (c) 2019 Timothé Lapetite - nebukam@gmail.com.
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

using System;
using System.Collections.Generic;
using Nebukam.JobAssist;
using Unity.Collections;


namespace Nebukam.Slate
{

    public interface IClusterProvider<T, S> : IProcessor
        where S : ISlot
        where T : struct, ISlotInfos<S>
    {
        ISlotCluster<S> slotCluster { get; }
        List<S> lockedSlots { get; }
        NativeArray<T> outputSlotInfos { get; }
        NativeHashMap<ByteTrio, int> outputSlotCoordinateMap { get; }
    }

    public class ClusterProvider<T, S> : Processor<Unemployed>, IClusterProvider<T, S>
        where S : Slot, ISlot
        where T : struct, ISlotInfos<S>
    {

        protected ISlotCluster<S> m_slotCluster = null;
        protected List<S> m_lockedSlots = new List<S>();
        protected NativeArray<T> m_outputSlotInfos = new NativeArray<T>(0, Allocator.Persistent);
        protected NativeHashMap<ByteTrio, int> m_outputSlotCoordMap = new NativeHashMap<ByteTrio, int>(0, Allocator.Persistent);

        public ISlotCluster<S> slotCluster { get { return m_slotCluster; } }
        public List<S> lockedSlots { get { return m_lockedSlots; } }
        public NativeArray<T> outputSlotInfos { get { return m_outputSlotInfos; } }
        public NativeHashMap<ByteTrio, int> outputSlotCoordinateMap { get { return m_outputSlotCoordMap; } }

        protected override void InternalLock()
        {
            int count = m_slotCluster.Count;
            m_lockedSlots.Clear();
            m_lockedSlots.Capacity = count;
            for (int i = 0; i < count; i++) { m_lockedSlots.Add(m_slotCluster[i]); }
        }

        protected override void Prepare(ref Unemployed job, float delta)
        {

            int slotCount = m_lockedSlots.Count;

            if (m_outputSlotInfos.Length != slotCount)
            {
                m_outputSlotInfos.Dispose();
                m_outputSlotInfos = new NativeArray<T>(slotCount, Allocator.Persistent);
            }

            m_outputSlotCoordMap.Clear();

            S slot;
            T slotInfos;

            for(int i = 0; i < slotCount; i++)
            {

                slot = m_lockedSlots[i];
                slotInfos = new T();
                slotInfos.Capture(slot);

                m_outputSlotInfos[i] = slotInfos;
                m_outputSlotCoordMap.TryAdd(slot.m_coordinates, i);

            }

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
