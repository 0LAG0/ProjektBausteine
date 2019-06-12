using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class MeshDebug : MonoBehaviour
{
    public Mesh meshToDebug;

    private void Start()
    {
        for (int i = 0; i < meshToDebug.triangles.Length; i += 3)
        {
            Vector3 a = meshToDebug.vertices[meshToDebug.triangles[i]];
            Vector3 b = meshToDebug.vertices[meshToDebug.triangles[i+1]];
            Vector3 c = meshToDebug.vertices[meshToDebug.triangles[i+2]];

            Vector3 aN = meshToDebug.normals[meshToDebug.triangles[i]];
            Vector3 bN = meshToDebug.normals[meshToDebug.triangles[i + 1]];
            Vector3 cN = meshToDebug.normals[meshToDebug.triangles[i + 2]];

            Vector3 normal = (aN + bN + cN) / 3;
            Vector3 center = (a + b + c) / 3;
            Debug.DrawLine(
                                    //start
                                    new Vector3(center.x, center.y, center.z),
                                    //end
                                    new Vector3(center.x, center.y, center.z) +
                                    new Vector3(normal.x, normal.y, normal.z) / 1.5f
                                    , Color.magenta, 10000.0f);
        }
    }
}
