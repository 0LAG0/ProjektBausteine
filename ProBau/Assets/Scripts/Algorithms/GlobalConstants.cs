using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class GlobalConstants
{
    public const float VoxelWidth = 1f;
    public const float VoxelHeight = 1.2f;

    public static readonly List<Vector3Int> BlockTypes;
    public static readonly List<Vector3Int> BlockDirections;

    static GlobalConstants()
    {
        BlockTypes = new List<Vector3Int>()
        {
            new Vector3Int(2,1,8),
            new Vector3Int(2,1,6),
            new Vector3Int(2,1,4),
            new Vector3Int(2,1,3),
            new Vector3Int(2,1,2),
            new Vector3Int(1,1,8),
            new Vector3Int(1,1,6),
            new Vector3Int(1,1,4),
            new Vector3Int(1,1,3),
            new Vector3Int(1,1,2),
            new Vector3Int(1,1,1)
        };

        BlockDirections = new List<Vector3Int>()
        {
            new Vector3Int(1,1,1),
            new Vector3Int(-1,1,1),
            new Vector3Int(1,1,-1),
            new Vector3Int(1,-1,1)
        };

    }

}
