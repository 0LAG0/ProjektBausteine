using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriBoxIntersection : MonoBehaviour
{
    public float gridsize;
    public Mesh mesh;
    private void Start()
    {
        var startT = System.DateTime.Now;
        float voxelcount = 0;
        var verticez = mesh.vertices;
        var triangles = mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 a = verticez[triangles[i]];
            Vector3 b = verticez[triangles[i + 1]];
            Vector3 c = verticez[triangles[i + 2]];
            Vector3 min = Vector3.Min(a, Vector3.Min(b, c));
            Vector3 max = Vector3.Max(a, Vector3.Max(b, c));
            for (float x = SnapToGrid(min.x) - gridsize; x < SnapToGrid(max.x) + gridsize; x += gridsize)
            {
                for (float y = SnapToGrid(min.y) - gridsize; y < SnapToGrid(max.y) + gridsize; y += gridsize)
                {
                    for (float z = SnapToGrid(min.z) - gridsize; z < SnapToGrid(max.z) + gridsize; z += gridsize)
                    {
                        if(TestTriangleBoxOverlap(new Vector3(x ,y ,z), new Vector3(gridsize/2, gridsize / 2, gridsize / 2), new Vector3[] { a, b, c })){
                            VoxelTools.MakeCube(new Vector3(x, y, z), VoxelTools.GetRandomColor(), gridsize);
                            voxelcount++;
                        }
                    }
                }
            }
        }
        Debug.Log("Lag: Time needed: " + (System.DateTime.Now - startT) + " for " + voxelcount + " Voxels");
    }


    public float SnapToGrid(float val)
    {
            return (Mathf.Round(val / gridsize) * gridsize);
    }




    //stolen from here: http://fileadmin.cs.lth.se/cs/Personal/Tomas_Akenine-Moller/code/tribox3.txt
    public static bool TestTriangleBoxOverlap(Vector3 boxCenter, Vector3 boxHalfSize, Vector3[] vertices)
    {
        // Move the triangle into the box's local coordinates
        vertices[0] -= boxCenter;
        vertices[1] -= boxCenter;
        vertices[2] -= boxCenter;

        // Find the triangle's edges
        Vector3 edge0 = vertices[1] - vertices[0];
        Vector3 edge1 = vertices[2] - vertices[1];
        Vector3 edge2 = vertices[0] - vertices[2];

        int x = 0, y = 1, z = 2;

        if (AxisTest(edge0, vertices[0], vertices[2], boxHalfSize, y, z)) return false;
        if (AxisTest(edge0, vertices[0], vertices[2], boxHalfSize, z, x)) return false;
        if (AxisTest(edge0, vertices[1], vertices[2], boxHalfSize, x, y)) return false;

        if (AxisTest(edge1, vertices[0], vertices[2], boxHalfSize, y, z)) return false;
        if (AxisTest(edge1, vertices[0], vertices[2], boxHalfSize, z, x)) return false;
        if (AxisTest(edge1, vertices[0], vertices[1], boxHalfSize, x, y)) return false;

        if (AxisTest(edge2, vertices[0], vertices[1], boxHalfSize, y, z)) return false;
        if (AxisTest(edge2, vertices[0], vertices[1], boxHalfSize, z, x)) return false;
        if (AxisTest(edge2, vertices[1], vertices[2], boxHalfSize, x, y)) return false;

        // Test overlap in x direction
        float min = Mathf.Min(vertices[0].x, Mathf.Min(vertices[1].x, vertices[2].x));
        float max = Mathf.Max(vertices[0].x, Mathf.Max(vertices[1].x, vertices[2].x));
        if (min > boxHalfSize.x || max < -boxHalfSize.x) return false;

        // Test overlap in y direction
        min = Mathf.Min(vertices[0].y, Mathf.Min(vertices[1].y, vertices[2].y));
        max = Mathf.Max(vertices[0].y, Mathf.Max(vertices[1].y, vertices[2].y));
        if (min > boxHalfSize.y || max < -boxHalfSize.y) return false;

        // Test overlap in z direction
        min = Mathf.Min(vertices[0].z, Mathf.Min(vertices[1].z, vertices[2].z));
        max = Mathf.Max(vertices[0].z, Mathf.Max(vertices[1].z, vertices[2].z));
        if (min > boxHalfSize.z || max < -boxHalfSize.z) return false;

        Vector3 normal = Vector3.Cross(edge0, edge1);
        return PlaneBoxOverlap(normal, vertices[0], boxHalfSize);
    }

    private static bool AxisTest(Vector3 edge, Vector3 va, Vector3 vb, Vector3 boxSize, int firstAxis, int secondAxis)
    {
        float a = edge[secondAxis];
        float b = edge[firstAxis];

        float p0 = a * va[firstAxis] - b * va[secondAxis];
        float p1 = a * vb[firstAxis] - b * vb[secondAxis];

        float max = Mathf.Max(p0, p1);
        float min = Mathf.Min(p0, p1);

        float radius = Mathf.Abs(a) * boxSize[firstAxis] + Mathf.Abs(b) * boxSize[secondAxis];

        return (min > radius || max < -radius);
    }

    private static float GetAxisValue(Vector3 v, string axisName)
    {
        var axis = typeof(Vector3).GetProperty(axisName);
        return System.Convert.ToSingle(axis.GetValue(v, null));
    }

    private static bool PlaneBoxOverlap(Vector3 normal, Vector3 point, Vector3 size)
    {
        float v;
        Vector3 min = new Vector3();
        Vector3 max = new Vector3();

        for (int i = 0; i < 3; ++i)
        {
            v = point[i];

            if (normal[i] > 0)
            {
                min[i] = -size[i] - v;
                max[i] = size[i] - v;
            }
            else
            {
                min[i] = size[i] - v;
                max[i] = -size[i] - v;
            }
        }

        if (Vector3.Dot(normal, min) > 0) return false;
        if (Vector3.Dot(normal, max) >= 0) return true;

        return false;
    }
}
