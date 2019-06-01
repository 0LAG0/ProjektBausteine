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
            Color color = Color.red;
            if (bb.extends.x == 2)
            {
                color = Color.blue;
            }

            if (bb.isFlipped)
            {
                Vector3 position = new Vector3(bb.pos.x, bb.pos.y, bb.pos.z);
                VoxelTools.MakeCube(position, color, new Vector3(bb.extends.z - 0.1f, bb.extends.y - 0.1f, bb.extends.x - 0.1f));
            }
            else
            {
                Vector3 position = new Vector3(bb.pos.x, bb.pos.y, bb.pos.z);
                VoxelTools.MakeCube(position, color, new Vector3(bb.extends.x - 0.1f, bb.extends.y - 0.1f, bb.extends.z - 0.1f));
            }

            //VoxelTools.MakeCube(bb.pos, VoxelTools.GetRandomColor(), bb.blockType.extends);
        }
    }
}

