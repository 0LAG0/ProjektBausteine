using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if (UNITY_EDITOR) 
public class ManipulateMesh : MonoBehaviour
{
    public Transform selectedGameObject;
    public float height;
    public Quaternion Quaternion;

    public string saveName;
    // Start is called before the first frame update

    void Start()
    {
        MeshUtils.RescaleCenterPivotRotateInMesh(selectedGameObject.GetComponentInChildren<MeshFilter>().mesh, height,Quaternion);
        //MeshUtils.RescaleAndCenterPivot(mesh, height);
        SaveAsset();
    }

    void SaveAsset()
    {
        var mf = selectedGameObject.GetComponentInChildren<MeshFilter>();
        if (mf)
        {
            var savePath = "Assets/" + saveName + ".asset";
            Debug.Log("Saved Mesh to:" + savePath);
            AssetDatabase.CreateAsset(mf.mesh, savePath);
        }
    }
}
#endif
