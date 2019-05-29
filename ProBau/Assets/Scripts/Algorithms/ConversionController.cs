using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// The actual Pipeline calling everything from voxelization to instacing the layouted bricks.
/// </summary>
public class ConversionController : MonoBehaviour
{
    public Mesh mesh;
    public Texture2D tex;
    /// <summary>
    /// Height in cm.
    /// </summary>
    public float targetHeight;

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            VoxelTools.MakeAllCubesFall();
        }
    }

    private void Start()
    {
        BlockSelector selector = new BlockSelector(null);
        var buildingBlocks = selector.calculateBlocksSpiral(Voxelizer.Voxelize(mesh, tex, targetHeight));
        //Debug.Log(buildingBlocks.Count);
        foreach (BuildingBlock bb in buildingBlocks)
        {
            Color color = Color.blue;
            if (bb.isFlipped)
            {
                color = Color.red;
            }

            if (bb.isFlipped)
            {
                VoxelTools.MakeCube(bb.pos, color, new Vector3(bb.extends.z, bb.extends.y, bb.extends.x));
            }
            else
            {
                VoxelTools.MakeCube(bb.pos, color, new Vector3(bb.extends.x, bb.extends.y, bb.extends.z));
            }

            //VoxelTools.MakeCube(bb.pos, VoxelTools.GetRandomColor(), bb.blockType.extends);
        }
    }
}

