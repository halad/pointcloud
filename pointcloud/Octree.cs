using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace pointcloud
{
    /// <summary>
    /// Corresponds to the octree structure developed during Daniel 
    /// Girardeau-Montaut's PhD (see PhD manuscript, Chapter 4).
    /// </summary>
    public class Octree
    {
        /// <summary>
        /// Associated cloud
        /// </summary>
        private readonly PointCloud _cloud;

        private int _numberOfProjectedPoints;

        /// <summary>
        /// Min coordinates of the octree bounding box
        /// </summary>
        private Vector3 _bbMin;

        /// <summary>
        /// Max coordinates of the octree bounding box
        /// </summary>
        private Vector3 _bbMax;

        /// <summary>
        /// Min coordinates of the bounding box of the set of points projected in the octree
        /// </summary>
        private Vector3 _pointsMin;

        /// <summary>
        /// Max coordinates of the bounding box of the set of points projected in the octree
        /// </summary>
        private Vector3 _pointsMax;

        /// <summary>
        /// Min and Max occupied cell indices, for all dimensions and every subdivision level
        /// </summary>
        private int[] _fillIndices;

        public bool Is64Bit { get; set; }


        /// <summary>
        /// Returns the binary shift for a given level of subdivision
        /// This binary shift is used to truncate a full cell code in order
        /// to deduce the cell code for a given level of subdivision
        /// </summary>
        /// <param name="level">Level of the subdivision</param>
        /// <returns>Binary shift</returns>
        public static short GetBitShift(short level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the octree length (in term of cells) for a given level of
        /// subdivision
        /// </summary>
        /// <param name="level">Level of subdivision</param>
        /// <returns>2^level</returns>
        public static int Length(int level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Max octree subdivision level
        /// Number of bits used to code the cells position: 3*MaxLevel
        /// For 64 bit, max is 21 levels but twice more memory!
        /// </summary>
        /// <returns></returns>
        public int MaxLevel => Is64Bit ? 21 : 10;

        /// <summary>
        /// Max length (number of cells) at last level of subdivision
        /// </summary>
        public int MaxLength => 1 << MaxLevel;

        /// <summary>
        /// Type of the code of an octree cell
        /// 3 bits per level required
        /// </summary>
        public Type CellCodeType => Is64Bit ? typeof(ulong) : typeof(long);

        /// <summary>
        /// codes container
        /// </summary>
        private IList _cellCodes;

        /// <summary>
        /// indices container
        /// </summary>
        private IList<uint> _cellIndexes;

        public Octree(PointCloud cloud) : this()
        {
            _cloud = cloud;
        }

        public Octree()
        {
            _cellCodes = Is64Bit ? (IList) new List<ulong>() : new List<uint>();
            _cellIndexes = new List<uint>();
        }


        private void Clear()
        {
            _numberOfProjectedPoints = 0;
            _bbMin = new Vector3(0);
            _pointsMin = new Vector3(0);
            _pointsMax = new Vector3(0);
            _bbMin = new Vector3(0);

            _fillIndices = new int[(MaxLevel+1)*6];

        }


        /// <summary>
        /// Structure used during nearest neighbor search
        /// Association between a point, its index, and its square distance to the query point.
        /// Comparison operator for fast sorting
        /// </summary>
        struct PointDescriptor 
        {
            /// <summary>
            /// Point
            /// </summary>
            private Vector3 _point;

            /// <summary>
            /// Point Index
            /// </summary>
            private uint _pointIndex;

            /// <summary>
            /// Point assocated distance value
            /// </summary>
            private double _squareDistance;

            //public PointDescriptor() 
            //{
            //    _point = new Vector3(0);
            //    _pointIndex = 0;
            //    _squareDistance = -1.0d;
            //}

            public PointDescriptor(Vector3 point, uint index)
            {
                _point = point;
                _pointIndex = index;
                _squareDistance = -1.0d;
            }

            public PointDescriptor(Vector3 point, uint index, double d2)
            {
                _point = point;
                _pointIndex = index;
                _squareDistance = d2;
            }

            /// <summary>
            /// Return whether the square distance associated to a is smaller 
            /// than the square distance assocated with b
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static bool DistanceCompare(PointDescriptor a, PointDescriptor b)
            {

                return a._squareDistance < b._squareDistance;
            }
        }

        /// <summary>
        /// Set of neighbors
        /// </summary>
        private IList<PointDescriptor> _neighborSet;

        /// <summary>
        /// Structure used during nearest neighbor search
        /// </summary>
        struct CellDescriptor
        {
            /// <summary>
            /// Cell center
            /// </summary>
            private Vector3 _center;

            /// <summary>
            /// First point index in associated neigbor set
            /// </summary>
            private uint _index;

            public CellDescriptor(Vector3 center, uint i)
            {
                _center = center;
                _index = i;
            }

        }

        /// <summary>
        /// A set of neighbor cells 
        /// </summary>
        private IList<CellDescriptor> _neighborCellsSet;

        /// <summary>
        /// Container of in/out parameters for nearest neighbor search
        /// Useful when searching nearest neighbors around points that lie
        /// in the same octree cell.
        /// Information about the cell is given to the search algorithm through this
        /// structure. 
        /// </summary>
        struct NearestNeighborSearch
        {
            /// <summary>
            /// query point, updated with every iteration
            /// </summary>
            private Vector3 _queryPoint;

            /// <summary>
            /// Level of subdivision at which to start the search
            /// Set once for all
            /// </summary>
            private short _level;

            /// <summary>
            /// Minimal number of neighbors to find 
            /// </summary>
            private uint _minNumberOfNeighbors;

            /// <summary>
            /// Position in the octree of the cell contianing the query point
            /// The position is expressed for the level of the subdivision at which the search will
            /// be processed. 
            /// </summary>
            private Vector<int> _cellposition;

            /// <summary>
            /// Coodinates of the center of the cell including the query point
            /// </summary>
            private Vector3 _cellCenter;

            /// <summary>
            /// Max Neighbors Distance
            /// The search process will stop if it reaches this radius even if it hasn't found
            /// any neighbors. 
            /// To disable this behavior set to something less or equal to 0.
            /// </summary>
            private double _maxSearchSquareDistd;




        }
    }
}