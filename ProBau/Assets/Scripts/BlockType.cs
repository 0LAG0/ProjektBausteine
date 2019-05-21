using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New BlockData", menuName = "Block Data", order = 51)]
public class BlockType : ScriptableObject
{
    /// <summary>
    /// Block extends in voxels.
    /// </summary>
    [SerializeField]
    public Vector3Int extends;

    [SerializeField]
    private string blockTypeName;

    [SerializeField]
    private Mesh mesh;

}