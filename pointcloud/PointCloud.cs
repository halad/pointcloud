using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using FastBitmapLib;

namespace pointcloud
{

    public class BoudingBox
    {
        public Vector3 Min { get; set; }

        public Vector3 Max { get; set; }
    }

    public class Grid
    {
        /// <summary>
        /// Grid Width (number of colums)
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Grid Height (number of rows)
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Number of valid indices
        /// </summary>
        public int ValidCount { get; set; }

        /// <summary>
        /// Minimum Valid Index
        /// </summary>
        public int MinValidIndex { get; set; }

        /// <summary>
        /// Maximum Valid Index
        /// </summary>
        public int MaxValidIndex { get; set; }

        /// <summary>
        /// Sensor position relative to the cloud points
        /// </summary>
        public ScannerPosition ScannerPosition { get; set; }

        /// <summary>
        /// Grid indices (size:  w x h)
        /// </summary>
        public IList<int> _indices;

        /// <summary>
        /// Grid colors (size: w x h)
        /// </summary>
        public IList<FastColor> _colors; 

        /// <summary>
        /// Converts gri to an RGM image
        /// </summary>
        public FastBitmap ToImage()
        {
            var bitmap = new FastBitmap(Width, Height);
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++)
                {
                    var color = _colors[j * Width + i];
                    bitmap.SetPixel(i,j, color);
                }
            }

            return bitmap;
        }

        public void ReadPoints(StreamReader sr)
        {
            var firstPoint = true;
            var hasColors = false;
            var loadColors = false;
            var loadGridColors = false;
            int gridIndex = 0;

            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < Width; j++, gridIndex++)
                {
                    //var color = _colors[j * Width + i];
                    var line = sr.ReadLine();
                    var tokens = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    if (firstPoint)
                    {
                        hasColors = (tokens.Length == 7);   
                    }

                    var values = new double[4];
                    for (var k = 0; k < 4; k++)
                    {
                        values[k] = double.Parse(tokens[k]);
                    }

                    var pointIsValid = (Math.Pow(values[0] * values[1], 2) != 0);
                    if (!pointIsValid) continue; //skip empty cells

                    //if first point: check for big coordinates
                    if (firstPoint)
                    {
                        firstPoint = false;
                    }

                    //add point
                   // var point = new Vector3(values[0], values[1], values[2]);
                    //point[0], point[1], point[2] + PshiftCloud

                }
            }

        }

    }

    /// <summary>
    /// 3D point cloud with associated features (color, normals, scalar fields, etc.)
    /// color:  RGB
    /// normals: compressed
    /// scalar fields:
    /// octree structure
    /// per-point visibility information
    /// other children object
    /// 
    /// with index-based 
    /// and persistent access to points.
    /// </summary>
    public class PointCloud
    {
        public PointCloud()
        {
        }

        //some sort of points container
        //matrix? 
        // split by first dimension (x) or use grid structure
        //

        /// <summary>
        /// Number of points, cloud size
        /// </summary>
        public long NumberOfPoints { get; set; }


        public Matrix4x4 Transformation { get; set; }

        // get bounding bod
        //Cloud Bounding Box Limits
        //public  BoundingBox { get; set; }

        //ForEach (fast iteration mechanism)

        /*//! Returns a given point visibility state (relatively to a sensor for instance)
	/**	Generic method to request a point visibility
         * (should be overloaded if this functionality is required).
		The point visibility is such as defined in Daniel Girardeau-Montaut's PhD manuscript (see Chapter 2, 
		section 2-3-3). In this case, a ground based laser sensor model should be used to determine it.
		This method is called before performing any point-to-cloud comparison. If the result is not
		POINT_VISIBLE, then the comparison won't be performed and the scalar field value associated
		to this point will be this visibility value.
		\param P the 3D point to test
		\return visibility (default: POINT_VISIBLE)
	**/
        // virtual inline unsigned char testVisibility(const CCVector3& P) const { return POINT_VISIBLE; }

        //! Sets the cloud iterator at the beginning
        /**	Virtual method to handle the cloud global iterator
        **/
        // virtual void placeIteratorAtBegining() = 0;

        //! Returns the next point (relatively to the global iterator position)
        /**	Virtual method to handle the cloud global iterator.
            Global iterator position should be increased by one each time
            this method is called.
            Warning:
            - the returned object may not be persistent!
            - THIS METHOD MAY NOT BE COMPATIBLE WITH PARALLEL STRATEGIES
            (see the DgmOctree::executeFunctionForAllCellsAtLevel_MT and
            DgmOctree::executeFunctionForAllCellsAtStartingLevel_MT methods).
            \return pointer on next point (or 0 if no more)
        **/
        //h	virtual const CCVector3* getNextPoint() = 0;

        //!	Enables the scalar field associated to the cloud
        /** If the scalar field structure is not yet initialized/allocated,
            this method gives the signal for its creation. Otherwise, if possible
            the structure size should be pre-reserved with the same number of
            elements as the point cloud.
        **/
        //virtual bool enableScalarField() = 0;

        //! Returns true if the scalar field is enabled, false otherwise
        //virtual bool isScalarFieldEnabled() const = 0;

        //! Sets the ith point associated scalar value
        //   virtual void setPointScalarValue(unsigned pointIndex, ScalarType value) = 0;

        //! Returns the ith point associated scalar value
        //	virtual ScalarType getPointScalarValue(unsigned pointIndex) const = 0;
        //   */

        //! Returns the ith point
        /**	Virtual method to request a point with a specific index.
            WARNINGS:
            - the returned object may not be persistent!
            - THIS METHOD MAY NOT BE COMPATIBLE WITH PARALLEL STRATEGIES
            (see the DgmOctree::executeFunctionForAllCellsAtLevel_MT and
            DgmOctree::executeFunctionForAllCellsAtStartingLevel_MT methods).
            Consider the other version of getPoint instead or the 
            GenericIndexedCloudPersist class.
            \param index of the requested point (between 0 and the cloud size minus 1)
            \return the requested point (undefined behavior if index is invalid)
        **/
        //virtual const CCVector3* getPoint(unsigned index) = 0;

        //! Returns the ith point
        /**	Virtual method to request a point with a specific index.
            Index must be valid (undefined behavior if index is invalid)
            \param index of the requested point (between 0 and the cloud size minus 1)
            \param P output point
        **/
        //virtual void getPoint(unsigned index, CCVector3& P) const = 0;
    }
}