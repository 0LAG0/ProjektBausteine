using BrickIt.Vector3Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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
    public static Voxel[,,] Voxelize(Mesh mesh, Texture2D tex, float height, int depth)
    {
        var startT = System.DateTime.Now;
        float voxelcount = 0;
        bool hasUV = mesh.uv != null && mesh.uv.Length != 0;



        mesh = MeshUtils.OptimizeMesh(mesh, height);
        mesh.RecalculateNormals();
        float[] minMax = MeshUtils.GetBoundsPerDimension(mesh);

        int Height = (int)(minMax[3] / GlobalConstants.VoxelHeight) + 2;
        int Width = (int)(minMax[1] / GlobalConstants.VoxelWidth) + 2;
        int Depth = (int)(minMax[5] / GlobalConstants.VoxelWidth) + 2;
        var container = new Voxel[Width, Height, Depth];
        var verticez = mesh.vertices;
        var triangles = mesh.triangles;
        var normals = mesh.normals;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 a = verticez[triangles[i]];
            Vector3 b = verticez[triangles[i + 1]];
            Vector3 c = verticez[triangles[i + 2]];

            Vector3 aN = normals[triangles[i]];
            Vector3 bN = normals[triangles[i + 1]];
            Vector3 cN = normals[triangles[i + 2]];

            Vector3 normal = (aN + bN + cN) / 3;
            Vector3 min = Vector3.Min(a, Vector3.Min(b, c));
            Vector3 max = Vector3.Max(a, Vector3.Max(b, c));
            Color voxelColor = GlobalConstants.stockColor;
            if (hasUV)
            {
                voxelColor = tex.GetPixelBilinear(mesh.uv[triangles[i]].x, mesh.uv[triangles[i]].y);
            }

            for (int x = SnapToWidth(min.x); x <= SnapToWidth(max.x); x++)
            {
                for (int y = SnapToHeight(min.y); y <= SnapToHeight(max.y); y++)
                {
                    for (int z = SnapToWidth(min.z); z <= SnapToWidth(max.z); z++)
                    {
                        container[x, y, z].reverseNormal -= normal;
                        if (container[x, y, z].id == null)
                        {
                            var centerAbs = new Vector3(x * GlobalConstants.VoxelWidth, y * GlobalConstants.VoxelHeight, z * GlobalConstants.VoxelWidth);
                            if (TestTriangleBoxOverlap(centerAbs, new Vector3(GlobalConstants.VoxelWidth / 2, GlobalConstants.VoxelHeight / 2, GlobalConstants.VoxelWidth / 2), new Vector3[] { a, b, c }))
                            {
                                container[x, y, z].id = 0;
                                container[x, y, z].color = voxelColor;
                                voxelcount++;
                            }
                        }
                    }
                }
            }
        }

        for (int x = 0; x < container.GetLength(0); x++)
        {
            for (int y = 0; y < container.GetLength(1); y++)
            {
                for (int z = 0; z < container.GetLength(2); z++)
                {
                    if (container[x, y, z].id != null)
                    {
                        var newNormal = container[x, y, z].reverseNormal.normalized;
                        container[x, y, z].reverseNormal = new Vector3(Mathf.Round(newNormal.x), Mathf.Round(newNormal.y), Mathf.Round(newNormal.z));
                    }
                }
            }
        }

        return container;
    }

    public static Voxel[,,] AddWidth(Voxel[,,] inputVoxels, int width)
    {
        for (int x = 0; x < inputVoxels.GetLength(0); x++)
        {
            for (int y = 0; y < inputVoxels.GetLength(1); y++)
            {
                for (int z = 0; z < inputVoxels.GetLength(2); z++)
                {
                    var curVoxel = inputVoxels[x, y, z];
                    var posVector = new Vector3Int(x, y, z);
                    if (curVoxel.id != null && curVoxel.id != -1)
                    {
                        List<Vector3Int> vectorsToCheck = getVectorsToCheck(curVoxel);
                        for (int i = 1; i <= width; i++)
                        {
                            var nextPos = (i * curVoxel.reverseNormal) + posVector;
                            if (nextPos.x < 0 || nextPos.y < 0 || nextPos.z < 0 || nextPos.x >= inputVoxels.GetLength(0) || nextPos.y >= inputVoxels.GetLength(1) || nextPos.z >= inputVoxels.GetLength(2) || inputVoxels[(int)nextPos.x, (int)nextPos.y, (int)nextPos.z].id != null)
                            {
                                break;
                            }
                            inputVoxels[(int)nextPos.x, (int)nextPos.y, (int)nextPos.z].id = -1;
                            Color colorFromFirstFill = getClosestColorOnLayer(inputVoxels, nextPos, width);
                            inputVoxels[(int)nextPos.x, (int)nextPos.y, (int)nextPos.z].color = colorFromFirstFill;
                            foreach (Vector3Int vec in vectorsToCheck)
                            {
                                for (int s = 0; s <= i; s++)
                                {
                                    var curPos = nextPos + (vec * s);
                                    var curId = inputVoxels[(int)curPos.x, (int)curPos.y, (int)curPos.z].id;
                                    if (curId == 0)
                                    {
                                        break;
                                    }
                                    if (curId == -1)
                                    {
                                        continue;
                                    }
                                    if (inputVoxels[(int)curPos.x, (int)curPos.y, (int)curPos.z].id == null)
                                    {
                                        Color colorFromSecondFill = getClosestColorOnLayer(inputVoxels, curPos, width);
                                        inputVoxels[(int)curPos.x, (int)curPos.y, (int)curPos.z].id = -1;
                                        inputVoxels[(int)curPos.x, (int)curPos.y, (int)curPos.z].color = colorFromSecondFill;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        for (int x = 0; x < inputVoxels.GetLength(0); x++)
        {
            for (int y = 0; y < inputVoxels.GetLength(1); y++)
            {
                for (int z = 0; z < inputVoxels.GetLength(2); z++)
                {
                    if (inputVoxels[x, y, z].id == -1)
                    {
                        inputVoxels[x, y, z].id = 0;
                    }
                }
            }
        }
        return inputVoxels;
    }
    
    private static Color getClosestColorOnLayer(Voxel[,,] inputVoxels, Vector3 pos, int widthToCheck)
    {
        for (int s = 1; s <= widthToCheck+1; s++)
        {
            List<Vector3> colorChechkVectors = new List<Vector3>()
            {
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1),
            };
            foreach (var vec in colorChechkVectors)
            {
                var nextPos = (s * vec) + pos;
                if (nextPos.x >= 0 && nextPos.y >= 0 && nextPos.z >= 0 && nextPos.x < inputVoxels.GetLength(0) && nextPos.y < inputVoxels.GetLength(1) && nextPos.z < inputVoxels.GetLength(2))
                {
                    var curVoxel = inputVoxels[(int)nextPos.x, (int)nextPos.y, (int)nextPos.z];
                    if (curVoxel.id == 0)
                    {
                        return curVoxel.color;
                    }
                }
            }
        }
        return GlobalConstants.stockColor;
    }

    private static List<Vector3Int> getVectorsToCheck(Voxel curVoxel)
    {
        List<Vector3Int> vectorsToCheck = new List<Vector3Int>();
        if (curVoxel.reverseNormal.magnitude > 1)
        {
            // diagonal
            if (Mathf.Abs(curVoxel.reverseNormal.x) == 1)
            {
                vectorsToCheck.Add(new Vector3Int((int)-curVoxel.reverseNormal.x, 0, 0));
            }
            if (Mathf.Abs(curVoxel.reverseNormal.y) == 1)
            {
                vectorsToCheck.Add(new Vector3Int(0, (int)-curVoxel.reverseNormal.y, 0));
            }
            if (Mathf.Abs(curVoxel.reverseNormal.z) == 1)
            {
                vectorsToCheck.Add(new Vector3Int(0, 0, (int)-curVoxel.reverseNormal.z));
            }
        }
        else
        {
            // straight
            if (Mathf.Abs(curVoxel.reverseNormal.x) == 1)
            {
                vectorsToCheck.AddRange(GlobalConstants.AllPossibleStoneDirections.Where(vec => vec.x == 0));
            }

            else if (Mathf.Abs(curVoxel.reverseNormal.y) == 1)
            {
                vectorsToCheck.AddRange(GlobalConstants.AllPossibleStoneDirections.Where(vec => vec.y == 0));
            }

            else
            {
                vectorsToCheck.AddRange(GlobalConstants.AllPossibleStoneDirections.Where(vec => vec.z == 0));
            }
        }

        return vectorsToCheck;
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
}