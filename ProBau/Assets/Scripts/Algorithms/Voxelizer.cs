using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class used for voxelization.
/// Uses TriBoxIntersection to determine if a block should get set.
/// </summary>
public class Voxelizer : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="tex"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    /// <remarks>NEEDS TO HANDLE COLOR!</remarks>
    public static bool[,,] Voxelize(Mesh mesh, Texture2D tex, float height)
    {
        var startT = System.DateTime.Now;
        //float voxelcount = 0;
        var textureCoordinates = mesh.uv;

        mesh = OptimizeMesh(mesh, height);
        float[] minMax = minMaxMesh(mesh);

        int Height = (int)(minMax[3] / GlobalConstants.VoxelHeight) + 2;
        int Width = (int)(minMax[1] / GlobalConstants.VoxelWidth) + 2;
        int Depth = (int)(minMax[5] / GlobalConstants.VoxelWidth) + 2;
        var container = new bool[Width, Height, Depth];
        var verticez = mesh.vertices;
        var triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 a = verticez[triangles[i]];
            Vector3 b = verticez[triangles[i + 1]];
            Vector3 c = verticez[triangles[i + 2]];
            Vector3 min = Vector3.Min(a, Vector3.Min(b, c));
            Vector3 max = Vector3.Max(a, Vector3.Max(b, c));
            //Color voxelColor = tex.GetPixelBilinear(textureCoordinates[triangles[i]].x, textureCoordinates[triangles[i]].y);
            for (int x = SnapToWidth(min.x); x <= SnapToWidth(max.x); x++)
            {
                for (int y = SnapToHeight(min.y); y <= SnapToHeight(max.y); y++)
                {
                    for (int z = SnapToWidth(min.z); z <= SnapToWidth(max.z); z++)
                    {
                        if (!container[x, y, z])
                        {
                            if (TestTriangleBoxOverlap(new Vector3(x * GlobalConstants.VoxelWidth, y * GlobalConstants.VoxelHeight, z * GlobalConstants.VoxelWidth)
                                , new Vector3(GlobalConstants.VoxelWidth / 2, GlobalConstants.VoxelHeight / 2, GlobalConstants.VoxelWidth / 2), new Vector3[] { a, b, c }))
                            {

                                //VoxelTools.MakeCube(new Vector3(x, y, z), Color.green, new Vector3(0.2f, 1.2f, 0.2f));
                                container[x, y, z] = true;
                                //voxelcount++;
                            }
                        }
                    }
                }
            }
        }
        return container;
    }

    private static Mesh OptimizeMesh(Mesh inputMesh, float height)
    {
        float[] minMax = minMaxMesh(inputMesh);//hat mal auf this.mesh refferenziert
        float origHeight = minMax[3] - minMax[2];
        float scale = height / origHeight;
        var nMesh = inputMesh;
        var vertsTemp = new Vector3[inputMesh.vertices.Length];
        //Debug.Log(inputMesh.vertices[50]);
        for (int n = 0; n < inputMesh.vertices.Length; n++)
        {
            Vector3 vert = inputMesh.vertices[n];
            vert.x -= minMax[0];
            vert.y -= minMax[2];
            vert.z -= minMax[4];
            vert *= scale;
            vertsTemp[n] = vert;
        }
        nMesh.vertices = vertsTemp;
        //Debug.Log(nMesh.vertices[50]);
        return nMesh;
    }

    private static int SnapToWidth(float val)
    {
        return (int)(Mathf.Round(val / GlobalConstants.VoxelWidth));
    }
    private static int SnapToHeight(float val)
    {
        return (int)(Mathf.Round(val / GlobalConstants.VoxelHeight));
    }

    //stolen from here: http://fileadmin.cs.lth.se/cs/Personal/Tomas_Akenine-Moller/code/tribox3.txt
    private static bool TestTriangleBoxOverlap(Vector3 boxCenter, Vector3 boxHalfSize, Vector3[] vertices)
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

    private static float[] minMaxMesh(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        float xLowest = Mathf.Infinity;
        float xHighest = 0;
        float yLowest = Mathf.Infinity;
        float yHighest = 0;
        float zLowest = Mathf.Infinity;
        float zHighest = 0;
        int i = 0;
        while (i < vertices.Length)
        {
            if (vertices[i].x < xLowest) xLowest = vertices[i].x;
            if (vertices[i].x > xHighest) xHighest = vertices[i].x;
            if (vertices[i].y < yLowest) yLowest = vertices[i].y;
            if (vertices[i].y > yHighest) yHighest = vertices[i].y;
            if (vertices[i].z < zLowest) zLowest = vertices[i].z;
            if (vertices[i].z > zHighest) zHighest = vertices[i].z;
            i++;
        }
        return new float[] { xLowest, xHighest, yLowest, yHighest, zLowest, zHighest };
    }
}