﻿using System;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using Cage.Parameters;

namespace Cage.Components
{
    public class HitByRayComponent : GH_Component
    {
        public HitByRayComponent() : base("cage Hit by Ray", "HitByRay", "", "Cage", "RTree")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new RTreeParameter(), "RTree", "R", "", GH_ParamAccess.item);
            pManager.AddPointParameter("Origin", "P", "", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "D", "", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometries", "G", "", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Indices", "i", "", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // --- input

            var rtree = default(RTree);
            var origin = default(Point3d);
            var direction = default(Vector3d);

            if (DA.GetData(0, ref rtree)) return;
            if (DA.GetData(1, ref origin)) return;
            if (DA.GetData(2, ref direction)) return;

            // --- compute

            var indices = rtree.HitByRay(origin, direction);

            // --- output

            DA.SetDataList(0, indices.Select(o => rtree.Geometries[o]));
            DA.SetDataList(1, indices);
        }

        protected override Bitmap Icon => null;

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        public override Guid ComponentGuid => new Guid("{9E596F9E-0A08-426B-AD44-FD7B5427CF11}");
    }
}