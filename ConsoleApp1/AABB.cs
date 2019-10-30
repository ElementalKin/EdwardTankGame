using System;
using System.Collections.Generic;
using System.Text;
using TankGame;
using Calculations;
namespace AABB
{
    class AABB
    {
        Vector3 min = new Vector3
       (float.NegativeInfinity,
        float.NegativeInfinity,
        float.NegativeInfinity);
        Vector3 max = new Vector3
       (float.PositiveInfinity,
        float.PositiveInfinity,
        float.PositiveInfinity);
        public AABB()
        {
        }
        public AABB(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }
        public Vector3 Center()
        {
            return (min + max) * 0.5f;
        }
        public Vector3 Extents()
        {
            return new Vector3(Math.Abs(max.x - min.x) * 0.5f,
            Math.Abs(max.y - min.y) * 0.5f,
            Math.Abs(max.z - min.z) * 0.5f);
        }
        public List<Vector3> Corners()
        {
            // ignoring z axis for 2D
            List<Vector3> corners = new List<Vector3>();
            corners.Add(min);
            corners.Add(new Vector3(min.x, max.y, min.z));
            corners.Add(max);
            corners.Add(new Vector3(max.x, min.y, min.z));
            return corners;
        }
        public void Fit(List<Vector3> points)
        {
            // invalidate the extents
            min = new Vector3(float.PositiveInfinity,
           float.PositiveInfinity,
           float.PositiveInfinity);
            max = new Vector3(float.NegativeInfinity,
           float.NegativeInfinity,
           float.NegativeInfinity);
            // find min and max of the points
            foreach (Vector3 p in points)
            {
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }
        }
        public bool Overlaps(Vector3 p)
        {
            // test for not overlapped as it exits faster
            return !(p.x < min.x || p.y < min.y ||
            p.x > max.x || p.y > max.y);
        }
        public bool Overlaps(AABB other)
        {
            // test for not overlapped as it exits faster
            return !(max.x < other.min.x || max.y < other.min.y ||
            min.x > other.max.x || min.y > other.max.y);
        }
        public Vector3 ClosestPoint(Vector3 p)
        {
            return Vector3.Clamp(p, min, max);
        }
        public void Fit(Vector3[] points)
        {
            // invalidate the extents
            min = new Vector3(float.PositiveInfinity,
            float.PositiveInfinity,
           float.PositiveInfinity);
            max = new Vector3(float.NegativeInfinity,
            float.NegativeInfinity,
           float.NegativeInfinity);
            // find min and max of the points
            foreach (Vector3 p in points)
            {
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }
        }
        public bool IsEmpty()
        {
            if (float.IsNegativeInfinity(min.x) &&
           float.IsNegativeInfinity(min.y) &&
           float.IsNegativeInfinity(min.z) &&
           float.IsInfinity(max.x) &&
           float.IsInfinity(max.y) &&
           float.IsInfinity(max.z))
                return true;
            return false;
        }
        public void Empty()
        {
            min = new Vector3(float.NegativeInfinity,
           float.NegativeInfinity,
           float.NegativeInfinity);
            max = new Vector3(float.PositiveInfinity,
           float.PositiveInfinity,
           float.PositiveInfinity);
        }
        public void SetToTransformedBox(AABB box, Calculations.Matrix3 m)
        {
            // If we're empty, then exit (an empty box defined as having the min/max
            // set to infinity)
            if (box.IsEmpty())
            {
                Empty();
                return;
            }
            // Examine each of the nine matrix elements
            // and compute the new AABB
            Vector3 min = new Vector3();
            Vector3 max = new Vector3();

            if (m.x1 > 0.0f) // x1 = x11 in the formula above
            {
                min.x += m.x1 * box.min.x; max.x += m.x1 * box.max.x;
            }
            else
            {
                min.x += m.x1 * box.max.x; max.x += m.x1 * box.min.x;
            }
            if (m.x2 > 0.0f) // y1 = x12 in the formula above
            {
                min.y += m.x2 * box.min.x; max.y += m.x2 * box.max.x;
            }
            else
            {
                min.y += m.x2 * box.max.x; max.y += m.x2 * box.min.x;
            }
            if (m.x3 > 0.0f) // z1 = x13 in the formula above
            {
                min.z += m.x3 * box.min.x; max.z += m.x3 * box.max.x;
            }
            else
            {
                min.z += m.x3 * box.max.x; max.z += m.x3 * box.min.x;
            }
 //---------------------------------------------------------------------------
            if (m.y1 > 0.0f) // x2 = y11 in the formula above
            {
                min.x += m.y1 * box.min.y; max.x += m.y1 * box.max.y;
            }
            else
            {
                min.x += m.y1 * box.max.y; max.x += m.y1 * box.min.y;
            }
            if (m.y2 > 0.0f) // y2 = y12 in the formula above
            {
                min.y += m.y2 * box.min.y; max.y += m.y2 * box.max.y;
            }
            else
            {
                min.y += m.y2 * box.max.y; max.y += m.y2 * box.min.y;
            }
            if (m.y3 > 0.0f) // z2 = y13 in the formula above
            {
                min.z += m.y3 * box.min.y; max.z += m.y3 * box.max.y;
            }
            else
            {
                min.z += m.y3 * box.max.y; max.z += m.y3 * box.min.y;
            }
//---------------------------------------------------------------------------
            if (m.z1 > 0.0f) // x3 = z11 in the formula above
            {
                min.x += m.z1 * box.min.z; max.x += m.z1 * box.max.z;
            }
            else
            {
                min.x += m.z1 * box.max.z; max.x += m.z1 * box.min.z;
            }
            if (m.z2 > 0.0f) // y2 = z12 in the formula above
            {
                min.y += m.z2 * box.min.z; max.y += m.z2 * box.max.z;
            }
            else
            {
                min.y += m.z2 * box.max.z; max.y += m.z2 * box.min.z;
            }
            if (m.z3 > 0.0f) // z1 = z13 in the formula above
            {
                min.z += m.z3 * box.min.z; max.z += m.z3 * box.max.z;
            }
            else
            {
                min.z += m.z3 * box.max.z; max.z += m.z3 * box.min.z;
            }

            box.min = min;
            box.max = max;

            // Continue like this for the remaining 6 matrix values
        }
    }
}

