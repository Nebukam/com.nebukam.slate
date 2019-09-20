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

using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;

namespace Nebukam.Slate
{

    public interface ISlotCluster<out V> : IVertexGroup<V>
        where V : ISlot
    {

        /// <summary>
        /// Model used for formatting slots
        /// </summary>
        SlotModel slotModel { get; set; }

        /// <summary>
        /// Cluster's slot size
        /// </summary>
        ByteTrio size { get; set; }

        /// <summary>
        /// Cluster's bounds
        /// </summary>
        Bounds bounds { get; }

        /// <summary>
        /// Clusters finite capacity
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Wrap mode over X axis
        /// </summary>
        WrapMode wrapX { get; set; }

        /// <summary>
        /// Wrap mode over Y axis
        /// </summary>
        WrapMode wrapY { get; set; }

        /// <summary>
        /// Wrap mode over Z axis
        /// </summary>
        WrapMode wrapZ { get; set; }

        /// <summary>
        /// Return the slot index of the given coordinates
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        int IndexOf(ByteTrio coord);

        /// <summary>
        /// Retrieve the coordinates that contain the given location,
        /// based on cluster location & slot's size
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        bool TryGetCoordOf(float3 location, out ByteTrio coord);

        void Init(ByteTrio clusterSize, SlotModel clusterSlotModel, bool fillCluster);

        /// <summary>
        /// Set the slot occupation at a given coordinate.
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="slot"></param>
        /// <param name="releaseExisting"></param>
        /// <returns></returns>
        V Set(ByteTrio coord, ISlot slot, bool releaseExisting = false);

        /// <summary>
        /// Create a slot at the given coordinates and return it.
        /// If a slot already exists at the given location, that slot is returned instead.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        V Add(ByteTrio coord);

        /// <summary>
        /// Remove the slot at the given coordinates and returns it.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        V Remove(ByteTrio coord);

        /// <summary>
        /// Gets the slot associated with the specified coordinates.
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        /// <remarks>Input coordinates are wrapped per-axis using each of the cluster's individual wrap mode.</remarks>
        bool TryGet(ByteTrio coord, out ISlot slot);

        /// <summary>
        /// Gets the slot associated with the specified coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        /// <remarks>Input coordinates are wrapped per-axis using each of the cluster's individual wrap mode.</remarks>
        bool TryGet(int x, int y, int z, out ISlot slot);

        /// <summary>
        /// Gets the slot containing the specified location.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        bool TryGet(float3 location, out ISlot slot);

        /// <summary>
        /// Return the world-space projection of the given coordinates, as projected by this cluster.
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        float3 ComputePosition(ref ByteTrio coords);

        /// <summary>
        /// Fills all empty slots in this cluster with.
        /// </summary>
        void Fill();
        void Clear(bool release = false);

    }

