using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Old voxelizier.
/// </summary>
/// <remarks>DEPRECATED - MARKED FOR DELETION</remarks>
public class VoxelizerOld : MonoBehaviour
{
    public Mesh mesh;
    //public float height;
    //private float gridSize = 0.8f;
    void Start()
    {
        var startT = System.DateTime.Now;
        //float[] minMax = minMaxMesh(mesh);
        //float scale = height / Mathf.Abs(minMax[2] - minMax[3]);
        for (float x = -22; x < 22; x += 2f)
        {
            for (float y = -8; y < 8; y += 2f)
            {
                for (float z = -22; z < 22; z += 2f)
                {
                    if (isPointInside(mesh, new Vector3(x, y, z)))
                    {
                        //VoxelTools.MakeCube(new Vector3(x, y, z), VoxelTools.GetRandomColor(), new Vector3(2,2,2));
                    }
                }
            }
        }
        Debug.Log("time needed: " + (System.DateTime.Now - startT));
    }

    public bool isPointInside(Mesh aMesh, Vector3 aLocalPoint)
    {
        int hitCount = 0;
        var vertices = aMesh.vertices;
        var triangles = aMesh.triangles;
        Ray ray = new Ray(aLocalPoint, new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)));
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 a = vertices[triangles[i]];
            Vector3 b = vertices[triangles[i + 1]];
            Vector3 c = vertices[triangles[i + 2]];
            bool moTro = Intersect(a, b, c, ray);
            if(moTro)
            {
                hitCount += 1;
            }
        }
        if ((hitCount/2) % 2 == 1)
        {
            return true;
        }
        return false;
    }

    public static bool Intersect(Vector3 p1, Vector3 p2, Vector3 p3, Ray ray)
    {
        // Vectors from p1 to p2/p3 (edges)
        Vector3 e1, e2;

        Vector3 p, q, t;
        float det, invDet, u, v;


        //Find vectors for two edges sharing vertex/point p1
        e1 = p2 - p1;
        e2 = p3 - p1;

        // calculating determinant 
        p = Vector3.Cross(ray.direction, e2);

        //Calculate determinat
        det = Vector3.Dot(e1, p);

        //if determinant is near zero, ray lies in plane of triangle otherwise not
        if (det > -Mathf.Epsilon && det < Mathf.Epsilon) { return false; }
        invDet = 1.0f / det;

        //calculate distance from p1 to ray origin
        t = ray.origin - p1;

        //Calculate u parameter
        u = Vector3.Dot(t, p) * invDet;

        //Check for ray hit
        if (u < 0 || u > 1) { return false; }

        //Prepare to test v parameter
        q = Vector3.Cross(t, e1);

        //Calculate v parameter
        v = Vector3.Dot(ray.direction, q) * invDet;

        //Check for ray hit
        if (v < 0 || u + v > 1) { return false; }

        if ((Vector3.Dot(e2, q) * invDet) > Mathf.Epsilon)
        {
            //ray does intersect
            return true;
        }

        // No hit at all
        return false;
    }

    public float[] minMaxMesh(Mesh mesh)
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
