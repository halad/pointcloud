using System.Collections.Generic;
using System.Numerics;

namespace pointcloud
{
    public class ScannerPosition
    {
        public ScannerPosition(IList<float> position, IList<float> xaxis, 
            IList<float> yaxis, IList<float> zaxis)
        {
            Position = new Vector3(position[0], position[1], position[2]);
            XAxis = new Vector3(xaxis[0], xaxis[1], xaxis[2]);
            YAxis = new Vector3(yaxis[0], yaxis[1], yaxis[2]);
            ZAxis = new Vector3(zaxis[0], zaxis[1], zaxis[2]);
        }
        public Vector3 Position { get; set; }
        public Vector3 XAxis { get; set; }
        public Vector3 YAxis { get; set; }

        public Vector3 ZAxis { get; set; }
    }
}