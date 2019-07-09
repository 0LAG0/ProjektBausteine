using UnityEngine;

public class MeshUtils
{
    public static Mesh OptimizeMesh(Mesh inputMesh, float height)
    {
        float[] minMax = GetBoundsPerDimension(inputMesh);//hat mal auf this.mesh refferenziert
        float origHeight = minMax[3] - minMax[2];
        float scale = height / origHeight;
        var nMesh = inputMesh;
        var vertsTemp = new Vector3[inputMesh.vertices.Length];
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
        nMesh.RecalculateBounds();
        return nMesh;
    }

    public static Mesh RescaleAndCenterPivot(Mesh inputMesh, float height)
    {
        float[] minMax = GetBoundsPerDimension(inputMesh);
        float origHeight = minMax[3] - minMax[2];
        float scale = height / origHeight;
        var nMesh = inputMesh;
        var vertsTemp = new Vector3[inputMesh.vertices.Length];
        for (int n = 0; n < inputMesh.vertices.Length; n++)
        {
            Vector3 vert = inputMesh.vertices[n];
            //Move pivot to lower Left
            vert.x -= minMax[0];
            vert.y -= minMax[2];
            vert.z -= minMax[4];

            //center!
            vert.x -= (minMax[1] - minMax[0]) / 2;
            vert.y -= (minMax[3] - minMax[2]) / 2;
            vert.z -= (minMax[5] - minMax[4]) / 2;

            vert *= scale;
            vertsTemp[n] = vert;
        }
        nMesh.vertices = vertsTemp;
        nMesh.RecalculateBounds();
        return nMesh;
    }

    public static float[] GetBoundsPerDimension(Mesh mesh)
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

    public static void RescaleCenterPivotRotateInMesh(Mesh inputMesh, float height, Quaternion rotation)
    {
        float[] minMax = GetBoundsPerDimension(inputMesh);
        float origHeight = minMax[3] - minMax[2];
        float scale = height / origHeight;
        var vertsTemp = new Vector3[inputMesh.vertices.Length];
        for (int n = 0; n < inputMesh.vertices.Length; n++)
        {
            Vector3 vert = inputMesh.vertices[n];
            //Move pivot to lower Left
            vert.x -= minMax[0];
            vert.y -= minMax[2];
            vert.z -= minMax[4];

            //center!
            vert.x -= (minMax[1] - minMax[0]) / 2;
            vert.y -= (minMax[3] - minMax[2]) / 2;
            vert.z -= (minMax[5] - minMax[4]) / 2;

            vert *= scale;

            vert = rotation * vert;
            vertsTemp[n] = vert;
        }
        inputMesh.vertices = vertsTemp;
        inputMesh.RecalculateBounds();
    }
}