    /// <summary>
    /// A GridChunk represent a 3D abstract grid with finite slot capacity.
    /// Each slot is stored at a given ByteTrio (x, y, z) location
    /// </summary>
    /// <typeparam name="V"></typeparam>
    public abstract class SlotCluster<V> : Vertex, ISlotCluster<V>, Pooling.IRequireCleanUp
        where V : Slot, ISlot, new()
    {

        protected internal SlotModel m_slotModel = null;
        protected internal ByteTrio m_size = ByteTrio.zero;
        protected internal WrapMode
            m_wrapX = WrapMode.NONE,
            m_wrapY = WrapMode.NONE,
            m_wrapZ = WrapMode.NONE;

        /// <summary>
        /// Model used for formatting slots
        /// </summary>
        public SlotModel slotModel
        {
            get { return m_slotModel; }
            set
            {
                m_slotModel = value;
                if (m_slotModel != null)
                {
                    OnModelChanged();
                }
            }
        }

        /// <summary>
        /// Return size difference on each individual X, Y & Z axis, compared
        /// to the previous size before it was updated
        /// </summary>
        /// <returns></returns>
        protected virtual void OnModelChanged()
        {
            UpdatePositions();
        }

        /// <summary>
        /// Cluster's slot size
        /// </summary>
        public ByteTrio size
        {
            get { return m_size; }
            set
            {
                if (m_size == value) { return; }
                ByteTrio oldSize = m_size;
                m_size = value;
                OnSizeChanged(oldSize);
            }
        }

        protected virtual int3 OnSizeChanged(ByteTrio oldSize)
        {
            return int3(
                m_size.x - oldSize.x,
                m_size.y - oldSize.y,
                m_size.z - oldSize.z);
        }

        public Bounds bounds
        {
            get
            {
                float3 s = m_size * m_slotModel.m_slotSize;
                return new Bounds(m_pos + s * 0.5f, s);
            }
        }

        /// <summary>
        /// Cluster;s finite capacity
        /// </summary>
        public int Capacity { get { return m_size.Volume(); } }

        /// <summary>
        /// Number of slots in the cluster
        /// </summary>
        public abstract int Count { get; }
        public abstract V this[int index] { get; }
        public abstract int this[IVertex v] { get; }

        /// <summary>
        /// Wrap mode over X axis
        /// </summary>
        public WrapMode wrapX
        {
            get { return m_wrapX; }
            set { m_wrapX = value; }
        }

        /// <summary>
        /// Wrap mode over Y axis
        /// </summary>
        public WrapMode wrapY
        {
            get { return m_wrapY; }
            set { m_wrapY = value; }
        }

        /// <summary>
        /// Wrap mode over Z axis
        /// </summary>
        public WrapMode wrapZ
        {
            get { return m_wrapZ; }
            set { m_wrapZ = value; }
        }

        /// <summary>
        /// Return the slot index of the given coordinates
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public abstract int IndexOf(ByteTrio coord);

        /// <summary>
        /// Retrieve the coordinates that contain the given location,
        /// based on cluster location & slot's size
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool TryGetCoordOf(float3 location, out ByteTrio coord)
        {

            if (!bounds.Contains(location))
            {
                coord = ByteTrio.zero;
                return false;
            }

            float3
                s = m_slotModel.size,
                loc = location - pos;

            float
                modX = loc.x % s.x,
                modY = loc.y % s.y,
                modZ = loc.z % s.z;

            int
                posX = (int)((loc.x - modX) / s.x),
                posY = (int)((loc.y - modY) / s.y),
                posZ = (int)((loc.z - modZ) / s.z);

            coord = new ByteTrio(posX, posY, posZ);
            coord.Clamp(ref m_size, ref m_wrapX, ref m_wrapY, ref m_wrapZ);
            return true;
        }

        public virtual void Init(ByteTrio clusterSize, SlotModel clusterSlotModel, bool fillCluster)
        {
            Clear(true);

            m_slotModel = clusterSlotModel;
            size = clusterSize;

            if (fillCluster)
                Fill();
        }

        /// <summary>
        /// Set the slot occupation at a given coordinate.
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="slot"></param>
        /// <param name="releaseExisting"></param>
        /// <returns></returns>
        public abstract V Set(ByteTrio coord, ISlot slot, bool releaseExisting = false);

        /// <summary>
        /// Create a slot at the given coordinates and return it.
        /// If a slot already exists at the given location, that slot is returned instead.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public abstract V Add(ByteTrio coord);

        /// <summary>
        /// Remove the slot at the given coordinates and returns it.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public abstract V Remove(ByteTrio coord);

        /// <summary>
        /// Gets the slot associated with the specified coordinates.
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        /// <remarks>Input coordinates are wrapped per-axis using each of the cluster's individual wrap mode.</remarks>
        public abstract bool TryGet(ByteTrio coord, out ISlot slot);

        /// <summary>
        /// Gets the slot associated with the specified coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        /// <remarks>Input coordinates are wrapped per-axis using each of the cluster's individual wrap mode.</remarks>
        public abstract bool TryGet(int x, int y, int z, out ISlot slot);

        /// <summary>
        /// Gets the slot containing the specified location.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public bool TryGet(float3 location, out ISlot slot)
        {

            if (TryGetCoordOf(location, out ByteTrio coord) && TryGet(coord, out slot))
                return true;

            slot = null;
            return false;

        }

        /// <summary>
        /// Callback when a slot is added to the cluster.
        /// </summary>
        /// <param name="slot"></param>
        protected virtual void OnSlotAdded(V slot)
        {
            slot.cluster = this;
            slot.pos = ComputePosition(ref slot.m_localCoordinates);
        }

        /// <summary>
        /// Callback when a slot is removed from the cluster.
        /// </summary>
        /// <param name="slot"></param>
        protected virtual void OnSlotRemoved(V slot)
        {
            if (slot.cluster == this)
                slot.cluster = null;
        }

        /// <summary>
        /// Return the world-space projection of the given coordinates, as projected by this cluster.
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual float3 ComputePosition(ref ByteTrio coords)
        {
            return pos + coords * m_slotModel.size + m_slotModel.m_offset;
        }

        /// <summary>
        /// Update all slot positions.
        /// </summary>
        protected abstract void UpdatePositions();

        /// <summary>
        /// Fills all empty slots in this cluster with.
        /// </summary>
        public abstract void Fill();

        /// <summary>
        /// Clear cluster & releases all slots.
        /// </summary>
        public abstract void Clear(bool release = false);

        #region PoolItem

        public virtual void CleanUp()
        {
            Clear();
            m_slotModel = null;
            m_size = int3(0);
            m_wrapX = WrapMode.NONE;
            m_wrapY = WrapMode.NONE;
            m_wrapZ = WrapMode.NONE;
        }

        #endregion

        #region Nearest vertex in group

        /// <summary>
        /// Return the vertex index in group of the nearest IVertex to a given IVertex v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public abstract int GetNearestVertexIndex(IVertex v);

        /// <summary>
        /// Return the the nearest IVertex in group to a given IVertex v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public abstract V GetNearestVertex(IVertex v);

        /// <summary>
        /// Return the vertex index in group of the nearest IVertex to a given v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public abstract int GetNearestVertexIndex(float3 v);

        /// <summary>
        /// Return the nearest IVertex in group of the nearest IVertex to a given v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public abstract V GetNearestVertex(float3 v);



        #endregion

    }
}
