using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulateMesh : MonoBehaviour
{
    public Mesh mesh;
    public float height;
    public Quaternion Quaternion;
    // Start is called before the first frame update
    void Start()
    {
        MeshUtils.RescaleCenterPivotRotateInMesh(mesh,height,Quaternion);
        //MeshUtils.RescaleAndCenterPivot(mesh, height);
    }

}
