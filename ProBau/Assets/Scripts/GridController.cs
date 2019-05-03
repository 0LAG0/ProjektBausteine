using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BrickIt.Vector3Extensions;

public class GridController : MonoBehaviour
{
    [SerializeField]
    float BrickHeight = 0.96f;

    [SerializeField]
    float BrickWidth = 0.8f;

    [SerializeField]
    public GameObject ToBeModeld;

    private Collider m_Collider;

    private Vector3 extends;
    private Vector3 halfExtends;

    private void Awake()
    {
        m_Collider = ToBeModeld.GetComponent<Collider>();

        extends = new Vector3(BrickWidth, BrickHeight, BrickWidth);
        halfExtends = extends * 0.5f;

        //if there is no meshcollider add one
        if (m_Collider == null)
        {
            var nMeshCol = ToBeModeld.AddComponent<MeshCollider>();
            nMeshCol.sharedMesh = ToBeModeld.GetComponentInChildren<MeshFilter>().sharedMesh;
            m_Collider = nMeshCol;
        }
    }

    private List<Vector3> resultPositions = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        var startT = System.DateTime.Now;
        float voxelcount = 0;
        GameObject container = new GameObject("BlockifiedObject");
        // Grid zu Object verschieben.
        Vector3 gridStart = m_Collider.bounds.min;
        Vector3 gridDepth = m_Collider.bounds.size.DivideComponentwise(extends).Ceil();
        for (int x = 0; x < gridDepth.x; x++)
        {
            for (int z = 0; z < gridDepth.z; z++)
            {
                for (int y = 0; y < gridDepth.y; y++)
                {
                    var pos = new Vector3(BrickWidth * x + BrickWidth / 2, BrickHeight * y + BrickHeight / 2, BrickWidth * z + BrickWidth / 2);
                    pos += gridStart;

                    if (IsWithin(pos))
                    {
                        resultPositions.Add(pos);
                        var minimalBrick = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        minimalBrick.transform.localScale = extends;
                        minimalBrick.transform.position = pos;
                        minimalBrick.transform.parent = container.transform;
                        voxelcount++;
                    }
                }
            }
        }
        Debug.Log("Jan: Time needed: " + (System.DateTime.Now - startT) + " for " + voxelcount + " Voxels");
    }

    private bool IsWithin(Vector3 pos)
    {
        return Physics.CheckBox(pos, halfExtends);
    }

    private bool IsWithin(Vector3 pos, int quaterization)
    {

        return false;
    }
}
