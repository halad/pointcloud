using System;
using System.IO;
using System.Linq;
using System.Numerics;

namespace pointcloud
{
    class Program
    {
        static void Main(string[] args)
        {
            var filepath = args[0]; //or file browser

            // 1. (Initiall) read points into memory 
            //      Assume registered and translated/transformed
            // 2.  Create octree structure
            // 3.  Load into SQL Server Express or another relational db for now we have SQL express installed so we'll go with that
            //      Schema = PointCloud
            // 4. Goal of PoC:  Be able to query by bounding box, max/min vectors

            //Console.WriteLine("Hello World!");

        }


        /*
        public static void ReadPtxFile(string filepath)
        {
            var pointcloud = new PointCloud();
            var lineNumber = 0;

            var pointCloud = new PointCloud();

            using (var sr = new StreamReader(filepath))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var numCols = int.Parse(line);  

                    line = sr.ReadLine();
                    var numRows = int.Parse(line);

                    //grid size of scan:  {numCols} x {numRow}

                    //sensor transformation matrix
                    //translation
                    var scannerPos = sr.ReadLine()?.Split(" ").Select(float.Parse).ToList();

                    //x,y,z axes
                    var scannerX = sr.ReadLine()?.Split(" ").Select(float.Parse).ToList();
                    var scannerY = sr.ReadLine()?.Split(" ").Select(float.Parse).ToList();
                    var scannerZ = sr.ReadLine()?.Split(" ").Select(float.Parse).ToList();

                    pointCloud.ScannerPosition = new ScannerPosition(scannerPos, scannerX, scannerY, scannerZ);

                    lineNumber += 4;

                    //cloud transformation matrix
                    var transform1 = sr.ReadLine()?.Split(" ").Select(float.Parse).ToList();
                    var transform2 = sr.ReadLine()?.Split(" ").Select(float.Parse).ToList();
                    var transform3 = sr.ReadLine()?.Split(" ").Select(float.Parse).ToList();
                    var transform4 = sr.ReadLine()?.Split(" ").Select(float.Parse).ToList();

                    var transform = new Matrix4x4(
                        transform1[0], transform1[1], transform1[2],transform1[3],
                        transform2[0], transform2[1], transform2[2], transform2[3],
                        transform3[0], transform3[1], transform3[2], transform3[3],
                        transform4[0], transform4[1], transform4[2], transform4[3]
                        );

                    pointCloud.Transformation = transform;

                    lineNumber += 4;

                  //now we read the grid cells


                    Console.WriteLine(line);
                }
            }
        }
    }
    */

        /*
         *
         *PTX point cloud header: 

            number of columns
            number of rows 

            st1 st2 st3 ; scanner registered position 
            sx1 sx2 sx3 ; scanner registered axis 'X' 
            sy1 sy2 sy3 ; scanner registered axis 'Y' 
            sz1 sz2 sz3 ; scanner registered axis 'Z' 

            ^ The first four lines of three numbers each are the position 
            ^ and primary axes of the scanner after any registration/transformation. 

            r11 r12 r13 0 ; transformation matrix 
            r21 r22 r23 0 ; this is a simple rotation and translation 4x4 matrix 
            r31 r32 r33 0 ; just apply to each point to get the transformed coordinate 
            tr1 tr2 tr3 1 ; use double-precision variables 

          ^The next four lines of four numbers each may look similar in some cases, 
          ^but if you have a non-identity UCS when the PTX was exported, 
          ^the numbers will look different. 
          ^If the cloud was untransformed by a registration (or not registered), 
          ^the first four lines of three numbers each would be 0,0,0; 1,0,0; 0,1,0; 0,0,1. 
          ^The 4x4 matrix may not be identity if there is a UCS applied or 
          ^UCS that is set to that scanner's registered position. 

         *the RGB value (0, 0, 0) is reserved to mean "no color".
         *
         * Cyclone exports PTX with 7 columns when the cloud has RGB values
         * from the digial camera (x, y, z, intensity, red, green, blue).
         * Red, Green, Blue have the integer range [0, 255]. 
         *
         * A cloud in PTX has 4 columns (x, y, z, intensity) when the cloud does not have RGB values.
         * PTX intensities use the decimal range [0, 1].
         * Individual values on the same line are separated by a blank space.
         * The coordinate unit is always in meters. 
         */

        //public class PtxReader
        //{
        //    public int NumRows { get; private set; }
        //    public int NumColumns { get; private set; }


        //    public void ProcessLine(string line, int lineNumber)
        //    {
        //        var pointcloud = new PointCloud();

        //        switch (lineNumber)
        //        {
        //            case 0:
        //                NumColumns = int.Parse(line);
        //                break;
        //            case 1:
        //                NumRows = int.Parse(line);
        //                break;


        //        }
        //    }
        //
    }

    public class HsfReader
    {

    }
}
